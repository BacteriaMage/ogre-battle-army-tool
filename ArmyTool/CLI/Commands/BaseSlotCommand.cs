// bacteriamage.wordpress.com

using System;
using System.IO;
using BacteriaMage.OgreBattle.ArmyTool.DataModel;
using BacteriaMage.OgreBattle.ArmyTool.GameSave;
using BacteriaMage.OgreBattle.ArmyTool.Metadata;
using BacteriaMage.OgreBattle.Common;
using BacteriaMage.OgreBattle.NamedValues;

namespace BacteriaMage.OgreBattle.ArmyTool.CLI.Commands
{
    internal abstract class BaseSlotCommand : BaseCommand
    {
        protected string SavePath { get; private set; }

        protected string CsvPath { get; private set; }

        protected SlotNumber SlotNumber { get; private set; }

        protected TableProvider TableProvider { get; private set; }

        public BaseSlotCommand(string savePath, string csvPath, SlotNumber? slotNumber)
        {
            SavePath = savePath;
            CsvPath = ComputeCsvPath(savePath, csvPath);
            SlotNumber = slotNumber ?? SlotNumber.Slot1;
            TableProvider = BuildTables();

            ValidatePaths();
        }

        #region Execution

        public abstract void Execute(SaveRam saveRam, Slot slot);

        public override void Execute()
        {
            SaveRam saveRam = LoadGame();
            Slot slot = GetSlot(saveRam);

            if (slot.IsNothing)
            {
                DisplaySlotIsNothing();
            }
            else
            {
                DisplaySlotInfo(slot);
                Execute(saveRam, slot);
            }
        }

        private void DisplaySlotIsNothing()
        {
            Console.WriteLine();
            Console.WriteLine($"{SlotNumber} is a NOTHING slot.");
        }

        private void DisplaySlotInfo(Slot slot)
        {
            int level = slot.Characters[0].Level;

            Console.WriteLine();
            Console.WriteLine($"{SlotNumber} is {slot.LeaderName} (Level {level}, Scene {slot.Scene}).");
        }

        private SaveRam LoadGame()
        {
            return SaveRam.LoadNew(SavePath);
        }

        private Slot GetSlot(SaveRam saveRam)
        {
            switch (SlotNumber)
            {
                case SlotNumber.Slot1:
                    return saveRam.Slot1;
                case SlotNumber.Slot2:
                    return saveRam.Slot2;
                case SlotNumber.Slot3:
                    return saveRam.Slot3;
                default:
                    throw new OgreBattleException("The slot number is not valid.");
            }
        }

        private void ValidatePaths()
        {
            if (string.IsNullOrWhiteSpace(SavePath))
            {
                throw new OgreBattleException("The path to the game save file is required.");
            }

            if (string.IsNullOrWhiteSpace(CsvPath))
            {
                throw new OgreBattleException("The path to the CSV data file is required.");
            }

            if (FileUtils.SameFile(SavePath, CsvPath))
            {
                throw new OgreBattleException("The game save and CSV paths cannot refer to the same file.");
            }
        }

        #endregion

        #region Compute property values

        private string ComputeCsvPath(string savePath, string csvPath)
        {
            if (!string.IsNullOrWhiteSpace(csvPath))
            {
                return csvPath;
            }
            else if (!string.IsNullOrWhiteSpace(savePath))
            {
                return Path.ChangeExtension(savePath, ".csv");
            }
            else
            {
                return null;
            }
        }

        private TableProvider BuildTables()
        {
            return new TableProvider()
            {
                NamesTable = ReadTable("Names.txt"),
                ItemsTable = ReadTable("Items.txt"),
                IdentitiesTable = ReadTable("Identities.txt"),
                ClassesTable = ReadTable("Classes.txt", ClassesTable.ReadTable),
            };
        }

        private ValuesTable ReadTable(string fileName)
        {
            return ReadTable(fileName, ValuesTable.ReadTable);
        }

        private T ReadTable<T>(string fileName, Func<string, T> reader) where T : ValuesTable
        {
            string filePath = GetTableFilePath(fileName);

            return reader(filePath);
        }

        private string GetTableFilePath(string fileName)
        {
            string exePath = AssemblyInfo.FileInfo.FullName;

            string basePath = Path.GetDirectoryName(exePath);

            return Path.Combine(basePath, fileName);
        }

        #endregion
    }
}
