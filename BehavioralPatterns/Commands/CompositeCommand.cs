using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static BehavioralPatterns.Commands.Command;

namespace BehavioralPatterns.Commands
{
    public class CompositeCommand
    {
        public class CompositeBankCommand : List<BankAccountCommand>, ICommand
        {
            public virtual bool Apply()
            {
                ForEach(cmd => cmd.Apply());
                return this.All(cmd => cmd.succeeded);
            }

            public virtual bool Undo()
            {
                foreach (var cmd in ((IEnumerable<BankAccountCommand>)this).Reverse())
                {
                    cmd.Undo();
                }

                return true;
            }
        }

        public class MoneyTransferCommand : CompositeBankCommand
        {
            public MoneyTransferCommand(BankAccount from, BankAccount to, int amount)
            {
                AddRange(new[]
                {
                new BankAccountCommand(from,BankAccountCommand.Action.Withdraw, amount),
                new BankAccountCommand(to,BankAccountCommand.Action.Deposit, amount)
             });
            }

            public override bool Apply()
            {
                bool ok = true;
                foreach (var cmd in this)
                {
                    if (ok)
                    {
                        cmd.Apply();
                        ok = cmd.succeeded;
                    }
                    else
                    {
                        cmd.succeeded = false;
                    }
                }

                return ok;
            }
        }

        public void Try()
        {
            var from = new BankAccount(); from.Deposit(100);
            var to = new BankAccount();

            Console.WriteLine(from);
            Console.WriteLine(to);

            var mtc = new MoneyTransferCommand(from, to, 1000);
            mtc.Apply();

            Console.WriteLine(from);
            Console.WriteLine(to);
        }
    }

}
