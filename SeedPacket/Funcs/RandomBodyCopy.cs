using SeedPacket.Interfaces;
using System.Text;

using System;
using System.Linq;
using System.Collections.Generic;
using WildHare.Extensions;

namespace SeedPacket.Functions
{
    public static partial class Funcs
    {
        // For string return use: string.Join("<br />", RandomBodyCopy(gen));
        public static List<string> RandomBodyCopy (this IGenerator generator,
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

			int paragraphCount = generator.RowRandom.Next(minParagraphs, maxParagraphs + 1);
			int startPosition = generator.RowRandom.Next(1, 999);

			for (int s = 1; s <= paragraphCount; s++)
			{
				paragraphs.Add(RandomLoremText(generator, minSentences, maxSentences));
			}
			return paragraphs;
		}
    }
}
