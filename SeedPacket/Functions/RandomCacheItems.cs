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
        public static List<T> RandomCacheItems<T> (IGenerator generator, string cacheListName, int min, int max, bool remove = true)
        {
            ExpandoObject cache = generator.Cache;
            var cacheList = cache.GetByItemName<List<T>>(cacheListName);

            return RandomCacheItems<T>(generator, cacheList, min, max, remove);
        }

        public static List<T> RandomCacheItems<T> (IGenerator generator, dynamic cacheList, int min, int max, bool remove = true)
        {
            // Adds 1 to random max so that max parameter is included. IE: 1-3 will include 1,2,3 
            int count = new Random(generator.RowNumber).Next(min, max + 1); 
            List<T> itemList = cacheList;

            return itemList.TakeRandomItems<T>(generator.RowRandom, count, remove).ToList();
        }
    }
}
