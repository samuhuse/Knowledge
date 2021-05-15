using Microsoft.EntityFrameworkCore;

using JwtBearerApiAuthorization.Data;
using JwtBearerApiAuthorization.Model;
using JwtBearerApiAuthorization.Repository.Base;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JwtBearerApiAuthorization.Repository
{
    public class DrinkRepository : RepositoryBase<Drink>, IDrinkRepository
    {
        public DrinkRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
