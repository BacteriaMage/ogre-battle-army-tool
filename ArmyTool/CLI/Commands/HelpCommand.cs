// bacteriamage.wordpress.com

using System;
using System.Collections.Generic;
using BacteriaMage.OgreBattle.Common;
using BacteriaMage.OgreBattle.NamedValues;

namespace BacteriaMage.OgreBattle.ArmyTool.CLI.Commands
{
    internal class HelpCommand : BaseCommand
    {
        public override void Execute()
        {
            Console.WriteLine();
            Console.WriteLine(GetHelp());
        }

        private string GetHelp()
        {
            if (UserUtils.RunFromConsole())
            {
                return GetConsoleHelp();
            }
            else
            {
                return GetShellHelp();
            }
        }

        private string GetConsoleHelp()
        {
            const string sram = "SRAM Path";
            const string csv = "CSV Path";

            string exeName = AssemblyInfo.FileNameWithoutExtension;
            string anySlot = EnumValues<SlotNumber>.ToList().Join(" | ");

            string[] lines = new string[]
            {
                $"{exeName} [{Action.Help}]",
                $"{exeName} {Action.Version}",
                $"{exeName} {Action.Export} <{sram}> [{csv}] [{anySlot}]",
                $"{exeName} {Action.Import} <{sram}> <{csv}> [{anySlot}]",
                $"{exeName} <{sram}> [{csv}]",
            };

            return new List<string>(lines).JoinLines();
        }

        private string GetShellHelp()
        {
            string[] lines = new string[]
            {
                "Drag a SRAM file for Ogre Battle: The March of the Black Queen from any emulator or cartridge dump ",
                "into this program. An export of all the characters and units from the saved game in slot 1 will be ",
                "saved in a CSV file with the same name and in the same folder as the SRAM file.",
                "\r\n\r\n",
                "The CSV (comma separated values) file can be opened and manipulated using whatever spreadsheet ",
                "software you prefer. Save your changes back to the file and ensure the CSV file format is used.",
                "\r\n\r\n",
                "Drag both the original SRAM file and the updated CSV file into the program to import you changes. ",
                "All the characters and units in the game in slot 1 will be replaced with those from the CSV file.",
                "\r\n\r\n",
                "You can also run this program directly from the command prompt where more options are available. See ",
                "the accompanying readme file for more information.",
            };

            return new List<string>(lines).Join().Wrap(Console.WindowWidth - 1);
        }
    }
}
