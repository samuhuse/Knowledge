using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BehavioralPatterns.Memento
{
    public class SimpleMemento
    {
        #region Model

        public class BankAccount
        {
            public BankAccount(int initialAmount)
            {
                Balance = initialAmount;
            }
            public int Balance { get; private set; }

            public void Deposit(int amount) { Balance += amount; }
            public void Withdraw(int amount) { Balance -= amount; }

            public void Restore(BankAccountMemento memento)
            {
                this.Balance = memento.State;
            }
        }

        public class BankAccountMemento
        {
            private int _state;
            public int State => _state;

            public BankAccountMemento(BankAccount bankAccount)
            {
                _state = bankAccount.Balance;
            }
        }

        #endregion

        [Test]
        public void TrySimpleMemento()
        {
            BankAccount bankAccount = new BankAccount(100);

            BankAccountMemento memento = new BankAccountMemento(bankAccount);

            bankAccount.Deposit(100);
            Console.WriteLine(bankAccount.Balance);

            bankAccount.Restore(memento);
            Console.WriteLine(bankAccount.Balance);
        }

    }
}
