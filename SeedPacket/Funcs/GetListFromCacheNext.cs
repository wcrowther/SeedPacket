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
        /// if the named [cacheList] does not exist. If there are available elements in the [cacheList], it will get a random number of items <br/>
        /// from [min] to [max] starting from the beginning of the list. If [remove] is true, it will delete the items from the source.
        /// </summary>
        public static List<T> GetListFromCacheNext<T> (this IGenerator generator, string cacheListName, int min, int max, bool remove = true)
        {
            ExpandoObject cache = generator.Cache;
            var cacheList = cache.Get<List<T>>(cacheListName);

            if (cacheList == null)
                return null;

            return GetListFromCacheNext<T>(generator, cacheList, min, max, remove);
        }

        /// <summary>Gets a list of items from the generator.cache matching the dynamic [cacheList] like (generator.cache.Invoices). It WILL throw <br/>
        /// an error if the named [cacheList] does not exist. If there are available elements in the [cacheList], it will get a random number <br/>
        /// of items from [min] to [max] starting from the beginning of the list. If [remove] is true, it will delete the items from the source.
        /// </summary>
        public static List<T> GetListFromCacheNext<T> (IGenerator generator, dynamic cacheList, int min, int max, bool remove = true)
        {
            int count = generator.RowRandom.Next(min, max + 1);
            List<T> itemList = cacheList;

            var rowItems = itemList.Take(count).ToList();

            if (remove)
                rowItems.ForEach(i => itemList.Remove(i));

            return rowItems;
        }
    }
}
