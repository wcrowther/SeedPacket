using SeedPacket.Interfaces;

namespace SeedPacket.Functions
{
    public static partial class Funcs
    {
        /// <summary>Returns true every nth row where n is [count].<br/>
        /// ie: var x = gen.OnceEvery(10) ? null : generator.GetElementNext("FirstName");
        /// </summary>
        public static bool OnceEvery (this IGenerator generator, int count)
        {
            return generator.RowNumber % count == 0;
        }
    }
}
