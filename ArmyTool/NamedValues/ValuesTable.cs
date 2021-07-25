// bacteriamage.wordpress.com

using System.Collections.Generic;
using System.IO;
using System.Reflection;
using BacteriaMage.OgreBattle.Common;
using BacteriaMage.OgreBattle.DataFiles;
using BacteriaMage.OgreBattle.DataFiles.Columns;

namespace BacteriaMage.OgreBattle.NamedValues
{
    public abstract class ValuesTable
    {
        public static ValuesTable ReadTable(string path)
        {
            return new ValuesTable<TableEntry>(path);
        }

        public abstract bool ContainsValueId(int valueId);

        public abstract bool ContainsValueName(string valueName);

        public abstract int GetValueId(string valueName);

        public abstract string GetValueName(int valueId);

        public abstract bool TryGetValueId(string valueName, out int valueId);

        public abstract bool TryGetValueName(int valueId, out string valueName);

        public abstract int? TryGetValueId(string valueName);

        public abstract string TryGetValueName(int valueId);
    }

    public class ValuesTable<T> : ValuesTable, IDataSet where T : TableEntry, new()
    {
        private readonly Dictionary<int, T> _idIndex;
        private readonly Dictionary<string, T> _nameIndex;

        public ValuesTable()
            : base()
        {
            _idIndex = new Dictionary<int, T>();
            _nameIndex = new Dictionary<string, T>(CaseInsensitiveComparer.Instance);
        }

        public ValuesTable(string path)
            : this()
        {
            _filePath = path;

            using (MetaReader reader = new MetaReader(this, path))
            {
                reader.OnReadError += OnReadError;
                reader.ReadDataSet();
            }
        }

        #region Add Entries

        public void Add(int valueId, string valueName)
        {
            Add(new T()
            {
                ValueId = valueId,
                ValueName = valueName,
            });
        }

        public void Add(T tableEntry)
        {
            if (!Add(tableEntry, out string message))
            {
                throw new OgreBattleException(ErrorMessage(tableEntry, message));
            }
        }

        public bool Add(T tableEntry, out string message)
        {
            if (!ValidateEntry(tableEntry, out message))
            {
                return false;
            }

            _idIndex.Add(tableEntry.ValueId.Value, tableEntry);
            _nameIndex.Add(tableEntry.ValueName, tableEntry);

            return true;
        }

        private bool ValidateEntry(T tableEntry, out string message)
        {
            if (tableEntry.ValueId == null)
            {
                message = "Value ID is required.";
                return false;
            }
            if (tableEntry.ValueId < 0x00 || tableEntry.ValueId > 0xffff)
            {
                message = "Invalid value ID; expected unsigned 16-bit integer.";
                return false;
            }
            if (!ValidateUniqueId(TryGetTableEntry(tableEntry.ValueId.Value), out message))
            {
                return false;
            }


            if (string.IsNullOrEmpty(tableEntry.ValueName))
            {
                message = "Value name is required.";
                return false;
            }
            if (!ValidateUniqueName(TryGetTableEntry(tableEntry.ValueName), out message))
            {
                return false;
            }

            if (!ValidateMetaData(tableEntry, out message))
            {
                return false;
            }

            return true;
        }

        private bool ValidateUniqueId(T duplicate, out string message)
        {
            if (duplicate == null)
            {
                message = null;
                return true;
            }
            else if (duplicate.LineNumber == null)
            {
                message = $"ID value {duplicate.ValueId.Value.FormatHex()} is already in use by value named {duplicate.ValueName}";
                return false;
            }
            else
            {
                message = $"ID value {duplicate.ValueId.Value.FormatHex()} is already in use by value named {duplicate.ValueName} on line {duplicate.LineNumber}";
                return false;
            }
        }

        private bool ValidateUniqueName(T duplicate, out string message)
        {
            if (duplicate == null)
            {
                message = null;
                return true;
            }
            else if (duplicate.LineNumber == null)
            {
                message = $"Name value {duplicate.ValueName} is already in use by ID value {duplicate.ValueId.Value.FormatHex()}";
                return false;
            }
            else
            {
                message = $"Name value {duplicate.ValueName} is already in use by ID value {duplicate.ValueId.Value.FormatHex()} on line {duplicate.LineNumber}";
                return false;
            }
        }

        protected virtual bool ValidateMetaData(T tableEntry, out string message)
        {
            message = null;
            return true;
        }

        #endregion

        #region Base Lookup Methods

        public override bool ContainsValueId(int valueId)
        {
            return _idIndex.ContainsKey(valueId);
        }

        public override bool ContainsValueName(string valueName)
        {
            return _nameIndex.ContainsKey(valueName);
        }

        public override int GetValueId(string valueName)
        {
            return GetTableEntry(valueName).ValueId.Value;
        }

        public override string GetValueName(int valueId)
        {
            return GetTableEntry(valueId).ValueName;
        }

        public override bool TryGetValueId(string valueName, out int valueId)
        {
            int? nullableId = TryGetValueId(valueName);

            valueId = nullableId ?? 0;

            return nullableId.HasValue;
        }

        public override bool TryGetValueName(int valueId, out string valueName)
        {
            valueName = TryGetValueName(valueId);

            return (valueName != null);
        }

        public override int? TryGetValueId(string valueName)
        {
            return TryGetTableEntry(valueName)?.ValueId ?? null;
        }

        public override string TryGetValueName(int valueId)
        {
            return TryGetTableEntry(valueId)?.ValueName;
        }

        #endregion

        #region Entry Lookup Methods

        public T GetTableEntry(int valueId)
        {
            return _idIndex[valueId];
        }

        public T GetTableEntry(string valueName)
        {
            return _nameIndex[valueName];
        }

        public bool TryGetTableEntry(int valueId, out T tableEntry)
        {
            return _idIndex.TryGetValue(valueId, out tableEntry);
        }

        public bool TryGetTableEntry(string valueName, out T tableEntry)
        {
            return _nameIndex.TryGetValue(valueName, out tableEntry);
        }

        public T TryGetTableEntry(int valueId)
        {
            return _idIndex.TryGetValue(valueId);
        }

        public T TryGetTableEntry(string valueName)
        {
            return _nameIndex.TryGetValue(valueName);
        }

        #endregion

        #region DataSet Interface

        T _currentRow;

        BaseColumn[] IDataSet.GetColumns()
        {
            List<BaseColumn> columns = new List<BaseColumn>();

            columns.AddRange(GetKeyColumns());
            columns.AddRange(GetMetaColumns());

            return columns.ToArray();
        }

        object IDataSet.GetRow()
        {
            return _currentRow;
        }

        object IDataSet.NewRow(int number)
        {
            _currentRow = new T()
            {
                LineNumber = number
            };

            return _currentRow;
        }

        void IDataSet.RowDone(bool okay)
        {
            if (okay)
            {
                Add(_currentRow);
            }

            _currentRow = null;
        }

        object IDataSet.NextRow()
        {
            throw new System.NotImplementedException();
        }

        #endregion

        #region DataSet Methods

        private readonly string _filePath;

        private IEnumerable<BaseColumn> GetKeyColumns()
        {
            yield return new HexColumn("ID", this, GetProperty(nameof(TableEntry.ValueId)));
            yield return new StringColumn("Name", this, GetProperty(nameof(TableEntry.ValueName)));
        }

        protected virtual IEnumerable<BaseColumn> GetMetaColumns()
        {
            yield break;
        }

        protected PropertyInfo GetProperty(string propertyName)
        {
            return typeof(T).GetProperty(propertyName);
        }

        #endregion

        #region Error Handling

        private string ErrorMessage(T tableEntry, string text)
        {
            return ErrorMessage(tableEntry.LineNumber, text);
        }

        private string ErrorMessage(int? lineNumber, string text)
        {
            if (!string.IsNullOrEmpty(_filePath) && lineNumber.HasValue)
            {
                return $"{Path.GetFileName(_filePath)}({lineNumber}): {text}";
            }
            else if (lineNumber.HasValue)
            {
                return $"(line {lineNumber}): {text}";
            }
            else
            {
                return text;
            }
        }

        private void OnReadError(object sender, ReadErrorEventArgs e)
        {
            throw new OgreBattleException(ErrorMessage(e.Row, e.Message));
        }

        #endregion
    }
}