using System.Globalization;
using System.Text.RegularExpressions;

namespace RateSetter.Sources.Extensions
{
    public static class StringExtensions
    {
        public static string ToTitleCase(this string input)
        {
            return string.IsNullOrEmpty(input)
                ? input
                : CultureInfo.InvariantCulture.TextInfo.ToTitleCase(input.ToLowerInvariant());
        }
        
        public static string TrimDuplicateSpaces(this string str)
        {
            if (string.IsNullOrEmpty(str)) return str;

            var regex = new Regex("[ ]{2,}", RegexOptions.None);
            str = regex.Replace(str, " ");

            return str.Trim();
        }

        public static string TrimSpecialCharacters(this string str)
        {
            if (string.IsNullOrEmpty(str)) return str;

            var regEx = new Regex("[^A-Za-z0-9]");
            return Regex.Replace(regEx.Replace(str, " "), @"\s+", " ");
        }
    }
}