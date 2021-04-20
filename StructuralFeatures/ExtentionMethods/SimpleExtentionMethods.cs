using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructuralFeatures
{
 
    public class Person
    {
        public string  Name { get; set; } 
        public int Age { get; set; }

        public Person(string name, int age)
        {
            Name = name;
            Age = age;
        }
    }

    public static class PersonExtentions
    {
        public static string GetName(this Person foo)
        {
            return foo.Name;
        }

        public static Person ToPerson(this (string name, int age) data)
        {
            return new Person (data.name, data.age);
        }

        public static Stopwatch Measure(this Func<string> action)
        {
            var st = new Stopwatch();
            st.Start();
            action();
            st.Stop();
            return st;
        }
    }



    public class SimpleExtentionMethods
    {
        [Test]
        public void Try()
        {
            Person person = new Person("Samuele", 22);

            string name = person.GetName();

            person = ("Samuele", 22).ToPerson();

            Func<string> calculate = delegate
            {
                return person.GetName();
            };

            long ms = calculate.Measure().ElapsedMilliseconds;
        }


    }
}
