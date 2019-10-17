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
        /// <summary>Gets a single item from the generator.cache matching the string [cacheListname] like ('Invoices').
        /// It will NOT throw  an error <br/>if the named [cacheList] does not exist. If [remove] is true,
        /// it will delete the items from the source.
        /// </summary>
        public static T GetOneFromCacheNext<T>(this IGenerator generator, string cacheListName, bool remove = true)
        {
            return GetListFromCacheNext<T>(generator, cacheListName, 1, 1, remove).SingleOrDefault();
        }

        /// <summary>Gets a single item from the generator.cache matching the dynamic [cacheList] like (generator.cache.Invoices).
        /// It WILL throw <br/>an error if the named [cacheList] does not exist. If [remove] is true, it will delete the item from the source.
        /// </summary>
        public static T GetOneFromCacheNext<T>(this IGenerator generator, dynamic cacheList, bool remove = true)
        {
            return GetListFromCacheNext<T>(generator, cacheList, 1, 1, remove).SingleOrDefault();
        }
    }
}
