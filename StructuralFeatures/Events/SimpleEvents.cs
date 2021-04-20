using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructuralFeatures.Events
{
    public class SimpleEvents
    {

        public class CustomEventArgs : EventArgs
        {
            public string Name { get; set; }
        }

        public class Sender
        {
            public delegate void DoneEventHandler(object sender, CustomEventArgs args);

            public event DoneEventHandler Done;

            public void Do()
            {
                Console.WriteLine("Doing");

                OnDone();
            }

            protected virtual void OnDone()
            {
                if (Done is not null) Done(this, new CustomEventArgs { Name = "Name" });
            }
        }

        public class SubscriberA
        {
            public void DoneA(object sender, CustomEventArgs args)
            {
                Console.WriteLine("Done A by SubscriberA " + args.Name);
            }
        }

        public class SubscriberB
        {
            public void DoneB(object sender, CustomEventArgs args)
            {
                Console.WriteLine("Done B by SubscriberB " + args.Name);
            }
        }

        [Test]
        public void Try()
        {
            Sender sender = new Sender();
            SubscriberA subscriberA = new SubscriberA();
            SubscriberB subscriberB = new SubscriberB();

            sender.Done += subscriberA.DoneA;
            sender.Done += subscriberB.DoneB;

            sender.Do();
        }
    }
}
