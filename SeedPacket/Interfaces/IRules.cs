using System;
using System.Collections.Generic;
using SeedPacket.Generators;

namespace SeedPacket.Interfaces
{
    public interface IRules 
    {
        Rule GetRuleByTypeAndName(Type ruleType, string propertyName);
        void Add (Rule rule);
        void AddRange (IEnumerable<Rule> rules, bool overwrite = false);
        void RemoveRuleByName (string ruleName);
        void Clear();
    }
} 

 
