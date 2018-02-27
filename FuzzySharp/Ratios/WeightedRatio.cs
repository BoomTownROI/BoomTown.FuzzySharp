using System;
using System.Linq;
using FuzzySharp.Algorithms;

namespace FuzzySharp.Ratios
{
    public class WeightedRatio : IRatio
    {            
        public int Score(string s1, string s2)
        {
            const double unbaseScale = .95;
            var tryPartials = true;
            var partialScale = .90;
            
            var len1 = s1.Length;
            var len2 = s2.Length;

            if (len1 == 0 || len2 == 0) { return 0; }
            
            var baseValue = new SimpleRatio().Score(s1, s2);
            var lenRatio = ((double) Math.Max(len1, len2)) / Math.Min(len1, len2);

            // if strings are similar length don't use partials
            if (lenRatio < 1.5) 
                tryPartials = false;

            // if one string is much shorter than the other
            if (lenRatio > 8) 
                partialScale = .6;

            if (tryPartials)
            {

                var partial = new PartialRatio().Score(s1, s2) * partialScale;
                var partialSort =  TokenSort.Score(s1, s2, new PartialRatio()) * unbaseScale * partialScale;
                var partialSet = TokenSet.Score(s1, s2, new PartialRatio()) * unbaseScale * partialScale;

                return Convert.ToInt32(new[] {baseValue, partial, partialSort, partialSet}.Max());
            }

            var tokenSort = TokenSort.Score(s1, s2, new SimpleRatio()) * unbaseScale;
            var tokenSet = TokenSet.Score(s1, s2, new SimpleRatio()) * unbaseScale;

            return Convert.ToInt32(new[] {baseValue, tokenSort, tokenSet}.Max());
        }
    }
}