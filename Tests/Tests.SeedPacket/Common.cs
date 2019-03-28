using System;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Tests.SeedPacket
{
	public static class Common
	{
		public static string GetApplicationRoot()
		{
			var exePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase);
			var appPathMatcher = new Regex(@"(?<!fil)[A-Za-z]:\\+[\S\s]*?(?=\\+bin)");
			var appRoot = appPathMatcher.Match(exePath).Value;

			return appRoot;
		}
	}
}
