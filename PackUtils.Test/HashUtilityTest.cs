using Xunit;

namespace PackUtils.Test
{
    public static class HashUtilityTest
    {
        [Fact]
        public static void GenerateRandomSha256_Should_Return_New_And_Diff_Hashs()
        {
            // arrange / act
            var hash1 = HashUtility.GenerateRandomSha256();
            var hash2 = HashUtility.GenerateRandomSha256();

            // assert
            Assert.NotEqual(hash1, hash2);
            Assert.Equal(hash1.Trim().Length, 64);
            Assert.Equal(hash2.Trim().Length, 64);
        }

        [Fact]
        public static void GenerateSha256_Should_Return_Hash_For_Specific_String()
        {
            // arrange / act
            var hash = HashUtility.GenerateSha256("test");

            // assert
            Assert.Equal(hash, "9f86d081884c7d659a2feaa0c55ad015a3bf4f1b2b0b822cd15d6c15b0f00a08");
        }

        [Fact]
        public static void GenerateRandomSha256_Should_Return_Hash_For_Specific_String_With_Gap()
        {
            // arrange / act
            var hash = HashUtility.GenerateSha256("test","gap");

            // assert
            Assert.Equal(hash, "2b104763f73d857260e81bd2394c89e98cf87df5e8fb99bc82a508b693aadc94");
        }
    }
}
