using SeedPacket.Interfaces;

namespace SeedPacket.Functions
{
    public static partial class Funcs
    {
        /// <summary>Returns true every nth row where n is [count].</summary>
        public static bool OnceEvery (IGenerator generator, int count)
        {
            return generator.RowNumber % count == 0;
        }
    }
}
