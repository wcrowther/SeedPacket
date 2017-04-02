using SeedPacket.Extensions;
using SeedPacket.Interfaces;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace SeedPacket.Functions
{
    public static partial class func
    {
        // This overload passes in a cacheList by name string like: "Items" -> (will throw NOT an error if does not exist)
        public static List<T> NextCacheItems<T> (IGenerator generator, string cacheListName, int min, int max, bool remove = true)
        {
            ExpandoObject cache = generator.Cache;
            var cacheList = cache.GetByItemName<List<T>>(cacheListName);

            if (cacheList == null)
                return null;

            return NextCacheItems<T>(generator, cacheList, min, max, remove);
        }


        // This overload passes in a cacheList like: g.Cache.Items -> (will throw an error if does not exist)
        // Adds 1 to random max so that max parameter is included. IE: 1-3 will include 1,2,3 
        public static List<T> NextCacheItems<T> (IGenerator generator, dynamic cacheList, int min, int max, bool remove = true)
        {
            int count = new Random(generator.RowNumber).Next(min, max + 1);
            List<T> itemList = cacheList;

            var rowItems = itemList.Take(count).ToList();

            if (remove)
                rowItems.ForEach(i => itemList.Remove(i));

            return rowItems;
        }
    }
}
