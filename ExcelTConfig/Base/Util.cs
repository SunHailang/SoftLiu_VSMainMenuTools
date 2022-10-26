using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace ExcelTConfig.Base
{
    public static class Util
    {
        public static readonly DateTime unixTimeStampStartDateTime = new DateTime(1970, 1, 1);
        public static readonly DateTime unixTimeStampStartLocalDateTime = TimeZone.CurrentTimeZone.ToLocalTime(unixTimeStampStartDateTime);

        public static int DateTime2TimeStamp(DateTime dateTime)
        {
            return (int)(dateTime - unixTimeStampStartLocalDateTime).TotalSeconds;
        }

        public static DateTime TimeStamp2DateTime(int timeStamp)
        {
            return unixTimeStampStartLocalDateTime.AddSeconds(timeStamp);
        }

        public static string CalculateRelativePath(string from, string to)
        {
            var fromSit = Path.GetFullPath(from).Replace('\\', '/').Replace("//", "/").Split('/');
            var toSit = Path.GetFullPath(to).Replace('\\', '/').Replace("//", "/").Split('/');

            int index = 0;
            for (; index < fromSit.Length && index < toSit.Length; index++)
            {
                if (fromSit[index] != toSit[index]) break;
            }

            var sb = new StringBuilder();
            for (int it = index + 1; it < fromSit.Length; it++) sb.Append("../");
            for (int it = index; it < toSit.Length; it++)
            {
                sb.Append(toSit[it]);
                if (it != toSit.Length - 1) sb.Append("/");
            }

            return sb.ToString();
        }

        public static unsafe int AscIIHash(this string source)
        {
            int h = 0;
            fixed (char* c = source)
            {
                byte* p = (byte*)c;
                int len = source.Length * 2;
                for (int i = 0; i < len; i += 2)
                {
                    h = 31 * h + p[i];
                }
            }
            return h;
        }

        public static bool IsValidName(string s)
        {
            if (string.IsNullOrEmpty(s)) return false;

            foreach (var c in s) if (!char.IsLetterOrDigit(c) && c != '_') return false;

            return !char.IsDigit(s[0]);
        }

        public static string FirstCharToUpper(string s)
        {
            return char.ToUpper(s[0]) + s.Substring(1);
        }

        private static StringBuilder sb;
        public static string GetToolTipsName(string key)
        {
            if (string.IsNullOrEmpty(key)) return key;

            if (sb == null) sb = new StringBuilder();
            else sb.Clear();

            var preChar = '_';

            bool hasContent = false;

            foreach (var c in key)
            {
                if (char.IsLower(c))
                {
                    if (!hasContent)
                    {
                        hasContent = true;
                        sb.Append(char.ToUpper(c));
                    }
                    else
                    {
                        if (char.IsLetter(preChar)) sb.Append(c);
                        else
                        {
                            sb.Append(' ');
                            sb.Append(char.ToUpper(c));
                        }
                    }
                }
                else if (char.IsUpper(c))
                {
                    if (!hasContent)
                    {
                        hasContent = true;
                        sb.Append(c);
                    }
                    else
                    {
                        if (!char.IsUpper(preChar)) sb.Append(' ');
                        sb.Append(c);
                    }
                }
                else if (char.IsDigit(c))
                {
                    hasContent = true;
                    sb.Append(c);
                }

                preChar = c;
            }

            return sb.ToString();
        }

        public struct HashEntry
        {
            public int key;
            public int value;
            public int next;
        }

        public struct TableItem
        {
            public int hash;
            public int key;
            public int value;
        }

        public static void WriteIDTable(BinaryWriter writer, IEnumerator<TableItem> enumerator, int count)
        {
            int size = HashHelpers.GetPrime(count);

            var map = new HashEntry[size];
            var hasValue = new bool[size];
            List<TableItem> extraValues = null;

            for (int it = 0; it < size; it++)
            {
                map[it].value = -1;
                map[it].next = -1;
            }

            while (enumerator.MoveNext())
            {
                var kv = enumerator.Current;
                int hash = kv.hash;
                int slotIndex = (int)(((uint)hash) % size);

                if (!hasValue[slotIndex])
                {
                    hasValue[slotIndex] = true;
                    map[slotIndex] = new HashEntry { key = kv.key, value = kv.value, next = -1 };
                }
                else
                {
                    if (extraValues == null) extraValues = new List<TableItem>();
                    extraValues.Add(kv);
                }
            }
            if (extraValues != null)
            {
                int maxDepth = 1;
                for (int itemIndex = extraValues.Count - 1; itemIndex >= 0; itemIndex--)
                {
                    var item = extraValues[itemIndex];
                    int slotIndex = (int)(((uint)item.hash) % size);

                    int depth = 1;
                    while (map[slotIndex].next >= 0)
                    {
                        depth++;
                        slotIndex = map[slotIndex].next;
                    }
                    if (depth > maxDepth) maxDepth = depth;

                    int emptySlotIndex = -1;
                    for (int it = 0; it < hasValue.Length; it++)
                    {
                        if (!hasValue[it])
                        {
                            emptySlotIndex = it;
                            break;
                        }
                    }
                    if (emptySlotIndex == -1) throw new Exception("should not reach here");

                    hasValue[emptySlotIndex] = true;
                    map[emptySlotIndex] = new HashEntry { key = item.key, value = item.value, next = -1 };
                    map[slotIndex].next = emptySlotIndex;
                }
            }

            writer.Write(size);
            foreach (var entry in map)
            {
                writer.Write(entry.key);
                writer.Write(entry.value);
                writer.Write(entry.next);
            }
        }

        private static IEnumerator<TableItem> ConvertDictionaryToTableItem(Dictionary<int, int> idTable)
        {
            foreach (var kv in idTable)
            {
                yield return new TableItem { hash = kv.Key, key = kv.Key, value = kv.Value };
            }
        }

        public static void WriteIDTable(BinaryWriter writer, Dictionary<int, int> idTable)
        {
            WriteIDTable(writer, ConvertDictionaryToTableItem(idTable), idTable.Count);
        }

        public static byte[] StructureToBytes<T>(T t) where T : struct
        {
            int size = Marshal.SizeOf<T>();
            IntPtr ptr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(t, ptr, false);
            var bytes = new byte[size];
            Marshal.Copy(ptr, bytes, 0, size);
            Marshal.FreeHGlobal(ptr);
            return bytes;
        }

        public static void WriteStructure<T>(BinaryWriter writer, T t) where T : struct
        {
            writer.Write(StructureToBytes(t));
        }

        public static class HashHelpers
        {
            // Table of prime numbers to use as hash table sizes. 
            // A typical resize algorithm would pick the smallest prime number in this array
            // that is larger than twice the previous capacity. 
            // Suppose our Hashtable currently has capacity x and enough elements are added 
            // such that a resize needs to occur. Resizing first computes 2x then finds the 
            // first prime in the table greater than 2x, i.e. if primes are ordered 
            // p_1, p_2, ..., p_i, ..., it finds p_n such that p_n-1 < 2x < p_n. 
            // Doubling is important for preserving the asymptotic complexity of the 
            // hashtable operations such as add.  Having a prime guarantees that double 
            // hashing does not lead to infinite loops.  IE, your hash function will be 
            // h1(key) + i*h2(key), 0 <= i < size.  h2 and the size must be relatively prime.
            public static readonly int[] primes = {
            3, 7, 11, 17, 23, 29, 37, 47, 59, 71, 89, 107, 131, 163, 197, 239, 293, 353, 431, 521, 631, 761, 919,
            1103, 1327, 1597, 1931, 2333, 2801, 3371, 4049, 4861, 5839, 7013, 8419, 10103, 12143, 14591,
            17519, 21023, 25229, 30293, 36353, 43627, 52361, 62851, 75431, 90523, 108631, 130363, 156437,
            187751, 225307, 270371, 324449, 389357, 467237, 560689, 672827, 807403, 968897, 1162687, 1395263,
            1674319, 2009191, 2411033, 2893249, 3471899, 4166287, 4999559, 5999471, 7199369};

            const Int32 HashPrime = 101;

            public static bool IsPrime(int candidate)
            {
                if ((candidate & 1) != 0)
                {
                    int limit = (int)Math.Sqrt(candidate);
                    for (int divisor = 3; divisor <= limit; divisor += 2)
                    {
                        if ((candidate % divisor) == 0)
                            return false;
                    }
                    return true;
                }
                return (candidate == 2);
            }

            public static int GetPrime(int min)
            {
                if (min < 0)
                    throw new ArgumentException("Arg_HTCapacityOverflow");

                for (int i = 0; i < primes.Length; i++)
                {
                    int prime = primes[i];
                    if (prime >= min) return prime;
                }

                //outside of our predefined table. 
                //compute the hard way. 
                for (int i = (min | 1); i < Int32.MaxValue; i += 2)
                {
                    if (IsPrime(i) && ((i - 1) % HashPrime != 0))
                        return i;
                }
                return min;
            }

            public static int GetMinPrime()
            {
                return primes[0];
            }

            // Returns size of hashtable to grow to.
            public static int ExpandPrime(int oldSize)
            {
                int newSize = 2 * oldSize;

                // Allow the hashtables to grow to maximum possible size (~2G elements) before encoutering capacity overflow.
                // Note that this check works even when _items.Length overflowed thanks to the (uint) cast
                if ((uint)newSize > MaxPrimeArrayLength && MaxPrimeArrayLength > oldSize)
                {
                    return MaxPrimeArrayLength;
                }

                return GetPrime(newSize);
            }


            // This is the maximum prime smaller than Array.MaxArrayLength
            public const int MaxPrimeArrayLength = 0x7FEFFFFD;
        }
    }

}
