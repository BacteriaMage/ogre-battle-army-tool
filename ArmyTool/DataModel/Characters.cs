// bacteriamage.wordpress.com

using System.Collections.Generic;
using System.Reflection;
using BacteriaMage.OgreBattle.ArmyTool.DataModel.Columns;
using BacteriaMage.OgreBattle.DataFiles;
using BacteriaMage.OgreBattle.DataFiles.Columns;
using BacteriaMage.OgreBattle.NamedValues;

namespace BacteriaMage.OgreBattle.ArmyTool.DataModel
{
    public class Characters : List<Character>, IDataSet
    {
        public TableProvider Tables
        {
            get; set;
        }

        public Characters(TableProvider tables)
        {
            Tables = tables;
        }

        #region CSV interface

        private int _currentRowIndex = -1;

        BaseColumn[] IDataSet.GetColumns()
        {
            return new List<BaseColumn>(YieldColumns()).ToArray();
        }

        object IDataSet.GetRow()
        {
            if (_currentRowIndex < 0 || _currentRowIndex >= Count)
            {
                return null;
            }
            else
            {
                return this[_currentRowIndex];
            }
        }

        object IDataSet.NextRow()
        {
            _currentRowIndex++;
            return (this as IDataSet).GetRow();
        }

        object IDataSet.NewRow(int number)
        {
            Character newRow = new Character(Tables)
            {
                LineNumber = number,
            };

            _currentRowIndex = Count;

            Add(newRow);

            return newRow;
        }

        void IDataSet.RowDone(bool okay)
        {
        }

        #endregion

        #region Column definitions

        private IEnumerable<BaseColumn> YieldColumns()
        {
            yield return CreateDecimalColumn(nameof(Character.Number));
            yield return CreateCharacterNameColumn();
            yield return CreateTableColumn(nameof(Character.Identity), Tables.IdentitiesTable);
            yield return CreateTableColumn(nameof(Character.Class), Tables.ClassesTable);
            yield return CreateDecimalColumn(nameof(Character.Level));
            yield return CreateDecimalColumn(nameof(Character.HitPoints));
            yield return CreateDecimalColumn(nameof(Character.Strength));
            yield return CreateDecimalColumn(nameof(Character.Agility));
            yield return CreateDecimalColumn(nameof(Character.Intelligence));
            yield return CreateDecimalColumn(nameof(Character.Charisma));
            yield return CreateDecimalColumn(nameof(Character.Alignment));
            yield return CreateDecimalColumn(nameof(Character.Luck));
            yield return CreateDecimalColumn(nameof(Character.Exp));
            yield return CreateDecimalColumn(nameof(Character.Salary));
            yield return CreateTableColumn(nameof(Character.EquippedItem), Tables.ItemsTable);

            yield return CreateDecimalColumn(nameof(Character.Unit));
            yield return CreateEnumColumn(nameof(Character.UnitRow));
            yield return CreateEnumColumn(nameof(Character.UnitPosition));
            yield return CreateBooleanColumn(nameof(Character.UnitLeader));
        }

        private BaseColumn CreateDecimalColumn(string name)
        {
            return new DecimalColumn(name, this, GetProperty(name));
        }

        private BaseColumn CreateHexColumn(string name)
        {
            return new HexColumn(name, this, GetProperty(name));
        }

        private BaseColumn CreateEnumColumn(string name)
        {
            PropertyInfo property = GetProperty(name);

            return new EnumColumn(name, this, property, property.PropertyType);
        }

        private BaseColumn CreateBooleanColumn(string name)
        {
            return new BooleanColumn(name, this, GetProperty(name));
        }

        private BaseColumn CreateTableColumn(string name, ValuesTable table)
        {
            return new TableValueColumn(name, this, GetProperty(name), table);
        }

        private BaseColumn CreateCharacterNameColumn()
        {
            const string nameofName = nameof(Character.Name);

            return new CharacterNameColumn(nameofName, this, GetProperty(nameofName), Tables.NamesTable);
        }

        private PropertyInfo GetProperty(string propertyName)
        {
            return typeof(Character).GetProperty(propertyName);
        }

        #endregion
    }
}
