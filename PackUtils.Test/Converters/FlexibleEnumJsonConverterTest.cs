using PackUtils.Converters;
using Newtonsoft.Json;
using Xunit;

namespace PackUtils.Test
{
    public static class FlexibleEnumJsonConverterTest
    {
        [Fact]
        public static void FlexibleEnumJsonConverter_Should_Parser_Int_Value()
        {
            // arrange
            var converter = new FlexibleEnumJsonConverter();
            string json = "{ \"myEnum\" : 2, \"myEnumNullable\" : 4, \"SomeString\" : \"test\" }";


            // act
            var obj = JsonConvert.DeserializeObject<TestEnumClass>(json, converter);

            // assert
            Assert.NotNull(obj);
            Assert.Equal(TestEnum.Value1, obj.MyEnum);
            Assert.Equal(TestEnum.Value2, obj.MyEnumNullable.Value);
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

    public class TestEnumClass
    {
        public TestEnum MyEnum { get; set; }

        public TestEnum? MyEnumNullable { get; set; }

        public string SomeString { get; set; }
    }

    public class TestEnumClass2
    {
        public TestEnum2 MyEnum { get; set; }

        public TestEnum2? MyEnumNullable { get; set; }

        public string SomeString { get; set; }
    }

    public enum TestEnum
    {
        Undefined = 1,
        Value1 = 2,
        Value2 = 4
    }

    public enum TestEnum2
    {
        Value1 = 2,
        Value2 = 4
    }
}
