﻿using JsonMasking;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using PackUtils.Converters;
using System;
using System.Linq;

namespace PackUtils
{
    /// <summary>
    /// Json utility (with newtonsoft)
    /// </summary>
    public static class JsonUtility
    {
        private readonly static object Lock = new object();

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
                            settings.Converters.Add(new StringEnumConverter());
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

                            settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                            settings.Converters.Add(new StringEnumConverter());
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
                            settings.Converters.Add(new StringEnumConverter());
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
                            settings.Converters.Add(new StringEnumConverter());
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
                var serializer = new JsonSerializer();
                serializer.NullValueHandling = NullValueHandling.Ignore;
                serializer.ContractResolver = new CamelCasePropertyNamesContractResolver();
                serializer.Converters.Add(new StringEnumConverter());

                return serializer;
            }
        }

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
                            serializer.Converters.Add(new StringEnumConverter());

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
                            serializer.Converters.Add(new StringEnumConverter());

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
                            serializer.Converters.Add(new StringEnumConverter());

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
    }

    /// <summary>
    /// Resolve property names to lowercase only
    /// </summary>
    public class LowerCaseNamingResolver : NamingStrategy
    {
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
            this.NamingStrategy = new LowerCaseNamingResolver();
        }
    }

    /// <summary>
    /// Resolve property names original name
    /// </summary>
    public class OriginalCaseNamingResolver : NamingStrategy
    {
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
            this.NamingStrategy = new OriginalCaseNamingResolver();
        }
    }

    /// <summary>
    /// Snake case contract resolver
    /// </summary>
    public class SnakeCasePropertyNamesContractResolver : DefaultContractResolver
    {
        public SnakeCasePropertyNamesContractResolver()
        {
            this.NamingStrategy = new SnakeCaseNamingStrategy();
        }
    }
}
