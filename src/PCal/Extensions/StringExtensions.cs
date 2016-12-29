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
    }
}