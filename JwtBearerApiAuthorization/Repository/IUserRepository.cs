using JwtBearerApiAuthorization.Model.Authorization;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JwtBearerApiAuthorization.Repository
{
    public interface IUserRepository
    {
        bool IsUserunique(string username);
        User Authenticate(string username, string password);
        User Register(string username, string password);
    }
}
