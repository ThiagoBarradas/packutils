using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;
using System.Linq;

namespace PackUtils
{
    /// <summary>
    /// Json utility (with newtonsoft)
    /// </summary>
    public static class JsonUtility
    {
        /// <summary>
        /// Snake case json serializer settings
        /// </summary>
        public static JsonSerializerSettings SnakeCaseJsonSerializerSettings
        {
            get
            {
                var settings = new JsonSerializerSettings();

                settings.ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new SnakeCaseNamingStrategy()
                };
                settings.Converters.Add(new StringEnumConverter());
                settings.NullValueHandling = NullValueHandling.Ignore;

                return settings;
            }
        }

        /// <summary>
        /// Camel case json serializer settings
        /// </summary>
        public static JsonSerializerSettings CamelCaseJsonSerializerSettings
        {
            get
            {
                var settings = new JsonSerializerSettings();

                settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                settings.Converters.Add(new StringEnumConverter());
                settings.NullValueHandling = NullValueHandling.Ignore;

                return settings;
            }
        }

        /// <summary>
        /// Lower case json serializer settings
        /// </summary>
        public static JsonSerializerSettings LowerCaseJsonSerializerSettings
        {
            get
            {
                var settings = new JsonSerializerSettings();

                settings.ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new LowerCaseNamingResolver()
                };
                settings.Converters.Add(new StringEnumConverter());
                settings.NullValueHandling = NullValueHandling.Ignore;

                return settings;
            }
        }

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
                var serializer = new JsonSerializer();
                serializer.NullValueHandling = NullValueHandling.Ignore;
                serializer.ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new SnakeCaseNamingStrategy()
                };
                serializer.Converters.Add(new StringEnumConverter());

                return serializer;
            }
        }

        /// <summary>
        /// Lower case json serialize
        /// </summary>
        public static JsonSerializer LowerCaseJsonSerializer
        {
            get
            {
                var serializer = new JsonSerializer();
                serializer.NullValueHandling = NullValueHandling.Ignore;
                serializer.ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new LowerCaseNamingResolver()
                };
                serializer.Converters.Add(new StringEnumConverter());

                return serializer;
            }
        }

        /// <summary>
        /// Mask fields
        /// </summary>
        /// <param name="token">JToken</param>
        /// <param name="blackList">JToken</param>
        public static void MaskFields(JToken token, List<string> blackList)
        {
            MaskFields(token, blackList, "******");
        }

        /// <summary>
        /// Mask fields
        /// </summary>
        /// <param name="token">JToken</param>
        /// <param name="blackList">JToken</param>
        /// <param name="mask">mask</param>
        public static void MaskFields(JToken token, List<string> blackList, string mask)
        {
            JContainer container = token as JContainer;
            if (container == null)
            {
                return;
            }

            List<JToken> removeList = new List<JToken>();
            foreach (JToken el in container.Children())
            {
                var prop = el as JProperty;

                if (prop != null && blackList.Any(f => f.Equals(prop.Name, System.StringComparison.CurrentCultureIgnoreCase)))
                {
                    removeList.Add(el);
                }
                MaskFields(el, blackList, mask);
            }

            foreach (JToken el in removeList)
            {
                var prop = (JProperty)el;
                prop.Value = mask;
            }
        }
    }

    /// <summary>
    /// Resolve property names to lowercase only
    /// </summary>
    class LowerCaseNamingResolver : NamingStrategy
    {
        protected override string ResolvePropertyName(string name)
        {
            return name.ToLowerInvariant();
        }
    }
}
