// bacteriamage.wordpress.com

using System;
using System.Collections.Generic;
using BacteriaMage.OgreBattle.MemorySpan;

using static BacteriaMage.OgreBattle.ArmyTool.GameSave.Constants;

namespace BacteriaMage.OgreBattle.ArmyTool.GameSave
{
    public class Slot
    {
        private readonly ByteSpan slotSpan;
        private readonly LeaderName leaderName;

        public bool IsNothing
        {
            get; private set;
        }

        public IReadOnlyList<Character> Characters
        {
            get; private set;
        }

        public IReadOnlyList<Unit> Units
        {
            get; private set;
        }

        public ByteSpan CharacterPool
        {
            get; private set;
        }

        public string LeaderName
        {
            get => leaderName.GetString();
            set => leaderName.SetString(value);
        }

        public int Scene
        {
            get => slotSpan[MapCountOffset] + 1;
        }

        internal Slot(ByteSpan slotSpan)
        {
            this.slotSpan = slotSpan ?? throw new ArgumentNullException();

            Characters = CreateCharacters();
            Units = CreateUnits();
            CharacterPool = slotSpan.Slice(CharacterPoolOffset, MaxCharacters);
            leaderName = new LeaderName(slotSpan);
        }

        internal void AfterLoad()
        {
            // this class can't create a save from scratch so, just like the game,
            // consider any slot that can't be verified as empty ("Nothing").
            IsNothing = !VerifySlot();
        }

        internal void BeforeSave()
        {
            if (!IsNothing)
            {
                // if the slot was fllled to begin with then store the updated
                // checksum so that it reflects any changes. otherwise it was
                // empty so just ignore it
                slotSpan.SetWordAt(ChecksumOffset, ComputeChecksum());
            }
        }

        public bool TrySetLeaderName(string leaderName)
        {
            return this.leaderName.TrySetString(leaderName, out string _);
        }

        public bool TrySetLeaderName(string leaderName, out string errorMessage)
        {
            return this.leaderName.TrySetString(leaderName, out errorMessage);
        }

        public void ClearSlot()
        {
            slotSpan.Fill(0xff);
            IsNothing = true;
        }

        public void ClearArmy()
        {
            foreach (Character character in Characters)
            {
                character.Clear();
            }

            foreach (Unit unit in Units)
            {
                unit.Clear();
            }

            CharacterPool.Fill(UnusedIndexId);
        }

        private IReadOnlyList<Character> CreateCharacters()
        {
            List<Character> characters = new List<Character>();

            CharacterSpans spans = new CharacterSpans(slotSpan);

            for (int i = 0; i < MaxCharacters; i++)
            {
                characters.Add(new Character(spans, i));
            }

            return characters;
        }

        private IReadOnlyList<Unit> CreateUnits()
        {
            List<Unit> units = new List<Unit>();

            for (int i = 0; i < MaxUnits; i++)
            {
                units.Add(new Unit(slotSpan, i));
            }

            return units;
        }

        private bool VerifySlot()
        {
            int structureSize = slotSpan.GetWordAt(SlotSizeOffset);
            int storedChecksum = slotSpan.GetWordAt(ChecksumOffset);

            int computedChecksum = ComputeChecksum();

            bool structureSizeCorrect = (structureSize == SlotSize - WordSize);
            bool checksumCorrect = (storedChecksum == computedChecksum);

            return structureSizeCorrect && checksumCorrect;
        }

        private int ComputeChecksum()
        {
            ushort checksum = 0;

            // checksumed bytes between the structure size word and the checksum word
            int startChecksumIndex = SlotSizeOffset + WordSize;
            int stopChecksumIndex = ChecksumOffset;

            for (int i = startChecksumIndex; i < stopChecksumIndex; i++)
            {
                checksum = (ushort)(checksum + slotSpan[i]);
            }

            return checksum;
        }
    }
}
