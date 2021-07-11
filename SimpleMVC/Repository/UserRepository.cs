using Newtonsoft.Json;

using SimpleMvcConsumer.Model.Authorization;
using SimpleMvcConsumer.Repository.Base;
using SimpleMvcConsumer.Util;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SimpleMvcConsumer.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public UserRepository(IHttpClientFactory httpClientFactory) 
        {
            _httpClientFactory = httpClientFactory;
        }

        private async Task<User> PostAsync(string url, Credential credential)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            if (credential is null) return null;

            request.Content = new StringContent(JsonConvert.SerializeObject(credential), Encoding.UTF8, "Application/Json");

            var client = _httpClientFactory.CreateClient();
            HttpResponseMessage response = await client.SendAsync(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<User>(jsonString);
            }
            return null;
        }

        public async Task<User> LogInAsync(Credential credential)
        {
            return await PostAsync(StaticDetails.JwtBearerApi.LogInApiUrl, credential);
        }

        public async Task<User> RegisterAsync(Credential credential)
        {
            return await PostAsync(StaticDetails.JwtBearerApi.RegisterApiUrl, credential);
        }
    }
}
