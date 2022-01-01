using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BehavioralFeatures.Dynamics
{
    public class ExpandoObjects
    {
        [Test]
        public void TryExpandoObject()
        {
            dynamic expandoObject = new ExpandoObject();

            expandoObject.Name = "Samuele";
            expandoObject.Age = 22;

            expandoObject.SayHello = new Action(() => Console.WriteLine("Hello"));
            expandoObject.SayHello();

            Console.WriteLine($"name:{expandoObject.Name} age:{expandoObject.Age}");
        }
    }
}
