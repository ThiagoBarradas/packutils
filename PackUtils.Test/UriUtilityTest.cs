using Xunit;

namespace PackUtils.Test
{
    public class UriUtilityTest
    {
        [Fact]
        public static void AddQueryString_Should_Return_Original_Uri_With_Empty_Parameter()
        {
            // arrange
            var uri = "http://www.test.com/";

            // act
            var newUri = UriUtility.AddQueryString(uri, null, null);

            // assert
            Assert.Equal(uri, newUri);
        }

        [Fact]
        public static void AddQueryString_Should_Return_Original_Uri_With_Empty_Uri()
        {
            // arrange
            var uri = "";

            // act
            var newUri = UriUtility.AddQueryString(uri, "param", "value");

            // assert
            Assert.Equal(uri, newUri);
        }

        [Fact]
        public static void AddQueryString_Should_Return_Uri_With_First_QueryString_Param()
        {
            // arrange
            var uri = "http://www.google.com/";

            // act
            var newUri = UriUtility.AddQueryString(uri, "param", "value");

            // assert
            Assert.Equal(uri + "?param=value", newUri);
        }

        [Fact]
        public static void AddQueryString_Should_Return_Uri_With_Second_QueryString_Param()
        {
            // arrange
            var uri = "http://www.google.com/?param1=value1";

            // act
            var newUri = UriUtility.AddQueryString(uri, "param2", "value2");

            // assert
            Assert.Equal(uri + "&param2=value2", newUri);
        }

        [Fact]
        public static void AddQueryString_Should_Return_Uri_With_Updated_QueryString_Param()
        {
            // arrange
            var uri = "http://www.google.com/?param=wrong-value";

            // act
            var newUri = UriUtility.AddQueryString(uri, "param", "value");

            // assert
            Assert.Equal(uri.Replace("wrong-value", "value"), newUri);
        }

        [Fact]
        public static void AddQueryString_Should_Return_Uri_With_QueryString_Param_And_Https()
        {
            // arrange
            var uri = "https://www.google.com/";

            // act
            var newUri = UriUtility.AddQueryString(uri, "param", "value");

            // assert
            Assert.Equal(uri + "?param=value", newUri);
        }

        [Fact]
        public static void AddQueryString_Should_Return_Uri_With_QueryString_Param_And_Diff_Port()
        {
            // arrange
            var uri = "https://www.google.com:505/";

            // act
            var newUri = UriUtility.AddQueryString(uri, "param", "value");

            // assert
            Assert.Equal(uri + "?param=value", newUri);
        }
    }
}
