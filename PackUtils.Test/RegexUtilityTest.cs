using System.Collections.Generic;
using Xunit;

namespace PackUtils.Test
{
    public static class RegexUtilityTest
    {
        [Fact]
        public static void GetIpRegex_Should_Return_Success_Match()
        {
            // arrange
            var ips = new List<string> { "127.0.0.1", "255.0.10.23" };

            // act
            var result = RegexUtility.IsMatch(ips, RegexUtility.GetIpRegex());

            // assert
            Assert.True(result);
        }

        [Fact]
        public static void GetIpRegex_Should_Return_Failed_Match()
        {
            // arrange
            var ips = new List<string> { "127.0.0.1", "256.0.10.23" };

            // act
            var result = RegexUtility.IsMatch(ips, RegexUtility.GetIpRegex());

            // assert
            Assert.False(result);
        }

        [Fact]
        public static void GetUrlOrIpRegex_Should_Return_Success_Match()
        {
            // arrange
            var ipsOrUrls = new List<string> { "127.0.0.1", "255.0.10.230", "http://www.google.com" };

            // act
            var result = RegexUtility.IsMatch(ipsOrUrls, RegexUtility.GetUrlOrIpRegex());

            // assert
            Assert.True(result);
        }

        [Fact]
        public static void GetUrlOrIpRegex_Should_Return_Failed_Match()
        {
            // arrange
            var ipsOrUrl = new List<string> { "127.0.0.1", "255.0.10.23", "http//www.google.com" };

            // act
            var result = RegexUtility.IsMatch(ipsOrUrl, RegexUtility.GetUrlOrIpRegex());

            // assert
            Assert.False(result);
        }

        [Fact]
        public static void GetUrlRegex_Should_Return_Success_Match()
        {
            // arrange
            var urls = new List<string> { "http://www.google.com", "http://www.bing.com" };

            // act
            var result = RegexUtility.IsMatch(urls, RegexUtility.GetUrlRegex());

            // assert
            Assert.True(result);
        }

        [Fact]
        public static void GetUrlRegex_Should_Return_Failed_Match()
        {
            // arrange
            var urls = new List<string> { "http://www.google.com", "http//www.bing.com" };

            // act
            var result = RegexUtility.IsMatch(urls, RegexUtility.GetUrlRegex());

            // assert
            Assert.False(result);
        }

        [Fact]
        public static void GetAlphaRegex_Should_Return_Success_Match()
        {
            // arrange
            var values = new List<string> { "asdfgh", "asd" };

            // act
            var result = RegexUtility.IsMatch(values, RegexUtility.GetAlphaRegex(3,6));

            // assert
            Assert.True(result);
        }

        [Fact]
        public static void GetAlphaRegex_Should_Return_Failed_Match_By_Size()
        {
            // arrange
            var values = new List<string> { "aasdfgh", "asd" };

            // act
            var result = RegexUtility.IsMatch(values, RegexUtility.GetAlphaRegex(3, 6));

            // assert
            Assert.False(result);
        }

        [Fact]
        public static void GetAlphaRegex_Should_Return_Failed_Match_By_Number()
        {
            // arrange
            var values = new List<string> { "asdfgh", "1asd" };

            // act
            var result = RegexUtility.IsMatch(values, RegexUtility.GetAlphaRegex(3, 6));

            // assert
            Assert.False(result);
        }

        [Fact]
        public static void GetAlphaNumericRegex_Should_Return_Success_Match()
        {
            // arrange
            var values = new List<string> { "a1dfgh", "asd" };

            // act
            var result = RegexUtility.IsMatch(values, RegexUtility.GetAlphaNumericRegex(3, 6));

            // assert
            Assert.True(result);
        }

        [Fact]
        public static void GetAlphaNumericRegex_Should_Return_Failed_Match()
        {
            // arrange
            var values = new List<string> { "aa%dfgh", "asd" };

            // act
            var result = RegexUtility.IsMatch(values, RegexUtility.GetAlphaNumericRegex(3, 6));

            // assert
            Assert.False(result);
        }

        [Fact]
        public static void GetPasswordRegex_Should_Return_Success_Match_All_Criterias()
        {
            // arrange
            var values = new List<string> { "ABcd12$%", "asdfgh ssR1$" };

            // act
            var result = RegexUtility.IsMatch(values, RegexUtility.GetPasswordRegex(8,16,true,true,true,true));

            // assert
            Assert.True(result);
        }

        [Fact]
        public static void GetPasswordRegex_Should_Return_Failed_Match_By_Size_All_Criterias()
        {
            // arrange
            var values = new List<string> { "ABcd12$%", "R$a1" };

            // act
            var result = RegexUtility.IsMatch(values, RegexUtility.GetPasswordRegex(8, 16, true, true, true, true));

            // assert
            Assert.False(result);
        }

        [Fact]
        public static void GetPasswordRegex_Should_Return_Failed_Match_By_Number_All_Criterias()
        {
            // arrange
            var values = new List<string> { "ABcd12$%", "R$aasdfgh" };

            // act
            var result = RegexUtility.IsMatch(values, RegexUtility.GetPasswordRegex(8, 16, true, true, true, true));

            // assert
            Assert.False(result);
        }

        [Fact]
        public static void GetPasswordRegex_Should_Return_Failed_Match_By_Upper_All_Criterias()
        {
            // arrange
            var values = new List<string> { "ABcd12$%", "a$1asdfgh" };

            // act
            var result = RegexUtility.IsMatch(values, RegexUtility.GetPasswordRegex(8, 16, true, true, true, true));

            // assert
            Assert.False(result);
        }

        [Fact]
        public static void GetPasswordRegex_Should_Return_Failed_Match_By_Lower_All_Criterias()
        {
            // arrange
            var values = new List<string> { "ABcd12$%", "R$1ASDASD" };

            // act
            var result = RegexUtility.IsMatch(values, RegexUtility.GetPasswordRegex(8, 16, true, true, true, true));

            // assert
            Assert.False(result);
        }

        [Fact]
        public static void GetPasswordRegex_Should_Return_Failed_Match_By_Special_All_Criterias()
        {
            // arrange
            var values = new List<string> { "ABcd12$%", "ar1ASDASD" };

            // act
            var result = RegexUtility.IsMatch(values, RegexUtility.GetPasswordRegex(8, 16, true, true, true, true));

            // assert
            Assert.False(result);
        }

        [Fact]
        public static void GetPasswordRegex_Should_Return_Success_Match_Only_Number_Requires()
        {
            // arrange
            var values = new List<string> { "123", "A$a5 " };

            // act
            var result = RegexUtility.IsMatch(values, RegexUtility.GetPasswordRegex(2, 5, false, true, false, false));

            // assert
            Assert.True(result);
        }

        [Fact]
        public static void GetPasswordRegex_Should_Return_Failed_Match_By_Only_Number_Requires()
        {
            // arrange
            var values = new List<string> { "ABcd", "1111" };

            // act
            var result = RegexUtility.IsMatch(values, RegexUtility.GetPasswordRegex(2, 5, false, true, false, false));

            // assert
            Assert.False(result);
        }

        [Fact]
        public static void GetPasswordRegex_Should_Return_Success_Match_Only_Lower_Requires()
        {
            // arrange
            var values = new List<string> { "123x", "A$a5 " };

            // act
            var result = RegexUtility.IsMatch(values, RegexUtility.GetPasswordRegex(2, 5, false, false, true, false));

            // assert
            Assert.True(result);
        }

        [Fact]
        public static void GetAlphaRegex_Should_Return_Success_Match_With_Null_Allowed()
        {
            // arrange
            List<string> values = null;

            // act
            var result = RegexUtility.IsMatch(values, RegexUtility.GetAlphaRegex(3, 6), true);

            // assert
            Assert.True(result);
        }

        [Fact]
        public static void GetAlphaRegex_Should_Return_Failed_Match_With_Null_Allowed()
        {
            // arrange
            List<string> values = null;

            // act
            var result = RegexUtility.IsMatch(values, RegexUtility.GetAlphaRegex(3, 6), false);

            // assert
            Assert.False(result);
        }
    }
}
