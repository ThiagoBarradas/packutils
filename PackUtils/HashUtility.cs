using System;
using System.Security.Cryptography;
using System.Text;

namespace PackUtils
{
    /// <summary>
    /// Hash utility
    /// </summary>
    public static class HashUtility
    {
        /// <summary>
        /// Generate random sha 256
        /// </summary>
        /// <returns></returns>
        public static string GenerateRandomSha256()
        {
            string token = Guid.NewGuid().ToString() +
                           (new Random().Next()).ToString();

            return HashUtility.GenerateSha256(token);
        }

        /// <summary>
        /// Generate Sha256
        /// </summary>
        /// <param name="value">value</param>
        /// <returns></returns>
        public static string GenerateSha256(this string value)
        {
            return GenerateSha256(value, null);
        }

        /// <summary>
        /// Generate Sha256
        /// </summary>
        /// <param name="value">value</param>
        /// <param name="gap">gap</param>
        /// <returns></returns>
        public static string GenerateSha256(this string value, string gap)
        {
            var newString = value + (gap ?? "");

            SHA256Managed crypt = new SHA256Managed();
            StringBuilder hash = new StringBuilder();
            byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(newString), 0, Encoding.UTF8.GetByteCount(newString));
            foreach (byte theByte in crypto)
            {
                hash.Append(theByte.ToString("x2"));
            }
            return hash.ToString();
        }

        /// <summary>
        /// Base 64 encode
        /// </summary>
        /// <param name="plainText"></param>
        /// <returns></returns>
        public static string Base64Encode(this string plainText)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }

        /// <summary>
        /// Base 64 decode
        /// </summary>
        /// <param name="base64"></param>
        /// <returns></returns>
        public static string Base64Decode(this string base64)
        {
            var plainTextBytes = Convert.FromBase64String(base64);
            return Encoding.UTF8.GetString(plainTextBytes);
        }
    }
}
