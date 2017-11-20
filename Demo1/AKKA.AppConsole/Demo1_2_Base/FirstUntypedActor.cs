using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;

namespace AKKA.Demo.Library
{
    public class FirstUntypedActor : UntypedActor
    {
        private string _hello = "";
        public FirstUntypedActor(string hello)
        {
            _hello = hello;
        }

        protected override void OnReceive(object message)
        {
            switch (message)
            {
                case SimpleMessage msg:
                    HandleSimpleMessage(msg);
                    break;
                case string str:
                    if (str.Length < 5)
                        ShortStringMessage(str);
                    else
                        StringMessage(str);

                    break;
                default:
                    break;
            }
        }

        public static Props Props(string hello)
        {
            return Akka.Actor.Props.Create(() => new FirstUntypedActor(hello));
        }

        private void ShortStringMessage(string msg)
        {
            Console.WriteLine($"Short Message:{msg}\nrom {Context.Self.Path}\n");
        }
        private void StringMessage(string msg)
        {
            //Console.WriteLine($"Message:{msg} \nreceived by {Context.Self.Path}\n");
            Console.WriteLine($"Hello:{_hello} - Message:{msg} \nreceived by {Context.Self.Path}\n");
        }
        private void HandleSimpleMessage(SimpleMessage msg)
        {
            //Console.WriteLine($"Message:{msg} \nreceived by {Context.Self.Path}\n");
            Console.WriteLine($"Hello:{_hello} - Message:{msg.Value} " +
                              $"\nreceived by {Context.Self.Path}" +
                              $"\nsender {Sender.Path}\n");
        }
    }
}
