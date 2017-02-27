using System;
using System.Collections.Generic;
using System.Globalization;
using NewLibrary.ForType;
using SeedPacket.Interfaces;

namespace SeedPacket.Generators
{
    public abstract class Generator : IGenerator 
    {

        public Generator (  DateTime? BaseDateTime = null,
                            Random BaseRandom = null )
        {
            baseRandom = BaseRandom ?? new Random(defaultSeed);
            baseDateTime = BaseDateTime;
            GetNextRowRandom();
        }

   #region Private Fields

        private Random baseRandom;
        private Random rowRandom;
        private int rowRandomNumber;
        private DateTime? baseDateTime;
        private Dictionary<string, object> currentRowValues = new Dictionary<string, object>();
        private int defaultSeed = 1234;
        private DateTime defaultDateTime = DateTime.Parse("1/1/2018");
        private bool debugging; // True to show Debug messages

        #endregion

        public Rules Rules { get; set; } = new Rules();

        public DateTime BaseDateTime
        {
            get { return baseDateTime ?? defaultDateTime; }
            set { baseDateTime = value; }
        }

        public bool Debugging
        {
            get { return debugging; }
            set { debugging = value; }
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
            rowRandomNumber = baseRandom.Next();
            rowRandom = new Random(rowRandomNumber);
        }
    }
} 
