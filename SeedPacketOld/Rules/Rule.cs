using SeedPacket.Interfaces;
using System;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using WildHare.Extensions;

namespace SeedPacket
{
    public class Rule 
    {
        public Rule(Type typeMatch, string nameMatch, Func<IGenerator, dynamic> func, string ruleName, string description = "")
        {
            this.typeMatch = typeMatch;
            this.nameMatch = nameMatch.IfNullOrEmpty().ToLower();
            this.func = func;

            RuleName = ruleName;
            Description = description;
            TypeMatch = typeMatch;
            NameMatch = nameMatch;
        }

        private Type typeMatch;
        private string nameMatch;
        private readonly Func<IGenerator, dynamic> func;


        [StringLength(30)]
        public string RuleName { get; }

        public string Description { get; }

        public Type TypeMatch { get; }

        public string NameMatch { get; }

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
                return NameMatches(nameMatch, propName.IfNullOrEmpty().ToLower());
            }

            // Must match on type, if not the same then false - no match for this Funcs
            if (propType.IsAssignableFrom(typeMatch))
            {
                // Will except comma-separated list strings for match.
                return NameMatches(nameMatch, propName.IfNullOrEmpty().ToLower());
            }
            return false;
        }

        public dynamic ApplyRule (IGenerator generator)
        {
            return func(generator);
        }

        public override string ToString ()
        {
            return $"{RuleName ?? "Not Named"} ({Description ?? "None"})";
        }

        // ===================================================================
        // Private Methods
        // ===================================================================

        private static bool NameMatches(string namematch, string propname)
        {
            // If comma in string, break into individual strings and loop through each
            if (namematch.Contains(","))
            {
                var nameArray = namematch.Split(new[]{','}, StringSplitOptions.RemoveEmptyEntries);
                foreach (var name in nameArray)
                {
                    if (IsNameMatch(name, propname))
                    {
                        return true;
                    }
                }
                return false;
            }

            return IsNameMatch(namematch, propname);
        } 

        private static bool IsNameMatch(string namematch, string propname)
        {
            // 1. type matches but namematch has not been defined for Rule then true
            // 2. type matches and namematch has wildcard. if wildcard matches then true otherwise false
            // 3. type matches but namematch does not match the one defined for this Rule then false

            if (namematch.IsNullOrEmpty())
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

    }
} 

 
