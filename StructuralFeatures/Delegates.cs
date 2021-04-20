using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructuralFeatures
{
    public class Delegates
    {
        public int Sum(int a, int b)
        {
            return a + b;
        }

        public int Moltiplicate(int a, int b)
        {
            return a * b;
        }

        delegate int Operation(int a, int b);

        [Test]
        public void Try()
        {
            Operation operation1 = Sum;
            Operation operation2 = Moltiplicate;

            Console.WriteLine(operation1(1, 2));
            Console.WriteLine(operation2(1, 2));

            List<Operation> operationList = new List<Operation> 
            { 
                Sum,
                Moltiplicate
            };

            operationList.ForEach(op => Console.WriteLine(op(1, 2)));
        }


    }
}
