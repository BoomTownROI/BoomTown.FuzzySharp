using BoomTown.FuzzySharp.Ratios;

namespace BoomTown.FuzzySharp.Algorithms
{
    public class TokenSort : Algoritm
    {
        private readonly IRatio _ratio;

        /// <summary>
        /// Create a new TokenSort Ratio
        /// </summary>
        /// <param name="ratio">The Ratio to use when calculating scores</param>
        public TokenSort(IRatio ratio)
        {
            _ratio = ratio;
        }
        
        public override int Score(string s1, string s2)
        {
            var sorted1 = SortAndJoin(Process(s1));
            var sorted2 = SortAndJoin(Process(s2));

            return _ratio.Score(sorted1, sorted2);
        }
    }
}
