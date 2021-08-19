using Ecommerce.Model;
using Newtonsoft.Json;
using System.Net.Http;
using System.Web;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System;

namespace UI.Helper
{
    public static class WebApiClient
    {
        public static async Task<T> Get<T>(string baseURL, string requestUri, string token)
        {

            T result = default;
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
                string url = baseURL + requestUri;
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                using (HttpResponseMessage response = await client.GetAsync(url))
                {
                    response.EnsureSuccessStatusCode();
                    string responseBody = await response.Content.ReadAsStringAsync();

                    result = JsonConvert.DeserializeObject<T>(responseBody);
                }
            }
            return result;

        }

        public static async Task<T> Post<T>(string uri, string path, string token, T obj) where T : new()
        {

            T result = default;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri(uri);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage response = await client.PostAsJsonAsync(path, obj);
                if (response.IsSuccessStatusCode)
                {
                    result = await response.Content.ReadAsAsync<T>();
                }


            }
            return result;



        }

        public static async Task<T> Put<T>(string uri, string path, string token,  T obj) where T : new()
        {

            T result = default;

            
            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri(uri);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage response = await client.PutAsJsonAsync(path, obj);
                if (response.IsSuccessStatusCode)
                {
                    result = await response.Content.ReadAsAsync<T>();
                }
              

            }
            return result;



        }

    }
}
