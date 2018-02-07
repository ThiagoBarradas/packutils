using Xunit;

namespace PackUtils.Test
{
    public static class SignatureUtilityTest
    {
        [Fact]
        public static void CreateSignature_Should_Return_A_Signature()
        {
            // arrange
            var message = "some content";
            var privateKey = "my-key";

            // act
            var sign = SignatureUtility.CreateSignature(privateKey, message);

            // assert
            Assert.Equal("ae00f9ad096b6d2ca828e972bcda212ed74b338ebc1332c56b2d96bb06fc845e", sign);
        }

        [Fact]
        public static void ValidateSignature_Should_Return_Success_Validate()
        {
            // arrange
            var message = "some content";
            var privateKey = "my-key";
            var sign = "ae00f9ad096b6d2ca828e972bcda212ed74b338ebc1332c56b2d96bb06fc845e";
            
            // act
            var result = SignatureUtility.ValidateSignature(sign, privateKey, message);

            // assert
            Assert.True(result);
        }

        [Fact]
        public static void ValidateSignature_Should_Return_Unsuccess_Validate()
        {
            // arrange
            var message = "some content2";
            var privateKey = "my-key";
            var sign = "ae00f9ad096b6d2ca828e972bcda212ed74b338ebc1332c56b2d96bb06fc845e";

            // act
            var result = SignatureUtility.ValidateSignature(sign, privateKey, message);

            // assert
            Assert.False(result);
        }

        [Fact]
        public static void CreateSignatureFromObject_Should_Return_A_Signature()
        {
            // arrange
            var obj = new TestSign
            {
                SomeString = "test-x",
                SomeInt = 1,
                SomeNull = null,
                SomeBool = true
            };
            var privateKey = "my-key";

            // act
            var sign = SignatureUtility.CreateSignatureFromObject(privateKey, obj);

            // assert
            Assert.Equal("24507965277b7d0a2e7db0b8d369eb3bf549c6d623523157125be92174363153", sign);
        }

        [Fact]
        public static void ValidateSignatureFromObject_Should_Return_Success_Validate()
        {
            // arrange
            var obj = new TestSign
            {
                SomeString = "test-x",
                SomeInt = 1,
                SomeNull = null,
                SomeBool = true
            };
            var privateKey = "my-key";
            var sign = "24507965277b7d0a2e7db0b8d369eb3bf549c6d623523157125be92174363153";

            // act
            var result = SignatureUtility.ValidateSignatureFromObject(sign, privateKey, obj);

            // assert
            Assert.True(result);
        }

        [Fact]
        public static void ValidateSignatureFromObject_Should_Return_Unsuccess_Validate()
        {
            // arrange
            var obj = new TestSign
            {
                SomeString = "test-y",
                SomeInt = 1,
                SomeNull = null,
                SomeBool = true
            };
            var privateKey = "my-key";
            var sign = "24507965277b7d0a2e7db0b8d369eb3bf549c6d623523157125be92174363153";

            // act
            var result = SignatureUtility.ValidateSignatureFromObject(sign, privateKey, obj);

            // assert
            Assert.False(result);
        }

        [Fact]
        public static void CreateSignatureFromObject_Should_Return_A_Signature_With_Ignored_Field()
        {
            // arrange
            var obj = new TestSign
            {
                SomeString = "test-x",
                SomeInt = 1,
                SomeNull = null,
                SomeBool = true
            };
            var privateKey = "my-key";

            // act
            var sign = SignatureUtility.CreateSignatureFromObject(privateKey, obj, "SomeInt");

            // assert
            Assert.Equal("403ceaa302f5e6c47c72b8049c5700f331542c802e8a44bdc80896ff090afd79", sign);
        }

        [Fact]
        public static void ValidateSignatureFromObject_Should_Return_Success_Validate_With_Ignored_Field()
        {
            // arrange
            var obj = new TestSign
            {
                SomeString = "test-x",
                SomeInt = 1,
                SomeNull = null,
                SomeBool = true
            };
            var privateKey = "my-key";
            var sign = "403ceaa302f5e6c47c72b8049c5700f331542c802e8a44bdc80896ff090afd79";

            // act
            var result = SignatureUtility.ValidateSignatureFromObject(sign, privateKey, obj, "SomeInt");

            // assert
            Assert.True(result);
        }

        [Fact]
        public static void ValidateSignatureFromObject_Should_Return_Unsuccess_Validate_With_Ignored_Field()
        {
            // arrange
            var obj = new TestSign
            {
                SomeString = "test-y",
                SomeInt = 1,
                SomeNull = null,
                SomeBool = true
            };
            var privateKey = "my-key";
            var sign = "403ceaa302f5e6c47c72b8049c5700f331542c802e8a44bdc80896ff090afd79";

            // act
            var result = SignatureUtility.ValidateSignatureFromObject(sign, privateKey, obj, "SomeInt");

            // assert
            Assert.False(result);
        }
    }

    class TestSign
    {
        public string SomeString { get; set; }

        public int SomeInt { get; set; }

        public object SomeNull { get; set; }

        public bool SomeBool { get; set; }
    }
}
