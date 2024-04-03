using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SpreadSheetEngine
{
    public class parse
    {
        string pattern = @"([a-zA-Z]+(\d[a-zA-Z]*)*|\d+\.\d+|\d+|[+-/*()])";
        string input = "x1y2 + abc123 * 3.14";
        public parse(string input2)
        {
            input = input2;
        }

        public string[] parsing()
        {
            var matches = Regex.Matches(input, pattern);
            string[] tokens = matches.Cast<Match>().Select(m => m.Value).ToArray();
            return tokens;
        }
        

        
    }
}
