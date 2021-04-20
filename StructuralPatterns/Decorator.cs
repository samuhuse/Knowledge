using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructuralPatterns
{
    public class Decorator
    {
        public interface IPerson
        {
            public void Greed();
            public void Eat();
        }

        public class Person : IPerson
        {
            public virtual void Greed()
            {
                Console.WriteLine("Hello");
            }

            public virtual void Eat()
            {
                Console.WriteLine("I'm Eating");
            }
        }

        public class MalePerson : Person, IPerson
        {
            private IPerson _person;
            public MalePerson(IPerson person)
            {
                _person = person;
            }

            public override void Greed()
            {
                _person.Greed();
                Console.WriteLine("And I'm a Male");
            }

            public override void Eat()
            {
                _person.Eat();
                Console.WriteLine("I'm eating as a Male");
            }
        }

        public class FemalePerson : Person, IPerson
        {
            private IPerson _person;
            public FemalePerson(IPerson person)
            {
                _person = person;
            }

            public override void Greed()
            {
                _person.Greed();
                Console.WriteLine("And I'm a Female");
            }

            public override void Eat()
            {
                _person.Eat();
                Console.WriteLine("I'm eating as a female");
            }
        }

        [Test]
        public void Try()
        {
            IPerson person = new Person();

            IPerson male = new MalePerson(person);
            IPerson female = new FemalePerson(person);

            male.Greed(); male.Eat();
            female.Greed(); female.Eat();
        }

    }
}
