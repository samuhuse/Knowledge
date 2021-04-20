using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ExtentionMethods
{
    public static class IntegerExtentions
    {
        public static void Times(this int integer, Action action)
        {
            for (int i = 0; i < integer; i++)
            {
                action.Invoke();
            }
        }
    }



    public class NetTypes
    {
        [Test]
        public void TryInteger()
        {
            int HelloWord(int value) { Console.WriteLine($"Hello Word {value}"); return value; }



            ((int)10).Times(() => HelloWord(1));
        }


    }
}
