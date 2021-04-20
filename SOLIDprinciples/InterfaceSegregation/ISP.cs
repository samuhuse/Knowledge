using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Text;

namespace SOLIDprinciples.NewFolder
{
    public class ISP
    {
        // Depending on something that carries baggage that you don't need
        // can cause you troubles that you didn't expect

        public interface IOperationA { public void AOperation(); }
        public interface IOperationB { public void BOperation(); }
        public interface IOperationC { public void COperation(); }

        public class Operation : IOperationA, IOperationB, IOperationC
        {
            public void AOperation() { }

            public void BOperation() { }

            public void COperation() { }
        }

        public class ClientA
        {
            public ClientA(IOperationA operation) // Doesn't know about B and C
            {
                operation.AOperation();
            }
        }

        public class ClientB
        {
            public ClientB(IOperationB operation) // Doesn't know about A and C
            {
                operation.BOperation();
            }
        }

        public class ClientC
        {
            public ClientC(IOperationC operation) // Doesn't know about A and B
            {
                operation.COperation();
            }
        }

        [Test]
        public void Run()
        {
            ClientA a = new ClientA(new Operation());
            ClientB b = new ClientB(new Operation());
            ClientC c = new ClientC(new Operation());
        }
    }
}
