using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Text;

namespace SOLIDprinciples.LiskovSubstitution
{
    public class LSP
    {
        // If for each object A there is an object B such that all programs P defined in terms of type A
        // the behavor of P is  unchanged when A objects are substituted for B object then B is a subtype of A

        public interface ILicense // A
        {
            public float CalcFee();
        }

        public class Billing // P
        {
            private readonly ILicense _license;

            public Billing(ILicense license)
            {
                _license = license;
            }
        }

        public class PersonalLicense : ILicense // B
        {
            public float CalcFee()
            {
                return 1;
            }
        }

        public class BusinesslLicense : ILicense // B'
        {
            public float CalcFee()
            {
                return 2;
            }
        }

        [Test]
        public void Run()
        {
            Billing billingA = new Billing(new PersonalLicense());
            Billing billingB = new Billing(new BusinesslLicense());
        }
    }
}
