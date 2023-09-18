using Newtonsoft.Json;

using PackUtils.Converters;

using System;

using Xunit;

namespace PackUtils.Test.Converters
{
    public class NullableEnumDefaultValueConverterTests
    {
        [Fact]
        public static void NullableEnumDefaultValueTest_Should_Deserialize_InvalidValue_And_SetDefaultValue()
        {
            // arrange
            var enumDefaultValue = NullableTestEnum.DefaultValue;
            var settings = JsonUtility.LowerCaseJsonSerializerSettings;
            settings.Converters.Clear();
            settings.Converters.Add(new NullableEnumDefaultValueConverter(enumDefaultValue));

            var json = "{\"myenumnullable\":\"invalid_value\"}";

            // act
            var objResult = JsonConvert.DeserializeObject<NullableTestEnumClass>(json, settings);

            // assert
            Assert.Equal(enumDefaultValue, objResult.MyEnumNullable.Value);
        }

        [Fact]
        public static void NullableEnumDefaultValueTest_Should_NotDeserializeWhenEnumIsNull()
        {
            // arrange
            var enumDefaultValue = NullableTestEnum.DefaultValue;
            var settings = JsonUtility.LowerCaseJsonSerializerSettings;
            settings.Converters.Clear();
            settings.Converters.Add(new NullableEnumDefaultValueConverter(enumDefaultValue));

            var json = "{\"not_my_myenumnullable\":\"invalid_value\"}";

            // act
            var objResult = JsonConvert.DeserializeObject<NullableTestEnumClass>(json, settings);

            // assert
            Assert.False(objResult.MyEnumNullable.HasValue);
        }

        [Fact]
        public static void NullableEnumDefaultValueTest_Should_Deserialize_ValidValue()
        {
            // arrange
            var enumDefaultValue = NullableTestEnum.DefaultValue;
            var settings = JsonUtility.LowerCaseJsonSerializerSettings;
            settings.Converters.Clear();
            settings.Converters.Add(new NullableEnumDefaultValueConverter(enumDefaultValue));

            var json = "{\"myenumnullable\":\"value_1\"}";

            // act
            var objResult = JsonConvert.DeserializeObject<NullableTestEnumClass>(json, settings);

            // assert
            Assert.Equal(NullableTestEnum.Value1, objResult.MyEnumNullable);
        }

        [Fact]
        public static void NullableEnumDefaultValueTest_Should_ThrowArgumentNullException_When_DefaultValueIsNotInformed()
        {
            // arrange
            var settings = JsonUtility.LowerCaseJsonSerializerSettings;
            settings.Converters.Clear();

            // act
            Action act = () =>
            {
                settings.Converters.Add(new NullableEnumDefaultValueConverter());
            };

            // assert
            Assert.Throws<ArgumentNullException>(act);
        }

        [Fact]
        public static void NullableEnumDefaultValueTest_Should_ThrowArgumentNullException_When_NullValueIsNotInformed()
        {
            // arrange
            var settings = JsonUtility.LowerCaseJsonSerializerSettings;
            settings.Converters.Clear();

            // act
            Action act = () =>
            {
                settings.Converters.Add(new NullableEnumDefaultValueConverter(null));
            };

            // assert
            Assert.Throws<ArgumentNullException>(act);
        }

        public class NullableTestEnumClass
        {
            public NullableTestEnum? MyEnumNullable { get; set; }
        }

        public enum NullableTestEnum
        {
            DefaultValue = 1,
            Value1 = 2,
            Value2 = 4
        }
    }
}
