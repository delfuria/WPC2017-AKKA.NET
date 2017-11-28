using Akka.Actor;
using System;

namespace AKKA.Library.Demo
{
    public class FirstReceivedActor : ReceiveActor
    {
        private string _prefix;

        public FirstReceivedActor()
        {
            Receive<string>(msg => msg.Length < 5, msg => HandleShortStringMessage(msg));
            Receive<string>(msg => HandleStringMessage(msg));
            Receive<SimpleMessage>(msg => HandleSimpleMessage(msg));
        }

        public FirstReceivedActor(string prefix) : this()
        {
            _prefix = prefix;
        }

        public static Props Props(string hello)
        {
            return Akka.Actor.Props.Create(() => new FirstReceivedActor(hello));
        }

        private void HandleShortStringMessage(string msg)
        {
            Console.WriteLine($"Short string:{msg} " +
                              $"\nreceived by {Context.Self.Path}" +
                              $"\nsender {Sender.Path}" +
                              $"\n");
        }

        private void HandleStringMessage(string msg)
        {
            Console.WriteLine($"string:{msg} " +
                              $"\nreceived by {Context.Self.Path}" +
                              $"\nsender {Sender.Path}" +
                              $"\n");
        }

        private void HandleSimpleMessage(SimpleMessage msg)
        {
            Console.WriteLine($"Prefix:{_prefix} - Message:{msg.Value} " +
                              $"\nreceived by {Context.Self.Path}" +
                              $"\nsender {Sender.Path}" +
                              $"\n");
        }
    }
}