using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BehavioralPatterns.MultiThreading
{
    public class ConsumerProducerQueue
    {
        Queue<Action> Queue = new Queue<Action>();
        CancellationTokenSource CancellationSource = new CancellationTokenSource();

        public class Producer 
        {
            private readonly Queue<Action> _queue;
            private readonly Random _random;

            public Producer(Queue<Action> queue)
            {
                _queue = queue;
                _random = new Random();
            }

            public Task Produce(Action action, CancellationToken token)
            {
                return new Task(() =>
                {
                    int c = 0;
                    while (true)
                    {
                        Console.WriteLine("Producing " + Task.CurrentId);
                        lock (_queue)
                        {
                            _queue.Enqueue(action);
                        }

                        Task.Delay(_random.Next(1, 2) * 1000).Wait();
                        token.ThrowIfCancellationRequested();
                    }
                });
            }
        }

        public class Consumer
        {
            private readonly Queue<Action> _queue;

            public Consumer(Queue<Action> queue)
            {
                _queue = queue;
            }

            public Task Work(CancellationToken token)
            {
                return new Task(() =>
                {
                    Console.WriteLine(Task.CurrentId);
                    while (true)
                    {
                        Action action = null;

                        lock (_queue)
                        {
                            if (_queue.Count > 0)
                                action = _queue.Dequeue();
                        }

                        action?.Invoke();

                        token.ThrowIfCancellationRequested();
                    }
                });    
            }
        }

        [Test]
        public void Try()
        {
            Action action = () => Console.WriteLine($"Writed by {Thread.CurrentThread.ManagedThreadId}");

            new Producer(Queue).Produce(action, CancellationSource.Token).Start();

            List<Task> consumerlist = new List<Task>()
            {
                new Consumer(Queue).Work(CancellationSource.Token),
                new Consumer(Queue).Work(CancellationSource.Token),
                new Consumer(Queue).Work(CancellationSource.Token),
                new Consumer(Queue).Work(CancellationSource.Token)
            };
            consumerlist.ForEach(t => t.Start());

            Task.Delay(10 * 1000).Wait();

            CancellationSource.Cancel();
        }

    }
}
