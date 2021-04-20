using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BehavioralPatterns
{
    public class Strategy
    {
        public interface IGreed
        {
            public string Greed { get; }
        }

        public class GreedingPerson
        {
            private readonly string _name;

            public GreedingPerson(string name, IGreed greed)
            {
                _name = name;
                Greed = greed;
            }

            public IGreed Greed { get; set; }

            public void Introduce()
            {
                Console.WriteLine($"{Greed.Greed} {_name}");
            }
        }

        public class ItalianGreed : IGreed
        {
            public string Greed => "Ciao, mi chiamo";
        }

        public class EnglishGreed : IGreed
        {
            public string Greed => "Hi, my name is";
        }

        [Test]
        public void Try()
        {
            GreedingPerson person = new GreedingPerson("Samuele", new ItalianGreed());

            person.Introduce();

            person.Greed = new EnglishGreed();

            person.Introduce();
        }

    }
}
