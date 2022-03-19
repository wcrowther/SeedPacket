using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using WildHare.Extensions;

namespace SeedPacket.Examples.Helpers
{
    public static class Common
	{
		/// <summary>Experimental. Turns a relative path in an application into an absolute file path similar the old MapPath function. 
		/// This can have different meanings it different types of  applications, so check that your usage 
		/// fully meets your needs before proceeding...</summary>
		public static string ToMapPath(this string fileName)
		{
			var appRoot = GetApplicationRoot();
			string[] characters = { "~", "/", @"\" };
			string filePath = fileName.RemoveStart(characters).Replace("/", @"\"); ;

			return Path.Combine(appRoot, filePath);
		}

		/// <summary>Experimental. Gets the root path of an application. This can have different meanings it different types of 
		/// applications, so check that your usage fully meets your needs before proceeding...</summary>
		/// <example>
		///		string pathToTestXmlFile = $@"{Common.GetApplicationRoot()}\Logic\SourceFiles\xmlSeedSourcePlus.xml"
		/// </example>
		public static string GetApplicationRoot()
		{
			string exePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

			var appPathMatcher = new Regex(@"(?<!fil)[A-Za-z]:\\+[\S\s]*?(?=\\+bin)");
			var appRoot = appPathMatcher.Match(exePath).Value;

			return appRoot;
		}
	}
}
