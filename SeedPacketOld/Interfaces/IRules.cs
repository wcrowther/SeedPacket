using System;
using System.Collections.Generic;
using SeedPacket.Generators;
using System.Collections;

namespace SeedPacket.Interfaces
{
    public interface IRules : ICollection
    {
        Rule GetRuleByTypeAndName(Type ruleType, string propertyName);
        void Add (Rule rule);
        void AddRange (IEnumerable<Rule> rules, bool overwrite = false);
        void RemoveRuleByName (string ruleName);
        void Clear();
    }
} 

 
