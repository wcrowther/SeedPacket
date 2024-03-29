using SeedPacket.Interfaces;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using WildHare.Extensions;

namespace SeedPacket.Functions
{
    public static partial class Funcs
    {
        /// <summary>Gets a list of items from the generator.cache matching the string [cacheListname] like ('Invoices'). It will throw NOT an error <br/> 
        /// if the named cacheList does not exist. If there are available elements in the cacheList, it will get a random number of items <br/>
        /// from [min] to [max] starting from a RANDOM location in the list. If [remove] is true, it will delete the items from the source.
        /// </summary>
        public static List<T> GetListFromCacheRandom<T> (this IGenerator generator, string cacheListName, int min, int max, bool remove = true)
        {
            ExpandoObject cache = generator.Cache;
            var cacheList = cache.Get<List<T>>(cacheListName);
            int count = generator.RowRandom.Next(min, max + 1);

                        if (cacheList == null)
                return null;

            return cacheList.TakeRandom<T>(count, generator.RowRandom, remove).ToList();
        }
    }
}
