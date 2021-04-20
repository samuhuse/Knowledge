using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructuralFeatures
{
    public class CustomAttributes
    {
        [
            System.AttributeUsage(System.AttributeTargets.Class | System.AttributeTargets.Struct
            ,AllowMultiple = true
            ,Inherited = true)                  
        ]
        public class AuthorAttribute : Attribute
        {
            string name;
            public double version;

            public AuthorAttribute(string name, double version = 1.0)
            {
                this.name = name;
                this.version = version;
            }

            public string GetName()
            {
                return name;
            }
        }
        
        [Author("Walter Voell", 1.1)]
        public class HackersBlackBook
        {
        }

        [Test]
        public void Try()
        {
            AuthorAttribute attribute = (AuthorAttribute)Attribute.GetCustomAttributes(typeof(HackersBlackBook))
                                        .Where(t => t.GetType() == typeof(AuthorAttribute))
                                        .First();

            Console.WriteLine(attribute.GetName() + " " + attribute.version);
        }

    }
}
