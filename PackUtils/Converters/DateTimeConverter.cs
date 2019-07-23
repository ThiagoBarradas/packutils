using Microsoft.AspNetCore.Http;
using Nancy;
using Newtonsoft.Json;
using System;
using System.Linq;

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

            date = TimeZoneInfo.ConvertTimeToUtc(date, GetCurrentTimeZoneInfoInvokingFunction());

            return date;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            DateTime? convertedDate = null;

            if (value != null)
            {
                var originalDate = DateTime.Parse(value.ToString());

                convertedDate = TimeZoneInfo.ConvertTimeFromUtc(originalDate, this.GetCurrentTimeZoneInfoInvokingFunction());
            }

            writer.WriteValue(convertedDate);
        }

        public static TimeZoneInfo GetTimeZoneByAspNetHeader(IHttpContextAccessor httpContextAccessor, string headerName)
        {
            var httpContext = httpContextAccessor.HttpContext;

            try
            {
                var timezone = httpContext.Request.Headers[headerName];
                return TimeZoneInfo.FindSystemTimeZoneById(timezone);
            }
            catch (Exception)
            {
                return DefaultTimeZone;
            }
        }

        public static TimeZoneInfo GetTimeZoneByNancyHeader(NancyContext nancyContext, string headerName)
        {
            try
            {
                var timezone = nancyContext.Request.Headers[headerName].FirstOrDefault();
                return TimeZoneInfo.FindSystemTimeZoneById(timezone);
            }
            catch (Exception)
            {
                return DefaultTimeZone;
            }
        }

        private TimeZoneInfo GetCurrentTimeZoneInfoInvokingFunction()
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
