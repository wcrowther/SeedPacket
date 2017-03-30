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
        public static List<T> NextCacheItems<T> (IGenerator generator, string cacheListName, int min, int max, bool remove = true)
        {
            ExpandoObject cache = generator.Cache;
            var cacheList = cache.GetByItemName<List<T>>(cacheListName);

            if (cacheList == null)
                return null;

            return NextCacheItems<T>(generator, cacheList, min, max, remove);
        }

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
