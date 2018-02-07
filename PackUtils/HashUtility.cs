using System;
using System.Globalization;
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
        public static string GenerateSha256(string value)
        {
            return GenerateSha256(value, null);
        }

        /// <summary>
        /// Generate Sha256
        /// </summary>
        /// <param name="value">value</param>
        /// <param name="gap">gap</param>
        /// <returns></returns>
        public static string GenerateSha256(string value, string gap)
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
    }
}
