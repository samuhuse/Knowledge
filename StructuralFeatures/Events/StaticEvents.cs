using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructuralFeatures.Events
{
    public class StaticEvents
    {
        public class Sender
        {
            public delegate void DoneEventHandler(object sender, EventArgs args);

            public static event EventHandler Done;

            public void Do()
            {
                Console.WriteLine("Doing");

                OnDone();
            }

            protected virtual void OnDone()
            {
                if (Done is not null) Done(this, EventArgs.Empty);
            }
        }

        public static class Subscriber 
        {
            public static void Done(object sender, EventArgs args)
            {
                Console.WriteLine("Done by Subscriber");
            }
        }

        [Test]
        public void Try()
        {
            Sender.Done += Subscriber.Done;

            new Sender().Do();
        }
    }
}
