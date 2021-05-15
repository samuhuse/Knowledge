using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using SimpleApi.Data;
using SimpleApi.Model;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleApi.Util
{
    public static class DataSeeder
    {
        static List<string> _names =new List<string>               
        {
            "SMITH"
            ,"BROWN"
            ,"WILSON"
            ,"THOMSON"
            ,"ROBERTSON"
            ,"CAMPBELL"
            ,"STEWART"
            ,"ANDERSON"
            ,"MACDONALD"
            ,"SCOTT"
            ,"REID"
            ,"MURRAY"
            ,"TAYLOR"
            ,"CLARK"
            ,"MITCHELL"
            ,"ROSS"
            ,"WALKER"
            ,"PATERSON"
            ,"YOUNG"
        };

        static List<Company> _companiesList = new List<Company>()
        {
            new Company { Id=1, Name = "Google", Contacts = new List<Contact>() },
            new Company { Id=2, Name = "Netflix", Contacts = new List<Contact>() },
            new Company { Id=3, Name = "Amazon", Contacts = new List<Contact>() },
            new Company { Id=4, Name = "Facebook", Contacts = new List<Contact>() },
            new Company { Id=5, Name = "Twitter", Contacts = new List<Contact>() }
        };

        public static void SeedsContacts(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationDbContext(serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                if (context.Contacts.Any())
                {
                    return;
                }

                GenerateCompanies();

                context.Companies.AddRange(_companiesList);
                    
                context.SaveChanges();
            }
        }
        private static void GenerateCompanies()
        {
            Random random = new Random();
            string GetRandomName()
            {
                return _names.ElementAt(random.Next(_names.Count));
            }

            List<Contact> bufferCollision = new List<Contact>();

            int c = _names.Count().GetPermutation();
            while (c > 1)
            {
                Contact contact = null;
                Company company = _companiesList[random.Next(1,_companiesList.Count) - 1];
                while (contact is null)
                {
                    contact = Contact.ContactFactory.Create(c, GetRandomName(), GetRandomName(), company);
                    if (bufferCollision.Any(c => c.Name == contact.Name && c.Surname == contact.Surname)) contact = null; 
                    else 
                    { 
                        bufferCollision.Add(contact);
                        company.Contacts.Add(contact);
                    }
                }
                c--;
            }
        }

    }
}
