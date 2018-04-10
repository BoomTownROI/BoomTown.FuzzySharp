using System.Collections.Generic;
using System.Linq;
using BoomTown.FuzzySharp.Models;
using BoomTown.FuzzySharp.Ratios;

namespace BoomTown.FuzzySharp
{
    /// <summary>
    /// Extract top results from a list of strings
    /// </summary>
    public class Extractor
    {
        private readonly int _cutoff;

        public Extractor()
        {
            _cutoff = 0;
        }

        /// <summary>
        /// Create an Extractor that will remove all results with a score below a cutoff
        /// </summary>
        /// <param name="cutoff">The minimum score to return</param>
        public Extractor(int cutoff)
        {
            _cutoff = cutoff;
        }
        
        /// <summary>
        /// Returns the list of choices with their associated scores of similarity in a list
        /// </summary>
        /// <param name="query">The query string</param>
        /// <param name="choices">The list of choices</param>
        /// <param name="ratio">Optional - The comparison ratio to use</param>
        /// <returns>An IEnumerable to Results with a Score and Value</returns>
        public IEnumerable<ExtractedResult> ExtractWithoutOrder(string query, IEnumerable<string> choices, IRatio ratio = null)
        {
            ratio = ValidateRatio(ratio);
            
            var results = new List<ExtractedResult>();
            
            foreach (var choice in choices)
            {
                var score = ratio.Score(query, choice);

                if (score >= _cutoff)
                {
                    results.Add(new ExtractedResult{ Score = score, Value = choice});
                }
            }

            return results;
        }

        /// <summary>
        /// Find the single best match in a list of choices
        /// </summary>
        /// <param name="query">The query string</param>
        /// <param name="choices">The list of choices</param>
        /// <param name="ratio">Optional - The comparison ratio to use</param>
        /// <returns>An <see cref="ExtractedResult"/></returns>
        public ExtractedResult ExtractBest(string query, IEnumerable<string> choices, IRatio ratio = null)
        {
            ratio = ValidateRatio(ratio);

            var extracted = ExtractWithoutOrder(query, choices, ratio);

            return extracted.Aggregate((x, y) => x.Score >= y.Score ? x : y);
        }

        /// <summary>
        /// Gets an ordered list of the highest scored number of results.
        /// </summary>
        /// <param name="query">The query string</param>
        /// <param name="choices">The list of choices</param>
        /// <param name="limit">The max number of <see cref="ExtractedResult"/> to return</param>
        /// <param name="ratio">Optional - The comparison ratio to use</param>
        /// <returns>An ordered list of <see cref="ExtractedResult"/></returns>
        public IEnumerable<ExtractedResult> ExtractTop(string query, IEnumerable<string> choices, 
            int limit = int.MaxValue, IRatio ratio = null)
        {
            ratio = ValidateRatio(ratio);

            var extracted = ExtractWithoutOrder(query, choices, ratio);

            return extracted.OrderByDescending(x => x.Score).Take(limit);
        }

        private static IRatio ValidateRatio(IRatio ratio)
        {
            return ratio ?? new WeightedRatio();
        }
    }
}