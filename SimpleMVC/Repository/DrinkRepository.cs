using Microsoft.AspNetCore.Http;

using SimpleMvcConsumer.Models;
using SimpleMvcConsumer.Repository.Base;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

using static SimpleMvcConsumer.Util.StaticDetails.JwtBearerApi;

namespace SimpleMvcConsumer.Repository
{
    public class DrinkRepository : RepositoryBase<Drink>, IDrinkRepository
    {
        public DrinkRepository(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
        {
            
        }
        public async Task<bool> CreateAsync(Drink obj, string token = "")
        {
            return await base.CreateAsync(DrinkApiUrl, obj, token);
        }

        public async Task<bool> DeleteAsync(int id, string token = "")
        {
            return await base.DeleteAsync(DrinkApiUrl, id, token);
        }

        public async Task<IEnumerable<Drink>> GetAllAsync(string token = "")
        {
            return await base.GetAllAsync(DrinkApiUrl);
        }

        public Task<Drink> GetAsync(int id, string token = "")
        {
            return base.GetAsync(DrinkApiUrl, id, token);
        }

        public async Task<bool> UpdateAsync(Drink obj, string token = "")
        {
            return await base.UpdateAsync(DrinkApiUrl, obj, token);
        }

    }
}
