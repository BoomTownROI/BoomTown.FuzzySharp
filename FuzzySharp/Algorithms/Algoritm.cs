using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace FuzzySharp.Algorithms
{
    public abstract class Algoritm
    {
        internal static string SortAndJoin(IEnumerable<string> words)
        {
            var joined = string.Join(" ", words.OrderBy(x => x).AsEnumerable());
            
            return joined.Trim();
        }

        internal static IEnumerable<string> Process(string value)
        {
            return Regex.Split(value, "\\s+");
        }
    }
}