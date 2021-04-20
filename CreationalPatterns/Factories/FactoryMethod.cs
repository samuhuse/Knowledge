using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreationalPatterns.Factories
{
    public class FactoryMethod
    {
        public class SmartPhone
        {
            private readonly string _brand;
            private readonly string _model;
            private readonly int _memory;

            public SmartPhone(string brand, string model, int memory)
            {
                _brand = brand;
                _model = model;
                _memory = memory;
            }

            public static class Factory
            {
                public static SmartPhone IPhoneX(int memory)
                {
                    return new SmartPhone("Apple","IPhoneX", memory);
                }

                public static SmartPhone SamsungGalaxy(int memory)
                {
                    return new SmartPhone("Samsung","Galaxy", memory);
                }
            }
        }

        [Test]
        public void Try()
        {
            SmartPhone IPhone = SmartPhone.Factory.IPhoneX(64);

            SmartPhone Samsung = SmartPhone.Factory.SamsungGalaxy(128);
        }
    }
}
