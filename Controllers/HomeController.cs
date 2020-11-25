using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TwitterApiMVC.Controllers.API;
using TwitterApiMVC.Models;

namespace TwitterApiMVC.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            List<SelectListItem> listaNazw = new List<SelectListItem>() {
                new SelectListItem{Text="DNVGL", Value="DNVGL"},
                new SelectListItem{Text="DNVGL_S", Value="DNVGL_S"},
                new SelectListItem{Text="DNVGL_Energy", Value="DNVGL_Energy"},
                new SelectListItem{Text="DNVGL_maritime", Value="DNVGL_maritime"},
                new SelectListItem{Text="DNVGL_Oilgas", Value="DNVGL_Oilgas"},
                new SelectListItem{Text="DNVGL_Renewable", Value="DNVGL_Renewable"},
                new SelectListItem{Text="DNVGL_Assurance", Value="DNVGL_Assurance"},
                new SelectListItem{Text="DNVGL_SustAdv", Value="DNVGL_SustAdv"},
                new SelectListItem{Text="DNVGL_Energi", Value="DNVGL_Energi"}
            };
            ViewBag.TwitterName = listaNazw;

            return View();
        }
        public Task<JArray> getTweets(string nameOfDNVGL, string count)
        {

            var model = API.ApiHandler.CallTwitterApi(nameOfDNVGL, count);

            return model;
        }
        public async Task<ActionResult> getTweetsByModel(string nameOfDNVGL, string count, string hastag)
        {
            try
            {

                string TwitterApiUrl = "https://api.twitter.com";

                bool excludeReplies = false;
                bool includeRTs = true;

                var queryString = string.Format("/1.1/statuses/user_timeline.json?screen_name={0}&count={1}&exclude_replies={2}&include_rts={3}", nameOfDNVGL, count, excludeReplies.ToString().ToLower(), includeRTs.ToString().ToLower());
                if (!string.IsNullOrWhiteSpace(hastag))
                {
                    queryString = string.Format("/1.1/search/tweets.json?q=from%3A{0}%20%23{1}", nameOfDNVGL, hastag.ToLower());
                }

                var response = await BaseApi.Get(TwitterApiUrl, queryString);

                if (!string.IsNullOrWhiteSpace(hastag))
                {

                    if (string.IsNullOrWhiteSpace(response.ErrorMessage))
                    {
                        if (string.IsNullOrWhiteSpace(response.ResponseMessage))
                        {
                            throw new Exception("Brak rekordów");
                        }
                        
                        JObject TwitterApi = JObject.Parse(response.ResponseMessage);

                        IList<JToken> results = TwitterApi["statuses"].Children().ToList();

                        List<RootHash> searchResults = new List<RootHash>();
                        foreach (JToken result in results)
                        {
                            RootHash searchResult = result.ToObject<RootHash>();
                            searchResults.Add(searchResult);
                            
                        }

                        return PartialView("FeedsHash", searchResults);

                    }
                    else
                    {
                        var deserializedObject1 = JsonConvert.DeserializeObject(response.ErrorMessage);
                        throw new Exception(deserializedObject1.ToString());
                    }
                }
                else
                {
                    List<Root> deserializedObject = JsonConvert.DeserializeObject<List<Root>>(response.ResponseMessage);
                    return PartialView("Feeds", deserializedObject);

                }
            }
            catch (Exception e)
            {
                return View(e.Message);
            }
        }

    }
}
