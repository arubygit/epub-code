using System.IO;

namespace EpubSharp.Tests
{
    public static class Cwd
    {
        public static string Combine(string relativePath)
        {
            return Path.Combine(@"..\..\Samples", relativePath);
        }
    }
}
