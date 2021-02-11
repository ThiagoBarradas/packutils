using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Text;

namespace PackUtils
{
    public static class HeaderUtility
    {
        /// <summary>
        /// Get signature from header
        /// </summary>
        /// <param name="headers"></param>
        /// <param name="headerName"></param>
        /// <returns></returns>
        public static string GetSignature(this IHeaderDictionary headers, string headerName = "x-hub-signature")
        {
            var defaultReturn = "";

            try
            {
                if (!headers.Any(r => r.Key.ToLower() == headerName.ToLowerInvariant()))
                {
                    return defaultReturn;
                }

                return headers.First(r => r.Key.ToLower() == headerName.ToLowerInvariant()).Value.ToString();
            }
            catch (Exception)
            {
                return defaultReturn;
            }
        }

        /// <summary>
        /// Get basic auth from header
        /// </summary>
        /// <param name="headers"></param>
        /// <returns></returns>
        public static (string user, string pass) GetBasicAuth(this IHeaderDictionary headers)
        {
            var defaultReturn = ("", "");

            try
            {
                if (!headers.ContainsKey("Authorization"))
                {
                    return defaultReturn;
                }

                var authParts = headers["Authorization"].ToString().Split(' ');

                if (authParts.First().ToLower() != "basic")
                {
                    return defaultReturn;
                }

                var authHeader = Encoding.UTF8.GetString(Convert.FromBase64String(authParts.Last()));

                if (!authHeader.Contains(":"))
                {
                    return defaultReturn;
                }

                var basicParts = authHeader.Split(new[] { ':' }, 2);

                return (basicParts[0], basicParts[1]);
            }
            catch (Exception)
            {
                return defaultReturn;
            }
        }

        /// <summary>
        /// Get header value
        /// </summary>
        /// <param name="headers"></param>
        /// <param name="headerName"></param>
        /// <returns></returns>
        public static string GetHeaderValue(this IHeaderDictionary headers, string headerName)
        {
            var idempotencyKey = headers.FirstOrDefault(r => r.Key.ToLowerInvariant() == headerName.ToLowerInvariant()).Value;

            return (!string.IsNullOrWhiteSpace(idempotencyKey))
                ? idempotencyKey.ToString()
                : null;
        }

        /// <summary>
        /// Generate basic auth
        /// </summary>
        /// <param name="user"></param>
        /// <param name="pass"></param>
        /// <returns></returns>
        private static string GenerateBasicAuthValue(string user, string pass)
        {
            return "Basic " + $"{user}:{pass}".Base64Encode();
        }
    }
}
