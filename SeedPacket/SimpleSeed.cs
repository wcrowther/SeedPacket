using SeedPacket.DataSources;
using SeedPacket.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SeedPacket
{
    public class SimpleSeed
    {
        private readonly IDataSource dataSource;

        // Default Constructor
        public SimpleSeed(string sourcefilepath = null, string sourcestring = null, DataInputType datainputtype = DataInputType.Auto)
        {
            dataSource = new MultiDataSource(sourcefilepath, sourcestring, datainputtype);
        }

        // Alternative and unit testing constructor
        public SimpleSeed(IDataSource datasource)
        {
            dataSource = datasource;
        }

        /// <summary>Gets the next named element looping back to the start if beyond last element</summary>
        public string Next(string identifier, int number, string ifNull = null)
        {
            List<string> strings = dataSource.GetElementList(identifier);
            int count = strings.Count;
            if (count == 0)
            {
                return ifNull ?? Default;
            }

            int mod = (number - 1) % count;
            int position = mod;

            return strings?.ElementAtOrDefault(position) ?? ifNull ?? Default;
        }

        /// <summary>Gets a 'random' element based on seeded random so will always be same sequence. Pass in unseeded random for different elements each time.</summary>
        public string Randomize(string identifier, string ifNull = null)
        {
            List<string> strings = dataSource.GetElementList(identifier);
            int index = DefaultRandom.Next(strings.Count);

            return strings?.ElementAtOrDefault(index) ?? ifNull ?? Default;
        }

        public string Default { get; set; } = "";

        public Random DefaultRandom { get; set; } = new Random(123456789);

    }
}
