using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BehavioralFeatures.MultiThreading
{
    public class TaskSyncrhonization
    {

        #region Model

        public class BanckAccount
        {
            private int _balance = 0;
            public int Balance { get; set; }

            public void Deposit(int amount)
            {
                lock (this)
                {
                    Balance += amount;
                }
            }
            public void Withdraw(int amount)
            {
                lock (this)
                {
                    Balance -= amount;
                }
            }

            public void DepositInterlock(int amount)
            {
                Interlocked.Add(ref _balance, amount);
            }
            public void WithdrawInterlock(int amount)
            {
                Interlocked.Add(ref _balance, -amount);
            }
        }

        #endregion

        [Test]
        public void TryRaceCondition()
        {
            List<Task> tasks = new List<Task>();
            BanckAccount account = new BanckAccount();
            Enumerable.Range(1, 30).ToList().ForEach(x => tasks.Add(new Task(() => account.Deposit(100))));
            Enumerable.Range(1, 30).ToList().ForEach(x => tasks.Add(new Task(() => account.Withdraw(100))));

            tasks.ForEach(t => t.Start());
            Task.WaitAll(tasks.ToArray());

            Console.WriteLine(account.Balance);
            tasks.Clear();

            Enumerable.Range(1, 30).ToList().ForEach(x => tasks.Add(new Task(() => account.DepositInterlock(100))));
            Enumerable.Range(1, 30).ToList().ForEach(x => tasks.Add(new Task(() => account.WithdrawInterlock(100))));

            tasks.ForEach(t => t.Start());
            Task.WaitAll(tasks.ToArray());

            Console.WriteLine(account.Balance);
        }

        [Test]
        public void TryMutex()
        {
            void Transfer(BanckAccount from, BanckAccount to,int amount)
            {
                from.Balance -= amount;
                to.Balance += amount;
            }

            List<Task> tasks = new List<Task>();

            BanckAccount account1 = new BanckAccount();
            BanckAccount account2 = new BanckAccount();

            Mutex mutex1 = new Mutex();
            Mutex mutex2 = new Mutex();

            tasks.Add(new Task(() => 
            {
                for (int i = 0; i < 1000; i++)
                {
                    bool haveLock = mutex1.WaitOne();
                    try { account1.Balance += 1; }
                    finally { if(haveLock) mutex1.ReleaseMutex(); }
                }
            }));

            tasks.Add(new Task(() =>
            {
                for (int i = 0; i < 1000; i++)
                {
                    bool haveLock = mutex2.WaitOne();
                    try { account2.Balance += 1; }
                    finally { if(haveLock) mutex2.ReleaseMutex(); }
                }
            }));

            // Transfer needs to lock both accounts
            Enumerable.Range(1, 10).ToList().ForEach(x => tasks.Add(new Task(() => 
            {
                bool haveLock = Mutex.WaitAll(new[] { mutex1, mutex2 });
                try
                {
                    Transfer(account1, account2, 100);
                }
                finally { if(haveLock) { mutex1.ReleaseMutex(); mutex2.ReleaseMutex(); } }
            })));

            tasks.ForEach(t => t.Start());
            Task.WaitAll(tasks.ToArray());

            Console.WriteLine(account1.Balance);
            Console.WriteLine(account2.Balance);
        }

        [Test]
        public void TryReadWriteBlocks()
        {
            int y = 0;
            ReaderWriterLockSlim padLock = new ReaderWriterLockSlim();

            List<Task> tasks = new List<Task>();

            Enumerable.Range(1, 30).ToList().ForEach(x => tasks.Add(Task.Factory.StartNew(() => 
            {
                padLock.EnterReadLock(); // Can enter N thread
                Console.WriteLine("Enter Read " + Task.CurrentId);
                Console.WriteLine(y);
                padLock.ExitReadLock();
                Console.WriteLine("Exit Read " + Task.CurrentId);
            })));

            Enumerable.Range(1, 10).ToList().ForEach(x => tasks.Add(Task.Factory.StartNew(() =>
            {
                padLock.EnterWriteLock(); // Can wnter one thread while no ReadLock are opened
                Console.WriteLine("Enter Write " + Task.CurrentId);
                x = new Random().Next(10);
                Console.WriteLine(y);
                padLock.ExitWriteLock();
                Console.WriteLine("Exit Write " + Task.CurrentId);
            })));

            Task.WaitAll(tasks.ToArray());
        }

    }
}
