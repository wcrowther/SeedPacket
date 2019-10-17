using SeedPacket.Interfaces;

namespace SeedPacket.Functions
{
    public static partial class Funcs
    {
        public static string RandomZip (this IGenerator generator, bool plusfour = false)
        {
            string zip   = generator.RowRandom.Next(10001, 100000).ToString();
            string route = generator.RowRandom.Next(1000, 10000).ToString();

            return zip + (plusfour ? "-" + route : "");
        }
    }
}
