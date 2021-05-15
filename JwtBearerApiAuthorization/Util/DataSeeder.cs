using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using JwtBearerApiAuthorization.Data;
using JwtBearerApiAuthorization.Model;
using JwtBearerApiAuthorization.Model.Authorization;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JwtBearerApiAuthorization.Util
{
    public class DataSeeder
    {
        static private List<Drink> _drinks = new List<Drink>
        {
            new Drink
            {
                Name = "White Russian",
                IsAcholic = true,
                Grade = 20
            },
            new Drink
            {
                Name = "Margarita",
                IsAcholic = true,
                Grade = 25
            },
            new Drink
            {
                Name = "Sprite",
                IsAcholic = false,
                Grade = 0
            }
        };

        static private List<User> _users = new List<User>
        {
            new User
            {
                Id = 1,
                Username = "samuhuse",
                Password = "Password1234",
                Role = "admin"
            },
            new User
            {
                Id = 2,
                Username = "antonio",
                Password = "Password1234",
                Role = "guest"
            }
        };

        public static void SeedsContacts(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationDbContext(serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                if (context.Drinks.Any())
                {
                    return;
                }

                context.Drinks.AddRange(_drinks);
                context.Users.AddRange(_users);

                context.SaveChanges();
            }
        }
    }
}
