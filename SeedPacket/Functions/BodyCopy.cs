using SeedPacket.Interfaces;
using System.Text;
using NewLibrary.ForString;
using System;
using System.Linq;
using System.Collections.Generic;

namespace SeedPacket.Functions
{
    public static partial class Funcs
    {
        // For string return use: string.Join("<br />", BodyCopy(gen));
        public static List<string> BodyCopy (IGenerator generator, int minParagraphs = 3, int maxParagraphs = 10, int offset = 0)
        {
            int paragraphCount= generator.RowRandom.Next(minParagraphs, maxParagraphs + 1);
            int position = offset;
            var paragraphBuilder = new List<string>();

            for (int s = 1; s <= paragraphCount; s++)
            {
                string paragraph = LoremText(generator, offset: position);
                paragraphBuilder.Add(paragraph);
                position += paragraph.Split(null).Count();
            }
            return paragraphBuilder;
        }
    }
}
