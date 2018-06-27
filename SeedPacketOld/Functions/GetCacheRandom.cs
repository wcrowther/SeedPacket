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
        // This overload passes in a cacheList by name string like: "Items" -> (will throw NOT an error if does not exist)
        public static T GetCacheRandom<T>(IGenerator generator, string cacheListName, bool remove = true)
        {
            ExpandoObject cache = generator.Cache;
            var cacheList = cache.GetByItemName<List<T>>(cacheListName);

            return GetCacheRandom<T>(generator, cacheList, remove);
        }

        // This overload passes in a cacheList like: g.Cache.Items -> (will throw an error if does not exist)
        public static T GetCacheRandom<T>(IGenerator generator, dynamic cacheList, bool remove = true)
        {
            List<T> itemList = cacheList;

            return itemList.TakeFromListRandom<T>(generator.RowRandom, 1, remove).FirstOrDefault();
        }
    }
}
