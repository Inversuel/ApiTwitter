using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TwitterApiMVC.Models;

namespace TwitterApiMVC.Controllers.API
{
    public static class ApiHandler
    {
        private static readonly string TwitterApiUrl = "https://api.twitter.com";
        public static async Task<JArray> CallTwitterApi(string nameOfDNVGL, string count)
        {

            bool excludeReplies = false;
            bool includeRTs = true;

            var queryString = string.Format("/1.1/statuses/user_timeline.json?screen_name={0}&count={1}&exclude_replies={2}&include_rts={3}", nameOfDNVGL, count, excludeReplies.ToString().ToLower(), includeRTs.ToString().ToLower());

            var response = await BaseApi.Get(TwitterApiUrl, queryString);


            if (string.IsNullOrWhiteSpace(response.ErrorMessage))
            {
                JArray deserializedObject = JsonConvert.DeserializeObject<JArray>(response.ResponseMessage);
                //var deserializedObject = JsonConvert.DeserializeObject<T>(response.ResponseMessage);
                return deserializedObject;
            }
            else
            {
                var deserializedObject = JsonConvert.DeserializeObject(response.ErrorMessage);
                throw new Exception(deserializedObject.ToString());
            }
        }

    }
}
