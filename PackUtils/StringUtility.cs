using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace PackUtils
{
    /// <summary>
    /// String utility
    /// </summary>
    public static class StringUtility
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="strategy"></param>
        /// <returns></returns>
        public static string ToCase(this string value, string strategy)
        {
            if(string.IsNullOrWhiteSpace(value))
            {
                return null;
            }

            strategy = strategy?.ToLowerInvariant().Trim();

            switch (strategy)
            {
                case "snake":
                case "snakecase":
                    return value.ToSnakeCase();
                case "camel":
                case "camelcase":
                    return value.ToCamelCase();
                case "lower":
                case "lowercase":
                    return value.ToLowerCase();
                default:
                    return value;
            }
        }

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

        /// <summary>
        /// To Snake Case 
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string ToSnakeCase(this string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return null;
            }

            text = string.Concat(text.Select((_char, i) => i > 0 && char.IsUpper(_char) ? $"_{_char.ToString()}" : _char.ToString())).ToLower();

            text = Regex.Replace(text, @"\d+", m => "_" + m.Value);

            return text;
        }

        /// <summary>
        /// To camel case
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string ToCamelCase(this string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return null;
            }

            var finalValue = "";
            var upperNext = false;
            for (int i = 0; i < text.Length - 1; i++)
            {
                finalValue += (upperNext) ? char.ToUpperInvariant(text[i]) : text[i];
                upperNext = text[i] == '_';
            }

            finalValue += text[text.Length - 1].ToString();

            finalValue = finalValue.Replace("_", "");
            if (finalValue.Length == 0)
            {
                return null;
            }

            finalValue = Regex.Replace(finalValue, "([A-Z])([A-Z]+)($|[A-Z])",
                m => m.Groups[1].Value + m.Groups[2].Value.ToLower() + m.Groups[3].Value);

            return char.ToLowerInvariant(finalValue[0]) + finalValue.Substring(1);
        }

        /// <summary>
        /// To lower case
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string ToLowerCase(this string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return null;
            }

            return text.ToLowerInvariant().Replace("_", "");
        }
    }
}
