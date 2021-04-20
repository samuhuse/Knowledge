using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Text;

namespace StructuralPatterns
{
    public class Facade
    {
        public class Person
        {
            PersonFacade _facade;

            public Person(string name, string surname, int age, bool isMale)
            {
                Name = name;
                Surname = surname;
                Age = age;
                IsMale = isMale;

                _facade = new PersonFacade(this);
            }

            public string Name { get; set; }
            public string Surname { get; set; }
            public int Age { get; set; }
            public bool IsMale { get; set; }

            public string Greed()
            {
                return "Hello";
            }

            public void Introduce()
            {
                Console.WriteLine(_facade.IntroduceFacade());
            }
        }
        public class PersonFacade
        {
            private readonly Person _person;

            public PersonFacade(Person person)
            {
                _person = person;
            }

            public string IntroduceFacade()
            {
                StringBuilder stringBuilder = new StringBuilder();

                stringBuilder.Append(_person.Greed());
                stringBuilder.Append(",");
                stringBuilder.Append(Environment.NewLine);
                stringBuilder.Append("My Name is ");
                stringBuilder.Append(_person.Name);
                stringBuilder.Append(" ");
                stringBuilder.Append(_person.Surname);
                stringBuilder.Append(Environment.NewLine);
                stringBuilder.Append("I'm a ");
                stringBuilder.Append(GetGender(_person));
                stringBuilder.Append(" and I'm ");
                stringBuilder.Append(_person.Age);

                return stringBuilder.ToString();
            }

            internal string GetGender(Person person)
            {
                if (person.IsMale) return "Man";
                else return "Woman";
            }
        }

        [Test]
        public static void Try()
        {
            new Person("Samuele", "Lombardi", 22, isMale: true).Introduce();
        }

    }
}
