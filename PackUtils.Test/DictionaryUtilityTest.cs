using System;
using Xunit;

namespace PackUtils.Test
{
    public class DictionaryUtilityTest
    {
        [Fact]
        public static void ToDictionary_Should_Throws_Exception_With_Null_Value()
        {
            // arrange / act / assert
            ArgumentNullException ex = Assert.Throws<ArgumentNullException>(() =>
                DictionaryUtility.ToDictionary(null));
            Assert.Equal(ex.ParamName, "source");
        }

        [Fact]
        public static void ToDictionary_Should_Return_A_Dictionary_With_Object_Values()
        {
            // arrange 
            PlainObject obj = new PlainObject()
            {
                TestInt = 5,
                TestString = "test",
                TestNull = null
            };

            // act
            var dic = DictionaryUtility.ToDictionary(obj);

            // assert
            Assert.Equal(dic.Count, 2);
            Assert.Equal(dic["TestInt"], "5");
            Assert.Equal(dic["TestString"], "test");
        }
    }

    class PlainObject
    {
        public string TestString { get; set; }

        public int TestInt { get; set; }

        public object TestNull { get; set; }
    }
}
