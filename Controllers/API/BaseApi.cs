using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using TwitterApiMVC.Model;
using TwitterApiMVC.Models;

namespace TwitterApiMVC.Controllers.API
{
    public class BaseApi
    {
        public static async Task<Response> Get(string url, string queryString)
        {
            //try
            //{
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "AAAAAAAAAAAAAAAAAAAAAE23JwEAAAAApSWc6GuAe%2BIj3oT%2FlgUTOhMkVBQ%3Dzvds3CKUO9XN5RvEdF4MteuR9ne8L2534HIpRZDVRAhQpTuU62");
                //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage responseMessage = await client.GetAsync(queryString);
                var response = new Response();
                List<Root> root = new List<Root>();
                if (responseMessage.IsSuccessStatusCode)
                {
                    response.ResponseMessage = await responseMessage.Content.ReadAsStringAsync();
                }
                else
                {
                    response.ErrorMessage = await responseMessage.Content.ReadAsStringAsync();
                }
                return response;


            }
            //}
            //catch (Exception e)
            //{
            //    throw e;
            //}
        }
    }
}
