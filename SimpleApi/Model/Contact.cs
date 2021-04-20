using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleApi.Model
{
    public record Contact(string Name, string Surname, string Email );

    public static class ContactFactory
    {
        public static Contact Create(string name, string surname)
        {
            return new Contact(name,surname,$"{name}@{surname}.com");
        }
    
    }
}
