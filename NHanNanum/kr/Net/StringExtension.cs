using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace System
{
    public static class StringExtension
    {
        public static bool Matches(this string txt, string pattern)
        {
            Regex exp = new Regex(pattern);
            return exp.IsMatch(txt);
        }

        public static string ReplaceFirst(this string txt, string pattern, string replacement)
        {
            Regex exp = new Regex(pattern);
            return exp.Replace(txt, replacement, 1);
        }

        public static string[] Split(this string txt, string pattern)
        {
            Regex exp = new Regex(pattern);
            string [] splits = exp.Split(txt);
            List<string> results = new List<string>();
            for (int i = 0; i < splits.Length - 1; i++)
            {
                results.Add(splits[i]);
            }

            if (string.IsNullOrEmpty(splits[splits.Length - 1]) == false)
            {
                results.Add(splits[splits.Length - 1]);
            }

            return results.ToArray();
        }

        public static string[] Split(this string txt, string pattern, int count)
        {
            Regex exp = new Regex(pattern);
            return exp.Split(txt, count);
        }

        public static T[] ToArray<T>(this LinkedList<T> list)
        {
            T[] arrays = new T[list.Count];
            list.CopyTo(arrays, 0);

            return arrays;
        }

        public static T[] ToArray<T>(this LinkedList<T> list, T [] elements)
        {
            if (elements.Length < list.Count)
            {
                elements = new T[list.Count];
            }

            list.CopyTo(elements, 0);

            return elements;
        }

        public static T Get_Renamed<T>(this LinkedList<T> list, int position)
        {
            int start = 0;

            foreach (var item in list)
            {
                if (start == position)
                {
                    return item;
                }

                start++;
            }

            return default(T);
        }
    }
}

namespace System.IO
{
    public static class StreamReaderExtension
    {
        public static bool MarkSupported(this StreamReader reader)
        {
            return true;
        }

        public static void Mark(this StreamReader reader, int maxValue, Dictionary<StreamReader, long> marks)
        {
            marks[reader] = reader.BaseStream.Position;
        }

        public static void Reset(this StreamReader reader, Dictionary<StreamReader, long> marks)
        {
            if (marks.ContainsKey(reader) == true)
            {
                reader.BaseStream.Position = marks[reader];
            }
        }
    }
}
