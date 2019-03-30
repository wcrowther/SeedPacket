using System;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using WildHare.Extensions;

namespace Tests.SeedPacket.Core
{
	public static class Common
	{
		/// <summary>Turns a relative path in an application into an absolute file path similar the old MapPath function.</summary>
		public static string ToMapPath2(this string fileName)
		{
			var appRoot = GetApplicationRoot();
			string[] characters = { "~", "/", @"\" };
			string filePath = fileName.RemoveStart(characters).Replace("/", @"\"); ;

			return Path.Combine(appRoot, filePath);
		}

		/// <summary>Gets the root path of an application. This can have different meanings it different types of 
		/// applications, so check that your usage fully meets your needs before proceeding...</summary>
		public static string GetApplicationRoot()
		{
			var exePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase);
			var appPathMatcher = new Regex(@"(?<!fil)[A-Za-z]:\\+[\S\s]*?(?=\\+bin)");
			var appRoot = appPathMatcher.Match(exePath).Value;

			return appRoot;
		}
	}
}
