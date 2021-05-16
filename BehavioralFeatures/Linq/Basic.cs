using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BehavioralFeatures.Linq
{
    public class Basic
    {
        #region Model

        public enum Gender
        {
            Male,
            Female
        }

        public class Person
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public int Id { get; set; }
            public int Height { get; set; }
            public int Age { get; set; }

            public Gender Gender { get; set; }
        }

        internal class Buyer : Person
        {
            public int Buget { get; set; }
        }

        internal class Supplier : Person
        {
            public string Product { get; set; }
        }

        #endregion

        List<Person> people;

        void Print(IEnumerable<Person> people)
        {
            people.ToList().ForEach(p => Console.WriteLine($"Name: {p.FirstName}, Surname: {p.LastName}, Age: {p.Age}"));
        }

        [SetUp]
        public void SetUp()
        {
            people = new List<Person>()
            {
                new Person{FirstName="Tod", LastName="Vachev", Id=1, Height=180, Age = 26, Gender =Gender.Male },
                new Person{FirstName="John", LastName="Johnson",Id= 2, Height=170, Age = 21,Gender = Gender.Male},
                new Person{FirstName="Anna", LastName="Maria",Id= 3, Height=150,Age =  22,Gender = Gender.Female},
                new Person{FirstName="Kyle", LastName="Wilson",Id= 4,Height= 164, Age = 29,Gender = Gender.Male},
                new Person{FirstName="Anna", LastName="Williams",Id= 5,Height= 164, Age = 28,Gender = Gender.Male},
                new Person{FirstName="Maria",LastName= "Ann", Id=6, Height=160,Age =  19, Gender =Gender.Female},
                new Person{FirstName="John", LastName="Jones",Id= 7,Height= 160,Age =  22,Gender = Gender.Female},
                new Person{FirstName="Samba", LastName="TheLion",Id= 8,Height= 175, Age = 23,Gender = Gender.Male},
                new Person{FirstName="Aaron", LastName="Myers",Id= 9, Height=182, Age = 21, Gender =Gender.Male},
                new Person{FirstName="Aby", LastName="Wood",Id= 10, Height=165, Age = 20, Gender =Gender.Female},
                new Supplier{FirstName="Maddie",LastName="Lewis",Id=  11,Height= 160,Age =  19, Gender =Gender.Female, Product="Tea"},
                new Buyer{FirstName="Lara",LastName= "Croft", Id=12, Height=162, Age = 23, Gender =Gender.Female, Buget= 100}
            };
        }

        [Test]
        public void Where()
        {
            IEnumerable<Person> result = people.Where(p => p.Age > 21);
            Print(result);
        }

        [Test]
        public void Select()
        {
            IEnumerable<int> result = people.Select(p => p.Age);
            result.ToList().ForEach(a => Console.WriteLine($"Age: {a}"));
        }

        [Test]
        public void Orderings()
        {
            IEnumerable<Person> result = people.OrderBy(p => p.Age).ThenByDescending(p=> p.FirstName);
            Print(result);
        }

        [Test]
        public void Aggregations()
        {
            int ageMax = people.Max(p => p.Age);
            Console.WriteLine(ageMax);

            int ageMin = people.Min(p => p.Age);
            Console.WriteLine(ageMin);

            int ageSum = people.Sum(p => p.Age);
            Console.WriteLine(ageSum);

            double ageAverage = people.Average(p => p.Age);
            Console.WriteLine(ageAverage);

            long customAggregation = people.Aggregate(seed:1, (x, p) => p.Age * x);
            Console.WriteLine(customAggregation);
        }

        [Test]
        public void Qualifiers()
        {
            bool atLeastOne = people.Any(p=> p.Age == 21);
            Console.WriteLine(atLeastOne);

            bool ifAlls = people.All(p => p.Age > 21);
            Console.WriteLine(ifAlls);

            bool containsReference = people.Contains(people.FirstOrDefault());
            Console.WriteLine(containsReference);

        }

        [Test]
        public void EnumerableComparison()
        {
            string[] catNames = { "Lucky", "Bella", "Luna", "Oreo", "Simba", "Toby", "Loki", "Oscar" };
            string[] catNames2 = { "Lucky", "Bella", "Luna", "Oreo", "Simba", "Toby", "Loki", "Oscar" };

            Console.WriteLine(catNames == catNames2);             // False
            Console.WriteLine(Equals(catNames, catNames2));       // False
            Console.WriteLine(catNames.Equals(catNames2));        // False
            Console.WriteLine(catNames.SequenceEqual(catNames2)); // True
        }

        [Test]
        public void SetOperations()
        {
            void Print<T>(IEnumerable<T> set)
            {
                set.ToList().ForEach(i => Console.WriteLine(i));
            }

            string st1 = "I am a cat";
            string st2 = "I am a dog";
            List<int> ints = new List<int>() { 1, 2, 2, 2, 3, 3, 4, 5, 6, 5, 6, 5, 6, 5, 3, 4, 5, 6, 7, 8, 8, 4, 3 };
            List<int> ints2 = new List<int>() { 3, 2, 3, 5, 8, 43, 5, 67, 1, 2, 3, 7, 7, 7, 6, 5, 2, 1, 1, 1, 1, 1 };

            var distinct = st1.Distinct(); // gets all unique items, disregarding their repetitions
            Print(st1);
            var distinct2 = st2.Distinct();
            Print(st2);

            var intDistinct = ints.Distinct();
            Print(intDistinct);

            var intersect = st1.Intersect(st2); // gets all matching unique items between two collections
            Print(intersect);
            var intIntersect = ints.Intersect(ints2);
            Print(intIntersect);

            var union = st1.Union(st2); // gets all unique items from both collections that are no repeating, like two distincts
            Print(union);
            var union2 = st2.Union(st1); // same result, just in different order
            Print(union2);
            var intUnion = ints.Union(ints2);
            Print(intUnion);

            var except = st1.Except(st2); // gets all items from st1 that are not present in st2
            Print(except);
            var except2 = st2.Except(st1); // gets all items from st2 that are not present in st1
            Print(except2);

            var intExcept = ints.Except(ints2);
            Print(intExcept);
            var intExcept2 = ints2.Except(ints);
            Print(intExcept2);
        }

        [Test]
        public void Conversions()
        {
            IEnumerable<Person> result = people.OfType<Buyer>() // Filter by type
                                               .ToList()
                                               .ConvertAll(b => new Supplier() { Product = "Software" });
            Print(result);

            List<int> numbers = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            List<string> stringNumbers = numbers.ConvertAll(n => n.ToString());
            stringNumbers.ForEach(n => Console.WriteLine(n));
        }

        [Test]
        public void Partitioning()
        {
            var result = people.OrderBy(p => p.Age).TakeWhile(p => p.Age < 25).Take(2);
            Print(result);

            result = people.OrderBy(p => p.Age).SkipWhile(p => p.Age < 25).Take(2);
            Print(result);
        }

    }
}
