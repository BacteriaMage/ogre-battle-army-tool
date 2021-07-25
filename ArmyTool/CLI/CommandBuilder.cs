// bacteriamage.wordpress.com

using BacteriaMage.OgreBattle.ArmyTool.CLI.Commands;

namespace BacteriaMage.OgreBattle.ArmyTool.CLI
{
    internal class CommandBuilder
    {
        public CommandArgs Args { get; set; }

        public CommandBuilder(string[] args)
            : this(new CommandArgs(args))
        {
        }

        public CommandBuilder(CommandArgs args)
        {
            Args = args;
        }

        public BaseCommand BuildCommand()
        {
            if (ShouldExport())
            {
                return CreateExportCommand();
            }
            else if (ShouldImport())
            {
                return CreateImportCommand();
            }
            else if (ShouldShowVersion())
            {
                return CreateShowVersion();
            }
            else
            {
                return CreateHelpCommand();
            }
        }

        private bool ShouldExport()
        {
            return (Action == Action.Export) || (!HaveAction && HaveSavePath && !HaveCsvPath);
        }

        private bool ShouldImport()
        {
            return (Action == Action.Import) || (!HaveAction && HaveSavePath && HaveCsvPath);
        }

        private bool ShouldShowVersion()
        {
            return Action == Action.Version;
        }

        private BaseCommand CreateExportCommand()
        {
            return new ExportCommand(SavePath, CsvPath, Slot);
        }

        private BaseCommand CreateImportCommand()
        {
            return new ImportCommand(SavePath, CsvPath, Slot);
        }

        private BaseCommand CreateShowVersion()
        {
            return new VersionCommand();
        }

        private BaseCommand CreateHelpCommand()
        {
            return new HelpCommand();
        }

        #region Computed properties

        private bool HaveAction
        {
            get => Args.Action.HasValue;
        }

        private Action Action
        {
            get => Args.Action ?? Action.Help;
        }

        private bool HaveSavePath
        {
            get => !string.IsNullOrWhiteSpace(Args.SavePath);
        }

        private string SavePath
        {
            get => Args.SavePath ?? string.Empty;
        }

        private bool HaveCsvPath
        {
            get => !string.IsNullOrWhiteSpace(Args.CsvPath);
        }

        private string CsvPath
        {
            get => Args.CsvPath ?? string.Empty;
        }

        private SlotNumber? Slot
        {
            get => Args.Slot;
        }

        #endregion
    }
}
