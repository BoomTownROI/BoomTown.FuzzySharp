using System;
using System.Collections.Generic;
using BoomTown.FuzzySharp.InternalDiffUtils.Models;

// ReSharper disable InconsistentNaming

namespace BoomTown.FuzzySharp.InternalDiffUtils
{
    
/**
 * This is a port of the Java based fuzzywuzzy implementation. https://github.com/xdrop/fuzzywuzzy
 * That guy did all the real work as Java and c# are stupidly similar.
 * Anyway, his comment below is still relevant here.
 * 
 * This is a port of all the functions needed from python-levenshtein C implementation.
 * The code was ported line by line but unfortunately it was mostly undocumented,
 * so it is mostly non readable (eg. var names)
 *
 */
    internal static class DiffUtils
    {
        internal static double GetRatio(string s1, string s2)
        {
            var totalLength = s1.Length + s2.Length;

            if (totalLength == 0)
                return 0;
            
            var levDistance = GetLevDistance(s1, s2);
            var ratio = (totalLength - levDistance) / (double) totalLength;

            return ratio;
        }
        
        internal static IEnumerable<MatchingBlock> GetMatchingBlocks(string s1, string s2)
        {
            return GetMatchingBlocks(s1.Length, s2.Length, GetOperations(s1, s2));
        }

        private static int GetLevDistance(string s1, string s2)
        {
            int i;

            var c1 = s1.ToCharArray();
            var c2 = s2.ToCharArray();

            var str1 = 0;
            var str2 = 0;

            var len1 = s1.Length;
            var len2 = s2.Length;

            /* strip common prefix */
            while (len1 > 0 && len2 > 0 && c1[str1] == c2[str2])
            {

                len1--;
                len2--;
                str1++;
                str2++;

            }

            /* strip common suffix */
            while (len1 > 0 && len2 > 0 && c1[str1 + len1 - 1] == c2[str2 + len2 - 1])
            {
                len1--;
                len2--;
            }

            /* catch trivial cases */
            if (len1 == 0)
                return len2;
            if (len2 == 0)
                return len1;

            /* make the inner cycle (i.e. str2) the longer one */
            if (len1 > len2)
            {

                var nx = len1;
                var temp = str1;

                len1 = len2;
                len2 = nx;

                str1 = str2;
                str2 = temp;

                var t = c2;
                c2 = c1;
                c1 = t;

            }

            /* check len1 == 1 separately */
            if (len1 == 1)
            {
                return len2 + 1 - 2 * Memchr(c2, str2, c1[str1], len2);
            }

            len1++;
            len2++;

            var row = new int[len2];
            var end = len2 - 1;

            for (i = 0; i < len2; i++)
                row[i] = i;


            /* go through the matrix and compute the costs.  yes, this is an extremely
             * obfuscated version, but also extremely memory-conservative and relatively
             * fast.  */
            for (i = 1; i < len1; i++)
            {
                var p = 1;

                var ch1 = c1[str1 + i - 1];
                var c2p = str2;

                var D = i;
                var x = i;

                while (p <= end)
                {

                    if (ch1 == c2[c2p++])
                    {
                        x = --D;
                    }
                    else
                    {
                        x++;
                    }

                    D = row[p];
                    D++;

                    if (x > D)
                        x = D;
                    row[p++] = x;

                }
            }

            i = row[end];

            return i;
        }

        private static int Memchr(char[] haystack, int offset, char needle, int num)
        {

            if (num != 0)
            {
                var p = 0;

                do
                {

                    if (haystack[offset + p] == needle)
                        return 1;

                    p++;

                } while (--num != 0);

            }

            return 0;
        }

        private static IEnumerable<MatchingBlock> GetMatchingBlocks(int len1, int len2, EditOperations[] ops)
        {
            var n = ops.Length;

            int i;

            var numberOfMatchingBlocks = 0;

            var o = 0;

            var spos = 0;
            var dpos = 0;

            EditType type;

            for (i = n; i != 0;)
            {


                while (ops[o].Type == EditType.Keep && --i != 0)
                {
                    o++;
                }

                if (i == 0)
                    break;

                if (spos < ops[o].Spos || dpos < ops[o].Dpos)
                {

                    numberOfMatchingBlocks++;
                    spos = ops[o].Spos;
                    dpos = ops[o].Dpos;

                }

                type = ops[o].Type;

                switch (type)
                {
                    case EditType.Replace:
                        do
                        {
                            spos++;
                            dpos++;
                            i--;
                            o++;
                        } while (i != 0 && ops[o].Type == type &&
                                 spos == ops[o].Spos && dpos == ops[o].Dpos);

                        break;

                    case EditType.Delete:
                        do
                        {
                            spos++;
                            i--;
                            o++;
                        } while (i != 0 && ops[o].Type == type &&
                                 spos == ops[o].Spos && dpos == ops[o].Dpos);

                        break;

                    case EditType.Insert:
                        do
                        {
                            dpos++;
                            i--;
                            o++;
                        } while (i != 0 && ops[o].Type == type &&
                                 spos == ops[o].Spos && dpos == ops[o].Dpos);

                        break;
                }
            }

            if (spos < len1 || dpos < len2)
            {
                numberOfMatchingBlocks++;
            }

            var matchingBlocks = new MatchingBlock[numberOfMatchingBlocks + 1];

            o = 0;
            spos = dpos = 0;
            var mbIndex = 0;


            for (i = n; i != 0;)
            {

                while (ops[o].Type == EditType.Keep && --i != 0)
                    o++;

                if (i == 0)
                    break;

                if (spos < ops[o].Spos || dpos < ops[o].Dpos)
                {
                    var mb = new MatchingBlock
                    {
                        Spos = spos,
                        Dpos = dpos,
                        Length = ops[o].Spos - spos
                    };

                    spos = ops[o].Spos;
                    dpos = ops[o].Dpos;

                    matchingBlocks[mbIndex++] = mb;

                }

                type = ops[o].Type;

                switch (type)
                {
                    case EditType.Replace:
                        do
                        {
                            spos++;
                            dpos++;
                            i--;
                            o++;
                        } while (i != 0 && ops[o].Type == type &&
                                 spos == ops[o].Spos && dpos == ops[o].Dpos);

                        break;

                    case EditType.Delete:
                        do
                        {
                            spos++;
                            i--;
                            o++;
                        } while (i != 0 && ops[o].Type == type &&
                                 spos == ops[o].Spos && dpos == ops[o].Dpos);

                        break;

                    case EditType.Insert:
                        do
                        {
                            dpos++;
                            i--;
                            o++;
                        } while (i != 0 && ops[o].Type == type &&
                                 spos == ops[o].Spos && dpos == ops[o].Dpos);

                        break;
                }
            }

            if (spos < len1 || dpos < len2)
            {
                if (len1 - spos != len2 - dpos)
                    throw new ArgumentException(@"¯\_(ツ)_/¯");

                var mb = new MatchingBlock
                {
                    Spos = spos,
                    Dpos = dpos,
                    Length = len1 - spos
                };

                matchingBlocks[mbIndex++] = mb;
            }

            if (numberOfMatchingBlocks != mbIndex) 
                throw new ArgumentException(@"¯\_(ツ)_/¯");

            var finalBlock = new MatchingBlock
            {
                Spos = len1,
                Dpos = len2,
                Length = 0
            };

            matchingBlocks[mbIndex] = finalBlock;

            return matchingBlocks;
        }

        private static EditOperations[] GetOperations(string s1, string s2)
        {
            var len1 = s1.Length;
            var len2 = s2.Length;

            int i;

            var c1 = s1.ToCharArray();
            var c2 = s2.ToCharArray();

            var p1 = 0;
            var p2 = 0;

            var len1O = 0;

            while (len1 > 0 && len2 > 0 && c1[p1] == c2[p2])
            {
                len1--;
                len2--;

                p1++;
                p2++;

                len1O++;
            }

            var len2O = len1O;

            /* strip common suffix */
            while (len1 > 0 && len2 > 0 && c1[p1 + len1 - 1] == c2[p2 + len2 - 1])
            {
                len1--;
                len2--;
            }

            len1++;
            len2++;

            var matrix = new int[len2 * len1];

            for (i = 0; i < len2; i++)
                matrix[i] = i;
            for (i = 1; i < len1; i++)
                matrix[len2 * i] = i;

            for (i = 1; i < len1; i++)
            {

                var ptrPrev = (i - 1) * len2;
                var ptrC = i * len2;
                var ptrEnd = ptrC + len2 - 1;

                var char1 = c1[p1 + i - 1];
                var ptrChar2 = p2;

                var x = i;

                ptrC++;

                while (ptrC <= ptrEnd)
                {

                    var c3 = matrix[ptrPrev++] + (char1 != c2[ptrChar2++] ? 1 : 0);
                    x++;

                    if (x > c3)
                    {
                        x = c3;
                    }

                    c3 = matrix[ptrPrev] + 1;

                    if (x > c3)
                    {
                        x = c3;
                    }

                    matrix[ptrC++] = x;
                }
            }

            return EditOpsFromCostMatrix(len1, c1, p1, len1O, len2, c2, p2, len2O, matrix);
        }


        private static EditOperations[] EditOpsFromCostMatrix(int len1, char[] c1, int p1, int o1,
            int len2, char[] c2, int p2, int o2, int[] matrix)
        {
            var dir = 0;

            var pos = matrix[len1 * len2 - 1];

            var ops = new EditOperations[pos];

            var i = len1 - 1;
            var j = len2 - 1;

            var ptr = len1 * len2 - 1;

            while (i > 0 || j > 0)
            {

                if (dir < 0 && j != 0 && matrix[ptr] == matrix[ptr - 1] + 1)
                {

                    var eop = new EditOperations();

                    pos--;
                    ops[pos] = eop;
                    eop.Type = EditType.Insert;
                    eop.Spos = i + o1;
                    eop.Dpos = --j + o2;
                    ptr--;

                    continue;
                }

                if (dir > 0 && i != 0 && matrix[ptr] == matrix[ptr - len2] + 1)
                {

                    var eop = new EditOperations();

                    pos--;
                    ops[pos] = eop;
                    eop.Type = EditType.Delete;
                    eop.Spos = --i + o1;
                    eop.Dpos = j + o2;
                    ptr -= len2;

                    continue;
                }

                if (i != 0 && j != 0 && matrix[ptr] == matrix[ptr - len2 - 1]
                    && c1[p1 + i - 1] == c2[p2 + j - 1])
                {

                    i--;
                    j--;
                    ptr -= len2 + 1;
                    dir = 0;

                    continue;
                }

                if (i != 0 && j != 0 && matrix[ptr] == matrix[ptr - len2 - 1] + 1)
                {
                    pos--;

                    var eop = new EditOperations();
                    ops[pos] = eop;

                    eop.Type = EditType.Replace;
                    eop.Spos = --i + o1;
                    eop.Dpos = --j + o2;

                    ptr -= len2 + 1;
                    dir = 0;
                    continue;

                }

                if (dir == 0 && j != 0 && matrix[ptr] == matrix[ptr - 1] + 1)
                {
                    pos--;
                    var eop = new EditOperations();
                    ops[pos] = eop;
                    eop.Type = EditType.Insert;
                    eop.Spos = i + o1;
                    eop.Dpos = --j + o2;
                    ptr--;
                    dir = -1;

                    continue;
                }

                if (dir == 0 && i != 0 && matrix[ptr] == matrix[ptr - len2] + 1)
                {
                    pos--;
                    var eop = new EditOperations();
                    ops[pos] = eop;

                    eop.Type = EditType.Delete;
                    eop.Spos = --i + o1;
                    eop.Dpos = j + o2;
                    ptr -= len2;
                    dir = 1;
                    continue;
                }

                throw new ArgumentException(@"¯\_(ツ)_/¯");
            }

            return ops;
        }
    }
}