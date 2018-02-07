using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xunit;

namespace PackUtils.Test
{
    public static class EnumUtilityTest
    {
        [Fact]
        public static void ConvertToEnum_Should_Return_Parsed_Enum()
        {
            // arrange
            var enumToParse = EnumTest2.Test1;

            // act
            var enumValue = EnumUtility.ConvertToEnum<EnumTest1>(enumToParse);

            // assert
            Assert.Equal(EnumTest1.Test1, enumValue);
        }

        [Fact]
        public static void ConvertToEnum_Should_Return_Parsed_Enum_With_Null_Param()
        {
            // arrange
            string enumString = null;

            // act
            var enumValue = EnumUtility.ConvertToEnum<EnumTest1>(enumString);

            // assert
            Assert.Equal(EnumTest1.Undefined, enumValue);
        }

        [Fact]
        public static void ConvertToEnum_Should_Return_Parsed_Enum_Unkown_Param()
        {
            // arrange
            string enumString = "Some";

            // act
            var enumValue = EnumUtility.ConvertToEnum<EnumTest1>(enumString);

            // assert
            Assert.Equal(EnumTest1.Undefined, enumValue);
        }

        [Fact]
        public static void ConvertToEnum_Should_Throws_Exception_With_Non_Enum_T()
        {

            // arrange / act / assert
            ArgumentException ex = Assert.Throws<ArgumentException>(() =>
                EnumUtility.ConvertToEnum<String>("Bla"));
            Assert.Equal(ex.Message, "T must be an enumerated type.");
        }

        [Fact]
        public static void ConvertToEnumList_Should_Throws_Exception_With_Non_Enum_T()
        {
            // arrange / act / assert
            ArgumentException ex = Assert.Throws<ArgumentException>(() =>
                EnumUtility.ConvertToEnum<String>(new List<string> { "Bla" }));
            Assert.Equal(ex.Message, "T must be an enumerated type.");
        }

        [Fact]
        public static void ConvertToEnumList_Should_Return_Parsed_Enum_With_Null_Param()
        {
            // arrange
            List<string> enumList = null;

            // act
            var enumValue = EnumUtility.ConvertToEnum<EnumTest1>(enumList);

            // assert
            Assert.Equal(0, enumValue.Count);
        }

        [Fact]
        public static void ConvertToEnumList_Should_Return_Parsed_Enum_List()
        {
            // arrange
            List<string> enumList = new List<string> { "Test1", "Bla" };

            // act
            var enumValue = EnumUtility.ConvertToEnum<EnumTest1>(enumList);

            // assert
            Assert.Equal(2, enumValue.Count);
            Assert.Equal(EnumTest1.Test1, enumValue[0]);
            Assert.Equal(EnumTest1.Undefined, enumValue[1]);
        }

        [Fact]
        public static void GetDescriptionFromEnum_Should_Return_Description()
        {
            // arrange
            EnumTest1 enumm = EnumTest1.Test1;

            // act
            var description = EnumUtility.GetDescriptionFromEnum(enumm);

            // assert
            Assert.Equal("test 1", description);
        }

        [Fact]
        public static void GetDescriptionFromEnum_Should_Return_Empty_Description()
        {
            // arrange
            EnumTest1 enumm = EnumTest1.Undefined;

            // act
            var description = EnumUtility.GetDescriptionFromEnum(enumm);

            // assert
            Assert.Equal("", description);
        }

        [Fact]
        public static void GetEnumFromDescription_Should_Throws_Exception_With_Non_Enum_T()
        {
            // arrange / act / assert
            ArgumentException ex = Assert.Throws<ArgumentException>(() =>
                EnumUtility.GetEnumFromDescription<String>("Bla"));
            Assert.Equal(ex.Message, "T must be an enumerated type.");
        }

        [Fact]
        public static void GetEnumFromDescription_Should_Return_Enum_Value()
        {
            // arrange
            string description = "test 1";

            // act
            var enumm = EnumUtility.GetEnumFromDescription<EnumTest1>(description);

            // assert
            Assert.Equal(EnumTest1.Test1, enumm);
        }

        [Fact]
        public static void GetEnumFromDescription_Should_Return_Default_Enum()
        {
            // arrange
            string description = "test 12";

            // act
            var enumm = EnumUtility.GetEnumFromDescription<EnumTest1>(description);

            // assert
            Assert.Equal(EnumTest1.Undefined, enumm);
        }

        [Fact]
        public static void GetEnumFromDescription_Should_Throws_Exception_With_NotFound_Disabled()
        {
            // arrange / act / assert
            KeyNotFoundException ex = Assert.Throws<KeyNotFoundException>(() =>
                EnumUtility.GetEnumFromDescription<EnumTest1>("Bla", false));
            Assert.Equal(ex.Message, "Bla not found in EnumTest1");
        }

        [Fact]
        public static void IsValidToParse_Should_Throws_Exception_With_Non_Enum_T()
        {
            // arrange / act / assert
            ArgumentException ex = Assert.Throws<ArgumentException>(() =>
                EnumUtility.IsValidToParse<String>("Bla"));
            Assert.Equal(ex.Message, "T must be an enumerated type.");
        }

        [Fact]
        public static void IsValidToParse_Should_Return_True()
        {
            // arrange
            string enumString = "Test1";

            // act
            var result = EnumUtility.IsValidToParse<EnumTest1>(enumString);

            // assert
            Assert.True(result);
        }

        [Fact]
        public static void IsValidToParse_Should_Return_False()
        {
            // arrange
            string enumString = "Test123";

            // act
            var result = EnumUtility.IsValidToParse<EnumTest1>(enumString);

            // assert
            Assert.False(result);
        }

        [Fact]
        public static void IsValidToParse_Should_Return_True_With_AcceptNull()
        {
            // arrange
            string enumString = null;

            // act
            var result = EnumUtility.IsValidToParse<EnumTest1>(enumString, true);

            // assert
            Assert.True(result);
        }

        [Fact]
        public static void IsValidToParse_Should_Return_False_With_AcceptNull()
        {
            // arrange
            string enumString = null;

            // act
            var result = EnumUtility.IsValidToParse<EnumTest1>(enumString, false);

            // assert
            Assert.False(result);
        }

        [Fact]
        public static void GetDescriptionsFromEnums_Should_Return_Only_Enums_With_Description()
        {
            // arrange
            List<EnumTest1> enumList = new List<EnumTest1> { EnumTest1.Undefined, EnumTest1.Test1 };

            // act
            var descriptions = EnumUtility.GetDescriptionsFromEnums<EnumTest1>(enumList);

            // assert
            Assert.Equal(1, descriptions.Count);
            Assert.Equal("test 1", descriptions[0]);
        }

        [Fact]
        public static void GetAllEnumsWithDescription_Should_Return_Only_Enums_With_Description()
        {
            // act
            var enumsWithDescriptions = EnumUtility.GetAllEnumsWithDescription<EnumTest1>();

            // assert
            Assert.Equal(2, enumsWithDescriptions.Count);
            Assert.Equal(EnumTest1.Test1, enumsWithDescriptions[0]);
            Assert.Equal(EnumTest1.Test2, enumsWithDescriptions[1]);
        }
    }

    enum EnumTest1
    {
        Undefined,

        [Description("test 1")]
        Test1,

        [Description("test 2")]
        Test2
    }

    enum EnumTest2
    {
        Undefined,

        [Description("test 1.1")]
        Test1,

        [DisplayName("t3")]
        Test3
    }
}
