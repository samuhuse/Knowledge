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
    public class ConcurrentCollections
    {
        [Test]
        public void TryConcurrentDictionary()
        {
            ConcurrentDictionary<string, int> StockMarket = new ConcurrentDictionary<string, int>();

            List<Task> tasks = new List<Task>();

            Enumerable.Range(1, 20).ToList().ForEach(x => tasks.Add( new Task(() =>
            {
                Thread.Sleep(new Random().Next(4) * 1000);
                int value = new Random().Next(100);
                int difference = StockMarket.AddOrUpdate("Oil", value, (k, old) => value - old);

                Console.WriteLine($"id {Task.CurrentId}: Oil is {value}, changed by {difference}");          
            })));

            Enumerable.Range(1, 4).ToList().ForEach(x => tasks.Add( new Task(() =>
            {
                Thread.Sleep(new Random().Next(4) * 1000);
                int value = new Random().Next(100);
                bool hasAdded = StockMarket.TryAdd("Gold", value);

                Console.WriteLine($"id {Task.CurrentId}:" + (hasAdded ? "I did added Gold": " I didn't added Gold"));
            })));

            Enumerable.Range(1, 4).ToList().ForEach(x => tasks.Add(new Task(() =>
            {
                Thread.Sleep(new Random().Next(4) * 1000);
                int exValue;
                bool hasRemoved = StockMarket.TryRemove("Gold", out exValue);

                Console.WriteLine($"id {Task.CurrentId}:" + (hasRemoved ? $"I did removed Gold {exValue}" : " I didn't removed Gold"));
            })));

            tasks.ForEach(t => t.Start());
            Task.WaitAll(tasks.ToArray());
        }

        [Test]
        public void TryConcurrentQueue()
        {
            ConcurrentQueue<int> numberQueue = new ConcurrentQueue<int>();

            List<Task> tasks = new List<Task>();

            Enumerable.Range(1, 20).ToList().ForEach(x => tasks.Add(new Task(() =>
            {
                Thread.Sleep(new Random().Next(4) * 1000);
                int value = new Random().Next(100);
                numberQueue.Enqueue(value);
                Console.WriteLine($"id {Task.CurrentId}: I enquequed {value}");
            })));

            Enumerable.Range(1, 20).ToList().ForEach(x => tasks.Add(new Task(() =>
            {
                int value;
                while (true)
                {
                    if (numberQueue.TryDequeue(out value))
                    {
                        Console.WriteLine($"id {Task.CurrentId}: I denquequed {value}");
                        break;
                    }
                    else { Console.WriteLine($"id {Task.CurrentId}: Can't dequeued, I'll Ty again"); }
                    Thread.Sleep(new Random().Next(2) * 1000);
                }
            })));

            tasks.ForEach(t => t.Start());
            Task.WaitAll(tasks.ToArray());
        }

        [Test]
        public void TryConcurrentStack()
        {
            ConcurrentStack<string> platePile = new ConcurrentStack<string>();

            List<Task> tasks = new List<Task>();

            Enumerable.Range(1, 20).ToList().ForEach(x => tasks.Add(new Task(() =>
            {
                Thread.Sleep(new Random().Next(10) * 100);
                platePile.Push("Plaate number " + new Random().Next(1000));
            })));


            Enumerable.Range(1, 20).ToList().ForEach(x => tasks.Add(new Task(() =>
            {
                Thread.Sleep(new Random().Next(10) * 100);
                string plate;
                if (platePile.TryPop(out plate))
                {
                    Console.WriteLine($"id {Task.CurrentId}: Popped {plate}");
                }
                else { Console.WriteLine($"id { Task.CurrentId}: Cudn't Pop"); }
            })));

            tasks.ForEach(t => t.Start());
            Task.WaitAll(tasks.ToArray());
        }

        [Test]
        public void TryConcurrentBag()
        {
            // concurrent bag provides NO ordering guarantees
            ConcurrentBag<int> bag = new ConcurrentBag<int>();

            List<Task> tasks = new List<Task>();

            Enumerable.Range(1, 20).ToList().ForEach(x => tasks.Add(new Task(() =>
            {
                Thread.Sleep(new Random().Next(10) * 100);
                bag.Add(new Random().Next(1000));
            })));


            Enumerable.Range(1, 20).ToList().ForEach(x => tasks.Add(new Task(() =>
            {
                Thread.Sleep(new Random().Next(10) * 100);
                int number;
                if (bag.TryTake(out number))
                {
                    Console.WriteLine($"id {Task.CurrentId}: Taked {number}");
                }
                else { Console.WriteLine($"id { Task.CurrentId}: Cudn't take"); }
            })));

            tasks.ForEach(t => t.Start());
            Task.WaitAll(tasks.ToArray());
        }

        [Test]
        public void TryBlockingConcurrentCollection()
        {
            IProducerConsumerCollection<int> queue = new ConcurrentQueue<int>();
            BlockingCollection<int> blockingCollection = new BlockingCollection<int>(queue, 10/*max element*/);

            List<Task> tasks = new List<Task>();

            // Producer
            Enumerable.Range(1, 20).ToList().ForEach(x => tasks.Add(new Task(() =>
            {
                Thread.Sleep(new Random().Next(10) * 100);
                blockingCollection.Add(new Random().Next(1000)); // if there already 10 element the thread blocks here
                Console.WriteLine($"id { Task.CurrentId}: Insert value");
            })));

            // Consumer
            Enumerable.Range(1, 20).ToList().ForEach(x => tasks.Add(new Task(() =>
            {
                Thread.Sleep(new Random().Next(10) * 1000);
                int number;
                if (blockingCollection.TryTake(out number))
                {
                    Console.WriteLine($"id {Task.CurrentId}: Taked {number}");
                }
                else { Console.WriteLine($"id { Task.CurrentId}: Cudn't take"); }
            })));

            tasks.ForEach(t => t.Start());
            Task.WaitAll(tasks.ToArray());
        }

    }
}
