using Newtonsoft.Json;
using PackUtils.Converters;
using System;
using System.Diagnostics;
using System.Linq;
using Xunit;

namespace PackUtils.Test.Converters
{
    public class DateTimeConverterTests
    {
        [Theory]
        [InlineData("E. South America Standard Time", "2019-07-22T22:00:00", "2019-07-22T23:00:00", "2019-07-23T01:00:00.000Z", "2019-07-23T02:00:00.000Z")] // -03 with default
        [InlineData("E. South America Standard Time", "2019-07-22T00:00:00", "2019-07-22T01:00:00", "2019-07-22T03:00:00.000Z", "2019-07-22T04:00:00.000Z")] // -03 with default
        [InlineData("Pacific SA Standard Time","2019-07-22T13:00:00", null, "2019-07-22T17:00:00.000Z", null)] // -04
        [InlineData("Pacific SA Standard Time", "2019-07-22T13:00:00", "2019-07-22T12:00:00", "2019-07-22T17:00:00.000Z", "2019-07-22T16:00:00.000Z")] // -04
        [InlineData(null, "2019-07-22T13:00:00", null, "2019-07-22T16:00:00.000Z", null)] // -03 with default
        [InlineData(null, "2019-07-22T13:00:00", "2019-07-22T12:00:00", "2019-07-22T16:00:00.000Z", "2019-07-22T15:00:00.000Z")] // -03 with default
        [InlineData(null, "2019-01-22T13:00:00", null, "2019-01-22T15:00:00.000Z", null)] // -02 with default (daylight saving)

        public static void DateTimeConverter_Should_Deserialize_And_Serialize_Considering_TimeZone(
            string timeZone,
            string dateTime, 
            string nullableDateTime, 
            string expectedDateTime,
            string expectedNullableDateTime)
        {
            // arrange
            var converter = (string.IsNullOrWhiteSpace(timeZone))
                ? new DateTimeConverter()
                : new DateTimeConverter(() => TimeZoneInfo.FindSystemTimeZoneById(timeZone));

            var originalObject = new DateTimeClassTest
            {
                MyDateTime = DateTime.Parse(dateTime),
                MyNullableDateTime = (string.IsNullOrWhiteSpace(nullableDateTime)
                    ? (DateTime?)null
                    : DateTime.Parse(nullableDateTime)),
            };

            string originalJson = JsonConvert.SerializeObject(originalObject);

            // act 
            var deserializeResult = JsonConvert.DeserializeObject<DateTimeClassTest>(originalJson, converter);
            var deserializeResultString = JsonConvert.SerializeObject(deserializeResult, Formatting.Indented);

            var serializeResultString = JsonConvert.SerializeObject(deserializeResult, Formatting.Indented, converter);
            var serializeResult = JsonConvert.DeserializeObject<DateTimeClassTest>(serializeResultString);

            // assert 
            Assert.NotNull(deserializeResult);
            Assert.NotNull(serializeResultString);
            Assert.NotNull(serializeResult);
            
            var deserializeResultDateTime = deserializeResult.MyDateTime.ToString("yyyy-MM-ddTHH:mm:ss.fffK");
            var serializeResultDateTime = serializeResult.MyDateTime.ToString("yyyy-MM-ddTHH:mm:ss");
            var deserializeResultNullableDateTime =
                deserializeResult.MyNullableDateTime?.ToString("yyyy-MM-ddTHH:mm:ss.fffK");
            var serializeResultNullableDateTime =
                serializeResult.MyNullableDateTime?.ToString("yyyy-MM-ddTHH:mm:ss");

            Assert.Equal(expectedDateTime, deserializeResultDateTime);
            Assert.Equal(dateTime, serializeResultDateTime);
            Assert.Equal(expectedNullableDateTime, deserializeResultNullableDateTime);
            Assert.Equal(nullableDateTime, serializeResultNullableDateTime);
        }

        [Theory]
        [InlineData("E. South America Standard Time", "2019-07-22T22:00:00", "2019-07-22T23:00:00", "2019-07-23T01:00:00.000Z", "2019-07-23T02:00:00.000Z")] // -03 with default
        [InlineData("E. South America Standard Time", "2019-07-22T00:00:00", "2019-07-22T01:00:00", "2019-07-22T03:00:00.000Z", "2019-07-22T04:00:00.000Z")] // -03 with default
        [InlineData("Pacific SA Standard Time", "2019-07-22T13:00:00", null, "2019-07-22T17:00:00.000Z", null)] // -04
        [InlineData("Pacific SA Standard Time", "2019-07-22T13:00:00", "2019-07-22T12:00:00", "2019-07-22T17:00:00.000Z", "2019-07-22T16:00:00.000Z")] // -04
        [InlineData(null, "2019-07-22T13:00:00", null, "2019-07-22T16:00:00.000Z", null)] // -03 with default
        [InlineData(null, "2019-07-22T13:00:00", "2019-07-22T12:00:00", "2019-07-22T16:00:00.000Z", "2019-07-22T15:00:00.000Z")] // -03 with default
        [InlineData(null, "2019-01-22T13:00:00", null, "2019-01-22T15:00:00.000Z", null)] // -02 with default (daylight saving)
        public static void DateTimeConverter_Should_Serialize_Considering_TimeZone(
        string timeZone,
        string dateTime,
        string nullableDateTime,
        string expectedDateTime,
        string expectedNullableDateTime)
        {
            // arrange
            var converter = (string.IsNullOrWhiteSpace(timeZone))
                ? new DateTimeConverter()
                : new DateTimeConverter(() => TimeZoneInfo.FindSystemTimeZoneById(timeZone));

            var originalObject = new DateTimeClassTest
            {
                MyDateTime = DateTime.Parse(dateTime),
                MyNullableDateTime = (string.IsNullOrWhiteSpace(nullableDateTime)
                    ? (DateTime?)null
                    : DateTime.Parse(nullableDateTime)),
            };
            originalObject.OnlyDate = originalObject.MyDateTime;
            originalObject.OnlyDateNullable = originalObject.MyNullableDateTime;

            string originalJson = JsonConvert.SerializeObject(originalObject);

            // act 
            var deserializeResult = JsonConvert.DeserializeObject<DateTimeClassTest>(originalJson, converter);
            var deserializeResultString = JsonConvert.SerializeObject(deserializeResult, Formatting.Indented);

            var serializeResultString = JsonConvert.SerializeObject(deserializeResult, Formatting.Indented, converter);
            var serializeResult = JsonConvert.DeserializeObject<DateTimeClassTest>(serializeResultString);

            Debug.WriteLine("Deserialize: ");
            Debug.WriteLine(deserializeResultString);

            Debug.WriteLine("Serialize: ");
            Debug.WriteLine(serializeResultString);

            // assert 
            Assert.NotNull(deserializeResult);
            Assert.NotNull(serializeResultString);
            Assert.NotNull(serializeResult);

            var deserializeResultDateTime = deserializeResult.MyDateTime.ToString("yyyy-MM-ddTHH:mm:ss.fffK");
            var serializeResultDateTime = serializeResult.MyDateTime.ToString("yyyy-MM-ddTHH:mm:ss");

            var deserializeResultOnlyDate = deserializeResult.OnlyDate.ToString("yyyy-MM-dd");
            var serializeResultOnlyDate = serializeResult.OnlyDate.ToString("yyyy-MM-dd");

            Assert.Equal(expectedDateTime, deserializeResultDateTime);
            Assert.Equal(dateTime, serializeResultDateTime);

            Assert.Equal(dateTime.Split("T").First(), deserializeResultOnlyDate);
            Assert.Equal(deserializeResult.OnlyDate.ToString("yyyy-MM-dd"), serializeResultOnlyDate);

            if (string.IsNullOrWhiteSpace(nullableDateTime))
            {
                Assert.Null(deserializeResult.MyNullableDateTime);
                Assert.Null(serializeResult.MyNullableDateTime);
                Assert.Null(deserializeResult.OnlyDateNullable);
                Assert.Null(serializeResult.OnlyDateNullable);
            }
            else
            {
                var deserializeResultNullableDateTime =
                    deserializeResult.MyNullableDateTime.Value.ToString("yyyy-MM-ddTHH:mm:ss.fffK");
                var serializeResultNullableDateTime =
                    serializeResult.MyNullableDateTime.Value.ToString("yyyy-MM-ddTHH:mm:ss");

                var deserializeResultOnlyDateNullable = deserializeResult.OnlyDateNullable.Value.ToString("yyyy-MM-dd");
                var serializeResultOnlyDateNullable = serializeResult.OnlyDateNullable.Value.ToString("yyyy-MM-dd");

                Assert.Equal(expectedNullableDateTime, deserializeResultNullableDateTime);
                Assert.Equal(nullableDateTime, serializeResultNullableDateTime);

                Assert.Equal(nullableDateTime.Split("T").First(), deserializeResultOnlyDateNullable);
                Assert.Equal(deserializeResult.OnlyDateNullable.Value.ToString("yyyy-MM-dd"), serializeResultOnlyDateNullable);
            }
        }

        [Theory]
        [InlineData("E. South America Standard Time", "2019-07-22T22:00:00", "2019-07-22T00:00:00")]
        [InlineData("E. South America Standard Time", "2019-07-22T23:59:59", "2019-07-22T00:00:00")]
        [InlineData("E. South America Standard Time", "2019-07-22T18:00:00", "2019-07-22T00:00:00")]
        [InlineData("E. South America Standard Time", "2019-07-23T00:00:00", "2019-07-23T00:00:00")]
        [InlineData("E. South America Standard Time", "2019-07-23T01:00:00", "2019-07-23T00:00:00")]
        [InlineData("E. South America Standard Time", "2019-07-23T02:00:00", "2019-07-23T00:00:00")]
        [InlineData("E. South America Standard Time", "2019-07-23T03:00:00", "2019-07-23T00:00:00")]
        [InlineData("Pacific SA Standard Time", "2019-07-22T23:59:59", "2019-07-22T00:00:00")]
        [InlineData("Pacific SA Standard Time", "2019-07-23T01:00:00", "2019-07-23T00:00:00")]
        [InlineData(null, "2019-07-22T23:59:59", "2019-07-22T00:00:00")]
        [InlineData(null, "2019-07-23T01:00:00", "2019-07-23T00:00:00")]
        public static void DateConverter_Should_Deserialize_DateTime_Without_Timezone(
            string timeZone,
            string dateTime,
            string expectedDateTime)
        {
            // arrange
            var converter = (string.IsNullOrWhiteSpace(timeZone))
                ? new DateTimeConverter()
                : new DateTimeConverter(() => TimeZoneInfo.FindSystemTimeZoneById(timeZone));

            var originalObject = new DateTimeClassTest
            {
                OnlyDate = DateTime.Parse(dateTime)
            };

            string originalJson = JsonConvert.SerializeObject(originalObject);

            // act 
            var deserializeResult = JsonConvert.DeserializeObject<DateTimeClassTest>(originalJson, converter);
            var deserializeResultOnlyDate = deserializeResult.OnlyDate.ToString("yyyy-MM-ddTHH:mm:ss");

            // assert 
            Assert.NotNull(deserializeResult);
            Assert.Equal(expectedDateTime, deserializeResultOnlyDate);
        }

        [Theory]
        [InlineData("E. South America Standard Time", "2019-07-22T22:00:00", "2019-07-22T00:00:00")]
        [InlineData("E. South America Standard Time", "2019-07-22T23:59:59", "2019-07-22T00:00:00")]
        [InlineData("E. South America Standard Time", "2019-07-22T18:00:00", "2019-07-22T00:00:00")]
        [InlineData("E. South America Standard Time", "2019-07-23T00:00:00", "2019-07-23T00:00:00")]
        [InlineData("E. South America Standard Time", "2019-07-23T01:00:00", "2019-07-23T00:00:00")]
        [InlineData("E. South America Standard Time", "2019-07-23T02:00:00", "2019-07-23T00:00:00")]
        [InlineData("E. South America Standard Time", "2019-07-23T03:00:00", "2019-07-23T00:00:00")]
        [InlineData("Pacific SA Standard Time", "2019-07-22T23:59:59", "2019-07-22T00:00:00")]
        [InlineData("Pacific SA Standard Time", "2019-07-23T01:00:00", "2019-07-23T00:00:00")]
        [InlineData(null, "2019-07-22T23:59:59", "2019-07-22T00:00:00")]
        [InlineData(null, "2019-07-23T01:00:00", "2019-07-23T00:00:00")]
        [InlineData("E. South America Standard Time", null, null)]
        [InlineData("Pacific SA Standard Time", null, null)]
        [InlineData(null, null, null)]
        public static void DateConverter_Should_Deserialize_DateTimeNullable_Without_Timezone(
            string timeZone,
            string dateTime,
            string expectedDateTime)
        {
            // arrange
            var converter = (string.IsNullOrWhiteSpace(timeZone))
                ? new DateTimeConverter()
                : new DateTimeConverter(() => TimeZoneInfo.FindSystemTimeZoneById(timeZone));

            var originalObject = new DateTimeClassTest
            {
                OnlyDateNullable = (dateTime == null) ? (DateTime?) null : DateTime.Parse(dateTime)
            };

            string originalJson = JsonConvert.SerializeObject(originalObject);

            // act 
            var deserializeResult = JsonConvert.DeserializeObject<DateTimeClassTest>(originalJson, converter);
            var deserializeResultOnlyDateNullable = deserializeResult.OnlyDateNullable?.ToString("yyyy-MM-ddTHH:mm:ss");

            // assert 
            Assert.NotNull(deserializeResult);
            Assert.Equal(expectedDateTime, deserializeResultOnlyDateNullable);
        }

        [Theory]
        [InlineData("E. South America Standard Time", "2019-07-22T22:00:00", "2019-07-22T00:00:00")]
        [InlineData("E. South America Standard Time", "2019-07-22T23:59:59", "2019-07-22T00:00:00")]
        [InlineData("E. South America Standard Time", "2019-07-22T18:00:00", "2019-07-22T00:00:00")]
        [InlineData("E. South America Standard Time", "2019-07-23T00:00:00", "2019-07-23T00:00:00")]
        [InlineData("E. South America Standard Time", "2019-07-23T01:00:00", "2019-07-23T00:00:00")]
        [InlineData("E. South America Standard Time", "2019-07-23T02:00:00", "2019-07-23T00:00:00")]
        [InlineData("E. South America Standard Time", "2019-07-23T03:00:00", "2019-07-23T00:00:00")]
        [InlineData("Pacific SA Standard Time", "2019-07-22T23:59:59", "2019-07-22T00:00:00")]
        [InlineData("Pacific SA Standard Time", "2019-07-23T01:00:00", "2019-07-23T00:00:00")]
        [InlineData(null, "2019-07-22T23:59:59", "2019-07-22T00:00:00")]
        [InlineData(null, "2019-07-23T01:00:00", "2019-07-23T00:00:00")]
        public static void DateConverter_Should_Serialize_DateTime_Without_Timezone(
            string timeZone,
            string dateTime,
            string expectedDateTime)
        {
            // arrange
            var converter = (string.IsNullOrWhiteSpace(timeZone))
                ? new DateTimeConverter()
                : new DateTimeConverter(() => TimeZoneInfo.FindSystemTimeZoneById(timeZone));

            var originalObject = new DateTimeClassTest
            {
                OnlyDate = DateTime.Parse(dateTime)
            };

            // act 
            string serializeJson = JsonConvert.SerializeObject(originalObject, converter);
            var serializeObject = JsonConvert.DeserializeObject<DateTimeClassTest>(serializeJson);
            var serializeObjectOnlyDate = serializeObject.OnlyDate.ToString("yyyy-MM-ddTHH:mm:ss");

            // assert 
            Assert.NotNull(serializeObject);
            Assert.Equal(expectedDateTime, serializeObjectOnlyDate);
        }

        [Theory]
        [InlineData("E. South America Standard Time", "2019-07-22T22:00:00", "2019-07-22T00:00:00")]
        [InlineData("E. South America Standard Time", "2019-07-22T23:59:59", "2019-07-22T00:00:00")]
        [InlineData("E. South America Standard Time", "2019-07-22T18:00:00", "2019-07-22T00:00:00")]
        [InlineData("E. South America Standard Time", "2019-07-23T00:00:00", "2019-07-23T00:00:00")]
        [InlineData("E. South America Standard Time", "2019-07-23T01:00:00", "2019-07-23T00:00:00")]
        [InlineData("E. South America Standard Time", "2019-07-23T02:00:00", "2019-07-23T00:00:00")]
        [InlineData("E. South America Standard Time", "2019-07-23T03:00:00", "2019-07-23T00:00:00")]
        [InlineData("Pacific SA Standard Time", "2019-07-22T23:59:59", "2019-07-22T00:00:00")]
        [InlineData("Pacific SA Standard Time", "2019-07-23T01:00:00", "2019-07-23T00:00:00")]
        [InlineData(null, "2019-07-22T23:59:59", "2019-07-22T00:00:00")]
        [InlineData(null, "2019-07-23T01:00:00", "2019-07-23T00:00:00")]
        [InlineData("E. South America Standard Time", null, null)]
        [InlineData("Pacific SA Standard Time", null, null)]
        [InlineData(null, null, null)]
        public static void DateConverter_Should_Serialize_DateTimeNullable_Without_Timezone(
            string timeZone,
            string dateTime,
            string expectedDateTime)
        {
            // arrange
            var converter = (string.IsNullOrWhiteSpace(timeZone))
                ? new DateTimeConverter()
                : new DateTimeConverter(() => TimeZoneInfo.FindSystemTimeZoneById(timeZone));

            var originalObject = new DateTimeClassTest
            {
                OnlyDateNullable = (dateTime == null) ? (DateTime?)null : DateTime.Parse(dateTime)
            };

            // act 
            string serializeJson = JsonConvert.SerializeObject(originalObject, converter);
            var serializeObject = JsonConvert.DeserializeObject<DateTimeClassTest>(serializeJson);
            var serializeObjectOnlyDateNullable = serializeObject.OnlyDateNullable?.ToString("yyyy-MM-ddTHH:mm:ss");

            // assert 
            Assert.NotNull(serializeObject);
            Assert.Equal(expectedDateTime, serializeObjectOnlyDateNullable);
        }

        public class DateTimeClassTest
        {
            public DateTime MyDateTime { get; set; }

            public DateTime? MyNullableDateTime { get; set; }

            [JsonConverter(typeof(DateConverter))]
            public DateTime OnlyDate { get; set; }

            [JsonConverter(typeof(DateConverter), "yyyy-MM-dd")]
            public DateTime? OnlyDateNullable { get; set; }
        }
    }
}
