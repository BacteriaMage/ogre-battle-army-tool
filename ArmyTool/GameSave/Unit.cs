// bacteriamage.wordpress.com

using System.Collections.Generic;
using BacteriaMage.OgreBattle.MemorySpan;

using static BacteriaMage.OgreBattle.ArmyTool.GameSave.Constants;

namespace BacteriaMage.OgreBattle.ArmyTool.GameSave
{
    public class Unit
    {
        public IReadOnlyList<UnitMember> Members
        {
            get; private set;
        }

        public void Clear()
        {
            foreach (UnitMember member in Members)
            {
                member.Clear();
            }
        }

        internal Unit(ByteSpan slotSpan, int unitIndex)
        {
            ByteSpan positionSpan = Slice(slotSpan, UnitPositionOffset, unitIndex);
            ByteSpan membershipSpan = Slice(slotSpan, UnitMembershipOffset, unitIndex);

            Members = CreateMembers(positionSpan, membershipSpan);
        }

        private static ByteSpan Slice(ByteSpan slotSpan, int baseOffset, int index)
        {
            return slotSpan.Slice(baseOffset + index * MaxUnitMembers, MaxUnitMembers);
        }

        private static IReadOnlyList<UnitMember> CreateMembers(ByteSpan positionSpan, ByteSpan membershipSpan)
        {
            List<UnitMember> members = new List<UnitMember>();

            for (int index = 0; index < MaxUnitMembers; index++)
            {
                members.Add(new UnitMember(positionSpan, membershipSpan, index));
            }

            return members;
        }
    }
}
