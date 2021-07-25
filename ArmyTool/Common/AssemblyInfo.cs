// bacteriamage.wordpress.com

using System;
using System.IO;
using System.Reflection;

namespace BacteriaMage.OgreBattle.Common
{
    internal static class AssemblyInfo
    {
        public static string Title
        {
            get => Asm.GetCustomAttribute<AssemblyTitleAttribute>().Title;
        }

        public static string Product
        {
            get => Asm.GetCustomAttribute<AssemblyProductAttribute>().Product;
        }

        public static string Company
        {
            get => Asm.GetCustomAttribute<AssemblyCompanyAttribute>().Company;
        }

        public static string Copyright
        {
            get => Asm.GetCustomAttribute<AssemblyCopyrightAttribute>().Copyright;
        }

        public static string Configuration
        {
            get => Asm.GetCustomAttribute<AssemblyConfigurationAttribute>().Configuration;
        }

        public static string InfoVersion
        {
            get => Asm.GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;
        }

        public static Version Version
        {
            get => Asm.GetName().Version;
        }

        public static DateTime BuildDateTime
        {
            get => new DateTime(2000, 1, 1).AddDays(Version.Build).AddSeconds(Version.Revision * 2);
        }

        public static FileInfo FileInfo
        {
            get => new FileInfo(Asm.Location);
        }

        public static string FileName
        {
            get => FileInfo.FullName;
        }

        public static string FileNameWithoutExtension
        {
            get => Path.GetFileNameWithoutExtension(FileName);
        }

        private static Assembly Asm
        {
            get => Assembly.GetEntryAssembly();
        }
    }
}
