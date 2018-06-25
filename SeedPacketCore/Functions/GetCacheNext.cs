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
        public static T GetCacheNext<T>(IGenerator generator, string cacheListName, int min, int max, bool remove = true)
        {
            ExpandoObject cache = generator.Cache;
            var cacheList = cache.GetByItemName<List<T>>(cacheListName);

            if (cacheList == null)
                return default(T);

            return GetCacheNext<T>(generator, cacheList, remove);
        }

        // This overload passes in a cacheList like: g.Cache.Items -> (will throw an error if does not exist)
        public static T GetCacheNext<T>(IGenerator generator, dynamic cacheList, bool remove = true)
        {
            if (!remove)
                throw new NotImplementedException("Currently only remove = true is supported for GetCacheNext");

            List<T> itemList = cacheList;

            T item = itemList.FirstOrDefault();

            if (remove && item != null)
                itemList.Remove(item);

            return item;
        }
    }
}
