// bacteriamage.wordpress.com

using System;
using BacteriaMage.OgreBattle.ArmyTool.DataModel;
using BacteriaMage.OgreBattle.ArmyTool.GameSave;
using BacteriaMage.OgreBattle.DataFiles;

namespace BacteriaMage.OgreBattle.ArmyTool.CLI.Commands
{
    internal class ExportCommand : BaseSlotCommand
    {
        public ExportCommand(string savePath, string csvPath, SlotNumber? slotNumber)
            : base(savePath, csvPath, slotNumber)
        {
        }

        public override void Execute(SaveRam saveRam, Slot slot)
        {
            Characters characters = ReadCharacters(slot);

            WriteCharacters(characters, slot);

            DisplayExportSuccessful(characters.Count);
        }

        private Characters ReadCharacters(Slot slot)
        {
            return new SlotReader(TableProvider, slot).ReadCharacters();
        }

        private void WriteCharacters(Characters characters, Slot slot)
        {
            using (CsvWriter writer = new CsvWriter(characters, CsvPath))
            {
                writer.WriteDataSet();
            }
        }

        private void DisplayExportSuccessful(int characterCount)
        {
            Console.WriteLine();

            if (characterCount == 1)
            {
                Console.WriteLine("Exported one character successfully.");
            }
            else
            {
                Console.WriteLine($"Exported {characterCount} characters successfully.");
            }
        }
    }
}
