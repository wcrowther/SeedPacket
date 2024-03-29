using SeedPacket.Interfaces;
using System.Text;

using System;
using System.Linq;
using WildHare.Extensions;

namespace SeedPacket.Functions
{
    public static partial class Funcs
    {
        public static string RandomLoremText (  this IGenerator generator,
                                                int minWords = 3, int maxWords = 16,
                                                int minSentences = 3, int maxSentences = 10,
                                                int offset = 0)
        {
            // Gets sentenceCount between minSentences and maxSentences, each consisting
            // of wordCount between minWords and maxWords. Offset governs which starting point int locums sequence.

            int sentenceCount= generator.RowRandom.Next(minSentences, maxSentences + 1);
            int position = offset;
            var textBuilder = new StringBuilder();

            for (int s = 1; s <= sentenceCount; s++)
            {
                string sentence = CreateSentence(generator, minWords, maxWords, ref position);
                textBuilder.Append(sentence);
            }
            return textBuilder.ToString().RemoveEnd(" ");
        }

        private static string CreateSentence(IGenerator generator, int minWords, int maxWords, ref int position)
        {
            int wordCount = generator.RowRandom.Next(minWords, maxWords + 1);
            var sentenceBuilder = new StringBuilder();

            for (int w = 1; w <= wordCount; w++)
            {
                // if datasource Lorem not populated, uses static "lorem"
                string text = GetElementNext(generator, "Lorem", position) ?? "lorem"; 

                text += (w == wordCount) ? ". " : " ";
                if (w == 1)
                    text = UpperCaseFirstLetter(text);

                sentenceBuilder.Append(text);
                position++;
            }
            string sentence = sentenceBuilder.ToString();

            int diceroll = DiceRoll(generator);
            sentence = AddCommas(sentence, diceroll);
            return sentence;
        }

        private static string AddCommas(string sentence , int diceroll)
        {
            var sentenceArray = sentence.Split(null);
            if (sentenceArray.Length > 11 && diceroll > 3) // 1d6
            {
                decimal x = sentenceArray.Length / 2;
                int i = (int) Math.Floor(x) - 2;
                sentenceArray[i] += ",";
            }
            return string.Join(" ", sentenceArray);
        }

        private static string UpperCaseFirstLetter(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            return char.ToUpper(s[0]) + s.Substring(1);
        }

    }
}
