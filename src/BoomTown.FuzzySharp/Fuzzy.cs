using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using BoomTown.FuzzySharp.Algorithms;
using BoomTown.FuzzySharp.Ratios;

namespace BoomTown.FuzzySharp
{
    public static class Fuzzy
    {
        /// <summary>
        /// Calculates a Levenshtein simple ratio between the strings.
        /// This indicates a measure of similarity
        /// </summary>
        /// <param name="s1">Input String</param>
        /// <param name="s2">Input String</param>
        /// <param name="options">Optional options to handle the input strings</param>
        /// <returns>The Simple Ratio</returns>
        public static int Ratio(string s1, string s2, params StringOptions[] options)
        {
            s1 = Prepare(s1, options);
            s2 = Prepare(s2, options);

            return new SimpleRatio().Score(s1, s2);
        }

        /// <summary>
        /// Inconsistent substrings lead to problems in matching. This ratio
        /// uses a heuristic called "best partial" for when two strings 
        /// are of noticeably different lengths.
        /// </summary>
        /// <param name="s1">Input String</param>
        /// <param name="s2">Input String</param>
        /// <param name="options">Optional options to handle the input strings</param>
        /// <returns>The Partial Ratio</returns>
        public static int PartialRatio(string s1, string s2, params StringOptions[] options)
        {
            s1 = Prepare(s1, options);
            s2 = Prepare(s2, options);

            return new PartialRatio().Score(s1, s2);
        }

        /// <summary>
        /// Find all alphanumeric tokens in the string, sort those tokens,
        /// and then take ratio of resulting joined strings.
        /// </summary>
        /// <param name="s1">Input String</param>
        /// <param name="s2">Input String</param>
        /// <param name="options">Optional options to handle the input strings</param>
        /// <returns>The full ratio of the strings</returns>
        public static int TokenSortRatio(string s1, string s2, params StringOptions[] options)
        {
            s1 = Prepare(s1, options);
            s2 = Prepare(s2, options);
            return new TokenSort(new SimpleRatio()).Score(s1, s2);
        }
        
        /// <summary>
        /// Find all alphanumeric tokens in the string, sort those tokens,
        /// and then take ratio of resulting joined strings.
        /// </summary>
        /// <param name="s1">Input String</param>
        /// <param name="s2">Input String</param>
        /// <param name="options">Optional options to handle the input strings</param>
        /// <returns>The Partial ratio of the strings</returns>
        public static int TokenSortPartialRatio(string s1, string s2, params StringOptions[] options)
        {
            s1 = Prepare(s1, options);
            s2 = Prepare(s2, options);
            return new TokenSort(new PartialRatio()).Score(s1, s2);
        }

        /// <summary>
        /// Splits the strings into tokens and computes intersections and remainders between the tokens of the two strings.
        /// A comparison string is then built up and is compared using the simple ratio algorithm.
        /// Useful for strings where words appear redundantly.
        /// </summary>
        /// <param name="s1">Input String</param>
        /// <param name="s2">Input String</param>
        /// <param name="options">Optional options to handle the input strings</param>
        /// <returns>The Ratio of similarity</returns>
        public static int TokenSetRatio(string s1, string s2, params StringOptions[] options)
        {
            s1 = Prepare(s1, options);
            s2 = Prepare(s2, options);
            return new TokenSet(new SimpleRatio()).Score(s1, s2);
        }
        
        /// <summary>
        /// Splits the strings into tokens and computes intersections and remainders between the tokens of the two strings.
        /// A comparison string is then built up and is compared using the simple ratio algorithm.
        /// Useful for strings where words appear redundantly.
        /// </summary>
        /// <param name="s1">Input String</param>
        /// <param name="s2">Input String</param>
        /// <param name="options">Optional options to handle the input strings</param>
        /// <returns>The Partial Ratio of similarity</returns>
        public static int TokenSetPartialRatio(string s1, string s2, params StringOptions[] options)
        {
            s1 = Prepare(s1, options);
            s2 = Prepare(s2, options);
            return new TokenSet(new PartialRatio()).Score(s1, s2);
        }

        /// <summary>
        /// Calculates a weighted ratio between the different algorithms for best results.
        /// </summary>
        /// <param name="s1">Input String</param>
        /// <param name="s2">Input String</param>
        /// <param name="options">Optional options to handle the input strings</param>
        /// <returns>The Ratio of similarity</returns>
        public static int WeightedRatio(string s1, string s2, params StringOptions[] options)
        {
            s1 = Prepare(s1, options);
            s2 = Prepare(s2, options);
            return new WeightedRatio().Score(s1, s2);
        }

        private static string Prepare(string value, params StringOptions[] options)
        {
            var distinctOptions = new HashSet<StringOptions>(options);
            
            if (string.IsNullOrEmpty(value)) 
                throw new ArgumentNullException();

            if (distinctOptions.Contains(StringOptions.DoNotTouchMyString))
                return value;
            
            // Non ASCII Characters such as ± are removed and replaced with an empty string, this matches the 
            // Python implementation of the asciidammit(s) function
            if (!distinctOptions.Contains(StringOptions.PreserveNonAscii))
                value = Regex.Replace(value, @"[^\u0020-\u007E]", string.Empty);

            // All characters except letters, numbers, or an underscore are replaced with an empty space.  
            // Same as Python replace_non_letters_non_numbers_with_whitespace function
            if (!distinctOptions.Contains(StringOptions.PreserveNonAlphaNumeric))
                value = Regex.Replace(value, @"[^a-zA-Z0-9 _]", " ");
            
            if (!distinctOptions.Contains(StringOptions.CaseSensitive))
                value = value.ToLower();

            if (!distinctOptions.Contains(StringOptions.PreserveWhitespace))
                value = value.Trim();

            return value;
        }
    }
}