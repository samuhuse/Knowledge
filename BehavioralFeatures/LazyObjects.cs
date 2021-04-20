using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BehavioralFeatures
{
    public class LazyObjects
    {
        #region Model

        public class Person
        {
            public string Name;
            public string Surname;

            public Person(string name, string surname)
            {
                Name = name;
                Surname = surname;
                Console.WriteLine($"Hi my name is {Name} {Surname} and I just wake up");
            }

            public static Person Init(string name, string surname)
            {
                return new Person(name, surname);
            }

            public void Greed()
            {
                Console.WriteLine($"Hi, My name is {Name} {Surname}");
            }
        }

        #endregion

        [Test]
        public void Try()
        {
            /*
            Use lazy initialization to defer the creation of a large or resource-intensive 
            object, or the execution of a resource-intensive task, particularly when such 
            creation or execution might not occur during the lifetime of the program.
            */

            var me = new Lazy<Person>(() => new Person("samuele", "lombardi"));
            var alsoMe = new Lazy<Person>(() => Person.Init("samuele2", "lombardi2"));

            me.Value.Greed();
            alsoMe.Value.Greed();

            me.Value.Greed();
            alsoMe.Value.Greed();
        }

    }
}
