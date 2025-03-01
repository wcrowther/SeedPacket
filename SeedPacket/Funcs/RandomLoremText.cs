using SeedPacket.Interfaces;
using System.Text;

using System;
using System.Linq;
using WildHare.Extensions;
using System.Data;
using System.Runtime.Intrinsics.X86;

namespace SeedPacket.Functions
{
    public static partial class Funcs
    {
        public static string RandomLoremText (  this IGenerator generator,
													 int minSentences = -1, 
													 int maxSentences = -1)
        {
			string[] lorem =
			[
				"Lorem ipsum dolor sit amet, consectetur adipiscing elit. ",
				"Sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. ",
				"Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. ",
				"Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. ",
				"Excepteur sint occaecat cupidatat non proident. ",
				"Sunt in culpa qui officia deserunt mollit anim id est laborum. "
			];

			minSentences = minSentences < 0 ? 2 : minSentences;
			maxSentences = maxSentences < 0 ? 6 : maxSentences;	

			int sentenceCount	= generator.RowRandom.Next(minSentences, maxSentences + 1);
			int startPosition	= generator.RowRandom.Next(1, 999);

			var sb = new StringBuilder();
			for (int s = 1; s <= sentenceCount; s++)
			{ 
				sb.Append(lorem.ElementIn(startPosition + s));
            }
            return sb.ToString().RemoveEnd(" ");
        }
    }
}
