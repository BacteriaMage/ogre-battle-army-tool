// bacteriamage.wordpress.com

using System;
using System.IO;
using BacteriaMage.OgreBattle.ArmyTool.DataModel;
using BacteriaMage.OgreBattle.ArmyTool.GameSave;
using BacteriaMage.OgreBattle.DataFiles;

namespace BacteriaMage.OgreBattle.ArmyTool.CLI.Commands
{
    internal class ImportCommand : BaseSlotCommand
    {
        private int _errorCount;

        private MessageDisplay _messageDisplay = new MessageDisplay();

        public ImportCommand(string savePath, string csvPath, SlotNumber? slotNumber)
            : base(savePath, csvPath, slotNumber)
        {
        }

        public override void Execute(SaveRam saveRam, Slot slot)
        {
            Characters characters = LoadCharacters();

            if (WriteCharacters(characters, slot))
            {
                SaveGame(saveRam);
                DisplayImportSuccessful(characters.Count);
            }
            else
            {
                DisplayImportFailed();
            }
        }

        private Characters LoadCharacters()
        {
            Characters characters = new Characters(TableProvider);

            using (CsvReader reader = new CsvReader(characters, new StreamReader(CsvPath)))
            {
                reader.OnReadError += OnCsvError;

                if (reader.ReadDataSet())
                {
                    return characters;
                }
            }

            return null;
        }

        private bool WriteCharacters(Characters characters, Slot slot)
        {
            if (characters != null)
            {
                SlotWriter writer = new SlotWriter(characters, slot);

                writer.OnErrorMessage += OnWriteError;
                writer.OnNoteMessage += OnWriteWarning;

                writer.WriteCharacters();

                return writer.Success;
            }

            return false;
        }

        private void SaveGame(SaveRam saveRam)
        {
            saveRam.Save(SavePath);
        }

        private void DisplayImportSuccessful(int characterCount)
        {
            Console.WriteLine();

            if (characterCount == 1)
            {
                Console.WriteLine("Imported one character successfully.");
            }
            else
            {
                Console.WriteLine($"Imported {characterCount} characters successfully.");
            }
        }

        private void DisplayImportFailed()
        {
            Console.WriteLine();

            if (_errorCount < 2)
            {
                Console.WriteLine("Import was unsuccessful.");
            }
            else
            {
                Console.WriteLine($"Import was unsuccessful due to {_errorCount} errors.");
            }
        }

        private void OnCsvError(object sender, ReadErrorEventArgs e)
        {
            _messageDisplay.WriteError(e.Row, e.Message);
            _errorCount++;
        }

        private void OnWriteError(object sender, MessageEventArgs e)
        {
            _messageDisplay.WriteError(e.Line, e.Message);
            _errorCount++;
        }

        private void OnWriteWarning(object sender, MessageEventArgs e)
        {
            _messageDisplay.WriteMessage(e.Line, e.Message);
        }
    }
}
