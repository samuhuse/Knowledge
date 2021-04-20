using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace StructuralFeatures.Reflection
{
    public class Events
    {
        public class Sender
        {
            public delegate void DoneEventHandler(object sender, EventArgs args);

            public event DoneEventHandler Done;
            public virtual void OnDone()
            {
                if (Done is not null) Done(this, new EventArgs());
            }
        }

        public class Subsriber
        {
            public static void Do(object sender, EventArgs args)
            {
                Console.WriteLine("From Subsciber");
            }          
        }

        [Test]
        public void Try()
        {
            EventInfo eventInfo = typeof(Sender).GetEvent("Done");
            MethodInfo methodInfo = typeof(Subsriber).GetMethod("Do");

            Delegate del = Delegate.CreateDelegate(eventInfo.EventHandlerType, methodInfo);

            Sender sender = new();

            eventInfo.AddEventHandler(sender, del);

            sender.OnDone();
        }

    }
}
