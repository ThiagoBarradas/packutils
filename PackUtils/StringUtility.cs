using System;

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
            string[] temp;

            temp = originalValue.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            return String.Join(newValue, temp);
        }
    }
}
