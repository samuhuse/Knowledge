using SimpleMvcConsumer.Models;
using SimpleMvcConsumer.Repository.Base;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleMvcConsumer.Repository
{
    public interface IDrinkRepository : IRepository<Drink>
    {
        Task<Drink> GetAsync(int id, string token);
        Task<IEnumerable<Drink>> GetAllAsync(string token);
        Task<bool> CreateAsync(Drink obj, string token);
        Task<bool> UpdateAsync(Drink obj, string token);
        Task<bool> DeleteAsync(int id, string token);
    }
}
