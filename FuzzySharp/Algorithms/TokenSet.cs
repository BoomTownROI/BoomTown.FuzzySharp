using System.Collections.Generic;
using System.Linq;
using FuzzySharp.Ratios;

namespace FuzzySharp.Algorithms
{
    public class TokenSet : Algoritm
    {
        public static int Score(string s1, string s2, IRatio ratio)
        {
            var tokens1 = new HashSet<string>(Process(s1));
            var tokens2 = new HashSet<string>(Process(s2));

            var intersection = new HashSet<string>(tokens1.Intersect(tokens2));
            var diff1To2 = new HashSet<string>(tokens1.Except(tokens2));
            var diff2To1 = new HashSet<string>(tokens2.Except(tokens1));

            var sortedInter = SortAndJoin(intersection);
            var sorted1To2 = (sortedInter + " " + SortAndJoin(diff1To2)).Trim();
            var sorted2To1 = (sortedInter + " " + SortAndJoin(diff2To1)).Trim();

            var results = new List<int>
            {
                ratio.Score(sortedInter, sorted1To2),
                ratio.Score(sortedInter, sorted2To1),
                ratio.Score(sorted1To2, sorted2To1)
            };

            return results.Max();
        }
    }
}