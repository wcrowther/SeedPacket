using SeedPacket.Interfaces;
using System.Text;
using WildHare.Extensions;

namespace SeedPacket.Functions
{
	public static partial class Funcs
    {
		/// <summary>Generates random 'Lorem Ipsum' text. Optional parameters {minSentences} and {maxSentences} 
		/// for the minimum and maximum number of sentences generated defaults to min 2 and max 6. By default
		/// for performance, the function uses an internal list of 'lorem' sentences but {useDatasource} as true
		/// will pull the data using normal datasource routines.</summary>
		public static string RandomLoremText (  this IGenerator gen,
												int minSentences = -1, 
												int maxSentences = -1,
												bool useDatasource = false)
        {
			minSentences = minSentences < 0 ? 2 : minSentences;
			maxSentences = maxSentences < 0 ? 6 : maxSentences;	

			int sentenceCount	= gen.RowRandom.Next(minSentences, maxSentences + 1);
			int startPosition	= gen.RowRandom.Next(1, 999);

			var sb = new StringBuilder();
			for (int s = 1; s <= sentenceCount; s++)
			{ 
				string loremText = useDatasource ? GetElementNext(gen, "Lorem", startPosition + s)
												 : lorem.ElementIn(startPosition + s) ;
				sb.Append(loremText);
            }
            return sb.ToString().RemoveEnd(" ");
        }

		readonly static string[] lorem =
		[
			"Lorem ipsum dolor sit amet, consectetur adipiscing elit. ",
			"Sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. ",
			"Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. ",
			"Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. ",
			"Excepteur sint occaecat cupidatat non proident. ",
			"Sunt in culpa qui officia deserunt mollit anim id est laborum. "
		];
	}
}
