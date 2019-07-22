using Newtonsoft.Json;
using PackUtils.Converters;
using System;
using Xunit;

namespace PackUtils.Test.Converters
{
    public class DateTimeConverterTests
    {
        [Theory]
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
                    : DateTime.Parse(nullableDateTime))
            };

            string originalJson = JsonConvert.SerializeObject(originalObject);

            // act 
            var deserializeResult = JsonConvert.DeserializeObject<DateTimeClassTest>(originalJson, converter);

            var serializeResultString = JsonConvert.SerializeObject(deserializeResult, converter);
            var serializeResult = JsonConvert.DeserializeObject<DateTimeClassTest>(serializeResultString);

            // assert 
            Assert.NotNull(deserializeResult);
            Assert.NotNull(serializeResultString);
            Assert.NotNull(serializeResult);
            
            var deserializeResultDateTime = deserializeResult.MyDateTime.ToString("yyyy-MM-ddTHH:mm:ss.fffK");
            var serializeResultDateTime = serializeResult.MyDateTime.ToString("yyyy-MM-ddTHH:mm:ss");

            Assert.Equal(expectedDateTime, deserializeResultDateTime);
            Assert.Equal(dateTime, serializeResultDateTime);

            if (string.IsNullOrWhiteSpace(nullableDateTime))
            {
                Assert.Null(deserializeResult.MyNullableDateTime);
                Assert.Null(serializeResult.MyNullableDateTime);
            }
            else
            {
                var deserializeResultNullableDateTime =
                    deserializeResult.MyNullableDateTime.Value.ToString("yyyy-MM-ddTHH:mm:ss.fffK");
                var serializeResultNullableDateTime =
                    serializeResult.MyNullableDateTime.Value.ToString("yyyy-MM-ddTHH:mm:ss");

                Assert.Equal(expectedNullableDateTime, deserializeResultNullableDateTime);
                Assert.Equal(nullableDateTime, serializeResultNullableDateTime);
            }
        }

        public class DateTimeClassTest
        {
            public DateTime MyDateTime { get; set; }

            public DateTime? MyNullableDateTime { get; set; }
        }
    }
}
