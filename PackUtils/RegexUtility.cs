using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace PackUtils
{
    /// <summary>
    /// Regex utility
    /// </summary>
    public static class RegexUtility
    {
        private const string ALPHA_NUMERIC = "^[A-Za-z0-9]{{min},{max}}$";
        private const string ALPHA = "^[A-Za-z]{{min},{max}}$";
        private const string URL = "^(https?)\\:\\/\\/[a-zA-Z0-9\\-\\.]+\\.[a-zA-Z](:[a-zA-Z0-9]*)?/?([a-zA-Z0-9\\-\\._\\:?\\,\\'/\\+&amp;%\\$#\\=~])*[^\\.\\,\\)\\(\\s]$";
        private const string IP = "^(([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])\\.){3}([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])$";
        private const string URL_OR_IP = "^(((https?)\\:\\/\\/[a-zA-Z0-9\\-\\.]+\\.[a-zA-Z](:[a-zA-Z0-9]*)?\\/?([a-zA-Z0-9\\-\\._\\:?\\,\\'/\\+&amp;%\\$#\\=~])*[^\\.\\,\\)\\(\\s])|((([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])\\.){3}([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])))$";

        /// <summary>
        /// Multimatch regex
        /// </summary>
        /// <param name="texts">All texts to check</param>
        /// <param name="regex"Regex pattern></param>
        /// <returns></returns>
        public static bool IsMatch(List<string> texts, string regex)
        {
            return IsMatch(texts, regex, true);
        }

        /// <summary>
        /// Multimatch regex
        /// </summary>
        /// <param name="texts">All texts to check</param>
        /// <param name="regex"Regex pattern></param>
        /// <param name="allowEmptyList">Allow empty list</param>
        /// <returns></returns>
        public static bool IsMatch(List<string> texts, string regex, bool allowEmptyList)
        {
            if (texts == null || texts.Any() == false)
            {
                return allowEmptyList;
            }

            Regex regexObj = new Regex(regex);
            foreach (var text in texts)
            {
                if (regexObj.IsMatch(text) == false)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Return Regex for Ip
        /// </summary>
        /// <returns></returns>
        public static string GetIpRegex()
        {
            return RegexUtility.IP;
        }

        /// <summary>
        /// Return Regex for Url or Ip
        /// </summary>
        /// <returns></returns>
        public static string GetUrlOrIpRegex()
        {
            return RegexUtility.URL_OR_IP;
        }

        /// <summary>
        /// Return Regex for Url
        /// </summary>
        /// <returns></returns>
        public static string GetUrlRegex()
        {
            return RegexUtility.URL;
        }

        /// <summary>
        /// Return Regex for Alpha sensitive
        /// </summary>
        /// <returns></returns>
        public static string GetAlphaRegex(int min, int max)
        {
            return RegexUtility.ALPHA
                .Replace("{min}", min.ToString())
                .Replace("{max}", max.ToString());
        }

        /// <summary>
        /// Return Regex for Alpha Numeric sensitive
        /// </summary>
        /// <returns></returns>
        public static string GetAlphaNumericRegex(int min, int max)
        {
            return RegexUtility.ALPHA_NUMERIC
                .Replace("{min}", min.ToString())
                .Replace("{max}", max.ToString());
        }

        /// <summary>
        /// Return Regex for Alpha Numeric sensitive
        /// </summary>
        /// <returns></returns>
        public static string GetPasswordRegex(int min, int max, bool requiresSpecialChar, bool requiresNumericChar, bool requiresLowerChar, bool requiresUpperChar)
        {
            var expression = "";
            var specialChars = "\\\"\\\'!@#$%¨&*()_\\-+=§¬¢£¹²³\\\\\\/|,.<>;:?°ºª\\[\\]{}~^´`";
            
            expression += (requiresLowerChar)
                ? "(?=.*[a-z])"
                : "([a-z]?)";

            expression += (requiresUpperChar)
                ? "(?=.*[A-Z])"
                : "([A-Z]?)";

            expression += (requiresNumericChar)
                ? "(?=.*[0-9])"
                : "([0-9]?)";

            expression += (requiresSpecialChar)
                ? "(?=.*[" + specialChars + "])"
                : "([" + specialChars + "]?)";

            return "^{exp}.{{min},{max}}$"
                    .Replace("{exp}", expression)
                    .Replace("{min}", min.ToString())
                    .Replace("{max}", max.ToString());
        }
    }
}
