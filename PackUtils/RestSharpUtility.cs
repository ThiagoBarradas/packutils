using RestSharp;
using System.Collections.Generic;
using System.Linq;

namespace PackUtils
{
    public static class RestSharpUtility
    {
        public static void AddQueryString(this RestRequest request, IDictionary<string, List<string>> headers)
        {
            if (headers?.Any() != true)
            {
                return;
            }

            foreach (var queryItem in headers)
            {
                request.AddParameter(queryItem.Key, string.Join(",", queryItem.Value), ParameterType.QueryString);
            }
        }

        public static void AddJsonBodyAsString(this RestRequest request, string content)
        {
            if (request.Method != Method.Get && !string.IsNullOrWhiteSpace(content))
            {
                request.AddParameter("application/json", content, ParameterType.RequestBody);
            }
        }

        public static void AddBasicAuth(this RestRequest request, string user, string pass)
        {
            request.AddHeader("Authorization", $"Basic " + $"{user}:{pass}".Base64Encode());
        }

        public static void AddHeaderNotEmpty(this RestRequest request, string key, string value)
        {
            if (string.IsNullOrWhiteSpace(value) == false)
            {
                request.AddHeader(key, value);
            }
        }

        public static void AddQueryString(this RestRequest request, IDictionary<string, string> headers)
        {
            if (headers?.Any() != true)
            {
                return;
            }

            foreach (var queryItem in headers)
            {
                request.AddParameter(queryItem.Key, queryItem, ParameterType.QueryString);
            }
        }
    }
}
