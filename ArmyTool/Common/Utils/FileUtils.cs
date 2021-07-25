// bacteriamage.wordpress.com

using System.IO;

namespace BacteriaMage.OgreBattle.Common
{
    public static class FileUtils
    {
        public static string GetExtension(string path)
        {
            return Path.GetExtension(path)?.TrimStart('.') ?? string.Empty;
        }

        public static bool SameFile(string path1, string path2)
        {
            return Equals(new FileInfo(path1).FullName, new FileInfo(path2).FullName);
        }
    }
}
