using Newtonsoft.Json;
using RestSharp;
using RestSharp.Serializers;
using System;
using System.IO;

namespace PackUtils
{
    public class NewtonsoftRestsharpJsonSerializer : IRestSerializer, ISerializer, IDeserializer
    {
        public ISerializer Serializer => this;
        public IDeserializer Deserializer => this;
        
        public DataFormat DataFormat { get; } = DataFormat.Json;
        public SupportsContentType SupportsContentType => contentType => contentType.Value.EndsWith("json", StringComparison.InvariantCultureIgnoreCase);
        public ContentType ContentType { get; set; } = ContentType.Json;
        public string[] AcceptedContentTypes => ContentType.JsonAccept;

        private JsonSerializer NewtonsoftSerializer { get; set; }

        public NewtonsoftRestsharpJsonSerializer(JsonSerializer serializer)
        {
            this.NewtonsoftSerializer = serializer;
        }

        public string Serialize(object obj)
        {
            if (obj == null) return null;

            using var stringWriter = new StringWriter();
            using var jsonTextWriter = new JsonTextWriter(stringWriter);
            NewtonsoftSerializer.Serialize(jsonTextWriter, obj);
            return stringWriter.ToString();
        }

        public string Serialize(Parameter bodyParameter) 
            => Serialize(bodyParameter.Value);

        public T Deserialize<T>(RestResponse response)
        {
            var content = response.Content;
            
            if (content == null)
                throw new DeserializationException(response, new InvalidOperationException("Response content is null"));

            using var stringReader = new StringReader(content);
            using var jsonTextReader = new JsonTextReader(stringReader);
            return NewtonsoftSerializer.Deserialize<T>(jsonTextReader);
        }

    }
}
