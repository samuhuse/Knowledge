using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BehavioralPatterns.Memento
{
    public class UndoRedoMemento
    {
        #region Model

        public class Memento
        {
            public int Balance { get; }

            public Memento(int balance)
            {
                Balance = balance;
            }
        }

        public class BankAccount
        {
            private int _balance;
            private List<Memento> _changes = new List<Memento>();
            private int _current;

            public int Balance => _balance;

            public BankAccount(int balance)
            {
                this._balance = balance;
                _changes.Add(new Memento(balance));
            }

            public Memento Deposit(int amount)
            {
                _balance += amount;
                var m = new Memento(_balance);
                _changes.Insert(++_current,m);
                _changes.RemoveAll(x => _changes.IndexOf(x) > _current);
                return m;
            }

            public Memento Undo()
            {
                if (_current > 0)
                {
                    var m = _changes[--_current];
                    _balance = m.Balance;
                    return m;
                }
                return null;
            }

            public Memento Redo()
            {
                if (_current + 1 < _changes.Count)
                {
                    var m = _changes[++_current];
                    _balance = m.Balance;
                    return m;
                }
                return null;
            }
        }

        #endregion

        [Test]
        public void TryUndoRedoMemento()
        {
            BankAccount bankAccount = new BankAccount(100);

            bankAccount.Deposit(100);
            Console.WriteLine(bankAccount.Balance);
            bankAccount.Deposit(50);
            Console.WriteLine(bankAccount.Balance);

            bankAccount.Undo();
            Console.WriteLine(bankAccount.Balance);
            bankAccount.Undo();
            Console.WriteLine(bankAccount.Balance);

            bankAccount.Redo();
            Console.WriteLine(bankAccount.Balance);

            bankAccount.Deposit(50);
            bankAccount.Redo(); // Don't affect
            Console.WriteLine(bankAccount.Balance);
        }
    }
}
