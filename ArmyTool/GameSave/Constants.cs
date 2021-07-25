// bacteriamage.wordpress.com

namespace BacteriaMage.OgreBattle.ArmyTool.GameSave
{
    public static class Constants
    {
        // sizes of various data structures and data types in bytes
        public const int SaveRamSize = 8192;
        public const int SlotSize = 2729;
        public const int WordSize = 2;
        public const int LeaderNameSize = 8;

        // maximum number of various entities in the game
        public const int MaxSlots = 3;
        public const int MaxCharacters = 100;
        public const int MaxUnitMembers = 5;
        public const int MaxUnits = 25;

        // values with predefined meanings
        public const int OpinionLeaderNameAddress = 0x07A4;
        public const int UnusedIndexId = 0xff;
        public const int UnequippedItemId = 0x00;

        // class id subranges
        public const int FirstClassId = 0x01;
        public const int LastClassId = 0x65;
        public const int FirstIdentityId = 0x65;
        public const int LastIdentityId = 0x7D;

        // character flags
        public const int UnusedFlag1 = 0x01;
        public const int UnusedFlag2 = 0x02;
        public const int VampyreFlag = 0x04;
        public const int TigermanFlag = 0x08;
        public const int WerewolfFlag = 0x10;
        public const int UndeadFlag = 0x20;
        public const int LargeFlag = 0x40;
        public const int UnitLeaderFlag = 0x80;

        // offsets to slots in save ram
        public const int Slot1Offset = 0x0002;
        public const int Slot2Offset = 0x0AAC;
        public const int Slot3Offset = 0x1556;

        // offsets to various values within each slot
        public const int SlotSizeOffset = 0x0000;
        public const int MapCountOffset = 0x0002;
        public const int LeaderNameOffset = 0x090F;
        public const int MoneyOffset = 0x092A;
        public const int ReputationOffset = 0x092E;
        public const int ChecksumOffset = 0x0AA7;

        // offsets to character statistic arrays within each slot
        public const int IdentityOffset = 0x0004;
        public const int ClassOffset = 0x0068;
        public const int FlagsOffset = 0x00CC;
        public const int LevelOffset = 0x0130;
        public const int ExpPointsOffset = 0x0194;
        public const int HitPointsOffset = 0x01F8;
        public const int StrengthOffset = 0x02C0;
        public const int AgilityOffset = 0x0324;
        public const int IntelligenceOffset = 0x0388;
        public const int CharismaOffset = 0x03EC;
        public const int AlignmentOffset = 0x0450;
        public const int LuckOffset = 0x04B4;
        public const int SalaryOffset = 0x0518;
        public const int EquippedItemOffset = 0x05E0;
        public const int NameOffset = 0x0644;

        // offsets to unit data arrays within each slot
        public const int UnitPositionOffset = 0x070C;
        public const int UnitMembershipOffset = 0x0789;
        public const int CharacterPoolOffset = 0x0806;
    }
}
