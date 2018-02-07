using System;
using System.Security.Cryptography;
using System.Text;

namespace PackUtils
{
    /// <summary>
    /// Hash utility
    /// </summary>
    public class HashUtility
    {
        /// <summary>
        /// Generate random sha 256
        /// </summary>
        /// <returns></returns>
        public static string GenerateRandomSha256()
        {
            string token = Guid.NewGuid().ToString() +
                           DateTime.UtcNow.ToString() +
                           (new Random().Next()).ToString();

            return HashUtility.GenerateSha256(token);
        }

        /// <summary>
        /// Generate Sha256
        /// </summary>
        /// <param name="str">value</param>
        /// <returns></returns>
        public static string GenerateSha256(string str)
        {
            return GenerateSha256(str, null);
        }

        /// <summary>
        /// Generate Sha256
        /// </summary>
        /// <param name="str">value</param>
        /// <param name="gap">gap</param>
        /// <returns></returns>
        public static string GenerateSha256(string str, string gap)
        {
            str = str + (gap ?? "");

            SHA256Managed crypt = new SHA256Managed();
            StringBuilder hash = new StringBuilder();
            byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(str), 0, Encoding.UTF8.GetByteCount(str));
            foreach (byte theByte in crypto)
            {
                hash.Append(theByte.ToString("x2"));
            }
            return hash.ToString();
        }
    }
}
