// bacteriamage.wordpress.com

using System;
using BacteriaMage.OgreBattle.Common;

namespace BacteriaMage.OgreBattle.ArmyTool.CLI.Commands
{
    internal class VersionCommand : BaseCommand
    {
        public override void Execute()
        {
            Console.WriteLine();
            Console.WriteLine($"Build version: {GetVersion()}");
            Console.WriteLine($"Build time:    {AssemblyInfo.BuildDateTime}");
        }

        private string GetVersion()
        {
#if DEBUG
            return $"{AssemblyInfo.Version} ({AssemblyInfo.Configuration})";
#else
            return $"{AssemblyInfo.Version}";
#endif
        }
    }
}
