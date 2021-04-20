using System;
using System.Collections.Generic;
using System.Text;

namespace StructuralPatterns
{
    #region Model

    public interface IPerson
    {
        int Age { get; set; }
        string Name { get; set; }
        string Surname { get; set; }

        void Intruduce();
    }

    public class Person : IPerson
    {
        public Person(string name, string surname, int age)
        {
            Name = name;
            Surname = surname;
            Age = age;
        }

        public string Name { get; set; }
        public string Surname { get; set; }
        public int Age { get; set; }

        public void Intruduce()
        {
            Console.WriteLine($"Hi i'm {Name} {Surname} and i have {Age} years");
        }
    }
    public class Alien
    {
        public Alien(long id, int astronomicAge)
        {
            Id = id;
            AstronomicAge = astronomicAge;
        }

        public long Id { get; set; }
        public int AstronomicAge { get; set; }
    }

    #endregion

    public class PersonAlienAdapter : IPerson
    {
        private Alien _alien;

        public PersonAlienAdapter(Alien alien)
        {
            _alien = alien;
            Name = _alien.Id.ToString();
        }
        public string Name
        {
            get => _alien.Id.ToString().Substring(0, Decimal.ToInt32(_alien.Id.ToString().Length / 2));
            set
            {
                long id;
                if (!Int64.TryParse(value, out id))
                {
                    throw new Exception("Cant' convert to Alien id");
                }
                else { _alien.Id = id; }
            }
        }

        private decimal _convertrate = 2.43M;

        public int Age
        {
            get
            {
                return Decimal.ToInt32(_alien.AstronomicAge / _convertrate);
            }
            set
            {
                _alien.AstronomicAge = Decimal.ToInt32(value * _convertrate);
            }
        }
        public string Surname
        {
            get => Name.Substring(Decimal.ToInt32(Name.Length / 2));
            set => Name = value;
        }
        public void Intruduce()
        {
            Console.WriteLine($"Hi i'm {Name.Substring(0, Decimal.ToInt32(Name.Length / 2))} {Name.Substring(Decimal.ToInt32(Name.Length / 2))} and i have {Age} years");
        }
    }

    public class Adapter
    {
        public static void Try()
        {
            List<IPerson> people = new List<IPerson>();
            people.Add(new Person("samuele", "lombardi", 22));

            Alien alien = new Alien(0012912321431, 13200);
            PersonAlienAdapter adapter = new PersonAlienAdapter(alien);
            people.Add(adapter);

            people.ForEach(p => p.Intruduce());            
        }
    }
}
