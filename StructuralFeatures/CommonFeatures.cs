using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructuralFeatures
{
    public class CommonFeatures
    {

        [Test]
        public void PassPrimitiveByReference()
        {
            void AddTextWithRef(ref string str)
            {
                str = "Text " + str; 
            }

            void AddText(string str)
            {
                str = "Text " + str;
            }

            string str = "string";

            AddText(str);
            Console.WriteLine(str);

            AddTextWithRef(ref str);
            Console.WriteLine(str);
        }

    }
}
