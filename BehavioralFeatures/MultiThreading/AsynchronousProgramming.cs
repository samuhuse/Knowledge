using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BehavioralFeatures.MultiThreading
{
    public class AsynchronousProgramming
    {
        public async Task WriteAsync(string msg)
        {
            //await Task.Delay(100 * new Random().Next(10));
            Console.WriteLine(msg);
        }

        public void Consumer()
        {
            for (int i = 0; i < 100; i++)
            {
                WriteAsync(i.ToString());
                WriteAsync((i + 10).ToString());
            }
        }

        [Test]
        public void TrySimpleAsync()
        {
            Consumer();
            Task.Delay(5000).Wait();
        }
    }
}
