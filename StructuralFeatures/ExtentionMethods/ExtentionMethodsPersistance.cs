using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructuralFeatures.ExtentionMethods
{
    public static class ExtensionMethods
    {
        private static Dictionary<WeakReference, object> data = new Dictionary<WeakReference, object>();
          
        public static object GetTag(this object o)
        {
            var key = data.Keys.FirstOrDefault(k => k.IsAlive && k.Target == o);
            return key != null ? data[key] : null;
        }

        public static void SetTag(this object o, object tag)
        {
            var key = data.Keys.FirstOrDefault(k => k.IsAlive && k.Target == o);
            if (key != null)
            {
                data[key] = tag;
            }
            else
            {
                data.Add(new WeakReference(o), tag);
            }
        }
    }

    public class ExtentionMethodsPersistance
    {
        [Test]
        public void Try()
        {
            object ob = new object();
            ob.SetTag("Object A");
            Console.WriteLine(ob.GetTag());
        }

    }
}
