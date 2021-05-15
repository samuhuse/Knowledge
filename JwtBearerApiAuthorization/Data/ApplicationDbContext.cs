using Microsoft.EntityFrameworkCore;

using JwtBearerApiAuthorization.Model;
using JwtBearerApiAuthorization.Model.Authorization;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JwtBearerApiAuthorization.Data
{
    public class ApplicationDbContext : DbContext
    {   
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Drink> Drinks { get; set; }
        public DbSet<User> Users { get; set; }

    }
}
