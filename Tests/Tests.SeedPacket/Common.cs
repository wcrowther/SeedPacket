using System;
using System.IO;

namespace Tests.SeedPacket
{
    public static class Common
    {
        public static string GetTestDirectory()
        {
            // returns bin directory of this test project
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

            // go up 2 levels to root of this test project
            return Path.GetFullPath(Path.Combine(baseDirectory, @"..\..\"));
        }
    }
}
