using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BehavioralPatterns.State
{
    public class SwitchExpressionState
    {
        public enum State
        {
            Notcalling,
            Calling,
            Ringing,
            Connected
        }

        public enum Action
        {
            CallDialed,
            Hooked,
            Connect,
            HungUp
        }

        public class Call
        {
            public string PhoneNumber { get; set; }
            public State State { get; set; } = State.Notcalling;
        }

        public static State Transite(State state, Action action) =>
            (state, action) switch
            {
                (State.Notcalling, Action.CallDialed) => State.Calling,
                (State.Calling, Action.HungUp) => State.Notcalling,
                (State.Calling, Action.Hooked) => State.Ringing,
                (State.Ringing, Action.Connect) => State.Connected,
                (State.Ringing, Action.HungUp) => State.Notcalling,
                (State.Connected, Action.HungUp) => State.Notcalling,
                _ => throw new InvalidOperationException("Invalid transition")
            };


        [Test]
        public void Try()
        {
            Call call = new Call() { PhoneNumber = "3938904156" };

            // Calling
            call.State = Transite(call.State,Action.CallDialed);
            // Hooked
            call.State = Transite(call.State, Action.Hooked);
            // Connected 
            call.State = Transite(call.State, Action.Connect);

            // Exception: invalid transition
            call.State = Transite(call.State, Action.CallDialed);
        }
    }
}
