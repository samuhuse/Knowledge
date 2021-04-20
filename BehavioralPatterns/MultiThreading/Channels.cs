using NUnit.Framework;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BehavioralPatterns.MultiThreading
{
    public class Channels
    {
        public interface IRead<T>
        {
            Task<T> Read();
            bool IsComplete();
        }

        public interface IWrite<T>
        {
            void Push(T msg);
            void Complete();
        }

		public class Channel<T> : IRead<T>, IWrite<T>
		{
			private bool Finished;

			private ConcurrentQueue<T> _queue;
			private SemaphoreSlim _flag;

			public Channel()
			{
				_queue = new ConcurrentQueue<T>();
				_flag = new SemaphoreSlim(1);
			}

			public void Push(T msg)
			{
				_queue.Enqueue(msg);
				_flag.Release();
			}

			public async Task<T> Read()
			{
                await _flag.WaitAsync();

                if (_queue.TryDequeue(out var msg))
				{
					return msg;
				}

				return default;
			}

			public void Complete()
			{
				Finished = true;
			}

			public bool IsComplete()
			{
				return Finished && _queue.IsEmpty;
			}
		}


		[Test]
        public void TryChannel()
        {
			async Task Producer(IWrite<string> writer)
			{
				for (int i = 0; i < 10; i++)
				{
					writer.Push(i.ToString());
                    await Task.Delay(100);
				}
				writer.Complete();
			}

			async Task Consumer(IRead<string> reader)
			{
				while (!reader.IsComplete())
				{
					var msg = await reader.Read();
                    Console.WriteLine(msg);
				}
                Console.WriteLine("Finished");
			}
			var channel = new Channel<string>();

			List<Task> tasks = new List<Task>()
			{
				Task.Factory.StartNew(() => Consumer(channel)),
				Task.Factory.StartNew(() => Producer(channel))
			};

			Task.WaitAll(tasks.ToArray());
		}
	}
}
