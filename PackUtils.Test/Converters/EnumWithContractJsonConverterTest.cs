using PackUtils.Converters;
using Newtonsoft.Json;
using Xunit;

namespace PackUtils.Test
{
    public static class EnumWithContractJsonConverterTest
    {
        [Fact]
        public static void EnumWithContractJsonConverterTest_Should_Serialize_And_Deserialize_With_LowerCase()
        {
            // arrange
            var settings = JsonUtility.LowerCaseJsonSerializerSettings;
            settings.Converters.Clear();
            settings.Converters.Add(new EnumWithContractJsonConverter());

            string json = "{\"myenum\":\"valuetest1\",\"myenumnullable\":\"valuetest2\",\"myenumnullablewithnull\":null}";
            var obj = new TestContractEnumClass
            {
                MyEnum = TestContractEnum.ValueTest1,
                MyEnumNullable = TestContractEnum.ValueTest2,
                MyEnumNullableWithNull = null
            };

            // act
            var objResult = JsonConvert.DeserializeObject<TestContractEnumClass>(json, settings);
            var jsonResult = JsonConvert.SerializeObject(obj, settings);

            // assert
            Assert.NotNull(obj);
            Assert.Equal("{\"myenum\":\"valuetest1\",\"myenumnullable\":\"valuetest2\"}", jsonResult);
            Assert.Equal(TestContractEnum.ValueTest1, objResult.MyEnum);
            Assert.Equal(TestContractEnum.ValueTest2, objResult.MyEnumNullable.Value);
            Assert.Null(objResult.MyEnumNullableWithNull);
        }

        [Fact]
        public static void EnumWithContractJsonConverterTest_Should_Serialize_And_Deserialize_With_SnakeCase()
        {
            // arrange
            var settings = JsonUtility.SnakeCaseJsonSerializerSettings;
            settings.Converters.Clear();
            settings.Converters.Add(new EnumWithContractJsonConverter());

            string json = "{\"my_enum\":\"value_test1\",\"my_enum_nullable\":\"value_test2\",\"my_enum_nullable_with_null\":null}";
            var obj = new TestContractEnumClass
            {
                MyEnum = TestContractEnum.ValueTest1,
                MyEnumNullable = TestContractEnum.ValueTest2,
                MyEnumNullableWithNull = null
            };

            // act
            var objResult = JsonConvert.DeserializeObject<TestContractEnumClass>(json, settings);
            var jsonResult = JsonConvert.SerializeObject(obj, settings);

            // assert
            Assert.NotNull(obj);
            Assert.Equal("{\"my_enum\":\"value_test_1\",\"my_enum_nullable\":\"value_test_2\"}", jsonResult);
            Assert.Equal(TestContractEnum.ValueTest1, objResult.MyEnum);
            Assert.Equal(TestContractEnum.ValueTest2, objResult.MyEnumNullable.Value);
            Assert.Null(objResult.MyEnumNullableWithNull);
        }

        [Fact]
        public static void EnumWithContractJsonConverterTest_Should_Serialize_And_Deserialize_With_CamelCase()
        {
            // arrange
            var settings = JsonUtility.CamelCaseJsonSerializerSettings;
            settings.Converters.Clear();
            settings.Converters.Add(new EnumWithContractJsonConverter());

            string json = "{\"myEnum\":\"valueTest1\",\"myEnumNullable\":\"valueTest2\",\"myEnumNullableWithNull\":null}";
            var obj = new TestContractEnumClass
            {
                MyEnum = TestContractEnum.ValueTest1,
                MyEnumNullable = TestContractEnum.ValueTest2,
                MyEnumNullableWithNull = null
            };

            // act
            var objResult = JsonConvert.DeserializeObject<TestContractEnumClass>(json, settings);
            var jsonResult = JsonConvert.SerializeObject(obj, settings);

            // assert
            Assert.NotNull(obj);
            Assert.Equal("{\"myEnum\":\"valueTest1\",\"myEnumNullable\":\"valueTest2\"}", jsonResult);
            Assert.Equal(TestContractEnum.ValueTest1, objResult.MyEnum);
            Assert.Equal(TestContractEnum.ValueTest2, objResult.MyEnumNullable.Value);
            Assert.Null(objResult.MyEnumNullableWithNull);
        }

        [Fact]
        public static void FlexibleEnumJsonConverter_Should_Parser_String_Value()
        {
            // arrange
            var converter = new FlexibleEnumJsonConverter();
            string json = "{ \"myEnum\" : \"value1\", \"myEnumNullable\" : \"value2\", \"SomeString\" : \"test\" }";


            // act
            var obj = JsonConvert.DeserializeObject<TestEnumClass>(json, converter);

            // assert
            Assert.NotNull(obj);
            Assert.Equal(TestEnum.Value1, obj.MyEnum);
            Assert.Equal(TestEnum.Value2, obj.MyEnumNullable.Value);
        }

        [Fact]
        public static void FlexibleEnumJsonConverter_Should_Parser_Invalid_Int_Value()
        {
            // arrange
            var converter = new FlexibleEnumJsonConverter();
            string json = "{ \"myEnum\" : 7, \"myEnumNullable\" : 5, \"SomeString\" : \"test\" }";


            // act
            var obj = JsonConvert.DeserializeObject<TestEnumClass>(json, converter);

            // assert
            Assert.NotNull(obj);
            Assert.Equal(TestEnum.Undefined, obj.MyEnum);
            Assert.False(obj.MyEnumNullable.HasValue);
        }

        [Fact]
        public static void FlexibleEnumJsonConverter_Should_Parser_Invalid_Int_Value_Without_Undefined()
        {
            // arrange
            var converter = new FlexibleEnumJsonConverter();
            string json = "{ \"myEnum\" : 7, \"myEnumNullable\" : 5, \"SomeString\" : \"test\" }";


            // act
            var obj = JsonConvert.DeserializeObject<TestEnumClass2>(json, converter);

            // assert
            Assert.NotNull(obj);
            Assert.Equal(TestEnum2.Value1, obj.MyEnum);
            Assert.False(obj.MyEnumNullable.HasValue);
        }

        [Fact]
        public static void FlexibleEnumJsonConverter_Should_Parser_Invalid_String_Value()
        {
            // arrange
            var converter = new FlexibleEnumJsonConverter();
            string json = "{ \"myEnum\" : \"value3\", \"myEnumNullable\" : \"value4\", \"SomeString\" : \"test\" }";


            // act
            var obj = JsonConvert.DeserializeObject<TestEnumClass>(json, converter);

            // assert
            Assert.NotNull(obj);
            Assert.Equal(TestEnum.Undefined, obj.MyEnum);
            Assert.False(obj.MyEnumNullable.HasValue);
        }
    }

    public class TestContractEnumClass
    {
        public TestContractEnum MyEnum { get; set; }

        public TestContractEnum? MyEnumNullable { get; set; }

        public TestContractEnum? MyEnumNullableWithNull { get; set; }
    }

    public enum TestContractEnum
    {
        Undefined = 1,
        ValueTest1 = 2,
        ValueTest2 = 4,
    }
}
