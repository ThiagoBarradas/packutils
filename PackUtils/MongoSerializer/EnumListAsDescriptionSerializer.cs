using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using System;
using System.Collections.Generic;

namespace PackUtils.MongoSerializer
{
    public class EnumListAsDescriptionSerializer<TEnum> : IBsonArraySerializer where TEnum : Enum
    {
        public Type ValueType => typeof(List<TEnum>);

        public object Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            var type = context.Reader.GetCurrentBsonType();
            var deserializedEnumList = new List<TEnum>();

            switch (type)
            {
                case BsonType.Array:

                    context.Reader.ReadStartArray();

                    while (context.Reader.ReadBsonType() != BsonType.EndOfDocument)
                    {
                        string enumDescritpion = context.Reader.ReadString();
                        deserializedEnumList.Add(EnumUtility.GetEnumFromDescription<TEnum>(enumDescritpion));
                    }

                    context.Reader.ReadEndArray();

                    return deserializedEnumList;

                default:
                    throw new NotImplementedException($"No implementation to deserialize {type}");
            }
        }

        public void Serialize(BsonSerializationContext context, BsonSerializationArgs args, object value)
        {
            var enumList = (List<TEnum>)value;

            if (value != null)
            {
                context.Writer.WriteStartArray();

                foreach (TEnum enumElement in enumList)
                {
                    context.Writer.WriteString(EnumUtility.GetDescriptionFromEnum(enumElement));
                }

                context.Writer.WriteEndArray();
            }
        }

        public bool TryGetItemSerializationInfo(out BsonSerializationInfo serializationInfo)
        {
            string elementName = null;
            var serializer = BsonSerializer.LookupSerializer(typeof(List<TEnum>));
            var nominalType = typeof(TEnum);
            serializationInfo = new BsonSerializationInfo(elementName, serializer, nominalType);
            return true;
        }
    }
}
