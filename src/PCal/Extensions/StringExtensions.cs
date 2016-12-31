using System;
using System.Text.RegularExpressions;

namespace PCal.Extensions
{
    public static class StringExtensions
    {
        public static bool IsNullOrEmpty(this string text)
        {
            return string.IsNullOrEmpty(text);
        }

        public static bool IsNotNullOrEmpty(this string text)
        {
            return !text.IsNullOrEmpty();
        }

        public static string ToRavenId(this string id)
        {
            if (id == null) throw new ArgumentNullException(nameof(id));

            var pos = id.LastIndexOf("-", StringComparison.Ordinal) + 1;


            if (pos == 0 || pos > id.Length)
                throw new InvalidOperationException($"Unable to extract id number from {id}");

            return id.Substring(pos);
        }

        public static string CamelCaseToSpaces(this string value)
        {
            return Regex.Replace(value, "(?!^)([A-Z])", " $1");
        }
    }
}