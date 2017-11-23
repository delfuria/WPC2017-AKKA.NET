using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;

namespace AKKA.Demo.Library
{
    public class FirstReceivedActor : ReceiveActor
    {
        private string _hello;
        public FirstReceivedActor()
        {
            Receive<string>(msg => msg.Length < 5, msg => ShortStringMessage(msg));
            Receive<string>(msg => StringMessage(msg));
            Receive<SimpleMessage>(msg => HandleSimpleMessage(msg));

        }
        public FirstReceivedActor(string hello)
        {
            _hello = hello;
            Receive<string>(msg => msg.Length < 5, msg => ShortStringMessage(msg));
            Receive<string>(msg => StringMessage(msg));
            Receive<SimpleMessage>(msg => HandleSimpleMessage(msg));

        }

        public static Props Props(string hello)
        {
            return Akka.Actor.Props.Create(() => new FirstReceivedActor(hello));
        }
        private void ShortStringMessage(string msg)
        {
            Console.WriteLine($"Short Message:{msg} " +
                              $"\nreceived by {Context.Self.Path}" +
                              $"\nsender {Sender.Path}"+
                              $"\n");
        }
        private void StringMessage(string msg)
        {
            Console.WriteLine($"Message:{msg} " +
                              $"\nreceived by {Context.Self.Path}" +
                              $"\nsender {Sender.Path}" +
                              $"\n");
        }
        private void HandleSimpleMessage(SimpleMessage msg)
        {
            Console.WriteLine($"Message:{msg.Value} " +
                              $"\nreceived by {Context.Self.Path}" +
                              $"\nsender {Sender.Path}" +
                              $"\n");
        }
    }
}
