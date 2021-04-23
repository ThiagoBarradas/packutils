using JsonMasking;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using PackUtils.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace PackUtils
{
    /// <summary>
    /// Json utility (with newtonsoft)
    /// </summary>
    public static class JsonUtility
    {
        private readonly static object Lock = new object();

        public static List<JsonConverter> DefaultConverters = new List<JsonConverter>
        {
            new EnumWithContractJsonConverter(),
            new IsoDateTimeConverter
            {
                DateTimeFormat = "yyyy'-'MM'-'dd'T'HH':'mm':'ss.ffffff"
            }
        };

        /// <summary>
        /// Get Newtonsoft JsonSerializerSettings by strategy (snake/snakecase, camel/camelcase, lower/lowercase or default)
        /// </summary>
        /// <param name="strategy"></param>
        /// <returns></returns>
        public static JsonSerializerSettings GetNewtonsoftJsonSerializerSettings(this string strategy)
        {
            strategy = strategy?.ToLowerInvariant().Trim();

            switch (strategy)
            {
                case "snake":
                case "snakecase":
                    return JsonUtility.SnakeCaseJsonSerializerSettings;
                case "camel":
                case "camelcase":
                    return JsonUtility.CamelCaseJsonSerializerSettings;
                case "lower":
                case "lowercase":
                    return JsonUtility.LowerCaseJsonSerializerSettings;
                default:
                    return JsonUtility.OriginalCaseJsonSerializerSettings;
            }
        }

        /// <summary>
        /// Get Newtonsoft JsonSerializer by strategy (snake/snakecase, camel/camelcase, lower/lowercase or default)
        /// </summary>
        /// <param name="strategy"></param>
        /// <returns></returns>
        public static JsonSerializer GetNewtonsoftJsonSerializer(this string strategy)
        {
            strategy = strategy?.ToLowerInvariant().Trim();

            switch (strategy)
            {
                case "snake":
                case "snakecase":
                    return JsonUtility.SnakeCaseJsonSerializer;
                case "camel":
                case "camelcase":
                    return JsonUtility.CamelCaseJsonSerializer;
                case "lower":
                case "lowercase":
                    return JsonUtility.LowerCaseJsonSerializer;
                default:
                    return JsonUtility.OriginalCaseJsonSerializer;
            }
        }

        /// <summary>
        /// Snake case json serializer settings
        /// </summary>

        public static JsonSerializerSettings SnakeCaseJsonSerializerSettings
        {
            get
            {
                if (_snakeCaseJsonSerializerSettings == null)
                {
                    lock (Lock)
                    {
                        if (_snakeCaseJsonSerializerSettings == null)
                        {
                            var settings = new JsonSerializerSettings();

                            settings.ContractResolver = new SnakeCasePropertyNamesContractResolver();
                            DefaultConverters.ForEach(c => settings.Converters.Add(c));
                            settings.NullValueHandling = NullValueHandling.Ignore;

                            _snakeCaseJsonSerializerSettings = settings;
                        }
                    }   
                }

                return _snakeCaseJsonSerializerSettings;
            }
        }

        private static JsonSerializerSettings _snakeCaseJsonSerializerSettings;

        /// <summary>
        /// Camel case json serializer settings
        /// </summary>
        public static JsonSerializerSettings CamelCaseJsonSerializerSettings
        {
            get
            {
                if (_camelCaseJsonSerializerSettings == null)
                {
                    lock (Lock)
                    {
                        if (_camelCaseJsonSerializerSettings == null)
                        {
                            var settings = new JsonSerializerSettings();

                            settings.ContractResolver = new CustomCamelCasePropertyNamesContractResolver();
                            DefaultConverters.ForEach(c => settings.Converters.Add(c));
                            settings.NullValueHandling = NullValueHandling.Ignore;

                            _camelCaseJsonSerializerSettings = settings;
                        }
                    }
                }

                return _camelCaseJsonSerializerSettings;
            }
        }

        public static JsonSerializerSettings _camelCaseJsonSerializerSettings;

        /// <summary>
        /// Lower case json serializer settings
        /// </summary>
        public static JsonSerializerSettings LowerCaseJsonSerializerSettings
        {
            get
            {
                if (_lowerCaseJsonSerializerSettings == null)
                {
                    lock (Lock)
                    {
                        if (_lowerCaseJsonSerializerSettings == null)
                        {
                            var settings = new JsonSerializerSettings();

                            settings.ContractResolver = new LowerCasePropertyNamesContractResolver();
                            DefaultConverters.ForEach(c => settings.Converters.Add(c));
                            settings.NullValueHandling = NullValueHandling.Ignore;

                            _lowerCaseJsonSerializerSettings = settings;
                        }
                    }
                }


                return _lowerCaseJsonSerializerSettings;
            }
        }

        private static JsonSerializerSettings _lowerCaseJsonSerializerSettings;

        /// <summary>
        /// Original case json serializer settings
        /// </summary>
        public static JsonSerializerSettings OriginalCaseJsonSerializerSettings
        {
            get
            {
                if (_originalCaseJsonSerializerSettings == null)
                {
                    lock (Lock)
                    {
                        if (_originalCaseJsonSerializerSettings == null)
                        {
                            var settings = new JsonSerializerSettings();

                            settings.ContractResolver = new OriginalCasePropertyNamesContractResolver();
                            DefaultConverters.ForEach(c => settings.Converters.Add(c));
                            settings.NullValueHandling = NullValueHandling.Ignore;

                            _originalCaseJsonSerializerSettings = settings;
                        }
                    }
                }

                return _originalCaseJsonSerializerSettings;
            }
        }

        private static JsonSerializerSettings _originalCaseJsonSerializerSettings;

        /// <summary>
        /// Camel case json serialize
        /// </summary>
        public static JsonSerializer CamelCaseJsonSerializer
        {
            get
            {
                if (_camelCaseJsonSerializer == null)
                {
                    lock (Lock)
                    {

                        if (_camelCaseJsonSerializer == null)
                        {
                            var serializer = new JsonSerializer();
                            serializer.NullValueHandling = NullValueHandling.Ignore;
                            serializer.ContractResolver = new CustomCamelCasePropertyNamesContractResolver();
                            DefaultConverters.ForEach(c => serializer.Converters.Add(c));

                            _camelCaseJsonSerializer = serializer;
                        }
                    }
                }
                return _camelCaseJsonSerializer;
            }
        }

        private static JsonSerializer _camelCaseJsonSerializer;

        /// <summary>
        /// Snake case json serialize
        /// </summary>
        public static JsonSerializer SnakeCaseJsonSerializer
        {
            get
            {
                if (_snakeCaseJsonSerializer == null)
                {
                    lock (Lock)
                    {
                        if (_snakeCaseJsonSerializer == null)
                        {
                            var serializer = new JsonSerializer();
                            serializer.NullValueHandling = NullValueHandling.Ignore;
                            serializer.ContractResolver = new SnakeCasePropertyNamesContractResolver();
                            DefaultConverters.ForEach(c => serializer.Converters.Add(c));

                            _snakeCaseJsonSerializer = serializer;
                        }
                    }
                }

                return _snakeCaseJsonSerializer;
            }
        }

        private static JsonSerializer _snakeCaseJsonSerializer;

        /// <summary>
        /// Lower case json serialize
        /// </summary>
        public static JsonSerializer LowerCaseJsonSerializer
        {
            get
            {
                if (_lowerCaseJsonSerializer == null)
                {
                    lock (Lock)
                    {
                        if (_lowerCaseJsonSerializer == null)
                        {
                            var serializer = new JsonSerializer();
                            serializer.NullValueHandling = NullValueHandling.Ignore;
                            serializer.ContractResolver = new LowerCasePropertyNamesContractResolver();
                            DefaultConverters.ForEach(c => serializer.Converters.Add(c));
                            _lowerCaseJsonSerializer = serializer;
                        }
                    }
                }

                return _lowerCaseJsonSerializer;
            }
        }

        private static JsonSerializer _lowerCaseJsonSerializer;

        /// <summary>
        /// Original json serialize
        /// </summary>
        public static JsonSerializer OriginalCaseJsonSerializer
        {
            get
            {
                if (_originalCaseJsonSerializer == null)
                {
                    lock (Lock)
                    {
                        if (_originalCaseJsonSerializer == null)
                        {
                            var serializer = new JsonSerializer();
                            serializer.NullValueHandling = NullValueHandling.Ignore;
                            serializer.ContractResolver = new OriginalCasePropertyNamesContractResolver();
                            DefaultConverters.ForEach(c => serializer.Converters.Add(c));

                            _originalCaseJsonSerializer = serializer;
                        }
                    }
                }

                return _originalCaseJsonSerializer;
            }
        }

        private static JsonSerializer _originalCaseJsonSerializer;

        /// <summary>
        /// Deserialize json in recursive Dictionary[string,object]
        /// </summary>
        /// <param name="json"></param>
        public static object DeserializeAsObject(this string json)
        {
            return DeserializeAsObjectCore(JToken.Parse(json));
        }

        /// <summary>
        /// Cast jtoken to object
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        private static object DeserializeAsObjectCore(JToken token)
        {
            switch (token.Type)
            {
                case JTokenType.Object:
                    return token.Children<JProperty>()
                                .ToDictionary(prop => prop.Name,
                                              prop => DeserializeAsObjectCore(prop.Value));

                case JTokenType.Array:
                    return token.Select(DeserializeAsObjectCore).ToList();

                default:
                    return ((JValue)token).Value;
            }
        }

        /// <summary>
        /// Mask fields
        /// </summary>
        /// <param name="json"></param>
        /// <param name="blacklist"></param>
        /// <returns></returns>
        public static string MaskFields(string json, string[] blacklist)
        {
            return MaskFields(json, blacklist, "******");
        }

        /// <summary>
        /// Mask fields
        /// </summary>
        /// <param name="json"></param>
        /// <param name="blacklist"></param>
        /// <param name="mask"></param>
        /// <returns></returns>
        public static string MaskFields(string json, string[] blacklist, string mask)
        {
            return json.MaskFields(blacklist, mask);
        }

        /// <summary>
        /// returns original json if not null. if empty, returns a valid empty json "{}"
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static string GetJsonStringWhenEmpty(this string json)
        {
            if (string.IsNullOrWhiteSpace(json))
            {
                json = "{}";
            }

            return json;
        }

        /// <summary>
        /// alias to JsonConvert.DeserializeObject<T>(json);
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public static T Deserialize<T>(this string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

        /// <summary>
        /// alias to JsonConvert.SerializeObject(obj);
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string Serialize(this object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        /// <summary>
        /// Use json as flexible converter
        /// </summary>
        /// <param name="jsonSerializer"></param>
        /// <returns></returns>
        public static JsonSerializer UseFlexibleEnumJsonConverter(this JsonSerializer jsonSerializer)
        {
            jsonSerializer.Converters.Clear();
            jsonSerializer.Converters.Add(new FlexibleEnumJsonConverter());

            return jsonSerializer;
        }

        /// <summary>
        /// Use json as flexible converter
        /// </summary>
        /// <param name="jsonSerializerSettings"></param>
        /// <returns></returns>
        public static JsonSerializerSettings UseFlexibleEnumJsonConverter(this JsonSerializerSettings jsonSerializerSettings)
        {
            jsonSerializerSettings.Converters.Clear();
            jsonSerializerSettings.Converters.Add(new FlexibleEnumJsonConverter());

            return jsonSerializerSettings;
        }

        /// <summary>
        /// Try deserialize or ignore with cannot be possible
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T TryDeserialize<T>(this string value, JsonSerializerSettings jsonSerializerSettings = null)
        {
            if (string.IsNullOrWhiteSpace(value) == false)
            {
                if (jsonSerializerSettings == null)
                {
                    jsonSerializerSettings = JsonUtility.SnakeCaseJsonSerializerSettings;
                }

                try
                {
                    return JsonConvert.DeserializeObject<T>(value, jsonSerializerSettings);
                }
                catch (Exception)
                {
                    // Invalid data can be ignored
                }
            }

            return default(T);
        }

        /// <summary>
        /// Deserialize object as dic <string, object>
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static IDictionary<string, object> JsonStringToDictionary(this string json)
        {
            return JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
        }

        /// <summary>
        /// add a "container" to avoid array at json root
        /// </summary>
        /// <param name="json"></param>
        /// <param name="property"></param>
        /// <returns></returns>
        public static string AvoidArray(this string json, string property)
        {
            var isArray = json.Trim().Trim('\t').StartsWith("[");
            if (isArray)
            {
                json = "{ \"" + property + "\" : " + json + " }";
            }

            return json;
        }

        /// <summary>
        /// Rename a jproperty
        /// </summary>
        /// <param name="token"></param>
        /// <param name="newName"></param>
        public static void Rename(this JToken token, string newName)
        {
            if (token == null)
            {
                throw new ArgumentNullException(nameof(token));
            }

            JProperty property;

            if (token.Type == JTokenType.Property)
            {
                if (token.Parent == null)
                {
                    throw new InvalidOperationException("Cannot rename a property with no parent");
                }

                property = (JProperty)token;
            }
            else
            {
                if (token.Parent == null || token.Parent.Type != JTokenType.Property)
                {
                    throw new InvalidOperationException("This token's parent is not a JProperty; cannot rename");
                }

                property = (JProperty)token.Parent;
            }

            var existingValue = property.Value;
            property.Value = null;
            var newProperty = new JProperty(newName, existingValue);
            property.Replace(newProperty);
        }

        /// <summary>
        /// Remove empty childrens from jtoken
        /// </summary>
        /// <param name="jtoken"></param>
        /// <returns></returns>
        public static JToken RemoveEmptyChildren(this JToken jtoken)
        {
            if (jtoken == null)
            {
                return jtoken;
            }

            switch (jtoken.Type)
            {
                case JTokenType.Object:
                    return jtoken.RemoveEmptyChildrenFromObject();
                case JTokenType.Array:
                    return jtoken.RemoveEmptyChildrenFromArray();
                default:
                    return jtoken;
            }
        }

        /// <summary>
        /// Remove empty childrens from jtoken (when array)
        /// </summary>
        /// <param name="jtoken"></param>
        /// <returns></returns>
        private static JToken RemoveEmptyChildrenFromArray(this JToken jtoken)
        {
            JArray copy = new JArray();
            foreach (JToken item in jtoken.Children())
            {
                var child = item;
                if (child.HasValues)
                {
                    child = child.RemoveEmptyChildren();
                }
                if (!child.IsEmpty())
                {
                    copy.Add(child);
                }
            }
            return copy;
        }

        /// <summary>
        /// Remove empty childrens from jtoken (when object)
        /// </summary>
        /// <param name="jtoken"></param>
        /// <returns></returns>
        private static JToken RemoveEmptyChildrenFromObject(this JToken jtoken)
        {
            var copy = new JObject();
            foreach (JProperty prop in jtoken.Children<JProperty>())
            {
                var child = prop.Value;
                if (child.HasValues)
                {
                    child = child.RemoveEmptyChildren();
                }
                if (!child.IsEmpty())
                {
                    copy.Add(prop.Name, child);
                }
            }
            return copy;
        }

        /// <summary>
        /// Check if jtoken is empty
        /// </summary>
        /// <param name="jtoken"></param>
        /// <returns></returns>
        private static bool IsEmpty(this JToken jtoken)
        {
            return (jtoken.Type == JTokenType.Array && !jtoken.HasValues) ||
                   (jtoken.Type == JTokenType.Object && !jtoken.HasValues) ||
                   (jtoken.Type == JTokenType.String && jtoken.ToString() == String.Empty) ||
                   (jtoken.Type == JTokenType.Null);
        }

        /// <summary>
        /// Rename all property found in jtoken and childrens
        /// </summary>
        /// <param name="jtoken"></param>
        /// <param name="oldPropertyName"></param>
        /// <param name="newPropertyName"></param>
        /// <param name="handleValue"></param>
        /// <returns></returns>
        public static JToken RenameProperty(this JToken jtoken, string oldPropertyName, string newPropertyName, Func<JToken, JToken> handleValue)
        {
            if (string.IsNullOrWhiteSpace(oldPropertyName) == true)
            {
                throw new ArgumentNullException(nameof(oldPropertyName));
            }

            if (string.IsNullOrWhiteSpace(newPropertyName) == true)
            {
                throw new ArgumentNullException(nameof(newPropertyName));
            }

            if (jtoken == null)
            {
                return jtoken;
            }

            RenamePropertyFromJToken(jtoken, oldPropertyName, newPropertyName, handleValue);

            return jtoken;
        }

        /// <summary>
        /// Rename all property found in jtoken and childrens (core / recursive method)
        /// </summary>
        /// <param name="jtoken"></param>
        /// <param name="oldPropertyName"></param>
        /// <param name="newPropertyName"></param>
        /// <param name="handleValue"></param>
        /// <returns></returns>
        private static void RenamePropertyFromJToken(JToken token, string oldPropertyName, string newPropertyName, Func<JToken, JToken> handleValue)
        {
            JContainer container = token as JContainer;
            if (container == null)
            {
                return; // abort recursive
            }

            List<JToken> removeList = new List<JToken>();
            foreach (JToken jtoken in container.Children())
            {
                if (jtoken is JProperty prop)
                {
                    var matching = Regex.IsMatch(prop.Path, $"*.{oldPropertyName}".ToWildcardToRegular(), RegexOptions.IgnoreCase | RegexOptions.CultureInvariant) ||
                                    string.Equals(prop.Path, oldPropertyName, StringComparison.InvariantCultureIgnoreCase);

                    if (matching)
                    {
                        removeList.Add(jtoken);
                    }
                }

                // call recursive 
                RenamePropertyFromJToken(jtoken, oldPropertyName, newPropertyName, handleValue);
            }

            // replace 
            for (int i = 0; i < removeList.Count; i++)
            {
                var prop = (JProperty)removeList[i];

                if (handleValue != null)
                {
                    prop.Value = (string)handleValue.Invoke(prop.Value);
                }

                prop.Rename(newPropertyName);
            }
        }

        /// <summary>
        /// Cast wildcard (*) to regex
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private static string ToWildcardToRegular(this string value)
        {
            return "^" + Regex.Escape(value).Replace("\\*", ".*") + "$";
        }
    }

    /// <summary>
    /// Resolve property names to lowercase only
    /// </summary>
    public class LowerCaseNamingResolver : NamingStrategy
    {
        public LowerCaseNamingResolver()
        {
            this.ProcessDictionaryKeys = true;
            this.OverrideSpecifiedNames = true;
        }

        protected override string ResolvePropertyName(string name)
        {
            return name.ToLowerInvariant();
        }
    }

    /// <summary>
    /// Lowercase contract resolver
    /// </summary>
    public class LowerCasePropertyNamesContractResolver : DefaultContractResolver
    {
        public LowerCasePropertyNamesContractResolver()
        {
            this.NamingStrategy = new LowerCaseNamingResolver
            {
                ProcessDictionaryKeys = true,
                OverrideSpecifiedNames = true
            };
        }
    }

    /// <summary>
    /// Resolve property names original name
    /// </summary>
    public class OriginalCaseNamingResolver : NamingStrategy
    {
        public OriginalCaseNamingResolver()
        {
            this.ProcessDictionaryKeys = true;
            this.OverrideSpecifiedNames = true;
        }

        protected override string ResolvePropertyName(string name)
        {
            return name;
        }
    }

    /// <summary>
    /// Original contract resolver
    /// </summary>
    public class OriginalCasePropertyNamesContractResolver : DefaultContractResolver
    {
        public OriginalCasePropertyNamesContractResolver()
        {
            this.NamingStrategy = new OriginalCaseNamingResolver
            {
                ProcessDictionaryKeys = true,
                OverrideSpecifiedNames = true
            };
        }
    }

    /// <summary>
    /// Snake case contract resolver
    /// </summary>
    public class SnakeCasePropertyNamesContractResolver : DefaultContractResolver
    {
        public SnakeCasePropertyNamesContractResolver()
        {
            this.NamingStrategy = new SnakeCaseNamingStrategy
            {
                ProcessDictionaryKeys = true,
                OverrideSpecifiedNames = true
            };
        }
    }

    /// <summary>
    /// Camel case contract resolver
    /// </summary>
    public class CustomCamelCasePropertyNamesContractResolver : DefaultContractResolver
    {
        public CustomCamelCasePropertyNamesContractResolver()
        {
            this.NamingStrategy = new CamelCaseNamingStrategy
            {
                ProcessDictionaryKeys = true,
                OverrideSpecifiedNames = true
            };
        }
    }
}
