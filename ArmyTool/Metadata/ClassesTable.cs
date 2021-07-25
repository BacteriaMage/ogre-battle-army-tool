// bacteriamage.wordpress.com

using System;
using System.Collections.Generic;
using BacteriaMage.OgreBattle.DataFiles.Columns;
using BacteriaMage.OgreBattle.NamedValues;

namespace BacteriaMage.OgreBattle.ArmyTool.Metadata
{
    public class ClassesTable : ValuesTable<ClassEntry>
    {
        public int? WerewolfId => _werewolf.ValueId;
        public int? TigermanId => _tigerman.ValueId;
        public int? VampyreId => _vampyre.ValueId;

        public new static ClassesTable ReadTable(string path)
        {
            return new ClassesTable(path);
        }

        #region Meta columns definitions

        protected override IEnumerable<BaseColumn> GetMetaColumns()
        {
            yield return CreateEnumColumn(nameof(ClassEntry.ClassType), typeof(ClassTypes));
            yield return CreateEnumColumn(nameof(ClassEntry.UnitFlag), typeof(UnitFlags));
            yield return CreateBooleanColumn(nameof(ClassEntry.CanLeadUnit));
            yield return CreateDecimalColumn(nameof(ClassEntry.FrontRowTurns));
            yield return CreateDecimalColumn(nameof(ClassEntry.BackRowTurns));
        }

        private BaseColumn CreateEnumColumn(string propertyName, Type enumType)
        {
            return new EnumColumn(propertyName, this, GetProperty(propertyName), enumType);
        }

        private BaseColumn CreateBooleanColumn(string propertyName)
        {
            return new BooleanColumn(propertyName, this, GetProperty(propertyName));
        }

        private BaseColumn CreateDecimalColumn(string propertyName)
        {
            return new DecimalColumn(propertyName, this, GetProperty(propertyName));
        }

        #endregion

        #region Meta data validation

        protected override bool ValidateMetaData(ClassEntry tableEntry, out string message)
        {
            return ValidateClassType(tableEntry.ClassType, out message)
                && RememberWerewolfEntry(tableEntry, out message)
                && RememberTigermanEntry(tableEntry, out message)
                && RememberVampyreEntry(tableEntry, out message)
                && ValidateTurns(tableEntry.FrontRowTurns, out message)
                && ValidateTurns(tableEntry.BackRowTurns, out message);
        }

        private bool ValidateClassType(ClassTypes? type, out string message)
        {
            if (type == null)
            {
                message = "ClassType is required.";
                return false;
            }
            else
            {
                message = null;
                return true;
            }
        }

        private bool ValidateTurns(int? turns, out string message)
        {
            if (turns == null)
            {
                message = "Number of turns is required.";
                return false;
            }
            if (turns < 1 || turns > 9)
            {
                message = "Number of turns must be between 1 and 9 (inclusive).";
                return false;
            }
            else
            {
                message = null;
                return true;
            }
        }

        #endregion

        #region Special classes identification

        private ClassEntry _werewolf;
        private ClassEntry _tigerman;
        private ClassEntry _vampyre;

        private bool RememberWerewolfEntry(ClassEntry entry, out string message)
        {
            return RememberEntryForFlag(entry, UnitFlags.Werewolf, ref _werewolf, out message);
        }

        private bool RememberTigermanEntry(ClassEntry entry, out string message)
        {
            return RememberEntryForFlag(entry, UnitFlags.Tigerman, ref _tigerman, out message);
        }

        private bool RememberVampyreEntry(ClassEntry entry, out string message)
        {
            return RememberEntryForFlag(entry, UnitFlags.Vampyre, ref _vampyre, out message);
        }

        private bool RememberEntryForFlag(ClassEntry entry, UnitFlags flag, ref ClassEntry property, out string message)
        {
            if (entry.UnitFlag != flag)
            {
                message = null;
                return true;
            }
            else if (property != null && property.LineNumber != null)
            {
                message = $"{flag} class already exists on line {property.LineNumber}.";
                return false;
            }
            else if (property != null)
            {
                message = $"Duplicate {flag} class.";
                return false;
            }
            else
            {
                message = null;
                property = entry;
                return true;
            }
        }

        #endregion

        #region Constructors

        public ClassesTable()
        {
        }

        public ClassesTable(string path)
            : base(path)
        {
        }

        #endregion
    }
}