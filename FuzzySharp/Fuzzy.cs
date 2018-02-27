using System;
using System.Linq;
using System.Security.Cryptography;
using FuzzySharp.Algorithms;
using FuzzySharp.Ratios;

namespace FuzzySharp
{
    public static class Fuzzy
    {
        public static int Ratio(string s1, string s2, params StringOptions[] options)
        {
            s1 = Prepare(s1, options);
            s2 = Prepare(s2, options);

            return new SimpleRatio().Score(s1, s2);
        }

        public static int PartialRatio(string s1, string s2, params StringOptions[] options)
        {
            s1 = Prepare(s1, options);
            s2 = Prepare(s2, options);

            return new PartialRatio().Score(s1, s2);
        }

        public static int TokenSortRatio(string s1, string s2, params StringOptions[] options)
        {
            s1 = Prepare(s1, options);
            s2 = Prepare(s2, options);
            return TokenSort.Score(s1, s2, new SimpleRatio());
        }
        
        public static int TokenSortPartialRatio(string s1, string s2, params StringOptions[] options)
        {
            s1 = Prepare(s1, options);
            s2 = Prepare(s2, options);
            return TokenSort.Score(s1, s2, new PartialRatio());
        }

        public static int TokenSetRatio(string s1, string s2, params StringOptions[] options)
        {
            s1 = Prepare(s1, options);
            s2 = Prepare(s2, options);
            return TokenSet.Score(s1, s2, new SimpleRatio());
        }
        
        public static int TokenSetPartialRatio(string s1, string s2, params StringOptions[] options)
        {
            s1 = Prepare(s1, options);
            s2 = Prepare(s2, options);
            return TokenSet.Score(s1, s2, new PartialRatio());
        }

        public static int WeightedRatio(string s1, string s2, params StringOptions[] options)
        {
            s1 = Prepare(s1, options);
            s2 = Prepare(s2, options);
            return new WeightedRatio().Score(s1, s2);
        }
        
        //TODO: Port over the Extract Methods

        private static string Prepare(string value, params StringOptions[] options)
        {
            if (string.IsNullOrEmpty(value)) 
                throw new ArgumentNullException();

            if (options.All(x => x != StringOptions.CaseSensitive))
                value = value.ToLower();

            if (options.All(x => x != StringOptions.PreserveWhitespace))
                value = value.Trim();
                
            return value;
        }
    }
}