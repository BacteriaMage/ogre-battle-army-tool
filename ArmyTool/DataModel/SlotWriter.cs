// bacteriamage.wordpress.com

using System;
using System.Collections.Generic;
using System.Linq;
using BacteriaMage.OgreBattle.ArmyTool.GameSave;
using BacteriaMage.OgreBattle.Common;

using static BacteriaMage.OgreBattle.ArmyTool.GameSave.Constants;

using SlotCharacter = BacteriaMage.OgreBattle.ArmyTool.GameSave.Character;

namespace BacteriaMage.OgreBattle.ArmyTool.DataModel
{
    public class SlotWriter
    {
        private readonly Characters _characters;
        private readonly Slot _slot;
        
        public event EventHandler<MessageEventArgs> OnErrorMessage;
        public event EventHandler<MessageEventArgs> OnNoteMessage;

        public SlotWriter(Characters characters, Slot slot)
        {
            _characters = characters;
            _slot = slot;

            OnErrorMessage += ErrorMessageHandler;
        }

        public bool WriteCharacters()
        {
            try
            {
                TryWriteCharacters();
                return Success;
            }
            catch
            {
                SetValidationFailed();
                throw;
            }
        }

        private void TryWriteCharacters()
        {
            ValidateArmy();
            FixNumbers();

            ClearExistingArmy();

            BuildCharacters();
            BuildUnits();
        }

        private void ClearExistingArmy()
        {
            _slot.ClearArmy();
        }

        private void ErrorMessageHandler(object sender, MessageEventArgs eventArgs)
        {
            SetValidationFailed();
        }

        #region Army validation

        private void ValidateArmy()
        {
            ValidateArmySize();
            ValidateExactlyOneLord();
        }

        private void ValidateArmySize()
        {
            if (_characters.Count < 1)
            {
                throw new OgreBattleException("No characters found. There must be at least one.");
            }
            if (_characters.Count > MaxCharacters)
            {
                ReportError($"Too many characters to import. The maximum is {MaxCharacters}.");
            }
        }

        private void ValidateExactlyOneLord()
        {
            List<Character> lords = _characters.GetLordCharacters();

            if (lords.Count < 1 && _characters.Count > 0)
            {
                ReportError("A lord class character was not found. There must be one.");
            }
            if (lords.Count > 1)
            {
                string rowList = string.Join(",", lords.ToLines());

                ReportError($"Found lord class characters on lines: {rowList}. There must be exactly one lord.");
            }
        }

        #endregion

        #region Number assignment

        private void FixNumbers()
        {
            // put the rows in a predictable order so we can fix the number assignments;
            // the functions below all assume this list is in this ordering.
            SortCharacters();

            // remember the number for each character before fixing the assignments
            int?[] requestedAssignments = _characters.ToNumbers();

            // give each character an appropriate and unique number assignment
            FixAssignments();

            // get the final assignments for comparison
            int?[] assignedNumbers = _characters.ToNumbers();

            // report each character that didn't get the exact assignemnt desired.
            ReportReassignments(requestedAssignments, assignedNumbers);
        }

        private void SortCharacters()
        {
            _characters.Sort((Character a, Character b) =>
            {
                // sort the opinion leader to the top of the list since it must be number 1
                if (a.IsLord && b.IsLord)
                {
                    return 0;
                }
                else if (a.IsLord)
                {
                    return -1;
                }
                else if (b.IsLord)
                {
                    return 1;
                }

                // if both null or the same number then sort on the row number to maintain
                // the relative ordering of the imported rows
                else if (a.Number ==  b.Number)
                {
                    return a.LineNumber - b.LineNumber;
                }

                // sort nulls to the bottom so we can assign them whatever openings
                // are left available at the very end
                else if (a.Number == null)
                {
                    return 1;
                }
                else if (b.Number == null)
                {
                    return -1;
                }

                // sort the rest in the middle; they'll stay in the same relative order even
                // if we have to reassign the number
                else
                {
                    return a.Number.Value - b.Number.Value;
                }
            });
        }

        private void FixAssignments()
        {
            // give each character the number requested or a later number as that is as close
            // as possible. this will give a larger number of the request one is not available
            FixAssignmentsForward();

            // reassign any numbers past the maximum backward from the end in case of spill over
            FixAssignmentsBackward();

            // last, just assign the first available open slot to any characters without a number
            FixEmptyAssignments();
        }

        private void FixAssignmentsForward()
        {
            int nextAssignment = 1;

            for (int currentIndex = 0; currentIndex < _characters.Count; currentIndex++)
            {
                FixAssignmentForward(_characters[currentIndex], ref nextAssignment);
            }
        }

        private void FixAssignmentForward(Character c, ref int nextAssignment)
        {
            int? desiredAssignment = ComputeDesiredAssignment(c);

            if (desiredAssignment.HasValue)
            {
                c.Number = Math.Max(nextAssignment, desiredAssignment.Value);

                nextAssignment = c.Number.Value + 1;
            }
        }

        private int? ComputeDesiredAssignment(Character c)
        {
            if (c.IsLord)
            {
                return 1;
            }
            if (c.Number.HasValue)
            {
                return c.Number.Value;
            }
            else
            {
                return null;
            }
        }

        private void FixAssignmentsBackward()
        {
            // normally fix assignments backward from the maximum size of the
            // array in the game but if the import set is oversized then use
            // the size of the set so that everything will fit just so we can
            // finish validating the entire set.
            int nextAssignment = Math.Max(_characters.Count, MaxCharacters);

            for (int currentIndex = _characters.Count; currentIndex > 0; currentIndex--)
            {
                if (!FixAssignmentBackward(_characters[currentIndex - 1], ref nextAssignment))
                {
                    break;
                }
            }
        }

        private bool FixAssignmentBackward(Character c, ref int nextAssignment)
        {
            if (c.Number == null)
            {
                return true;
            }
            else if(c.Number.Value > nextAssignment)
            {
                c.Number = nextAssignment--;
                return true;
            }
            else
            {
                return false;
            }
        }

        private void FixEmptyAssignments()
        {
            Queue<int> availableNumbers = GetAvailableNumbers();

            foreach (Character character in _characters)
            {
                if (character.Number == null)
                {
                    character.Number = availableNumbers.Dequeue();
                }
            }
        }

        private Queue<int> GetAvailableNumbers()
        {
            HashSet<int> numberSet = new HashSet<int>();

            // we should just need available numbers up to the game's maximum
            // but if the import set is oversized make sure we allocate enough
            // so that every character can get a number just so we can finish
            // validating the entire set.
            int largestNumber = Math.Max(MaxCharacters, _characters.Count);

            // build a set of all possible assignments
            for (int number = 1; number <= largestNumber; number++)
            {
                numberSet.Add(number);
            }

            // remove those that are alread in use
            foreach (Character character in _characters)
            {
                if (character.Number.HasValue)
                {
                    numberSet.Remove(character.Number.Value);
                }
            }

            // return a queue of available values in ascending order
            return new Queue<int>(numberSet.OrderBy(number => number));
        }

        private void ReportReassignments(int?[] requestedNumbers, int?[] assignedNumbers)
        {
            if (requestedNumbers.Length > MaxCharacters)
            {
                // don't bother with this if the army is too large. the numbers assigned
                // in that case aren't valid so better to just hide all of it
                return;
            }

            for (int i = 0; i < _characters.Count; i++)
            {
                ReportReassignment(_characters[i], requestedNumbers[i], assignedNumbers[i]);
            }
        }

        private void ReportReassignment(Character c, int? requested, int? assigned)
        {
            if (requested.HasValue && assigned.HasValue && requested != assigned)
            {
                if (!c.IsLord)
                {
                    ReportNote(c, $"Number {requested.Value} is not available. {assigned.Value} was assigned instead.");
                }
                else if (assigned == 1)
                {
                    ReportNote(c, $"The lord character must be number 1. Automatically reassigned from {requested.Value}.");
                }
            }
        }

        #endregion

        #region Build characters

        private void BuildCharacters()
        {
            foreach (Character character in _characters)
            {
                BuildCharacter(character);
            }
        }

        private void BuildCharacter(Character modelCharacter)
        {
            SlotCharacter slotCharacter = GetSlotCharacter(modelCharacter);

            ValidateOpinionLeader(modelCharacter);
            ValidateUnitLeader(modelCharacter);
            ValidateHitPoints(modelCharacter);

            CopyName(modelCharacter, slotCharacter);
            CopyFields(modelCharacter, slotCharacter);
            SetFlags(modelCharacter, slotCharacter);
        }

        private SlotCharacter GetSlotCharacter(Character modelCharacter)
        {
            int characterNumber = modelCharacter.Number ?? 0;

            // if the character number isn't valid it means there's an upstream error;
            // just redirect to the first slot so we can still validate the rest of the
            // data. we'll end up clobbering the opinion leader slot but we can't write
            // the data back to the save anyway so it doesn't matter
            if (characterNumber < 1 || characterNumber > MaxCharacters)
            {
                characterNumber = 1;
            }

            // the data model uses 1 based indexes but the game data uses zero based
            return _slot.Characters[characterNumber - 1];
        }

        private void ValidateOpinionLeader(Character modelCharacter)
        {
            if (modelCharacter.IsLord)
            {
                bool isUnitOne = (modelCharacter.Unit == 1);
                bool isLeader = (modelCharacter.UnitLeader ?? false);

                if (!isUnitOne || !isLeader)
                {
                    ReportError(modelCharacter, "The lord character must be the leader of unit 1.");
                }

                if (modelCharacter.Identity.HasValue)
                {
                    ReportError(modelCharacter, "The lord character cannot have a special identity.");
                }
            }
        }

        private void ValidateUnitLeader(Character modelCharacter)
        {
            bool isLeader = modelCharacter.UnitLeader ?? false;

            if (isLeader && !modelCharacter.CanLeadUnit)
            {
                if (string.IsNullOrEmpty(modelCharacter.ClassName))
                {
                    ReportError(modelCharacter, "Character cannot lead units.");
                }
                else
                {
                    ReportError(modelCharacter, $"{modelCharacter.ClassName} characters cannot lead units.");
                }
            }
        }

        private void ValidateHitPoints(Character modelCharacter)
        {
            if (modelCharacter.IsUndead && modelCharacter.HitPoints != 0)
            {
                modelCharacter.HitPoints = 0;
                ReportNote(modelCharacter, "Auto-corrected non-zero HP for undead character.");
            }
            else if (!modelCharacter.IsUndead && modelCharacter.HitPoints == 0)
            {
                ReportError(modelCharacter, "Hit points must be greater than zero for living characters.");
            }
        }

        private void CopyName(Character modelCharacter, SlotCharacter slotCharacter)
        {
            if (string.IsNullOrEmpty(modelCharacter.Name.NameText))
            {
                ReportError(modelCharacter, "The character name is required.");
            }
            else
            {
                if (modelCharacter.IsLord)
                {
                    SetLeaderName(modelCharacter, slotCharacter);
                }
                else
                {
                    SetCharacterNameId(modelCharacter, slotCharacter);
                }
            }
        }

        private void SetLeaderName(Character modelCharacter, SlotCharacter slotCharacter)
        {
            if (!_slot.TrySetLeaderName(modelCharacter.Name.NameText, out string message))
            {
                ReportError(modelCharacter, message);
            }

            slotCharacter.Name = OpinionLeaderNameAddress;
        }

        private void SetCharacterNameId(Character modelCharacter, SlotCharacter slotCharacter)
        {
            int? nameId = modelCharacter.Name.NameId;

            if (nameId.HasValue)
            {
                slotCharacter.Name = nameId.Value;
            }
            else
            {
                ReportError(modelCharacter, $"\"{modelCharacter.Name.NameText}\" is not a valid character name. See Names.txt for available names.");
            }
        }

        private void CopyFields(Character modelCharacter, SlotCharacter slotCharacter)
        {
            slotCharacter.Identity = GetValidatedValue(modelCharacter, nameof(Character.Identity), 0, LastIdentityId, modelCharacter.Class ?? 0);
            slotCharacter.Class = GetValidatedValue(modelCharacter, nameof(Character.Class), FirstClassId, LastClassId);
            slotCharacter.Level = GetValidatedValue(modelCharacter, nameof(Character.Level), 1, 99);
            slotCharacter.HitPoints = GetValidatedValue(modelCharacter, nameof(Character.HitPoints), 0, 999);
            slotCharacter.Strength = GetValidatedValue(modelCharacter, nameof(Character.Strength), 1, 255);
            slotCharacter.Agility = GetValidatedValue(modelCharacter, nameof(Character.Agility), 1, 255);
            slotCharacter.Intelligence = GetValidatedValue(modelCharacter, nameof(Character.Intelligence), 1, 255);
            slotCharacter.Charisma = GetValidatedValue(modelCharacter, nameof(Character.Charisma), 0, 100);
            slotCharacter.Alignment = GetValidatedValue(modelCharacter, nameof(Character.Alignment), 0, 100);
            slotCharacter.Luck = GetValidatedValue(modelCharacter, nameof(Character.Luck), 0, 100);
            slotCharacter.ExpPoints = GetValidatedValue(modelCharacter, nameof(Character.Exp), 0, 99);
            slotCharacter.Salary = GetValidatedValue(modelCharacter, nameof(Character.Salary), 0, 65535);
            slotCharacter.EquippedItem = GetValidatedValue(modelCharacter, nameof(Character.EquippedItem), 0, 255, UnequippedItemId);
        }

        private int GetValidatedValue(Character character, string fieldName, int minimum, int maximum, int? defaultValue = null)
        {
            int? fieldValue = GetFieldValue(character, fieldName);

            if (fieldValue == null && defaultValue != null)
            {
                return defaultValue.Value;
            }

            if (fieldValue == null)
            {
                ReportError(character, $"{fieldName} is required.");
            }
            else if (fieldValue.Value < minimum || fieldValue.Value > maximum)
            {
                ReportError(character, $"{fieldValue.Value} is not a valid value for {fieldName}. {minimum} up to {maximum} is allowed.");
            }

            return fieldValue ?? 0;
        }

        private int? GetFieldValue(Character character, string fieldName)
        {
            return typeof(Character).GetProperty(fieldName).GetValue(character) as int?;
        }

        private void SetFlags(Character modelCharacter, SlotCharacter slotCharacter)
        {
            slotCharacter.IsVampyre = modelCharacter.IsVampyre;
            slotCharacter.IsTigerman = modelCharacter.IsTigerMan;
            slotCharacter.IsWerewolf = modelCharacter.IsWerewolf;
            slotCharacter.IsUndead = modelCharacter.IsUndead;
            slotCharacter.IsLarge = modelCharacter.IsLarge;
            slotCharacter.IsUnitLeader = modelCharacter.UnitLeader ?? false;
        }

        #endregion

        #region Build units

        private void BuildUnits()
        {
            for (int unitIndex = 0; unitIndex < MaxUnits; unitIndex++)
            {
                BuildUnit(_slot.Units[unitIndex], GetUnitCharacters(unitIndex + 1));
            }

            BuildCharacterPool();
        }

        private List<Character> GetUnitCharacters(int? unit)
        {
            return _characters
                .Where(character => CharacterMatchesUnitId(character, unit))
                .ToList();
        }

        private bool CharacterMatchesUnitId(Character character, int? unit)
        {
            if (IsValidUnitId(character.Unit))
            {
                // valid id; true if the id matches the search
                return character.Unit == unit;
            }
            else if (unit == null)
            {
                // invalid id; true if looking for character not in a unit
                return true;
            }
            else
            {
                // not a match
                return false;
            }
        }

        private bool IsValidUnitId(int? unitId)
        {
            return unitId.HasValue && (unitId.Value > 0) && (unitId.Value <= MaxUnits);
        }

        private void BuildUnit(Unit unit, List<Character> members)
        {
            ValidateUnit(unit, members);
            FindPositionConflicts(members);

            MoveLeaderToFirstIndex(members);
            AddUnitMembers(unit, members);
        }

        private void ValidateUnit(Unit unit, List<Character> members)
        {
            if (members.Count > 0)
            {
                ValidateUnitSize(members);
                ValidateUnitLeaderCount(members);
                ValidateUnitLargeCount(members);
            }
        }

        private void ValidateUnitSize(List<Character> members)
        {
            if (members.Count > MaxUnitMembers)
            {
                var extraMember = members[MaxUnitMembers];

                ReportError(extraMember, $"Too many characters in unit {extraMember.Unit}. The maximum is {MaxUnitMembers}.");
            }
        }

        private void ValidateUnitLeaderCount(List<Character> members)
        {
            List<Character> leaders = members.GetUnitLeaders();

            if (leaders.Count < 1)
            {
                ReportError(members[0], $"There is no unit leader for unit {members[0].Unit}.");
            }
            if (leaders.Count > 1)
            {
                ReportError(leaders[1], $"Unit {leaders[1].Unit} has more than one leader.");
            }
        }

        private void ValidateUnitLargeCount(List<Character> members)
        {
            List<Character> largeCharacters = members.GetLargeCharacters();
            List<Character> smallCharacters = members.GetSmallCharacters();

            if (largeCharacters.Count > 2)
            {
                ReportError(largeCharacters[2], $"Too many large characters in unit {largeCharacters[2].Unit}. The maximum is 2.");
            }
            if (largeCharacters.Count == 2 && smallCharacters.Count > 1)
            {
                ReportError(smallCharacters[1], $"Too many characters in unit {smallCharacters[1].Unit}. Total size limit is 3 for units with 2 large characters.");
            }
            if (largeCharacters.Count == 1 && smallCharacters.Count > 3)
            {
                ReportError(smallCharacters[3], $"Too many characters in unit {smallCharacters[3].Unit}. Total size limit is 4 for units with 1 large character.");
            }
        }

        private void FindPositionConflicts(List<Character> characters)
        {
            for (int indexA = 0; indexA < characters.Count; indexA++)
            {
                for (int indexB = indexA + 1; indexB < characters.Count; indexB++)
                {
                    ReportConflict(characters[indexA], characters[indexB]);
                }
            }
        }

        private void ReportConflict(Character a, Character b)
        {
            if (CharactersConflict(a, b))
            {
                ReportError(a, $"Unit position overlaps with character on line {b.LineNumber}.");
            }
        }

        private bool CharactersConflict(Character a, Character b)
        {
            // must be part of the same unit
            if (a.Unit == null || b.Unit == null || a.Unit.Value != b.Unit.Value)
            {
                return false;
            }

            // must be in the same row of the unit
            if (a.UnitRow == null || b.UnitRow == null || a.UnitRow.Value != b.UnitRow.Value)
            {
                return false;
            }

            // must be in the same or adjacent positions
            if (a.UnitPosition.HasValue && b.UnitPosition.HasValue)
            {
                int positionA = (int)a.UnitPosition.Value;
                int positionB = (int)b.UnitPosition.Value;

                return Math.Abs(positionB - positionA) < 2;
            }

            return false;
        }

        private void MoveLeaderToFirstIndex(List<Character> members)
        {
            members.StableSort(character => !(character.UnitLeader ?? false));
        }

        private void AddUnitMembers(Unit unit, List<Character> members)
        {
            for (int memberIndex = 0; memberIndex < members.Count; memberIndex++)
            {
                AddUnitMember(unit, members[memberIndex], memberIndex);
            }
        }

        private void AddUnitMember(Unit unit, Character character, int memberIndex)
        {
            UnitRow row = character.UnitRow ?? UnitRow.None;

            if (row == UnitRow.None)
            {
                ReportError(character, "Unit row is required.");
            }

            UnitPosition position = character.UnitPosition ?? UnitPosition.None;

            if (position == UnitPosition.None)
            {
                ReportError(character, "Unit position is required.");
            }

            if (memberIndex < unit.Members.Count)
            {
                UnitMember member = unit.Members[memberIndex];

                member.UnitId = character.Number.Value - 1;
                member.Row = row;
                member.Position = position;
                member.Turns = ComputeUnitMemberTurns(character, row);
            }
        }

        private int ComputeUnitMemberTurns(Character character, UnitRow row)
        {
            if (row == UnitRow.Front)
            {
                return character.FrontRowTurns;
            }
            else
            {
                return character.BackRowTurns;
            }
        }

        private void BuildCharacterPool()
        {
            List<Character> unassigned = GetUnitCharacters(null);

            for (int index = 0; index < unassigned.Count; index++)
            {
                AddPoolCharacter(unassigned[index], index);
            }
        }

        private void AddPoolCharacter(Character character, int index)
        {
            if (character.Unit.HasValue)
            {
                // characters with validate units numbers are processed with the unit so
                // if this isn't null here then it's a bad number.
                ReportError(character, $"{character.Unit} is not a valid unit number.");
            }
            else
            {
                if (character.UnitRow.HasValue)
                {
                    ReportError(character, $"Unit row is unexpected without a unit number.");
                }
                if (character.UnitPosition.HasValue)
                {
                    ReportError(character, $"Unit position is unexpected without a unit number.");
                }
                if (character.UnitLeader.HasValue)
                {
                    ReportError(character, $"Unit leader must be blank for characters without a unit number.");
                }

                _slot.CharacterPool[index] = character.Number.Value - 1;
            }
        }

        #endregion

        #region Message reprotings

        private void ReportError(string message)
        {
            OnErrorMessage?.Invoke(this, MessageEventArgs.Error(message));
        }

        private void ReportError(Character character, string message)
        {
            OnErrorMessage?.Invoke(this, MessageEventArgs.Error(character, message));
        }

        private void ReportNote(string message)
        {
            OnNoteMessage?.Invoke(null, MessageEventArgs.Note(message));
        }

        private void ReportNote(Character character, string message)
        {
            OnNoteMessage?.Invoke(null, MessageEventArgs.Note(character, message));
        }

        #endregion

        #region Validation failed flag

        private bool _validationFailed;

        public bool Success
        {
            get => !_validationFailed;
        }

        private void SetValidationFailed()
        {
            _validationFailed = true;
        }

        #endregion
    }
}
