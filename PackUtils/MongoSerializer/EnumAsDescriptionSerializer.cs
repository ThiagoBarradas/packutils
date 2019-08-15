using MongoDB.Bson.Serialization;
using System;

namespace PackUtils.MongoSerializer
{
    public class EnumAsDescriptionSerializer<TEnum> : IBsonSerializer
    {
        public Type ValueType => typeof(TEnum);

        public void Serialize(BsonSerializationContext context, BsonSerializationArgs args, object value)
        {
            var valueEnum = (Enum)Enum.Parse(typeof(TEnum), value.ToString());
            context.Writer.WriteString(EnumUtility.GetDescriptionFromEnum(valueEnum));
        }

        object IBsonSerializer.Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            var value = context.Reader.ReadString();
            return EnumUtility.GetEnumFromDescription<TEnum>(value);
        }

    }
