using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BehavioralFeatures.MultiThreading
{
    public class ParallelLoops
    {
        public static IEnumerable<int> Range(int start, int end, int step)
        {
            for (int i = start; i < end; i += step)
            {
                yield return i;
            }
        }
            [Test]
        public void TryParallelLoops()
        {
            // Invoke (Action)
            var a = new Action(() => Console.WriteLine($"First {Task.CurrentId}"));
            var b = new Action(() => Console.WriteLine($"Second {Task.CurrentId}"));
            var c = new Action(() => Console.WriteLine($"Third {Task.CurrentId}"));

            Parallel.Invoke(a, b, c);

            // For loop
            Parallel.For(1, 11, x =>
            {
                Console.Write($"{x * x}\t");
            });

            // Foreach loop
            IEnumerable<string> strings = new List<string>() { "Hi", "My", "Name", "Is", "Samuele" };
            Parallel.ForEach(strings, (s) => Console.WriteLine(s));

            Parallel.ForEach(strings,new ParallelOptions() { MaxDegreeOfParallelism = 2} , (s) => Console.WriteLine(s));
        }
    }
}
