using System.Text.RegularExpressions;
using FuzzySharp.Ratios;

namespace FuzzySharp.Algorithms
{
    public class TokenSort : Algoritm
    {
        public static int Score(string s1, string s2, IRatio ratio)
        {
            var sorted1 = SortAndJoin(Process(s1));
            var sorted2 = SortAndJoin(Process(s2));

            return ratio.Score(sorted1, sorted2);
        }
    }
}