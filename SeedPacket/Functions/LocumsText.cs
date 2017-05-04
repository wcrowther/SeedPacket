using SeedPacket.Interfaces;
using System.Text;
using NewLibrary.ForString;
using System;
using System.Linq;

namespace SeedPacket.Functions
{
    public static partial class Funcs
    {
        public static string LocumsText (IGenerator generator, int minWords = 3, int maxWords = 16, int minSentences = 3, int maxSentences = 10, int offset = 0)
        {
            // Gets sentenceCount between minSentences and maxSentences, each consisting
            // of wordCount between minWords and maxWords.

            int sentenceCount= generator.RowRandom.Next(minSentences, maxSentences + 1);
            int position = offset;
            var textBuilder = new StringBuilder();

            for (int s = 1; s <= sentenceCount; s++)
            {
                int wordCount = generator.RowRandom.Next(minWords, maxWords + 1);
                var sentenceBuilder = new StringBuilder();

                for (int w = 1; w <= wordCount; w++)
                {
                    string text = ElementNext(generator, "Lorem", position);
                    text += (w == wordCount) ? ". " : " ";
                    if (w == 1)
                        text = UpperCaseFirstLetter(text);

                    sentenceBuilder.Append(text);
                    position++;
                }
                string sentence = sentenceBuilder.ToString();

                int diceroll = DiceRoll(generator);
                sentence = AddCommas(sentence, diceroll);

                textBuilder.Append(sentence);
            }
            return textBuilder.ToString().removeEnd(" ");
        }

        private static string AddCommas(string sentence , int diceroll)
        {
            var sentenceArray = sentence.Split(null);
            if (sentenceArray.Length > 11 && diceroll >= 3)
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

        //var charArray = builder.ToString().ToCharArray();
        //    for (int i = charArray.Length - 1; i >= 0 ; i--)
        //    {
        //        if (charArray[i].ToString() == "." && i % 2 == 0)
        //        {
        //            charArray[i] = ',';
        //        }
        //}
        // return new string(charArray);
    }
}
