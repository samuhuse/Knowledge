using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructuralPatterns
{
    public class Composite
    {
        public abstract class Person
        {
            public string Name { get; set; }

            public Person(string name)
            {
                Name = name;
            }

            public string Step { get; set; } = ""; 

            public abstract string Lineage();

            public abstract string Introduce();
        }

        public class Parent : Person
        {
            private readonly List<Person> _sons;

            public Parent(string name, List<Person> sons) : base(name)
            {
                _sons = sons;
            }

            public Parent AddSon(Person person)
            {
                person.Step += Step + "    ";
                _sons.Add(person);
                return this;
            }

            public Parent AddSon(List<Person> persons)
            {
                persons.ForEach(p => p.Step = Step + " ");
                _sons.AddRange(persons);
                return this;
            }

            public Parent AddLastSon(Person person)
            {
                person.Step += Step + "    ";
                _sons.Add(person);
                return person as Parent;
            }

            public override string Introduce()
            {
                void AddSonsIntroduce(StringBuilder stringBuilder)
                {
                    stringBuilder.Append(Step);
                    if (_sons is null || _sons?.Count == 0)
                    {
                        stringBuilder.Append("I have no children");
                        stringBuilder.Append(Environment.NewLine);
                    }
                    else
                    {
                        stringBuilder.Append($"I have {_sons.Count} children");
                        stringBuilder.Append(Environment.NewLine);
                        stringBuilder.Append(Lineage());
                    }
                }

                StringBuilder stringBuilder = new StringBuilder();

                stringBuilder.Append($"Parent - Name: {Name}" + Environment.NewLine);
                AddSonsIntroduce(stringBuilder);

                return stringBuilder.ToString();
            }

            public override string Lineage()
            {
                StringBuilder stringBuilder = new StringBuilder();

                _sons.ForEach(s => { stringBuilder.Append(Step); stringBuilder.Append(s.Introduce()); });

                return stringBuilder.ToString();
            }
        }

        public class Son : Person
        {
            public Son(string name) : base(name)
            {

            }

            public override string Introduce()
            {
                return $"Son - Name: {Name}" + Environment.NewLine;
            }

            public override string Lineage()
            {
                return string.Empty;
            }
        }

        [Test]
        public void Try()
        {
            // Family Tree => "https://it.wikipedia.org/wiki/Anchise#Discendenza_di_Anchise"
            Parent pather = new Parent("Anchise", new List<Person>());

            pather
            .AddLastSon(new Parent("Enea", new List<Person>()))
            .AddSon(new Parent("Ascanio", new List<Person>() { new Parent("Silvio", new List<Person>() { new Son("Bruto di Troia") }) }))
            .AddLastSon(new Parent("Silvio", new List<Person>()))
            .AddLastSon(new Parent("Enea Silvio", new List<Person>()))
            .AddLastSon(new Parent("Latino Silvio", new List<Person>()))
            .AddLastSon(new Parent("Alba", new List<Person>()))
            .AddLastSon(new Parent("Atys", new List<Person>()))
            .AddLastSon(new Parent("Capys", new List<Person>()))
            .AddLastSon(new Parent("Capeto", new List<Person>()))
            .AddLastSon(new Parent("Tiberino Silvio", new List<Person>()))
            .AddLastSon(new Parent("Agrippa", new List<Person>()))
            .AddLastSon(new Parent("Romolo Silvio", new List<Person>()))
            .AddLastSon(new Parent("Aventino", new List<Person>()))
            .AddLastSon(new Parent("Proca", new List<Person>()))
            .AddSon(new Son("Amulio"))
            .AddLastSon(new Parent("Numitore", new List<Person>()))
            .AddLastSon(new Parent("Rea Silvia", new List<Person>()))
            .AddSon(new Son("Remo"))
            .AddLastSon(new Parent("Romolo", new List<Person>()))
            .AddSon(new Son("Prima"))
            .AddSon(new Son("Avilio"));

            Console.WriteLine(pather.Introduce());
        }

    }
}
