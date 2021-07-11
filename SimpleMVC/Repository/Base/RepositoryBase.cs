using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace SimpleMvcConsumer.Repository.Base
{
    public abstract class RepositoryBase<T> : IRepository<T> where T : class
    {
        protected readonly IHttpClientFactory HttpClientFactory;

        public RepositoryBase(IHttpClientFactory httpClientFactory)
        {
            HttpClientFactory = httpClientFactory;
        }

        public async Task<bool> CreateAsync(string url, T obj, string token = "") 
        {
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            if (obj is not null)
            {
                request.Content = new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");                    
            }
            else
            {
                return false;
            }

            var client = HttpClientFactory.CreateClient();

            if (token.Any()) client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await client.SendAsync(request);

            if (response.StatusCode == HttpStatusCode.Created)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> DeleteAsync(string url, int id, string token = "") 
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, url + id);

            var client = HttpClientFactory.CreateClient();
            if (token.Any()) client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await client.SendAsync(request);

            if (response.StatusCode == HttpStatusCode.NoContent)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<IEnumerable<T>> GetAllAsync(string url, string token = "") 
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url );

            var client = HttpClientFactory.CreateClient();
            if (token.Any()) client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await client.SendAsync(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IEnumerable<T>>(jsonString);                
            }
            else
            {
                return null;
            }
        }

        public async Task<T> GetAsync(string url, int id, string token = "") 
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url + id);

            var client = HttpClientFactory.CreateClient();
            if (token.Any()) client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await client.SendAsync(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(jsonString);
            }
            else
            {
                return null;
            }
        }

        public async Task<bool> UpdateAsync(string url, T obj, string token = "") 

        {
            var request = new HttpRequestMessage(HttpMethod.Patch, url);
            if (obj is not null)
            {
                request.Content = new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");
            }
            else
            {
                return false;
            }

            var client = HttpClientFactory.CreateClient();
            if (token.Any()) client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await client.SendAsync(request);

            if (response.StatusCode == HttpStatusCode.NoContent)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
