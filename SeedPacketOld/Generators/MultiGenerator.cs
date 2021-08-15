using SeedPacket.DataSources;
using SeedPacket.Interfaces;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Threading;

namespace SeedPacket.Generators
{
    public class MultiGenerator : Generator, IGenerator 
    {
        readonly CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;

        public MultiGenerator(  string sourceFilepath = null,
                                string sourceString = null, 
                                DataInputType dataInputType = DataInputType.Auto,
                                RulesSet rulesSet = RulesSet.Common,
                                Random baseRandom = null,
                                DateTime? baseDateTime =  null
                             ) : base(baseRandom: baseRandom, baseDateTime: baseDateTime)
        {
            dataSource = new MultiDataSource(sourceFilepath, sourceString, dataInputType);
            GetRules(rulesSet);
        }

        public MultiGenerator ( IDataSource datasource,
                                RulesSet rulesSet = RulesSet.Common
                              ) : base()
        {
            dataSource = datasource;
            GetRules(rulesSet);
        } 

        protected virtual void GetRules (RulesSet ruleSet)
        {
            Debug.WriteLine($"CurrentCulture: {currentCulture.Name}");

            switch (ruleSet)
            {
                case RulesSet.None:
                    // No rules loaded. Add rules manually
                    break;
                case RulesSet.Basic:
                    Rules.AddBasicRules();
                    break;
                case RulesSet.Common:
                    Rules.AddBasicRules();
                    AddCommonRulesByCulture(Rules);
                    break;
                default:
                    throw new NotImplementedException("For use in a derived custom Generator");
            }
        }

        private void AddCommonRulesByCulture(IRules rules)
        {

            switch (currentCulture.Name)
            {
                case "en-US": Rules.AddCommonRules(); break;

                // case "en-GB": Rules.AddCommonRules(); break;
                // Potentially other languages

                default: Rules.AddCommonRules(); break;
            }
        }
    }
} 

 
