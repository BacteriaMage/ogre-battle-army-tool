// bacteriamage.wordpress.com

using System;
using System.Collections.Generic;
using BacteriaMage.OgreBattle.Common;
using BacteriaMage.OgreBattle.NamedValues;

namespace BacteriaMage.OgreBattle.ArmyTool.CLI
{
    public enum Action
    {
        Help,
        Version,
        Export,
        Import,
    }

    public enum SlotNumber
    {
        Slot1,
        Slot2,
        Slot3,
    }

    internal class CommandArgs
    {
        public Action? Action { get; set; }

        public SlotNumber? Slot { get; set; }

        public string SavePath { get; set; }

        public string CsvPath { get; set; }

        public List<string> Unmatched { get; set; }

        public CommandArgs(string[] args)
            : this(new List<string>(args))
        {
        }

        private CommandArgs(List<string> args)
        {
            Action = MatchOption<Action>(args);
            Slot = MatchOption<SlotNumber>(args);

            SavePath = MatchFilePath(args, new string[] { "srm", "sav" });
            CsvPath = MatchFilePath(args, new string[] { "csv", "txt" });

            Unmatched = args;
        }

        private T? MatchOption<T>(List<string> args) where T : struct
        {
            EnumValues<T> values = new EnumValues<T>();

            return (T?)MatchArg(args,
                (arg) => values.ContainsKey(arg),
                (arg) => values[arg]);
        }

        private bool MatchFlag(List<string> args, object flag)
        {
            object matched = MatchArg(args,
                (arg) => (string.Compare(arg, flag.ToString(), true) == 0),
                (arg) => true);

            return matched != null;
        }

        private string MatchFilePath(List<string> args, string[] extensions)
        {
            return (string)MatchArg(args,
                (arg) => extensions.ContainsCaseInsensitive(FileUtils.GetExtension(arg)),
                (arg) => arg);
        }

        private string MatchUnexpected(List<string> args)
        {
            return (string)MatchArg(args,
                (arg) => true,
                (arg) => arg);
        }

        private object MatchArg(List<string> args, Func<string, bool> check, Func<string, object> get)
        {
            for (int i = 0; i < args.Count; i++)
            {
                string arg = args[i];

                if (check(arg))
                {
                    args.RemoveAt(i);
                    return get(arg);
                }
            }

            return null;
        }
    }
}
