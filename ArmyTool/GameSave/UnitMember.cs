// bacteriamage.wordpress.com

using BacteriaMage.OgreBattle.MemorySpan;

using static BacteriaMage.OgreBattle.ArmyTool.GameSave.Constants;

namespace BacteriaMage.OgreBattle.ArmyTool.GameSave
{
    public enum UnitRow
    {
        Front,
        Back,
        None,
    }

    public enum UnitPosition
    {
        RightFlank = 0x0,
        RightCenter = 0x1,
        Center = 0x2,
        LeftCenter = 0x3,
        LeftFlank = 0x4,
        None = 0xf,
    }

    public class UnitMember
    {
        private readonly ByteSpan _positionSpan;
        private readonly ByteSpan _membershipSpan;
        private readonly int _memberIndex;

        public int Turns
        {
            get => _positionSpan[_memberIndex] >> 4;
            set => _positionSpan[_memberIndex] = RawPosition | (value << 4);
        }

        public int RawPosition
        {
            get => _positionSpan[_memberIndex] & 0xf;
            set => _positionSpan[_memberIndex] = (Turns << 4) | (value & 0xf);
        }

        public int UnitId
        {
            get => _membershipSpan[_memberIndex];
            set => _membershipSpan[_memberIndex] = value;
        }

        public UnitRow Row
        {
            get => GetRow();
            set => RawPosition = MakeRawPosition(value, GetPosition(UnitPosition.RightFlank));
        }

        public UnitPosition Position
        {
            get => GetPosition();
            set => RawPosition = MakeRawPosition(GetRow(UnitRow.Front), value);
        }

        internal UnitMember(ByteSpan positionSpan, ByteSpan membershipSpan, int memberIndex)
        {
            _positionSpan = positionSpan;
            _membershipSpan = membershipSpan;
            _memberIndex = memberIndex;
        }

        public void Clear()
        {
            Turns = 0;
            Row = UnitRow.None;
            UnitId = UnusedIndexId;
        }

        private int MakeRawPosition(UnitRow row, UnitPosition position)
        {
            if (row == UnitRow.None || position == UnitPosition.None)
            {
                return (int)UnitPosition.None;
            }
            else if (row == UnitRow.Back)
            {
                return 5 + (int)position;
            }
            else
            {
                return (int)position;
            }
        }

        private UnitRow GetRow(UnitRow defaultRow = UnitRow.None)
        {
            if (RawPosition < 5)
            {
                return UnitRow.Front;
            }
            else if (RawPosition < 10)
            {
                return UnitRow.Back;
            }
            else
            {
                return defaultRow;
            }
        }

        private UnitPosition GetPosition(UnitPosition defaultPosition = UnitPosition.None)
        {
            if (RawPosition < 5)
            {
                return (UnitPosition)RawPosition;
            }
            else if (RawPosition < 10)
            {
                return (UnitPosition)(RawPosition - 5);
            }
            else
            {
                return defaultPosition;
            }
        }
    }
}
