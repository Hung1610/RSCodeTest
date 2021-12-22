using System;
using System.Text.RegularExpressions;

namespace RSTest.Utilities
{
    public static class String
    {
        // Remove special characters from string
        public static string RemoveSpecialCharacters(string str)
        {
            return Regex.Replace(str, "[^a-z0-9_.]+", "", RegexOptions.Compiled);
        }

        // Normalize string
        public static string NormalizeString(string str)
        {
            str = str.Trim().ToLower();
            return RemoveSpecialCharacters(str);
        }

        // Reverse string
        public static string Reverse(string s)
        {
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }

        // Check if a string is an anagram of another
        public static bool AreAnagrams(string string1, string string2)
        {
            if (string1.Length != string2.Length)
                return false;
            char[] str1 = string1.ToCharArray();
            char[] str2 = string2.ToCharArray();
            Array.Sort(str1);
            Array.Sort(str2);
            if (str1.Where((t, i) => t != str2[i]).Any())
            {
                return false;
            }

            return true;
        }
    }
}

