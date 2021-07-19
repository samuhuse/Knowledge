using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BehavioralPatterns.State
{
    public class BetterState
    {
        public enum State
        {
            OffHook,
            Connecting,
            Connected,
            OnHold
        }

        public enum Trigger
        {
            CallDialed,
            HungUp,
            CallConnected,
            PlacedOnHold,
            TakenOffHold,
            LeftMessage
        }

        private static Dictionary<State, List<(Trigger, State)>> rules
            = new Dictionary<State, List<(Trigger, State)>>
            {
                [State.OffHook] = new List<(Trigger, State)>
            {
        (Trigger.CallDialed, State.Connecting)
            },
                [State.Connecting] = new List<(Trigger, State)>
            {
        (Trigger.HungUp, State.OffHook),
        (Trigger.CallConnected, State.Connected)
            },
                [State.Connected] = new List<(Trigger, State)>
            {
        (Trigger.LeftMessage, State.OffHook),
        (Trigger.HungUp, State.OffHook),
        (Trigger.PlacedOnHold, State.OnHold)
            },
                [State.OnHold] = new List<(Trigger, State)>
            {
        (Trigger.TakenOffHold, State.Connected),
        (Trigger.HungUp, State.OffHook)
            }
            };

        [Test]
        public void Try()
        {
            var state = State.OffHook;
            for (int x = 0; x < 50; x++)
            {
                Console.WriteLine($"The phone is currently {state}");
                Console.WriteLine("Select a trigger:");
                var i = 0;
                // foreach to for
                while ( i < rules[state].Count )
                {                  
                    var (t, _) = rules[state][i];
                    Console.WriteLine($"{i}. {t}");
                    i++;
                }


                int input = new Random().Next(i);

                var (_, s) = rules[state][input];
                state = s;
            }
        }
    }
}
