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

            date = TimeZoneInfo.ConvertTimeToUtc(date, GetCurrentTimeZoneInfo());

            return date;
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

    public class DateConverter : JsonConverter
    {
        public static string DefaultFormat = "yyyy-MM-dd";

        public string Format { get; set; }

        public DateConverter() { }

        public DateConverter(string format)
        {
            this.Format = format;
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

            return date.Date;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            DateTime? convertedDate = null;

            if (value != null)
            {
                convertedDate = DateTime.Parse(value.ToString());
            }

            writer.WriteValue(convertedDate?.ToString(this.Format ?? DefaultFormat));
        }
    }
}
