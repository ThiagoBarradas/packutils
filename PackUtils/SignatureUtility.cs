using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace PackUtils
{
    /// <summary>
    /// Utility for signatures using HMAC SHA26
    /// Only plain objects
    /// </summary>
    public static class SignatureUtility
    {
        /// <summary>
        /// Create signature plain object
        /// </summary>
        /// <param name="privateKey">Private key</param>
        /// <param name="data">Object</param>
        /// <param name="ignoreField">Ignore a property</param>
        /// <returns></returns>
        public static string CreateSignature(string privateKey, object data, string ignoreField)
        {
            return SignatureUtility.CreateSignature(privateKey, data, SignatureUtility.GenerateIgnoreFields(ignoreField));
        }

        /// <summary>
        /// Create signature from plain object
        /// </summary>
        /// <param name="privateKey">Private key</param>
        /// <param name="data">Object</param>
        /// <param name="ignoreFields">Ignore some properties</param>
        /// <returns></returns>
        public static string CreateSignature(string privateKey, object data, List<string> ignoreFields)
        {
            if (ignoreFields == null) ignoreFields = new List<string>();
            var message = new StringBuilder();

            foreach (var propertyInfo in data.GetType().GetProperties().OrderBy(p => p.Name))
            {
                if (ignoreFields.Contains(propertyInfo.Name) == false)
                {
                    var propertyValue = propertyInfo.GetValue(data);

                    if (propertyInfo.PropertyType == typeof(bool))
                    {
                        propertyValue = propertyValue.ToString().ToLower();
                    }

                    if (propertyValue != null)
                    {
                        message.Append(propertyInfo.Name);
                        message.Append(propertyValue.ToString());
                    }
                }
            }

            var signature = SignatureUtility.Hash(privateKey, message.ToString());

            return signature;
        }

        /// <summary>
        /// Create signature from string
        /// </summary>
        /// <param name="privateKey"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static string CreateSignature(string privateKey, string message)
        {
            return SignatureUtility.Hash(privateKey, message.ToString());
        }

        /// <summary>
        /// Validate signature from plain object
        /// </summary>
        /// <param name="signature">Signature</param>
        /// <param name="privateKey">Private key</param>
        /// <param name="data">Object</param>
        /// <param name="ignoreField">Ignore a properties</param>
        /// <returns></returns>
        public static bool ValidateSignature(string signature, string privateKey, object data, string ignoreFields)
        {
            return SignatureUtility.ValidateSignature(signature, privateKey, data, SignatureUtility.GenerateIgnoreFields(ignoreFields));
        }

        /// <summary>
        /// Validate signature from plain object
        /// </summary>
        /// <param name="signature">Signature</param>
        /// <param name="privateKey">Private key</param>
        /// <param name="data">Object</param>
        /// <param name="ignoreFields">Ignore some properties</param>
        /// <returns></returns>
        public static bool ValidateSignature(string signature, string privateKey, object data, List<string> ignoreFields)
        {
            var computedSignature = SignatureUtility.CreateSignature(privateKey, data, ignoreFields);
            return string.Compare(computedSignature, signature, StringComparison.OrdinalIgnoreCase) == 0;
        }

        /// <summary>
        /// Validate signature from string
        /// </summary>
        /// <param name="signature">Signature</param>
        /// <param name="privateKey">Private key</param>
        /// <param name="message">Content</param>
        /// <returns></returns>
        public static bool ValidateSignature(string signature, string privateKey, string message)
        {
            return (signature == SignatureUtility.CreateSignature(privateKey, message));
        }

        /// <summary>
        /// Create Hash
        /// </summary>
        /// <param name="key">private key</param>
        /// <param name="message">Message</param>
        /// <returns></returns>
        private static string Hash(string key, string message)
        {
            var keyBytes = Encoding.UTF8.GetBytes(key);
            var hmac = new HMACSHA256(keyBytes);
            var messageBytes = Encoding.UTF8.GetBytes(message);
            var hashBytes = hmac.ComputeHash(messageBytes);
            var signature = BitConverter.ToString(hashBytes).Replace("-", string.Empty).ToLowerInvariant();

            return signature;
        }
        
        /// <summary>
        /// Generate ignored fields
        /// </summary>
        /// <param name="ignoreField"></param>
        /// <returns></returns>
        private static List<string> GenerateIgnoreFields(string ignoreField)
        {
            List<string> ignoreFields = new List<string>();

            if (string.IsNullOrWhiteSpace(ignoreField) == false)
            {
                ignoreFields.Add(ignoreField);
            }

            return ignoreFields;
        }
    }
}
