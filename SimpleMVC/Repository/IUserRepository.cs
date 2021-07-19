using SimpleMvcConsumer.Model.Authorization;
using SimpleMvcConsumer.Repository.Base;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleMvcConsumer.Repository
{
    public interface IUserRepository 
    {
        public Task<User> RegisterAsync(Credential credential);
        public Task<User> LogInAsync(Credential credential);
    }
}
