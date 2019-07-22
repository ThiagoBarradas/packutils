using Newtonsoft.Json;
using System;

namespace PackUtils.Converters
{
    /// <summary>
    ///  DateTime and NullableDataTime converter for newtonsoft
    ///  Considering time zone and only date format
    /// </summary>
    public class DateTimeConverter : JsonConverter
    {
        public static TimeZoneInfo DefaultTimeZone = TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time");

        public Func<TimeZoneInfo> GetTimeZoneInfo { get; set; }

        public DateTimeConverter() { }

        public DateTimeConverter(Func<TimeZoneInfo> getTimeZoneInfo)
        {
            this.GetTimeZoneInfo = getTimeZoneInfo;
        }

        public override bool CanConvert(Type objectType)
        {
            return
                (typeof(DateTime).IsAssignableFrom(objectType)) ||
                (typeof(DateTime?).IsAssignableFrom(objectType));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (string.IsNullOrWhiteSpace(reader.Value?.ToString()))
            {
                return null;
            }

            var date = DateTime.Parse(reader.Value.ToString());

            var utcDate = TimeZoneInfo.ConvertTimeToUtc(date, GetCurrentTimeZoneInfo());

            return utcDate;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            DateTime? convertedDate = null;

            if (value != null)
            {
                var originalDate = DateTime.Parse(value.ToString());
                convertedDate = TimeZoneInfo.ConvertTimeFromUtc(originalDate, this.GetCurrentTimeZoneInfo());
            }

            writer.WriteValue(convertedDate);
        }

        private TimeZoneInfo GetCurrentTimeZoneInfo()
        {
            return this.GetTimeZoneInfo?.Invoke() ?? DefaultTimeZone;
        }
    }
}
