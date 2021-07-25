// bacteriamage.wordpress.com

using System;
using System.IO;
using BacteriaMage.OgreBattle.ArmyTool.CLI;
using BacteriaMage.OgreBattle.Common;

namespace BacteriaMage.OgreBattle.ArmyTool
{
    public class Program
    {
        private CommandBuilder _commandBuilder;

        private void TryRun()
        {
            Console.WriteLine($"{AssemblyInfo.Product} v{AssemblyInfo.InfoVersion}");
            Console.WriteLine(AssemblyInfo.Company);

            _commandBuilder.BuildCommand().Execute();
        }

        private void Run()
        {
            try
            {
                TryRun();
            }
            catch (OgreBattleException e)
            {
                ShowErrorMessage(e);
            }
            catch (FileNotFoundException e)
            {
                ShowErrorMessage(e);
            }
            catch (DirectoryNotFoundException e)
            {
                ShowErrorMessage(e);
            }
            catch (DriveNotFoundException e)
            {
                ShowErrorMessage(e);
            }
            catch (PathTooLongException e)
            {
                ShowErrorMessage(e);
            }
            catch (UnauthorizedAccessException e)
            {
                ShowErrorMessage(e);
            }
            catch (ArgumentException e) when (e.ParamName == null)
            {
                ShowErrorMessage(e);
            }
            catch (IOException e)
            {
                ShowErrorMessage(e);
            }
        }

        private void ShowErrorMessage(Exception error)
        {
            Console.WriteLine();
            Console.WriteLine(error.Message);
        }

        private Program(string[] args)
        {
            _commandBuilder = new CommandBuilder(args);
        }

        static void Main(string[] args)
        {
            try
            {
                new Program(args).Run();
            }
            catch (Exception e)
            {
                Console.WriteLine();
                Console.WriteLine(e.ToString());
            }
            finally
            {
                Console.WriteLine();
                UserUtils.WaitIfRunFromShell();
            }
        }
    }
}
