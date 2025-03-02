using SeedPacket.Interfaces;
using System.Collections.Generic;

namespace SeedPacket.Functions
{
	public static partial class Funcs
    {
        // For string return use: .AsString());
        public static List<string> RandomBodyCopy ( this IGenerator gen,
													int minSentences = -1,
													int maxSentences = -1,
													int minParagraphs = -1, 
													int maxParagraphs = -1)
        {
			var paragraphs = new List<string>();

			minSentences = minSentences		< 0 ? 2  : minSentences;
			maxSentences = maxSentences		< 0 ? 6  : maxSentences;
			minParagraphs = minParagraphs	< 0 ? 5	 : minParagraphs;
			maxParagraphs = maxParagraphs	< 0 ? 10 : maxParagraphs;

			int paragraphCount = gen.RowRandom.Next(minParagraphs, maxParagraphs + 1);
			int startPosition = gen.RowRandom.Next(1, 999);

			for (int s = 1; s <= paragraphCount; s++)
			{
				paragraphs.Add(RandomLoremText(gen, minSentences, maxSentences));
			}
			return paragraphs;
		}
    }
}
