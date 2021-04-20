using System;
using System.Collections.Generic;
using System.Text;

namespace SOLIDprinciples.NewFolder
{
    public class NoISP
    {
        public interface Operations 
        {
            public void AOperation() { }

            public void BOperation() { }

            public void COperation() { }
        }

        public class ClientA
        {
            public ClientA(Operations operation) 
            {
                operation.AOperation();
            }
        }

        public class ClientB
        {
            public ClientB(Operations operation) 
            {
                operation.BOperation();
            }
        }

        public class ClientC
        {
            public ClientC(Operations operation) 
            {
                operation.COperation();
            }
        }
    }
}
