using System;
using System.Text.RegularExpressions;

namespace RSTest.Extensions
{
	public static class StringExtensions
	{
        // Remove special characters from string
        public static string RemoveSpecialCharacters(this string str)
        {
            return Regex.Replace(str, "[^a-z0-9_.]+", "", RegexOptions.Compiled);
        }

        // Normalize string
        public static string Standandize(this string str)
        {
            str = str.Trim().ToLower();
            return RemoveSpecialCharacters(str);
        }

        // Reverse string
        public static string Reverse(this string s)
        {
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }
    }
}

