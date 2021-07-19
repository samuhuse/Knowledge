using SimpleMvcConsumer.Models;

using SimpleMvcConsumer.Repository.Base;
using SimpleMvcConsumer.Util;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using static SimpleMvcConsumer.Util.StaticDetails.SimpleApi;


namespace SimpleMvcConsumer.Repository
{
    public class ContactRepository : RepositoryBase<Contact>, IContactRepository
    {
        public ContactRepository(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
        {
        }

        public async Task<bool> CreateAsync(Contact obj)
        {
            return await base.CreateAsync(ContactApiUrl, obj);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await base.DeleteAsync(ContactApiUrl, id);
        }

        public async Task<IEnumerable<Contact>> GetAllAsync()
        {
            return await base.GetAllAsync(ContactApiUrl);
        }

        public Task<Contact> GetAsync(int id)
        {
            return base.GetAsync(ContactApiUrl, id);
        }

        public async Task<bool> UpdateAsync(Contact obj)
        {
            return await base.UpdateAsync(ContactApiUrl, obj);
        }
    }
}
