using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace PackUtils
{
    /// <summary>
    /// Regex utility
    /// </summary>
    public class RegexUtility
    {
        private const string ALPHA_NUMERIC = "^[A-Za-z0-9]{{min},{max}}$";
        private const string ALPHA = "^[A-Za-z0-9]{{min},{max}}$";
        private const string URL = "^(https?)\\:\\/\\/[a-zA-Z0-9\\-\\.]+\\.[a-zA-Z](:[a-zA-Z0-9]*)?/?([a-zA-Z0-9\\-\\._\\:?\\,\\'/\\+&amp;%\\$#\\=~])*[^\\.\\,\\)\\(\\s]$";
        private const string IP = "^(([0 - 9]|[1-9] [0-9]|1[0-9]{2}|2[0-4] [0-9]|25[0-5])\\.){3}([0 - 9]|[1-9] [0-9]|1[0-9]{2}|2[0-4] [0-9]|25[0-5])$";
        private const string URL_OR_IP = "^(((https?)\\:\\/\\/[a-zA-Z0-9\\-\\.]+\\.[a-zA-Z](:[a-zA-Z0-9]*)?\\/?([a-zA-Z0-9\\-\\._\\:?\\,\\'/\\+&amp;%\\$#\\=~])*[^\\.\\,\\)\\(\\s])|((([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])\\.){3}([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])))$";

        /// <summary>
        /// Multimatch regex
        /// </summary>
        /// <param name="texts">All texts to check</param>
        /// <param name="regex"Regex pattern></param>
        /// <param name="allowEmptyList">Allow empty list</param>
        /// <returns></returns>
        public static bool IsMatch(List<string> texts, string regex, bool allowEmptyList = true)
        {
            if (texts == null || texts.Any() == false)
            {
                return allowEmptyList;
            }

            Regex rgx = new Regex(regex);
            foreach (var text in texts)
            {
                if (rgx.IsMatch(text) == false)
                    return false;
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
            var upper = "(?=.*[A-Z])";
            var lower = "(?=.*[a-z])";
            var numeric = "(?=.*[0-9])";
            var special = "(?=.*[\\\"\\\'!@#$%¨&*()_\\-+=§¬¢£¹²³\\\\\\/|,.<>;:?°ºª\\[\\]{}~^´`])";

            if (requiresLowerChar)
            {
                expression += lower;
            }

            if (requiresUpperChar)
            {
                expression += upper;
            }

            if (requiresNumericChar)
            {
                expression += numeric;
            }

            if (requiresSpecialChar)
            {
                expression += special;
            }

            return "^{exp}.{{min},{max}}$"
                    .Replace("{exp}", expression.ToString())
                    .Replace("{min}", min.ToString())
                    .Replace("{max}", max.ToString());
        }
    }
}
