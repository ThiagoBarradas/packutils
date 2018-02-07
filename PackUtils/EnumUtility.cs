using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace PackUtils
{
    /// <summary>
    /// Enum utility
    /// </summary>
    public static class EnumUtility
    {
        /// <summary>
        /// Cast enum
        /// </summary>
        /// <typeparam name="T">Enum type</typeparam>
        /// <param name="enumToConvert"></param>
        /// <returns></returns>
        public static T ConvertToEnum<T>(this object enumToConvert)
        {
            if (enumToConvert == null)
            {
                return default(T);
            }

            if (!typeof(T).GetTypeInfo().IsEnum)
            {
                throw new ArgumentException("T must be an enumerated type.");
            }

            T convertedEnum = default(T);

            try
            {
                convertedEnum = (T)Enum.Parse(typeof(T), enumToConvert.ToString(), true);
            }
            catch (Exception) { }

            return convertedEnum;
        }

        /// <summary>
        /// Convert enum list
        /// </summary>
        /// <typeparam name="T">Enum type</typeparam>
        /// <param name="objs">Obj list</param>
        /// <returns></returns>
        public static List<T> ConvertToEnum<T>(this IList objs)
        {
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException("T must be an enumerated type.");
            }

            List<T> list = new List<T>();

            if (objs == null)
            {
                return list;
            }

            foreach (var obj in objs)
            {
                try
                {
                    list.Add(ConvertToEnum<T>(obj));
                }
                catch (Exception) { }
            }

            return list;
        }
        
        /// <summary>
        /// Get description from enum
        /// </summary>
        /// <param name="value">Enum value</param>
        /// <returns></returns>
        public static string GetDescriptionFromEnum(this Enum value)
        {
            FieldInfo fieldInfo = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes =
                (DescriptionAttribute[])fieldInfo.GetCustomAttributes(
                typeof(DescriptionAttribute),
                false);

            if (attributes != null && attributes.Length > 0)
            {
                return attributes[0].Description;
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Get enum from description - if not found, returns default
        /// </summary>
        /// <typeparam name="T">Enum type</typeparam>
        /// <param name="description">description</param>
        /// <param name="notFoundReturnDefault">description</param>
        /// <returns></returns>
        public static T GetEnumFromDescription<T>(string description, bool notFoundReturnDefault = true)
        {
            var type = typeof(T);
            if (!type.IsEnum)
            {
                throw new InvalidOperationException();
            }

            foreach (var field in type.GetFields())
            {
                if (Attribute.GetCustomAttribute(field,
                    typeof(DescriptionAttribute)) is DescriptionAttribute attribute)
                {
                    if (attribute.Description == description)
                    {
                        return (T)field.GetValue(null);
                    }
                }
                else
                {
                    if (field.Name == description)
                    {
                        return (T)field.GetValue(null);
                    }
                }
            }

            if (notFoundReturnDefault == true)
            {
                return default(T);
            }

            throw new KeyNotFoundException(description + " not found in " + typeof(T).Name);
        }

        /// <summary>
        /// Is valid to parse
        /// </summary>
        /// <typeparam name="T">Enum type</typeparam>
        /// <param name="obj">value to parse</param>
        /// <returns></returns>
        public static bool IsValidToParse<T>(object obj)
        {
            return IsValidToParse<T>(obj, false);
        }

        /// <summary>
        /// Is valid to parse
        /// </summary>
        /// <typeparam name="T">Enum type</typeparam>
        /// <param name="obj">value to parse</param>
        /// <param name="acceptNull">Set if null is valid</param>
        /// <returns></returns>
        public static bool IsValidToParse<T>(object obj, bool acceptNull)
        {
            if (obj == null)
            {
                return acceptNull;
            }

            if (!typeof(T).GetTypeInfo().IsEnum)
            {
                throw new ArgumentException("T must be an enumerated type.");
            }

            T convertedEnum = default(T);

            try
            {
                convertedEnum = (T)Enum.Parse(typeof(T), obj.ToString());
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Get description from enums
        /// </summary>
        /// <typeparam name="T">Enum type</typeparam>
        /// <param name="enums">Enum values</param>
        /// <returns></returns>
        public static List<string> GetDescriptionsFromEnums<T>(List<T> enums)
        {
            var descriptions = enums.Select(r => (r as Enum).GetDescriptionFromEnum()).ToList();
            descriptions.RemoveAll(r => string.IsNullOrWhiteSpace(r));

            return descriptions;
        }

        /// <summary>
        /// Get all enums with description
        /// </summary>
        /// <typeparam name="T">Enum type</typeparam>
        /// <returns></returns>
        public static List<T> GetAllEnumsWithDescription<T>()
        {
            List<T> enums = Enum.GetValues(typeof(T)).Cast<T>().ToList();
            return enums.Where(r => string.IsNullOrWhiteSpace((r as Enum).GetDescriptionFromEnum()) == false).ToList();
        }
    }
}
