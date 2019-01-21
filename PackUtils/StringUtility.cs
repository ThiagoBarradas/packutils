using System;
using System.Globalization;
using System.Text;

namespace PackUtils
{
    /// <summary>
    /// String utility
    /// </summary>
    public static class StringUtility
    {
        /// <summary>
        /// Replace multi separators
        /// </summary>
        /// <param name="originalValue"></param>
        /// <param name="separators"></param>
        /// <param name="newValue"></param>
        /// <returns></returns>
        public static string Replace(this string originalValue, char[] separators, string newValue)
        {
            if (originalValue == null)
            {
                return string.Empty;
            }

            string[] temp;

            temp = originalValue.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            return String.Join(newValue, temp);
        }

        /// <summary>
        /// Remove diacritics ('á' is changed to 'a') 
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string RemoveDiacritics(this string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }

        /// <summary>
        /// Keep only a-z and A-Z
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string RemoveNumbersAndSpecialCharacters(this string str)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in str)
            {
                if ((c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == ' ')
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }
    }
}
