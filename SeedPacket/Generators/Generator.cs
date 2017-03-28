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

        public Generator ()
        {
            baseRandom = new Random(defaultSeed);
            GetNextRowRandom();
        }

        #region Private Fields

        private int defaultSeed = 12345;
        private int seedBegin = 1;
        private int seedEnd = 10;
        private Random baseRandom;
        private Random rowRandom;
        private int rowRandomNumber;
        private DateTime? baseDateTime;
        private Dictionary<string, object> currentRowValues = new Dictionary<string, object>();
        private DateTime defaultDateTime = DateTime.Parse("1/1/2020");
        private bool debugging; // True to show Debug messages
        private ExpandoObject cache = new ExpandoObject();
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

        public Rules Rules { get; set; } = new Rules();

        public Random BaseRandom
        {
            get { return baseRandom;  }
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

        public void GetNextRowRandom ()
        {
            rowRandomNumber = BaseRandom.Next();
            rowRandom = new Random(rowRandomNumber);
        }
    }
} 
