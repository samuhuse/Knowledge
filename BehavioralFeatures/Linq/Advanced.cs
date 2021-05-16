using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BehavioralFeatures.Linq
{
    public class Advanced
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

            public Gender Gender;

            public Person(string firstName, string lastName, int id, int height, int age, Gender gender)
            {
                FirstName = firstName;
                LastName = lastName;
                Id = id;
                Height = height;
                Gender = gender;
                Age = age;
            }
        }

        internal class Supplier
        {
            public string Name { get; set; }
            public string District { get; set; }
            public int Age { get; set; }
        }

        internal class Buyer
        {
            public string Name { get; set; }
            public string District { get; set; }
            public int Age { get; set; }
        }

        #endregion

        List<Person> people;

        List<Buyer> buyers;
        List<Supplier> suppliers;

        void PrintEnumerable<T>(IEnumerable<IGrouping<T, Person>> groups)
        {
            foreach (IGrouping<T, Person> item in groups)
            {
                Console.WriteLine($"{item.Key}:");
                foreach (var p in item)
                {
                    Console.WriteLine($" {p.FirstName}");
                }
            }
        }

        [SetUp]
        public void SetUp()
        {
            people = new List<Person>()
            {
                new Person("Tod", "Vachev", 1, 180, 26, Gender.Male),
                new Person("John", "Johnson", 2, 170, 21, Gender.Male),
                new Person("Anna", "Maria", 3, 150, 22, Gender.Female),
                new Person("Kyle", "Wilson", 4, 164, 29, Gender.Male),
                new Person("Anna", "Williams", 5, 164, 28, Gender.Male),
                new Person("Maria", "Ann", 6, 160, 19, Gender.Female),
                new Person("John", "Jones", 7, 160, 22, Gender.Female),
                new Person("Samba", "TheLion", 8, 175, 23, Gender.Male),
                new Person("Aaron", "Myers", 9, 182, 21, Gender.Male),
                new Person("Aby", "Wood", 10, 165, 20, Gender.Female),
                new Person("Maddie","Lewis",  11, 160, 19, Gender.Female),
                new Person("Lara", "Croft", 12, 162, 23, Gender.Female)
            };

            buyers = new List<Buyer>()
            {
                new Buyer() { Name = "Johny", District = "Fantasy District", Age = 22},
                new Buyer() { Name = "Peter", District = "Scientists District", Age = 40},
                new Buyer() { Name = "Paul", District = "Fantasy District", Age = 30 },
                new Buyer() { Name = "Maria", District = "Scientists District", Age = 35 },
                new Buyer() { Name = "Joshua", District = "EarthIsFlat District", Age = 40 },
                new Buyer() { Name = "Sylvia", District = "Developers District", Age = 22 },
                new Buyer() { Name = "Rebecca", District = "Scientists District", Age = 30 },
                new Buyer() { Name = "Jaime", District = "Developers District", Age = 35 },
                new Buyer() { Name = "Pierce", District = "Fantasy District", Age = 40 }
            };

            suppliers = new List<Supplier>()
            {
                new Supplier() { Name = "Harrison", District = "Fantasy District", Age = 22 },
                new Supplier() { Name = "Charles", District = "Developers District", Age = 40 },
                new Supplier() { Name = "Hailee", District = "Scientists District", Age = 35 },
                new Supplier() { Name = "Taylor", District = "EarthIsFlat District", Age = 30 }
            };
        }

        record yearAge(Gender gender, int age);

        [Test]
        public void Grouping()
        {
            IEnumerable<IGrouping<Gender, Person>> genderGroup = people.GroupBy(p => p.Gender);
            PrintEnumerable(genderGroup);

            IEnumerable<IGrouping<char, Person>> alphabeticalGroup = people.OrderBy(p => p.FirstName).GroupBy(p => p.FirstName[0]);
            PrintEnumerable(alphabeticalGroup);

            IEnumerable<IGrouping<yearAge, Person>> multiGroup = people.GroupBy(p => new yearAge(p.Gender, p.Age));
            PrintEnumerable(multiGroup);

            IEnumerable<IGrouping<yearAge, Person>> multiGroupOrdered = multiGroup.OrderBy(g => g.Count());
            PrintEnumerable(multiGroupOrdered);

            IEnumerable<IGrouping<int, Person>> GroupOrderedByKey = people.GroupBy(p => p.Age).OrderBy(g => g.Key);
            PrintEnumerable(multiGroupOrdered);

            IEnumerable<IGrouping<string, Person>> GroupByCondition = people.GroupBy(p => (p.Age % 2 == 0) ? "Even" : "Odd");
            PrintEnumerable(multiGroupOrdered);

            var GroupCount = people.GroupBy(p => p.Age).Select(g => new { Age = g.Key, Count = g.Count() });
            foreach (var amount in GroupCount)
            {
                Console.WriteLine($"{amount.Age}");
                Console.WriteLine($"{amount.Count}");
            }
        }

        [Test]
        public void Join()
        {
            var innerJoin = buyers.Join(suppliers,
                                        s => s.District,
                                        b => b.District,
                                        (b, s) => new { District = s.District, Supplier = s, Buyer = b });

            foreach (var item in innerJoin)
            {
                Console.WriteLine($"District: {item.District}, Supplier: {item.Supplier}, Buyer: {item.Buyer}");
            }

            var compositeInnerJoin = buyers.Join(suppliers,
                                                 s => new { s.District, s.Age },
                                                 b => new { b.District, b.Age },
                                                 (b, s) => new { Supplier = s, Buyer = b });

            foreach (var item in innerJoin)
            {
                Console.WriteLine($"Supplier: {item.Supplier}, Buyer: {item.Buyer}, Age:{item.Supplier.Age}");
            }
        }

        [Test]
        public void GroupJoin()
        {
            var matchingSuppliersWithBuyers = suppliers.GroupJoin(
                                          buyers,
                                          s => s.District,
                                          b => b.District,
                                          (s, buyersGroup) =>
                                             new
                                             {
                                                 Name = s.Name,
                                                 District = s.District,
                                                 Buyers = buyersGroup.OrderBy(b => b.Age)
                                             }
                                       );

            foreach (var item in matchingSuppliersWithBuyers)
            {
                Console.WriteLine($"Supplier: {item.Name}, District: {item.District} " +
                    $"\nBuyers:");

                foreach (var buyer in item.Buyers)
                {
                    Console.WriteLine($"  {buyer.Name}");
                }
            }
        }

        [Test]
        public void LeftOuterJoin()
        {
            var leftOuterJoinType = suppliers.GroupJoin(buyers,
                                                        s => s.District,
                                                        b => b.District,
                                                        (s, buyersGroup) =>
                                                            new
                                                            {
                                                                Name = s.Name,
                                                                District = s.District,
                                                                Buyers = buyersGroup.DefaultIfEmpty(new Buyer() { Name = "No name", District = "No place" })
                                                            }
                                                       );


            foreach (var item in leftOuterJoinType)
            {
                Console.WriteLine($"Supplier: {item.Name}, District: {item.District} " +
                    $"\nBuyers:");

                foreach (var buyer in item.Buyers)
                {
                    Console.WriteLine($"  {buyer.Name}");
                }
            }

            var leftOuterJoinAnon = suppliers.GroupJoin(buyers,
                                                        s => s.District,
                                                        b => b.District,
                                                        (s, buyersGroup) =>
                                                            new
                                                            {
                                                                s = s,
                                                                buyersGroup = buyersGroup
                                                            }
                                                        )
                                                        .SelectMany(
                                                            s => s.buyersGroup.DefaultIfEmpty(),
                                                            (s, b) =>
                                                                new
                                                                {
                                                                    Name = s.s.Name,
                                                                    District = s.s.District,
                                                                    BuyersName = (b.Name ?? "No one here"),
                                                                    BuyersDistrict = (b.District ?? "Nowhere")
                                                                }
                                                        );

            foreach (var item in leftOuterJoinAnon)
            {
                Console.WriteLine($"{item.District}");
                Console.WriteLine($"  {item.Name}, {item.BuyersName}");
            }
        }
    }
}
