﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using NewLibrary.ForString;
using NewLibrary.ForObject;
using SeedPacket.Interfaces;

namespace SeedPacket.Generators
{
    public class Rules : Collection<Rule>, IRules
    {
        
    #region Methods

        public Rule GetRuleByTypeAndName(Type propertyType, string propertyName)
        {
            // Rules are processed last to first

            Rule rule = null;
            int i = this.Count - 1; // zero based
            bool matchfound = false;

            while (i >= 0 && !matchfound)
            {
                if (this[i].IsMatch(propertyType, propertyName))
                {
                    matchfound = true;
                    rule = this[i];
                }
                i--;
            }
            return rule;
        }

        public void Add(Rule rule, bool overwrite = true)
        {
            // Should a RuleName be required?
            // if (rule.RuleName.isNullOrEmpty() && rule.RuleName.Length <= 3)
                // throw new Exception("Rule RuleName must be at least 3 characters in length.");

            if (this.Any(a => a.RuleName == rule.RuleName))
            {
                if (overwrite)
                {
                    RemoveRuleByName(rule.RuleName);
                }
                else
                {
                    throw new Exception("Rule RuleName already exists.");
                }
            }
            base.Add(rule);
        }

        public void AddRange(IEnumerable<Rule> rules, bool overwrite = true)
        {
            foreach (var rule in rules)
            {
                this.Add(rule, overwrite);
            }
        }

        public void RemoveRuleByName(string ruleName)
        {
            var rule = this.Where(a => a.RuleName == ruleName).FirstOrDefault();
            if (rule != null)
            {
                base.Remove(rule);
            }
        }

    #endregion
    }
} 