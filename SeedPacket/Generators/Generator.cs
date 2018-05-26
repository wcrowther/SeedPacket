using System;
using System.Collections.Generic;
using System.Globalization;
using NewLibrary.ForType;
using SeedPacket.Interfaces;
using System.Dynamic;
using SeedPacket.Exceptions;

namespace SeedPacket.Generators
{
    public abstract class Generator : IGenerator 
    {
        IRules rules;

        public Generator (IRules rules =  null, Random baseRandom = null)
        {
            RowNumber = seedBegin;
            this.rules = rules ?? new Rules();
            this.baseRandom = baseRandom ?? new Random(defaultSeed);
            GetNextRowRandom();
        }

        #region Private Fields

        private int defaultSeed = 123456789;
        private int seedBegin = 1;
        private int seedEnd = 10;
        private Random baseRandom;
        private Random rowRandom;
        private int rowRandomNumber;
        private DateTime? baseDateTime;
        private Dictionary<string, object> currentRowValues = new Dictionary<string, object>();
        private DateTime defaultDateTime = DateTime.Parse("1/1/2020");
        private bool debugging = true; // shows in console
        private ExpandoObject cache = new ExpandoObject();
        private string currentPropertyName = null;
        protected IDataSource dataSource;

        #endregion

        public int SeedBegin
        {
            get { return seedBegin; }
            set { seedBegin = value; }
        }

        public int SeedEnd
        {
            get { return seedEnd; }
            set { seedEnd = value; }
        }

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

        public bool Debugging
        {
            get { return debugging; }
            set { debugging = value; }
        }

        public int RowCount
        {
            get { return (SeedEnd - SeedBegin) + 1; }
        }

        public int RowNumber { get; set; }

        public Random RowRandom
        {
            get { return rowRandom; }
        }

        public int RowRandomNumber
        {
            get { return rowRandomNumber; }
        }

        // Temp dictionary of values in current seed
        public Dictionary<string, object> CurrentRowValues
        {
            get { return currentRowValues; }
        }

        public MetaProperty CurrentProperty { get; set; }

        public string CustomName {
            get { return currentPropertyName; }
            set { currentPropertyName = value; }
        }

        public void GetNextRowRandom()
        {
            rowRandomNumber = BaseRandom.Next();
            rowRandom = new Random(rowRandomNumber);
        }
    }
} 
