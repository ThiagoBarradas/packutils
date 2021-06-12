using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Linq;
using System.Reflection;

namespace PackUtils.Converters
{
    /// <summary>
    ///  Nullable enum converter for newtonsoft. 
    ///  When the sent enum value cannot be parsed, it will set a default value.
    /// </summary>
    public class NullableEnumDefaultValueConverter : JsonConverter
    {
        private readonly string _defaultValue;

        public NullableEnumDefaultValueConverter()
        {
            throw new ArgumentNullException("enumDefaultValue", "An enum default value is required.");
        }

        public NullableEnumDefaultValueConverter(Enum enumDefaultValue)
        {
            if(enumDefaultValue == null)
            {
                throw new ArgumentNullException(nameof(enumDefaultValue), "An enum default value is required.");
            }

            _defaultValue = enumDefaultValue.ToString();
        }

        public override bool CanConvert(Type objectType)
        {
            var type = IsNullableType(objectType)
                            ? Nullable.GetUnderlyingType(objectType)
                            : objectType;

            return type.GetTypeInfo().IsEnum;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var isNullable = IsNullableType(objectType);
            var enumType = isNullable
                ? Nullable.GetUnderlyingType(objectType)
                : objectType;

            if (!enumType.IsEnum)
            {
                return reader.Value;
            }

            var enumValues = Enum.GetNames(enumType);

            if (reader.TokenType == JsonToken.String)
            {
                string enumText = reader.Value.ToString();

                if (!string.IsNullOrEmpty(enumText))
                {
                    var match = enumValues
                        .Where(n => string.Equals(n.Replace("_", ""), enumText.Replace("_", ""), StringComparison.OrdinalIgnoreCase))
                        .FirstOrDefault();

                    if (match != null)
                    {
                        return Enum.Parse(enumType, match);
                    }

                    var firstEnumValue = enumValues.FirstOrDefault();
                    if (string.Equals(firstEnumValue.ToString(), _defaultValue))
                    {
                        return Enum.Parse(enumType, firstEnumValue);
                    }
                }
            }
            else if (reader.TokenType == JsonToken.Integer)
            {
                var enumVal = Convert.ToInt32(reader.Value);
                var values = (int[])Enum.GetValues(enumType);
                if (values.Contains(enumVal))
                {
                    return Enum.Parse(enumType, enumVal.ToString());
                }
            }

            if (!isNullable)
            {
                var defaultName = enumValues
                    .Where(n => string.Equals(n.Replace("_", ""), "Undefined", StringComparison.OrdinalIgnoreCase))
                    .FirstOrDefault();

                if (defaultName == null)
                {
                    defaultName = enumValues.First();
                }

                return Enum.Parse(enumType, defaultName);
            }

            return null;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var finalValue = value.ToString();

            if (serializer.ContractResolver is CamelCasePropertyNamesContractResolver ||
                serializer.ContractResolver is CustomCamelCasePropertyNamesContractResolver)
            {
                finalValue = finalValue.ToCamelCase();
            }
            else if (serializer.ContractResolver is SnakeCasePropertyNamesContractResolver)
            {
                finalValue = finalValue.ToSnakeCase();
            }
            else if (serializer.ContractResolver is LowerCasePropertyNamesContractResolver)
            {
                finalValue = finalValue.ToLowerCase();
            }

            writer.WriteValue(finalValue);
        }

        private bool IsNullableType(Type t)
        {
            return (t.GetTypeInfo().IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>));
        }
    }
}