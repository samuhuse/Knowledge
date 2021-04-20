using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BehavioralPatterns.Mediators
{
    public class Mediator
    {
        // Mediator
        public class ChatRoom
        {
            private List<Chatter> _people = new List<Chatter>();
            public void Join(Chatter person)
            {
                _people.Add(person);
                person.Room = this;
                BroadCast(person, $"{person.Name} has joined the room");
            }
            public void BroadCast(Chatter sender, string message)
            {
                _people.Where(p => p != sender).ToList().ForEach(p => SendMessage(sender, p, message));
            }
            public void SendMessage(Chatter sender, Chatter reciver, string message)
            {
                reciver.ReadMessage(sender, message);
            }
        }
        public class Chatter
        {
            public ChatRoom Room;

            public string Name { get; set; }
            public Chatter(string name, ChatRoom room = null)
            {
                Name = name;
                room?.Join(this);
            }
            public void Say(string message) { Room.BroadCast(this, message); }
            public void Say(Chatter reciver, string message) { Room.SendMessage(this, reciver, message); }
            public void ReadMessage(Chatter sender, string message) { Console.WriteLine($"[{Name}'s chat] - {sender.Name}: {message}"); }
        }

        [Test]
        public static void Try()
        {
            ChatRoom room = new ChatRoom();

            Chatter samu = new Chatter("Samu", room);
            Chatter enrico = new Chatter("Enrico", room);
            Chatter luca = new Chatter("Luca", room);

            samu.Say("Hi to every ones");
            enrico.Say(samu, "Hi, how are you?");

            Chatter anna = new Chatter("Anna");
            room.Join(anna);

            luca.Say("Here is Anna, a friend of mine");
            luca.Say(anna, "Hi Anna, how are you");
        }
    }
}
