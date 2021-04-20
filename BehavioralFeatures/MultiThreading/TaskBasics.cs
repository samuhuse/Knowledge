using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BehavioralFeatures.MultiThreading
{
    public class TaskBasics
    {
        #region Starting
        public void Writes(object c) // Convenction to be object for State 
        {
            int i = 1000;

            while (i-- > 0)
            {
                Console.Write(c);
            }
        }

        [Test]
        public void TrySilmple()
        {
            Task.Factory.StartNew(() => Writes('-'));

            Task t = new Task(() => Writes('?'));
            t.Start();

            Writes('.');
        }

        [Test]
        public void TryState()
        {
            Task.Factory.StartNew(Writes, 'x');

            Task t = new Task(Writes, 'y');
            t.Start();
        }

        public int MesureLenght(object s)
        {
            return s.ToString().Length;
        }

        [Test]
        public void tryReturnTrype()
        {
            string text1 = "testing", text2 = "this";
            var task1 = new Task<int>(MesureLenght, text1);
            task1.Start();
            var task2 = Task.Factory.StartNew(MesureLenght, text2);

            Console.WriteLine($"Length of '{text1}' is {task1.Result}.");// .Result is a blocking operation
            Console.WriteLine($"Length of '{text2}' is {task2.Result}.");
        }
        #endregion

        #region Cancelling 

        public void Write(string s, CancellationToken token)
        {
            int i = 1000;

            while (i-- > 0)
            {
                Console.Write(s);
                token.ThrowIfCancellationRequested();
            }
        }

        [Test]
        public void TrySimpleCancellation()
        {
            var source = new CancellationTokenSource();

            Task.Factory.StartNew(() => Write("x", source.Token));
            Task.Factory.StartNew(() => Writes("y"));
            Thread.Sleep(500);

            source.Cancel();
        }

        [Test]
        public void TryMonitoredCancellation()
        {
            var source = new CancellationTokenSource();
            var token = source.Token;

            token.Register(() => Console.WriteLine("Token Canceled")); // On the token, N(tasks cancelled) : 1(call)

            Task.Factory.StartNew(() =>
            {
                token.WaitHandle.WaitOne();
                Console.WriteLine("Wait handle released, thus cancelation was requested");
            }); // A New Task waiting for the cancellation

            Task.Factory.StartNew(() => Write("x", token));
            Task.Factory.StartNew(() => Writes("y"));
            Thread.Sleep(500);

            source.Cancel();

            Thread.Sleep(3 * 1000);
        }

        [Test]
        public void TryCompositeCancellation()
        {
            var source1 = new CancellationTokenSource();
            var source2 = new CancellationTokenSource();

            var source3 = CancellationTokenSource.CreateLinkedTokenSource(
                           source1.Token, source2.Token);

            Task.Factory.StartNew(() => Write("x", source1.Token));
            Task.Factory.StartNew(() => Write("y", source2.Token));
            Task.Factory.StartNew(() => Write("z", source3.Token));
            Thread.Sleep(500);

            source1.Cancel(); // stops x and z

            Thread.Sleep(3 * 1000);
        }

        [Test]
        public void TryWaitCancellation()
        {
            var source = new CancellationTokenSource();
            var token = source.Token;
            var t = new Task(() =>
            {
                Console.WriteLine("You have 5 seconds to disarm this bomb by pressing a key");
                bool canceled = token.WaitHandle.WaitOne(5000); // Waits for 5 seconds and retun if the token was cancellet with in
                Console.WriteLine(canceled ? "Bomb disarmed." : "BOOM!!!!");
            }, token);
            t.Start();

            Thread.Sleep(4 * 1000);
            source.Cancel();
        }

        #endregion

        #region Waiting


        public void StrangeOperation(int seconds, CancellationToken token)
        {
            Thread.Sleep((seconds / 2) * 1000);
            token.ThrowIfCancellationRequested();
            Thread.Sleep((seconds / 2) * 1000);
        }

        [Test]
        public void TrySimpleWait()
        {
            var source = new CancellationTokenSource();

            Task t = new Task(() => StrangeOperation(5, source.Token));
            t.Start();

            t.Wait(source.Token);
            // Waits for the System.Threading.Tasks.Task to complete execution.The wait terminates
            // if a cancellation token is canceled before the task completes.
        }

        [Test]
        public void TryWaitAll()
        {
            var source = new CancellationTokenSource();

            Task t1 = new Task(() => StrangeOperation(2, source.Token));
            Task t2 = new Task(() => StrangeOperation(5, source.Token));
            t1.Start();
            t2.Start();

            Task.WaitAll(t1, t2);

            // With timeout
            t1.Start();
            t2.Start();

            Task.WaitAll(new[] { t1, t2 }, 6000);

            // With cancellation
            t1.Start();
            t2.Start();

            Task.WaitAll(new[] { t1, t2 }, source.Token);
        }

        [Test]
        public void TryWaitOne()
        {
            var source = new CancellationTokenSource();

            Task t1 = new Task(() => StrangeOperation(5, source.Token));
            Task t2 = new Task(() => StrangeOperation(10, source.Token));
            t1.Start();
            t2.Start();

            Task.WaitAny(t1, t2);

            // With timeout
            t1.Start();
            t2.Start();

            Task.WaitAny(new[] { t1, t2 }, 6000);

            // With cancellation and timeout
            t1.Start();
            t2.Start();

            Task.WaitAny(new[] { t1, t2 }, 6000, source.Token);
        }

        [Test]
        public void TryHandlingException()
        {
            Task[] Starttasks()
            {
                var t = Task.Factory.StartNew(() =>
                {
                    throw new InvalidOperationException("Can't do this!") { Source = Task.CurrentId.ToString() };
                });

                var t2 = Task.Factory.StartNew(() =>
                {
                    throw new AccessViolationException("Can't access this!") { Source = Task.CurrentId.ToString() };
                });

                var t3 = Task.Factory.StartNew(() =>
                {
                    throw new OperationCanceledException("Operation cancelled") { Source = Task.CurrentId.ToString() };
                });

                return new[] { t, t2, t3 };
            }

            try
            {
                try
                {
                    Task.WaitAll(Starttasks());
                }
                catch (AggregateException aggregateException)
                {
                    aggregateException.Handle(e =>
                    {
                        if (e is OperationCanceledException)
                        {
                            Console.WriteLine("Whoops, tasks were canceled.");
                            return true; // exception was handled
                        }
                        else
                        {
                            return false; // exception was NOT handled
                        }
                    });


                }
            }
            catch (AggregateException aggregateException)
            {
                foreach (Exception e in aggregateException.InnerExceptions)
                {
                    Console.WriteLine(e.GetType() + " from " + e.Source);
                }
            }

            #endregion

        }
    }
}
