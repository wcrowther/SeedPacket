using SeedPacket.Interfaces;
using System;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using WildHare.Extensions;

namespace SeedPacket
{
    public class Rule(Type typeMatch, string nameMatch, Func<IGenerator, dynamic> func, string ruleName, string description = "")
	{
		private readonly Type typeMatch = typeMatch;
        private readonly string nameMatch = nameMatch.IfNullOrEmpty().ToLower();
        private readonly Func<IGenerator, dynamic> func = func;

		[StringLength(30)]
		public string RuleName { get; } = ruleName;

		public string Description { get; } = description;

		public Type TypeMatch { get; } = typeMatch;

		public string NameMatch { get; } = nameMatch;

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

		public dynamic ApplyRule(IGenerator generator) => func(generator);

		public override string ToString() => $"{RuleName ?? "Not Named"} ({Description ?? "None"})";

		// ===================================================================
		// Private Methods
		// ===================================================================

		private static bool NameMatches(string namematch, string propname)
        {
            // If comma in string, break into individual strings and loop through each
            if (namematch.Contains(','))
            {
                var nameArray = namematch.Split([','], StringSplitOptions.RemoveEmptyEntries);
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


		// 1. type matches but namematch has not been defined for Rule then true
		// 2. type matches and namematch has wildcard. if wildcard matches then true otherwise false
		// 3. type matches but namematch does not match the one defined for this Rule then false

		private static bool IsNameMatch(string namematch, string propname) => namematch switch
		{
			null or "" => true,
			var s when s.StartsWith('%') && s.EndsWith('%') => propname.Contains(s.Trim('%')),
			var s when s.StartsWith('%') => propname.EndsWith(s.TrimStart('%')),
			var s when s.EndsWith('%')	 => propname.StartsWith(s.TrimEnd('%')),
			var s when s == propname     => true,
			_ => false
		};

	}
} 

 
