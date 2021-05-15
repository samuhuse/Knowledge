using JwtBearerApiAuthorization.Model;
using JwtBearerApiAuthorization.Repository.Base;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JwtBearerApiAuthorization.Repository
{
    public interface IDrinkRepository: IRepository<Drink> 
    {
    }
}
