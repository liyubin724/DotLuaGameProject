using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace DotEngine.Config.WDB
{
    public static class WDBUtility
    {

        private const string VALIDATION_RULE_PATTERN = @"(^(?<name>[A-Za-z]{1,})$|^(?<name>[A-Za-z]{1,})[\s]*\((?<params>\S*)\))";
        public static bool ParserValidationRule(string rule, out string name, out string[] values)
        {
            if(string.IsNullOrEmpty(rule))
            {
                name = null;
                values = null;
                return false;
            }

            Match nameMatch = new Regex(VALIDATION_RULE_PATTERN).Match(rule);
            Group nameGroup = nameMatch.Groups["name"];
            Group paramsGroup = nameMatch.Groups["params"];

            name = nameGroup.Success ? nameGroup.Value.Trim() : null;
            values = null;
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }
            if (paramsGroup.Success && !string.IsNullOrEmpty(paramsGroup.Value))
            {
                values = (from v in paramsGroup.Value.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                          where !string.IsNullOrEmpty(v)
                          select v.Trim()
                          ).ToArray();
            }

            return true;
        }
    }
}
