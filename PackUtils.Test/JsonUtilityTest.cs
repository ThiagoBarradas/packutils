using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Text;
using Xunit;

namespace PackUtils.Test
{
    public static class JsonUtilityTest
    {
        [Fact]
        public static void CamelCaseJsonSerializerSettings_Should_Return_Parsed_Object()
        {
            // arrange
            var obj = new JsonExampleTest
            {
                SomeTest = "test"
            };

            // act
            var json = JsonConvert.SerializeObject(obj, JsonUtility.CamelCaseJsonSerializerSettings);

            // assert
            Assert.Contains("someTest", json);
        }

        [Fact]
        public static void SnakeCaseJsonSerializerSettings_Should_Return_Parsed_Object()
        {
            // arrange
            var obj = new JsonExampleTest
            {
                SomeTest = "test",
                CreateDDD = "true",
                CreateDDDCode = "false",
                Create123asd = "test"
            };

            // act
            var json = JsonConvert.SerializeObject(obj, JsonUtility.SnakeCaseJsonSerializerSettings);

            // assert
            Assert.Contains("some_test", json);
            Assert.Contains("create_ddd\"", json);
            Assert.Contains("create_ddd_code", json);
            Assert.Contains("create123asd", json);
        }

        [Fact]
        public static void LowerCaseJsonSerializerSettings_Should_Return_Parsed_Object()
        {
            // arrange
            var obj = new JsonExampleTest
            {
                SomeTest = "test"
            };

            // act
            var json = JsonConvert.SerializeObject(obj, JsonUtility.LowerCaseJsonSerializerSettings);

            // assert
            Assert.Contains("sometest", json);
        }

        [Fact]
        public static void CamelCaseJsonSerializer_Should_Return_Parsed_Object()
        {
            // arrange
            var obj = new JsonExampleTest
            {
                SomeTest = "test"
            };
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            JsonWriter jsonWriter = new JsonTextWriter(sw);

            // act
            JsonUtility.CamelCaseJsonSerializer.Serialize(jsonWriter, obj);

            // assert
            Assert.Contains("someTest", sb.ToString());
        }

        [Fact]
        public static void SnakeCaseJsonSerializer_Should_Return_Parsed_Object()
        {
            // arrange
            var obj = new JsonExampleTest
            {
                SomeTest = "test"
            };
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            JsonWriter jsonWriter = new JsonTextWriter(sw);

            // act
            JsonUtility.SnakeCaseJsonSerializer.Serialize(jsonWriter, obj);

            // assert
            Assert.Contains("some_test", sb.ToString());
        }

        [Fact]
        public static void LowerCaseJsonSerializer_Should_Return_Parsed_Object()
        {
            // arrange
            var obj = new JsonExampleTest
            {
                SomeTest = "test"
            };
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            JsonWriter jsonWriter = new JsonTextWriter(sw);

            // act
            JsonUtility.LowerCaseJsonSerializer.Serialize(jsonWriter, obj);

            // assert
            Assert.Contains("sometest", sb.ToString());
        }

        [Fact]
        public static void MaskFields_Should_Return_Password_Field_Masked()
        {
            // arrange
            var json = "{" +
                          "\"password\" : \"test\"," +
                          "\"sub\" : {" +
                              "\"some\" : \"test1\"," +
                              "\"password\" : \"test2\"" +
                          "}"+
                       "}";
            var blacklist = new string[] { "*password" };

            // act
            json = JsonUtility.MaskFields(json, blacklist);

            // assert
            var obj = (JObject)JsonConvert.DeserializeObject(json, JsonUtility.CamelCaseJsonSerializerSettings);
            Assert.Equal("******", obj["password"]);
            Assert.Equal("******", obj["sub"]["password"]);
            Assert.Equal("test1", obj["sub"]["some"]);
        }

        [Fact]
        public static void MaskFields_Should_Return_Full_Node_Masked()
        {
            // arrange
            var json = "{" +
                          "\"password\" : \"test\"," +
                          "\"sub\" : {" +
                              "\"some\" : \"test1\"," +
                              "\"password\" : \"test2\"" +
                          "}" +
                       "}";
            var blacklist = new string[] { "sub" };

            // act
            json = JsonUtility.MaskFields(json, blacklist, "xxx");

            // assert
            var obj = (JObject)JsonConvert.DeserializeObject(json, JsonUtility.CamelCaseJsonSerializerSettings);
            Assert.Equal("test", obj["password"]);
            Assert.Equal("xxx", obj["sub"]);
        }

        [Fact]
        public static void DeserializeAsObject_Should_Return_Parsed_Object()
        {
            // arrange
            var obj = new
            {
                SomeTest = "test",
                Number = 1,
                MyArray = new string[] { "x", "y" },
                DepthObj = new {
                    MyGuid = Guid.Empty,
                    DateTime = new DateTime(2010, 10, 25)
                }
            };

            var jsonString = JsonConvert.SerializeObject(obj);

            // act
            dynamic result = jsonString.DeserializeAsObject();

            // assert
            Assert.Equal("test", result["SomeTest"]);
            Assert.Equal(1, result["Number"]);
            Assert.Equal("x", result["MyArray"][0]);
            Assert.Equal("y", result["MyArray"][1]);
            Assert.Equal(Guid.Empty.ToString(), result["DepthObj"]["MyGuid"].ToString());
            Assert.Equal(new DateTime(2010, 10, 25).ToString(), result["DepthObj"]["DateTime"].ToString());
        }
    }

    public class JsonExampleTest
    {
        public string SomeTest { get; set; }

        public string CreateDDD { get; set; }

        public string CreateDDDCode { get; set; }

        public string Create123asd { get; set; }
    }
}
