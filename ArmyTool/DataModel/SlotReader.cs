// bacteriamage.wordpress.com

using System.Diagnostics;
using BacteriaMage.OgreBattle.ArmyTool.GameSave;
using BacteriaMage.OgreBattle.ArmyTool.Metadata;
using static BacteriaMage.OgreBattle.ArmyTool.GameSave.Constants;

using SlotCharacter = BacteriaMage.OgreBattle.ArmyTool.GameSave.Character;

namespace BacteriaMage.OgreBattle.ArmyTool.DataModel
{
    public class SlotReader
    {
        private readonly TableProvider _tables;
        private readonly ClassesTable _classes;
        private readonly Slot _slot;

        private Characters _characters;

        public SlotReader(TableProvider tables, Slot slot)
        {
            _tables = tables;
            _classes = tables?.ClassesTable;
            _slot = slot;
        }

        public Characters ReadCharacters()
        {
            if (_characters == null)
            {
                _characters = new Characters(_tables);

                ReadAllUnitsCharacters();
                ReadCharacterPool();
                SortCharacters();
            }

            return _characters;
        }

        private void ReadAllUnitsCharacters()
        {
            for (int unitIndex = 0; unitIndex < MaxUnits; unitIndex++)
            {
                ReadOneUnitCharacters(unitIndex);
            }
        }

        private void ReadOneUnitCharacters(int unitIndex)
        {
            for (int memberIndex = 0; memberIndex < MaxUnitMembers; memberIndex++)
            {
                UnitMember member = _slot.Units[unitIndex].Members[memberIndex];

                if (member.UnitId != UnusedIndexId)
                {
                    Character character = ReadCharacter(member.UnitId);

                    MergeMemberInfo(character, member, unitIndex);

                    AssertMemberMetadata(character, member);

                    AddCharacter(character);
                }
            }
        }

        private void MergeMemberInfo(Character character, UnitMember member, int unitIndex)
        {
            character.Unit = unitIndex + 1;
            character.UnitRow = member.Row;
            character.UnitPosition = member.Position;
        }

        private void ReadCharacterPool()
        {
            for (int arrayIndex = 0; arrayIndex < MaxCharacters; arrayIndex++)
            {
                int characterIndex = _slot.CharacterPool[arrayIndex];

                if (characterIndex != UnusedIndexId)
                {
                    Character character = ReadCharacter(characterIndex);

                    AddCharacter(character);
                }
            }
        }

        private Character ReadCharacter(int index)
        {
            SlotCharacter character = _slot.Characters[index];

            AssertUnusedFlags(character);
            AssertCharacterFlags(character);

            return new Character(_tables)
            {
                Number = index + 1,
                Name = FormatName(character.Name),
                Identity = FormatIdentity(character.Identity),
                Class = FormatClass(character),
                Level = character.Level,
                HitPoints = character.HitPoints,
                Strength = character.Strength,
                Agility = character.Agility,
                Intelligence = character.Intelligence,
                Charisma = character.Charisma,
                Alignment = character.Alignment,
                Luck = character.Luck,
                Exp = character.ExpPoints,
                Salary = character.Salary,
                EquippedItem = FormatEquipment(character.EquippedItem),
                UnitLeader = FormatIsUnitLeader(character.IsUnitLeader),
            };
        }

        private void AddCharacter(Character character)
        {
            _characters.Add(character);
        }

        private CharacterName FormatName(int nameIdentifier)
        {
            CharacterName name = new CharacterName();

            if (nameIdentifier == OpinionLeaderNameAddress)
            {
                name.NameText = _slot.LeaderName;
            }
            else
            {
                name.NameId = nameIdentifier;
            }

            return name;
        }

        private int? FormatIdentity(int identityId)
        {
            if (identityId < FirstIdentityId)
            {
                // characters without a special identity just fill a regular class ID in
                // this field which doesn't mean much. just leave it blank for tidiness.
                return null;
            }
            else
            {
                return identityId;
            }
        }

        private int FormatClass(SlotCharacter character)
        {
            int classId = character.Class;

            if (character.IsWerewolf)
            {
                // class might be Beastman so normalize to Werewolf
                return _classes?.WerewolfId ?? classId;
            }
            if (character.IsTigerman)
            {
                // class might be Beastman so normalize to Tigerman
                return _classes?.TigermanId ?? classId;
            }
            if (character.IsVampyre)
            {
                // class might be daytime coffin so normalize to night form
                return _classes?.VampyreId ?? classId;
            }

            return classId;
        }

        private int? FormatEquipment(int itemId)
        {
            if (itemId == UnequippedItemId)
            {
                return null;
            }

            return itemId;
        }

        private bool? FormatIsUnitLeader(bool isUnitLeader)
        {
            if (isUnitLeader)
            {
                return true;
            }
            else
            {
                return null;
            }
        }

        private void SortCharacters()
        {
            // order each unit and the character pool by number
            _characters.StableSort(character => character.Number ?? 0);
            // but put the leader first in the unit
            _characters.StableSort(character => !(character.UnitLeader ?? false));
            // order the units by number
            _characters.StableSort(character => character.Unit ?? 0);
            // character pool goes at the bottom of the list after the units
            _characters.StableSort(character => !character.Unit.HasValue);
        }

        [Conditional("DEBUG")]
        private void AssertUnusedFlags(SlotCharacter character)
        {
            Debug.Assert(character.GetUnusedFlags() == 0);
        }

        [Conditional("DEBUG")]
        private void AssertCharacterFlags(SlotCharacter character)
        {
            if (_classes.TryGetTableEntry(character.Class, out ClassEntry classInfo))
            {
                Debug.Assert(character.IsVampyre == classInfo.IsVampyre);
                Debug.Assert(character.IsTigerman == classInfo.IsTigerMan);
                Debug.Assert(character.IsWerewolf == classInfo.IsWerewolf);
                Debug.Assert(character.IsUndead == classInfo.IsUndead);
                Debug.Assert(character.IsLarge == classInfo.IsLarge);
            }
        }

        [Conditional("DEBUG")]
        private void AssertMemberMetadata(Character character, UnitMember member)
        {
            if (_classes.TryGetTableEntry(character.Class ?? 0, out ClassEntry classInfo))
            {
                if (member.Row == UnitRow.Front)
                {
                    Debug.Assert(member.Turns == classInfo.FrontRowTurns);
                }
                else if (member.Row == UnitRow.Back)
                {
                    Debug.Assert(member.Turns == classInfo.BackRowTurns);
                }
            }
        }
    }
}
