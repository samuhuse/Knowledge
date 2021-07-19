using NUnit.Framework;

using System;

namespace StructuralFeatures
{
    public class Polymorphism
    {
        #region Model

        private abstract class Car
        {
            protected int Speed = 0;

            public abstract void Run();

            public virtual void Accelerate()
            {
                Speed += 10;
            }

            public void Stop()
            {
                Console.WriteLine("Stopping the car");
            }
        }

        private class Toyota : Car
        {
            public override void Run()
            {
                Console.WriteLine($"Toyota i running at speed {Speed}");
            }
        }

        private class Ferrari : Car
        {
            public override void Run()
            {
                Console.WriteLine($"Ferrari is running at {Speed}");
            }

            public override void Accelerate()
            {
                Speed += 50;
            }

            public new void Stop()
            {
                Console.WriteLine("Ferrari is stopping");
            }
        }

        #endregion Model

        [Test]
        public void Try()
        {
            Car toyota = new Toyota();
            Car ferrari = new Ferrari();

            toyota.Accelerate(); // Speed +10 from Base Class, not overrited
            toyota.Run(); // Toyota class Run implementation
            toyota.Stop(); // From Base Class

            ferrari.Accelerate(); // Speed +50 from Ferrari Class, virtual overrited
            ferrari.Run(); // Ferrari class Run implementation
            ferrari.Stop(); // From Base Class

            ((Ferrari)ferrari).Stop(); // From Ferrari Class
        }
    }
}