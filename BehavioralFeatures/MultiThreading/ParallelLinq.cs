using NUnit.Framework;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BehavioralFeatures.MultiThreading
{
    public class ParallelLinq
    {
        public class Person 
        {
            public string Name { get; set; }
            public DateTime BirthDate { get; set; }
            public int Weigth { get; set; }
            public int Higth { get; set; }

            public Person(string name, DateTime birthDate, int weigth, int higth)
            {
                Name = name;
                BirthDate = birthDate;
                Weigth = weigth;
                Higth = higth;
            }

            // Long operation
            public int CalculateMagicNumber()
            {
                return (int)Math.Pow((Weigth * Higth), Name.Length)
                       / (DateTime.Now.Year - BirthDate.Year);                                        
            }
        }

        List<Person> personList = new List<Person>()
        {
            new Person("Samuele 1", new DateTime(1998,7,31), 65, 185),
            new Person("Nicola 2", new DateTime(1995,4,23),75,165),
            new Person("Alberto 3", new DateTime(1999,5,22),70,170),
            new Person("Samuele 4", new DateTime(1998,7,31), 65, 185),
            new Person("Nicola 5", new DateTime(1995,4,23),75,165),
            new Person("Alberto 6", new DateTime(1999,5,22),70,170),
            new Person("Samuele 7", new DateTime(1998,7,31), 65, 185),
            new Person("Nicola 8", new DateTime(1995,4,23),75,165),
            new Person("Alberto 9", new DateTime(1999,5,22),70,170)
        };

        [Test]
        public void TryAsParallel()
        {
            ConcurrentDictionary<Person, int> magicNumberDictionary = new ConcurrentDictionary<Person, int>();
            personList.AsParallel().ForAll(p => 
            {
                magicNumberDictionary.TryAdd(p, p.CalculateMagicNumber());
                Console.WriteLine(p.Name + " calculated by " + Task.CurrentId);
            });

            foreach (KeyValuePair<Person,int> keyValue in magicNumberDictionary)
            {
                Console.WriteLine(keyValue.Key.Name + " magic number: " + keyValue.Value);
            }
        }

        [Test]
        public void TryOrderedAsParallel()
        {
            var items = Enumerable.Range(1, 50).ToArray();

            var cubes = items.AsParallel().Select(x => x * x * x);
            foreach (var i in cubes) // Here will appen the actual computation
                Console.Write($"{i}\t");

            Console.WriteLine();

            cubes = items.AsParallel().AsOrdered().Select(x => x * x * x);
            foreach (var i in cubes)
                Console.Write($"{i}\t");

        }

        [Test]
        public void TryAsParallelHandlingException()
        {
            ParallelQuery<int> query = ParallelEnumerable.Range(1, 30);

            var result = query.Select(x => 
            {
                double y = Math.Log10(x);
                if (y > 1) throw new InvalidOperationException();
                Console.WriteLine(y + " computed by " + Task.CurrentId);
                return y;
            });

            try
            {
                foreach (var value in result) // Here will appen the actual computation
                {
                    Console.WriteLine(value);
                }
            }
            catch (AggregateException ae)
            {
                ae.Handle(e =>
                {
                    Console.WriteLine(e.Message); return true;
                });
            }

        }

        [Test]
        public void TryAsParallelCancellation()
        {
            ParallelQuery<int> query = ParallelEnumerable.Range(1, 30);
            CancellationTokenSource cts = new CancellationTokenSource();

            var result = query.WithCancellation(cts.Token).Select(x =>
            {
                double y = Math.Log10(x);
                Console.WriteLine(y + " computed by " + Task.CurrentId);
                return y;
            });

            try
            {
                foreach (var value in result) // Here will appen the actual computation
                {
                    if (value > 1) cts.Cancel();
                    Console.WriteLine(value);
                }
            }
            catch (OperationCanceledException e)
            {
                Console.WriteLine("Operation cancelled: " + e.Message);
            }

        }

        [Test]
        public void TryAsParallelMergeOption()
        {
            double Produce(int x) 
            {
                double y = (double)x;
                Console.WriteLine(y + " Produced");
                return y;
            }

            void Consume(double y)
            {
                Console.WriteLine(y + " Consumed");
            }

            ParallelQuery<int> query = ParallelEnumerable.Range(1, 30);

            Console.WriteLine("AutoBuffered");
            var result = query
                .Select(x => Produce(x));
            result.ForAll(x => Consume(x));

            Console.WriteLine(); Console.WriteLine(); Console.WriteLine();
            Console.WriteLine("FullyBuffered");
            result = query.WithMergeOptions(ParallelMergeOptions.FullyBuffered)
                .Select(x => Produce(x));

            foreach (var x in result) { Consume(x); }


            Console.WriteLine(); Console.WriteLine(); Console.WriteLine();
            Console.WriteLine("NotBuffered");
            result = query.WithMergeOptions(ParallelMergeOptions.NotBuffered)
                .Select(x => Produce(x));

            foreach (var x in result) { Consume(x); }
        }
    }
}
