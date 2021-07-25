// bacteriamage.wordpress.com

using BacteriaMage.OgreBattle.NamedValues;

namespace BacteriaMage.OgreBattle.ArmyTool.Metadata
{
    public class ClassEntry : TableEntry
    {
        public ClassTypes? ClassType { get; set; }
        public UnitFlags? UnitFlag { get; set; }
        public bool? CanLeadUnit { get; set; }
        public int? FrontRowTurns { get; set; }
        public int? BackRowTurns { get; set; }

        public bool IsLord
        {
            get => ClassType == ClassTypes.Lord;
        }

        public bool IsSpecial
        {
            get => ClassType == ClassTypes.Special;
        }

        public bool IsVampyre
        {
            get => UnitFlag == UnitFlags.Vampyre;
        }

        public bool IsTigerMan
        {
            get => UnitFlag == UnitFlags.Tigerman;
        }

        public bool IsWerewolf
        {
            get => UnitFlag == UnitFlags.Werewolf;
        }

        public bool IsUndead
        {
            get => UnitFlag == UnitFlags.Undead;
        }

        public bool IsLarge
        {
            get => UnitFlag == UnitFlags.Large;
        }

        public ClassEntry(int valueId, string valueName)
            : base(valueId, valueName)
        {
        }

        public ClassEntry(int lineNumber, int valueId, string valueName)
            : base(lineNumber, valueId, valueName)
        {
        }

        public ClassEntry()
            : base()
        {
        }
    }
}
