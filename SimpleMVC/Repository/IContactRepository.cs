using SimpleMvcConsumer.Models;
using SimpleMvcConsumer.Repository.Base;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleMvcConsumer.Repository
{
    public interface IContactRepository 
    {
        Task<Contact> GetAsync(int id);
        Task<IEnumerable<Contact>> GetAllAsync();
        Task<bool> CreateAsync(Contact obj);
        Task<bool> UpdateAsync(Contact obj);
        Task<bool> DeleteAsync(int id);
    }
}
