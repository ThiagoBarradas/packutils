using Xunit;

namespace PackUtils.Test
{
    public static class StringUtilityTest
    {
        [Theory]
        [InlineData(null, null)]
        [InlineData("", null)]
        [InlineData("test", "test")]
        [InlineData("test_testt", "testTestt")]
        [InlineData("Test", "test")]
        [InlineData("TestTTT", "testTtt")]
        [InlineData("_Test_TTT_", "testTtt")]
        [InlineData("test__testr", "testTestr")]
        public static void ToCamelCase_Should_Works(string text, string result)
        {
            // arrange & act
            var newValue = text.ToCamelCase();

            // assert
            Assert.Equal(result, newValue);
        }

        [Theory]
        [InlineData(null, null, null)]
        [InlineData("camel", null, null)]
        [InlineData("camel", "", null)]
        [InlineData("camel", "hello_sir", "helloSir")]
        [InlineData("camel", "Hello123", "hello123")]
        [InlineData("camel", "HelloXXX", "helloXxx")]
        [InlineData("camelcase", null, null)]
        [InlineData("camelcase", "", null)]
        [InlineData("camelcase", "hello_sir", "helloSir")]
        [InlineData("camelcase", "Hello123", "hello123")]
        [InlineData("camelcase", "HelloXXX", "helloXxx")]
        [InlineData("snake", null, null)]
        [InlineData("snake", "", null)]
        [InlineData("snake", "hello_sir", "hello_sir")]
        [InlineData("snake", "Hello123", "hello_123")]
        [InlineData("snake", "HelloXXX", "hello_xxx")]
        [InlineData("snakecase", null, null)]
        [InlineData("snakecase", "", null)]
        [InlineData("snakecase", "hello_sir", "hello_sir")]
        [InlineData("snakecase", "Hello123", "hello_123")]
        [InlineData("snakecase", "HelloXXX", "hello_xxx")]
        [InlineData("lower", null, null)]
        [InlineData("lower", "", null)]
        [InlineData("lower", "hello_sir", "hellosir")]
        [InlineData("lower", "Hello123", "hello123")]
        [InlineData("lower", "HelloXXX", "helloxxx")]
        [InlineData("lowercase", null, null)]
        [InlineData("lowercase", "", null)]
        [InlineData("lowercase", "hello_sir", "hellosir")]
        [InlineData("lowercase", "Hello123", "hello123")]
        [InlineData("lowercase", "HelloXXX", "helloxxx")]
        [InlineData("huhuhehe", null, null)]
        [InlineData("huhuhehe", "", null)]
        [InlineData("huhuhehe", "hello_sir", "helloSir")]
        [InlineData("huhuhehe", "Hello123", "Hello123")]
        [InlineData("huhuhehe", "HelloXXX", "HelloXXX")]
        public static void ToCamel_Should_Works(string strategy, string text, string result)
        {
            // arrange & act
            var newValue = text.ToCase(strategy);

            // assert
            Assert.Equal(result, newValue);
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData("", null)]
        [InlineData("test", "test")]
        [InlineData("testTestt", "test_testt")]
        [InlineData("Test", "test")]
        [InlineData("TestTTT", "test_t_t_t")]
        [InlineData("LineLine123123", "line_line_123123")]
        [InlineData("testTestr", "test_testr")]
        public static void ToSnakeCase_Should_Works(string text, string result)
        {
            // arrange & act
            var newValue = text.ToSnakeCase();

            // assert
            Assert.Equal(result, newValue);
        }

        [Fact]
        public static void Replace_Should_Return_Empty_String_With_Empty_OriginalValue()
        {
            // arrange
            string originalValue = "";

            // act
            var newValue = StringUtility.Replace(originalValue, new[] { ' ', '-' }, "_");

            // assert
            Assert.Equal(originalValue, newValue);
        }

        [Fact]
        public static void Replace_Should_Return_Empty_String_With_Null_OriginalValue()
        {
            // arrange
            string originalValue = null;

            // act
            var newValue = StringUtility.Replace(originalValue, new[] { ' ', '-' }, "_");

            // assert
            Assert.Equal(string.Empty, newValue);
        }

        [Fact]
        public static void Replace_Should_Return_OriginalString_With_Nothing_Matchs()
        {
            // arrange
            string originalValue = "test";

            // act
            var newValue = StringUtility.Replace(originalValue, new[] { ' ', '-' }, "_");

            // assert
            Assert.Equal(originalValue, newValue);
        }

        [Fact]
        public static void Replace_Should_Return_OriginalString_With_Single_Match()
        {
            // arrange
            string originalValue = "test test";

            // act
            var newValue = StringUtility.Replace(originalValue, new[] { ' ', '-' }, "_");

            // assert
            Assert.Equal(originalValue.Replace(" ","_"), newValue);
        }

        [Fact]
        public static void Replace_Should_Return_OriginalString_With_Multi_Matchs()
        {
            // arrange
            string originalValue = "test test-test";

            // act
            var newValue = StringUtility.Replace(originalValue, new[] { ' ', '-' }, "_");

            // assert
            Assert.Equal(originalValue.Replace(" ", "_").Replace("-", "_"), newValue);
        }

        [Theory]
        [InlineData("âÂéío123 lçüa", "aAeio123 lcua")]
        [InlineData("lcua", "lcua")]
        public static void RemoveDiacritics_Should_Works(string original, string expected)
        {
            // arrange & act
            var result = StringUtility.RemoveDiacritics(original);

            // assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("Á$%aA-_+=eio123 lcua", "aAeio lcua")]
        [InlineData("lcua", "lcua")]
        public static void RemoveNumbersAndSpecialCharacters_Should_Works(string original, string expected)
        {
            // arrange & act
            var result = StringUtility.RemoveNumbersAndSpecialCharacters(original);

            // assert
            Assert.Equal(expected, result);
        }
    }
}
