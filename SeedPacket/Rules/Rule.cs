using NewLibrary.ForString;
using NewLibrary.ForType;
using SeedPacket.Interfaces;
using System;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SeedPacket
{
    public class Rule 
    {

    #region Private Fields

        private string ruleName;
        private Type typeMatch;
        private string nameMatch;
        private Func<IGenerator, dynamic> rule; 

    #endregion

    #region Constructor

        public Rule(Type typeMatch, string nameMatch, Func<IGenerator, dynamic> rule, string ruleName, string description = "")
        {
            this.typeMatch = typeMatch;
            this.nameMatch = nameMatch.ifBlank().ToLower();
            this.rule = rule;
            this.ruleName = ruleName;
            this.Description = description;
        }

    #endregion

    #region Public

        [StringLength(30)]
        public string RuleName
        {
            get { return ruleName; }
            private set { ruleName = value; }
        }

        public string Description { get; set; }

        public bool IsMatch (Type propType, string propName)
        {
            // Try to match on Interface if typeMatch is interface
            if (typeMatch.IsInterface && propType.GetInterfaces().Any(a => a.Name == typeMatch.Name))
            {
                // Ignore IEnumerable on string
                if (propType == typeof(string) && typeMatch == typeof(IEnumerable))
                {
                    return false;
                }
                // Will except comma-separated list strings for match.
                return NameMatches(nameMatch, propName.ifBlank().ToLower());
            }

            // Must match on type, if not the same then false - no match for this rule
            if (propType.IsAssignableFrom(typeMatch))
            {
                // Will except comma-separated list strings for match.
                return NameMatches(nameMatch, propName.ifBlank().ToLower());
            }
            return false;
        }

        public dynamic ApplyRule (IGenerator generator)
        {
            return rule(generator);
        }

        public override string ToString ()
        {
            return $"{ruleName ?? "Not Named"} ({Description ?? "None"})";
        }

        #endregion

    #region Private Methods

        private static bool NameMatches(string namematch, string propname)
        {
            // If comma in string, break into individual strings and loop through each
            if (namematch.Contains(","))
            {
                var nameArray = namematch.Split(new[]{','}, StringSplitOptions.RemoveEmptyEntries);
                foreach (var name in nameArray)
                {
                    if (NameMatch(name, propname))
                    {
                        return true;
                    }
                }
                return false;
            }

            return NameMatch(namematch, propname);
        } 

        private static bool NameMatch(string namematch, string propname)
        {
            // 1. type matches but propname has not been defined for rule then true
            // 2. type matches and propname has wildcard. if wildcard matches then true otherwise false
            // 3. type matches but propName does not match one defined for this rule then false

            if (namematch.isNullOrEmpty())
            {
                return true;
            }
            else if (namematch.StartsWith("%") && namematch.EndsWith("%"))
            {
                return propname.Contains(namematch.TrimStart('%').TrimEnd('%'));
            }
            else if (namematch.StartsWith("%"))
            {
                return propname.EndsWith(namematch.TrimStart('%'));
            }
            else if (namematch.EndsWith("%"))
            {
                return propname.StartsWith(namematch.TrimEnd('%'));
            }
            else if (namematch == propname)
            {
                return true;
            }
            else
            {
                return false;
            }
        } 

    #endregion

    }
} 

 
