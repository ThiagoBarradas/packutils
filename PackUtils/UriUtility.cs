using System;
using System.Web;

namespace PackUtils
{
    /// <summary>
    /// Uri utility
    /// </summary>
    public static class UriUtility
    {
        /// <summary>
        /// Add or replace if exists a query string parameter in Uri
        /// </summary>
        /// <param name="uri">Uri - if null, returns null</param>
        /// <param name="parameter">Parameter name - if null, returns null</param>
        /// <param name="value">Value - if null, the value will be change to null</param>
        /// <returns></returns>
        public static string AddQueryString(this string uri, string parameter, string value)
        {
            if (string.IsNullOrWhiteSpace(uri) == true || string.IsNullOrWhiteSpace(parameter) == true)
            {
                return uri;
            }

            var uriBuilder = new UriBuilder(uri);
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);

            query[parameter] = value;

            uriBuilder.Query = query.ToString();
           
            if (uriBuilder.Port == 80 && uriBuilder.Scheme == "http")
            {
                uriBuilder.Port = -1;
            }

            if (uriBuilder.Port == 443 && uriBuilder.Scheme == "https")
            {
                uriBuilder.Port = -1;
            }

            return uriBuilder.ToString();
        }
    }
}
