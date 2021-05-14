using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace ComputerShopClientApp
{
    public static class APIClient
    {
        private static readonly HttpClient httpClient = new HttpClient();

        public static void Connect(IConfiguration configuration)
        {
            httpClient.BaseAddress = new Uri(configuration["IPAddress"]);
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public static T GetRequest<T>(string requestUri)
        {
            var response = httpClient.GetAsync(requestUri);
            var result = response.Result.Content.ReadAsStringAsync().Result;

            if(response.Result.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<T>(result);
            }
            else
            {
                throw new Exception(result);
            }
        }

        public static void PostRequest<T>(string requestUri, T model)
        {
            var json = JsonConvert.SerializeObject(model);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var response = httpClient.PostAsync(requestUri, data);

            var result = response.Result.Content.ReadAsStringAsync().Result;
            if(!response.Result.IsSuccessStatusCode)
            {
                throw new Exception(result);
            }
        }
    }
}
