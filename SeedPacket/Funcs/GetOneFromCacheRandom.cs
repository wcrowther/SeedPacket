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
        /// <summary>Gets a single item from the generator.cache matching the string [cacheListname] like ('Invoices'). It will throw NOT an error <br/> 
        /// if the named cacheList does not exist. If [remove] is true, it will delete the item from the source.
        /// </summary>
        public static T GetOneFromCacheRandom<T>(this IGenerator generator, string cacheListName, bool remove = true)
        {
            ExpandoObject cache = generator.Cache;
            var cacheList = cache.Get<List<T>>(cacheListName);

            return GetOneFromCacheRandom<T>(generator, cacheList, remove);
        }

        /// <summary>Gets a single item from the generator.cache matching the dynamic [cacheList] like (generator.cache.Invoices). It WILL throw <br/>
        /// an error if the named cacheList does not exist. If [remove] is true, it will delete the item from the source.
        /// </summary>
        public static T GetOneFromCacheRandom<T>(this IGenerator generator, dynamic cacheList, bool remove = true)
        {
            List<T> itemList = cacheList;

            return itemList.TakeRandom<T>(1, generator.RowRandom, remove).FirstOrDefault();
        }
    }
}
