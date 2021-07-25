// bacteriamage.wordpress.com

using System;
using System.Collections.Generic;
using System.IO;
using BacteriaMage.OgreBattle.Common;
using BacteriaMage.OgreBattle.MemorySpan;

using static BacteriaMage.OgreBattle.ArmyTool.GameSave.Constants;

namespace BacteriaMage.OgreBattle.ArmyTool.GameSave
{
    public sealed class SaveRam
    {
        private readonly byte[] saveRamBytes;

        public IReadOnlyList<Slot> Slots
        {
            get;
            private set;
        }

        public Slot Slot1
        {
            get;
            private set;
        }

        public Slot Slot2
        {
            get;
            private set;
        }

        public Slot Slot3
        {
            get;
            private set;
        }

        public SaveRam()
        {
            saveRamBytes = new byte[SaveRamSize];

            ByteSpan saveRamSpan = new ByteSpan(saveRamBytes);

            Slot1 = NewSlotObject(saveRamSpan, Slot1Offset);
            Slot2 = NewSlotObject(saveRamSpan, Slot2Offset);
            Slot3 = NewSlotObject(saveRamSpan, Slot3Offset);

            Slots = new List<Slot>() { Slot1, Slot2, Slot3 };
        }

        private static Slot NewSlotObject(ByteSpan saveRamSpan, int offset)
        {
            ByteSpan slotSpan = saveRamSpan.Slice(offset, SlotSize);

            return new Slot(slotSpan); 
        }

        public static SaveRam LoadNew(string filePath)
        {
            SaveRam saveRam = new SaveRam();

            saveRam.Load(filePath);

            return saveRam;
        }

        public void Load(string filePath)
        {
            ReadFileBytes(filePath);
            AfterLoad();
        }

        private void ReadFileBytes(string filePath)
        {
            using (FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                ReadStreamBytes(stream);
            }
        }

        private void ReadStreamBytes(Stream stream)
        {
            byte[] buffer = new byte[SaveRamSize];

            // first read to actually get the data
            int firstRead = stream.Read(buffer, 0, SaveRamSize);

            // second read to make sure we've reached the end of the file
            int secondRead = stream.Read(buffer, 0, SaveRamSize);

            // error out if the file is not the correct length
            if (firstRead != SaveRamSize || secondRead != 0)
            {
                throw new OgreBattleException("Not a SRAM file; the length is incorrect.");
            }

            Array.Copy(buffer, saveRamBytes, SaveRamSize);
        }

        private void AfterLoad()
        {
            foreach (Slot slot in Slots)
            {
                slot.AfterLoad();
            }
        }

        public void Save(string filePath)
        {
            BeforeSave();
            WriteFileBytes(filePath);
        }

        private void WriteFileBytes(string filePath)
        {
            File.WriteAllBytes(filePath, saveRamBytes);
        }

        private void BeforeSave()
        {
            foreach (Slot slot in Slots)
            {
                slot.BeforeSave();
            }
        }
    }
}
