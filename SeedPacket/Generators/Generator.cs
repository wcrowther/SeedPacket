using SeedPacket.Interfaces;
using System;
using System.Collections.Generic;
using System.Dynamic;
using WildHare;
using WildHare.Extensions;

namespace SeedPacket.Generators
{
    public abstract class Generator : IGenerator 
    {
        IRules rules;

        public Generator (IRules rules =  null, Random baseRandom = null)
        {
            RowNumber = SeedBegin;
            this.rules = rules ?? new Rules();
            this.baseRandom = baseRandom ?? new Random(defaultSeed);
            GetNextRowRandom();
        }

        private int defaultSeed = 123456789;
        private Random baseRandom;
        private DateTime? baseDateTime;
        private readonly DateTime defaultDateTime = DateTime.Parse("1/1/2020");
        private ExpandoObject cache = new ExpandoObject();
        protected IDataSource dataSource;

        public int SeedBegin { get; set; } = 1;

        public int SeedEnd { get; set; } = 10;

        public IDataSource Datasource
        {
            get { return dataSource; }
        }

        public dynamic Cache
        {
            get { return cache; }
            set { cache = value; }
        }

        public IRules Rules
        {
            get { return rules ?? new Rules(); }
            set { rules = value; }
        } 

        public Random BaseRandom
        {
            get { return baseRandom ?? new Random(defaultSeed); }
            set { baseRandom = value; }
        }

        public DateTime BaseDateTime
        {
            get { return baseDateTime ?? defaultDateTime; }
            set { baseDateTime = value ; }
        }

        public bool Debugging { get; set; } = true;

        public int RowCount
        {
            get { return (SeedEnd - SeedBegin) + 1; }
        }

        public int RowNumber { get; set; }

        public Random RowRandom { get; private set; }

        public int RowRandomNumber { get; private set; }

        /// <summary>Temp dictionary of values generated for current row. Only contains previously generated values <br/>
        /// for that row. Values are cleared at beginning of new row.</summary>
        public Dictionary<string, object> CurrentRowValues { get; } = new Dictionary<string, object>();

        public MetaProperty CurrentProperty { get; set; }

        public string CustomName { get; set; } = null;

        public void GetNextRowRandom()
        {
            RowRandomNumber = BaseRandom.Next();
            RowRandom = new Random(RowRandomNumber);
        }
    }
} 
