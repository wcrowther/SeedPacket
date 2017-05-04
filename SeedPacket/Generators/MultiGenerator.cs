using SeedPacket.DataSources;
using SeedPacket.Interfaces;
using System;

namespace SeedPacket.Generators
{
    public class MultiGenerator : Generator, IGenerator 
    {

        public MultiGenerator(  string sourceFilepath = null,
                                string sourceString = null, 
                                DataInputType dataInputType = DataInputType.Auto,
                                RulesSet rulesSet = RulesSet.Common
                             ) : base()
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

        public virtual void GetRules (RulesSet ruleSet)
        {
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
                    Rules.AddCommonRules();
                    break;
                default:
                    throw new NotImplementedException("For use in a derived custom Generator");
            }
        }
    }
} 

 
