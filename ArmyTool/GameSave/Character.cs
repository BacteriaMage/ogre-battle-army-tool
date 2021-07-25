// bacteriamage.wordpress.com

using static BacteriaMage.OgreBattle.ArmyTool.GameSave.Constants;

namespace BacteriaMage.OgreBattle.ArmyTool.GameSave
{
    public class Character
    {
        #region Character attributes

        public int Identity
        {
            get => _spans.Identity[_characterIndex];
            set => _spans.Identity[_characterIndex] = value;
        }

        public int Class
        {
            get => _spans.Class[_characterIndex];
            set => _spans.Class[_characterIndex] = value;
        }

        public int Flags
        {
            get => _spans.Flags[_characterIndex];
            set => _spans.Flags[_characterIndex] = value;
        }

        public bool IsVampyre
        {
            get => GetFlag(VampyreFlag);
            set => SetFlag(VampyreFlag, value);
        }

        public bool IsTigerman
        {
            get => GetFlag(TigermanFlag);
            set => SetFlag(TigermanFlag, value);
        }

        public bool IsWerewolf
        {
            get => GetFlag(WerewolfFlag);
            set => SetFlag(WerewolfFlag, value);
        }

        public bool IsUndead
        {
            get => GetFlag(UndeadFlag);
            set => SetFlag(UndeadFlag, value);
        }

        public bool IsLarge
        {
            get => GetFlag(LargeFlag);
            set => SetFlag(LargeFlag, value);
        }

        public bool IsUnitLeader
        {
            get => GetFlag(UnitLeaderFlag);
            set => SetFlag(UnitLeaderFlag, value);
        }

        public int Level
        {
            get => _spans.Level[_characterIndex];
            set => _spans.Level[_characterIndex] = value;
        }

        public int ExpPoints
        {
            get => _spans.ExpPoints[_characterIndex];
            set => _spans.ExpPoints[_characterIndex] = value;
        }

        public int HitPoints
        {
            get => _spans.HitPoints[_characterIndex];
            set => _spans.HitPoints[_characterIndex] = value;
        }

        public int Strength
        {
            get => _spans.Strength[_characterIndex];
            set => _spans.Strength[_characterIndex] = value;
        }

        public int Agility
        {
            get => _spans.Agility[_characterIndex];
            set => _spans.Agility[_characterIndex] = value;
        }

        public int Intelligence
        {
            get => _spans.Intelligence[_characterIndex];
            set => _spans.Intelligence[_characterIndex] = value;
        }

        public int Charisma
        {
            get => _spans.Charisma[_characterIndex];
            set => _spans.Charisma[_characterIndex] = value;
        }

        public int Alignment
        {
            get => _spans.Alignment[_characterIndex];
            set => _spans.Alignment[_characterIndex] = value;
        }

        public int Luck
        {
            get => _spans.Luck[_characterIndex];
            set => _spans.Luck[_characterIndex] = value;
        }

        public int Salary
        {
            get => _spans.Salary[_characterIndex];
            set => _spans.Salary[_characterIndex] = value;
        }

        public int EquippedItem
        {
            get => _spans.EquippedItem[_characterIndex];
            set => _spans.EquippedItem[_characterIndex] = value;
        }

        public int Name
        {
            get => _spans.Name[_characterIndex];
            set => _spans.Name[_characterIndex] = value;
        }

        #endregion

        #region Public methods

        public void Clear()
        {
            Identity = 0;
            Class = 0;
            Flags = 0;
            Level = 0;
            ExpPoints = 0;
            HitPoints = 0;
            Strength = 0;
            Agility = 0;
            Intelligence = 0;
            Charisma = 0;
            Alignment = 0;
            Luck = 0;
            Salary = 0;
            EquippedItem = 0;
            Name = 0;
        }

        public int GetUnusedFlags()
        {
            return _spans.Flags[_characterIndex] & (UnusedFlag1 | UnusedFlag2);
        }

        #endregion

        #region Flags support

        private bool GetFlag(int flag)
        {
            return (_spans.Flags[_characterIndex] & flag) > 0;
        }

        private void SetFlag(int flag, bool value)
        {
            if (value)
            {
                _spans.Flags[_characterIndex] = _spans.Flags[_characterIndex] | flag;
            }
            else
            {
                _spans.Flags[_characterIndex] = _spans.Flags[_characterIndex] & ~flag;
            }
        }

        #endregion

        #region Core

        private readonly CharacterSpans _spans;
        private readonly int _characterIndex;

        internal Character(CharacterSpans spans, int characterIndex)
        {
            _spans = spans;
            _characterIndex = characterIndex;
        }

        #endregion
    }
}
