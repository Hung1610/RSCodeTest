﻿using System;
using System.Text.RegularExpressions;

namespace RSTest.Utilities
{
    public static class Utility
    {
        // Check if provided coordinates are within distance
        public static bool IsWithinDistance(decimal distance,
                                            decimal longitude,
                                            decimal latitude,
                                            decimal otherLongitude,
                                            decimal otherLatitude)
        {
            decimal calcDistance = GetDistance(longitude, latitude, otherLongitude, otherLatitude);
            return distance >= calcDistance;
        }

        // Get distance between provided coordinates
        public static decimal GetDistance(decimal longitude,
                                          decimal latitude,
                                          decimal otherLongitude,
                                          decimal otherLatitude)
        {
            var baseRad = (decimal)Math.PI * latitude / 180;
            var targetRad = (decimal)Math.PI * otherLatitude / 180;
            var theta = longitude - otherLongitude;
            var thetaRad = (decimal)Math.PI * theta / 180;

            double dist =
                Math.Sin((double)baseRad) * Math.Sin((double)targetRad) + Math.Cos((double)baseRad) *
                Math.Cos((double)targetRad) * Math.Cos((double)thetaRad);
            dist = Math.Acos(dist);

            dist = dist * 180 / Math.PI;
            dist = dist * 60 * 1.1515;

            return 1609.34m * (decimal)dist;
        }

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

