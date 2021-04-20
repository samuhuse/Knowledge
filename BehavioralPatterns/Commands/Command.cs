using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Text;

using static BehavioralPatterns.Commands.Command;
using static System.Console;

namespace BehavioralPatterns.Commands
{
    public class Command
    {
        #region Model

        public class BankAccount
        {
            private int balance;
            private int overdraftLimit = -500;

            public void Deposit(int amount)
            {
                balance += amount;
                WriteLine($"Deposited ${amount}, balance is now {balance}");
            }

            public bool Withdraw(int amount)
            {
                if (balance - amount >= overdraftLimit)
                {
                    balance -= amount;
                    WriteLine($"Withdrew ${amount}, balance is now {balance}");
                    return true;
                }
                return false;
            }

            public override string ToString()
            {
                return $"{nameof(balance)}: {balance}";
            }
        }

        #endregion

        public interface ICommand
        {
            public bool Apply();
            public bool Undo();
        }

        public class BankAccountCommand : ICommand
        {
            private BankAccount account;

            public enum Action
            {
                Deposit, Withdraw
            }

            private Action action;
            private int amount;
            public bool succeeded;

            public BankAccountCommand(BankAccount account, Action action, int amount)
            {
                this.account = account ?? throw new ArgumentNullException(paramName: nameof(account));
                this.action = action;
                this.amount = amount;
            }

            public bool Apply()
            {
                switch (action)
                {
                    case Action.Deposit:
                        account.Deposit(amount);
                        succeeded = true;
                        break;
                    case Action.Withdraw:
                        succeeded = account.Withdraw(amount);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                return true;
            }

            public bool Undo()
            {
                if (!succeeded) return true;
                switch (action)
                {
                    case Action.Deposit:
                        account.Withdraw(amount);
                        break;
                    case Action.Withdraw:
                        account.Deposit(amount);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                return true;
            }
        }

        [Test]
        public static void Try()
        {
            BankAccount account = new BankAccount();
            BankAccountCommand depositCommand = new BankAccountCommand(account, BankAccountCommand.Action.Deposit, 1000);
            BankAccountCommand drawCommand = new BankAccountCommand(account, BankAccountCommand.Action.Withdraw, 100);

            depositCommand.Apply();
            drawCommand.Apply();
        }
    }
}
