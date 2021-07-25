// bacteriamage.wordpress.com

using BacteriaMage.OgreBattle.MemorySpan;

using static BacteriaMage.OgreBattle.ArmyTool.GameSave.Constants;

namespace BacteriaMage.OgreBattle.ArmyTool.GameSave
{
    internal struct CharacterSpans
    {
        public ByteSpan Identity { get; private set; }

        public ByteSpan Class { get; private set; }

        public ByteSpan Flags { get; private set; }

        public ByteSpan Level { get; private set; }

        public ByteSpan ExpPoints { get; private set; }

        public WordSpan HitPoints { get; private set; }

        public ByteSpan Strength { get; private set; }

        public ByteSpan Agility { get; private set; }

        public ByteSpan Intelligence { get; private set; }

        public ByteSpan Charisma { get; private set; }

        public ByteSpan Alignment { get; private set; }

        public ByteSpan Luck { get; private set; }

        public WordSpan Salary { get; private set; }

        public ByteSpan EquippedItem { get; private set; }

        public WordSpan Name { get; private set; }

        internal CharacterSpans(ByteSpan slotSpan)
        {
            Identity = ByteSlice(slotSpan, IdentityOffset);
            Class = ByteSlice(slotSpan, ClassOffset);
            Flags = ByteSlice(slotSpan, FlagsOffset);
            Level = ByteSlice(slotSpan, LevelOffset);
            ExpPoints = ByteSlice(slotSpan, ExpPointsOffset);
            HitPoints = WordSlice(slotSpan, HitPointsOffset);
            Strength = ByteSlice(slotSpan, StrengthOffset);
            Agility = ByteSlice(slotSpan, AgilityOffset);
            Intelligence = ByteSlice(slotSpan, IntelligenceOffset);
            Charisma = ByteSlice(slotSpan, CharismaOffset);
            Alignment = ByteSlice(slotSpan, AlignmentOffset);
            Luck = ByteSlice(slotSpan, LuckOffset);
            Salary = WordSlice(slotSpan, SalaryOffset);
            EquippedItem = ByteSlice(slotSpan, EquippedItemOffset);
            Name = WordSlice(slotSpan, NameOffset);
        }

        private static ByteSpan ByteSlice(ByteSpan slot, int offset)
        {
            return slot.Slice(offset, MaxCharacters);
        }

        private static WordSpan WordSlice(ByteSpan slot, int offset)
        {
            return WordSpan.Slice(slot, offset, MaxCharacters);
        }
    }
}
