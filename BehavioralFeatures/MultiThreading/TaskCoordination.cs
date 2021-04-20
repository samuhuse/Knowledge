using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BehavioralFeatures.MultiThreading
{
    public class TaskCoordination
    {
        [Test]
        public void TryContinuos()
        {
            Task task1 = Task.Factory.StartNew(() => Console.WriteLine("I'm " + Task.CurrentId));
            Task task2 = task1.ContinueWith((t) => Console.WriteLine("I'm " + Task.CurrentId + ". I continue " + t.Id));

            Task.WaitAll(task1, task2);

            List<Task> tasksList = new List<Task>();
            tasksList.Add(new Task(() => Console.WriteLine("I'm " + Task.CurrentId)));
            tasksList.Add(new Task(() => Console.WriteLine("I'm " + Task.CurrentId)));

            tasksList.ForEach(t => t.Start());

            Task task3 = Task.Factory.ContinueWhenAll(tasksList.ToArray(),
                (tasks) =>
                {
                    Console.WriteLine("I'm " + Task.CurrentId);
                    Console.WriteLine("Tasks completed");
                    foreach (Task t in tasks) Console.WriteLine("   -ID " + t.Id);
                });

            task3.Wait();
        }

        [Test]
        public void TrySimpleRelations()
        {
            Task parent = new Task(() =>
            {
                Task child = new Task(() => { Thread.Sleep(1000); Console.WriteLine("I'm child " + Task.CurrentId); }
                                      , TaskCreationOptions.AttachedToParent);
                child.Start();
                Console.WriteLine("I'm parent " + Task.CurrentId);
            });

            parent.Start();
            parent.Wait();
        }

        [Test]
        public void TryPathRelations()
        {
            Task parent = new Task(() =>
            {
                Console.WriteLine("I'm parent " + Task.CurrentId);
                //if (new Random().Next(1,2) == 2) 
                throw new Exception();
            });

            parent.ContinueWith(t => Console.WriteLine("Success")
                                , TaskContinuationOptions.AttachedToParent | TaskContinuationOptions.OnlyOnRanToCompletion);

            parent.ContinueWith(t => Console.WriteLine("Failure")
                                , TaskContinuationOptions.AttachedToParent | TaskContinuationOptions.OnlyOnFaulted);

            parent.Start();
            try { parent.Wait(); } catch { }
        }

        [Test]
        public void TryBarrier()
        {
            Barrier barrier = new Barrier(10, b => Console.WriteLine("Barrire Phase:" + b.CurrentPhaseNumber));
            List<Task> tasks = new List<Task>();
            Enumerable.Range(1, 10).ToList().ForEach(x =>
                tasks.Add
                (
                    new Task(() =>
                    {
                        for (int i = 0; i < 100; i++)
                        {
                            Console.WriteLine("id: " + Task.CurrentId + " - count: " + i);
                            barrier.SignalAndWait(); // When the barrierreach 10 signalit will continue
                        }
                    })
                )
            );

            tasks.ForEach(t => t.Start());
            Task.WaitAll(tasks.ToArray());
        }

        [Test]
        public void TryCountDown()
        {
            int taskCount = 5;
            CountdownEvent cte = new CountdownEvent(taskCount);
            Random random = new Random();

            Task[] tasks = new Task[taskCount];

            for (int i = 0; i < taskCount; i++)
            {
                tasks[i] = Task.Factory.StartNew(() =>
                {
                    Console.WriteLine($"Entering task {Task.CurrentId}.");
                    Thread.Sleep(random.Next(3000));
                    cte.Signal(); 
                    Console.WriteLine($"Exiting task {Task.CurrentId}.");
                });
            }

            var finalTask = Task.Factory.StartNew(() =>
            {
                Console.WriteLine($"Waiting for other tasks in task {Task.CurrentId}");
                cte.Wait();
                Console.WriteLine("All tasks completed.");
            });

            finalTask.Wait();
        }

        [Test]
        public void TrySemaphor()
        {
            var semaphore = new SemaphoreSlim(2); // Initial Count 2

            Enumerable.Range(1, 20).ToList().ForEach(x =>
             {
                 Task.Factory.StartNew(() =>
                 {
                     Console.WriteLine($"Entering task {Task.CurrentId}. time{DateTime.Now.Second}");
                     semaphore.Wait(); // ReleaseCount-- if count = 0 it wait
                     Console.WriteLine($"Processing task {Task.CurrentId}.time{DateTime.Now.Second}");
                 });
             });

            for (int i = 0; i < 20; i++)
            {
                Console.WriteLine($"Semaphore count: {semaphore.CurrentCount}");
                Thread.Sleep(1000);
                semaphore.Release(2); // ReleaseCount += n
            }
        }

    }
}
