using System;
using System.Collections.Generic;
using System.Linq;
using BoomTown.FuzzySharp.InternalDiffUtils;

namespace BoomTown.FuzzySharp.Ratios
{
    public class PartialRatio : IRatio
    {
        public int Score(string s1, string s2)
        {
            string shorter;
            string longer;

            if (s1.Length > s2.Length)
            {
                shorter = s2;
                longer = s1;
            }
            else
            {
                shorter = s1;
                longer = s2;
            }

            var matchingBlocks = DiffUtils.GetMatchingBlocks(shorter, longer);
            var scores = new List<double>();

            foreach (var block in matchingBlocks)
            {
                var dist = block.Dpos - block.Spos;

                var longStart = dist > 0 ? dist : 0;
                var longEnd = longStart + shorter.Length;

                if (longEnd > longer.Length)
                    longEnd = longer.Length;

                var longSubstring = longer.Substring(longStart, longEnd - longStart);

                var ratio = DiffUtils.GetRatio(shorter, longSubstring);

                if (ratio > .995)
                    return 100;

                scores.Add(ratio);
            }

            return Convert.ToInt32(scores.Max() * 100);
        }
    }
}