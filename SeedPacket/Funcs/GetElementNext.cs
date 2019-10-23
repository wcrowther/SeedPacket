using SeedPacket.Interfaces;
using System.Linq;
using WildHare.Extensions;

namespace SeedPacket.Functions
{
    public static partial class Funcs
    {

        /// <summary>Gets the next element named for the [identifier] from the datasource contained in the <br/>
        /// generator passed in from [generator] parameter. By default goes to first record in the 0-based list, <br/>
        /// offset by the [offset] parameter. If the number is greater than what exists in the list, then it wraps<br/>
        /// back around to the first element in the list. If no elements exist in the list, null is returned.
        /// </summary>
        public static string GetElementNext (this IGenerator generator, string identifier = null, int offset = 0, bool wrap = true)
        {
            var propertyName = identifier ?? generator.CustomName ?? generator?.CurrentProperty?.Name ?? "";
            var strings = generator.Datasource.GetElementList(propertyName);
            int count = strings.Count;

            if (count == 0)
                return null;

            int rowNumberWithOffset = (generator.RowNumber - 1) + offset;
            int mod = (rowNumberWithOffset) % count;
            int position = mod;

            if (wrap)
                return strings?.ElementInOrDefault(position); 
            else
                return strings?.ElementAtOrDefault(position);
        }
    }
}
