// bacteriamage.wordpress.com

using BacteriaMage.OgreBattle.ArmyTool.GameSave;
using BacteriaMage.OgreBattle.ArmyTool.Metadata;

namespace BacteriaMage.OgreBattle.ArmyTool.DataModel
{
    public class Character
    {
        // data model metadata
        public int LineNumber { get; set; }

        // core character data
        public int? Number { get; set; }
        public CharacterName Name { get; set; }
        public int? Identity { get; set; }
        public int? Class { get; set; }
        public int? Level { get; set; }
        public int? HitPoints { get; set; }
        public int? Strength { get; set; }
        public int? Agility { get; set; }
        public int? Intelligence { get; set; }
        public int? Charisma { get; set; }
        public int? Alignment { get; set; }
        public int? Luck { get; set; }
        public int? Exp { get; set; }
        public int? Salary { get; set; }
        public int? EquippedItem { get; set; }

        // unit membership data
        public int? Unit { get; set; }
        public UnitRow? UnitRow { get; set; }
        public UnitPosition? UnitPosition { get; set; }
        public bool? UnitLeader { get; set; }

        // classes table
        private readonly ClassesTable _classes;
        private ClassEntry ClassInfo => _classes?.TryGetTableEntry(Class ?? 0);

        // class metadata
        public string ClassName => ClassInfo?.ValueName;
        public bool IsLord => ClassInfo?.IsLord ?? false;
        public bool IsSpecial => ClassInfo?.IsSpecial ?? false;
        public bool IsVampyre => ClassInfo?.IsVampyre ?? false;
        public bool IsTigerMan => ClassInfo?.IsTigerMan ?? false;
        public bool IsWerewolf => ClassInfo?.IsWerewolf ?? false;
        public bool IsUndead => ClassInfo?.IsUndead ?? false;
        public bool IsLarge => ClassInfo?.IsLarge ?? false;
        public bool CanLeadUnit => ClassInfo?.CanLeadUnit ?? false;
        public int FrontRowTurns => ClassInfo?.FrontRowTurns ?? 1;
        public int BackRowTurns => ClassInfo?.BackRowTurns ?? 1;

        public Character(TableProvider tables)
        {
            _classes = tables?.ClassesTable;
        }
    }
}
