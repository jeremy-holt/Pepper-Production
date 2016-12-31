using System;

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
            var pos = id.LastIndexOf("-", StringComparison.Ordinal) + 1;


            if (pos == 0 || pos > id.Length)
                throw new InvalidOperationException($"Unable to extract id number from {id}");

            return id.Substring(pos);
        }
    }
}