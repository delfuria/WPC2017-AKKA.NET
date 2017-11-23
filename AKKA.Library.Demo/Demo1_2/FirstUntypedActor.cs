using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;

namespace AKKA.Library.Demo
{
    public class FirstUntypedActor : UntypedActor
    {
        private string _prefix = "";
        public FirstUntypedActor()
        {
        }
        public FirstUntypedActor(string prefix)
        {
            _prefix = prefix;
        }

        protected override void OnReceive(object message)
        {
            switch (message)
            {
                case SimpleMessage msg:
                    HandleSimpleMessage(msg);
                    break;
                case string str when str.Length < 5:
                    HandleShortStringMessage(str);
                    break;
                case string str:
                    HandleStringMessage(str);
                    break;
                default:
                    break;
            }
        }

        public static Props Props(string hello)
        {
            return Akka.Actor.Props.Create(() => new FirstUntypedActor(hello));
        }

        private void HandleShortStringMessage(string msg)
        {
            Console.WriteLine($"Short string:{msg}" +
                              $"\nreceived by {Context.Self.Path}" +
                              $"\nsender {Sender.Path}\n");
        }
        private void HandleStringMessage(string msg)
        {
            //Console.WriteLine($"Message:{msg} \nreceived by {Context.Self.Path}\n");
            Console.WriteLine($"string:{msg} " +
                              $"\nreceived by {Context.Self.Path}" +
                              $"\nsender {Sender.Path}\n");
        }
        private void HandleSimpleMessage(SimpleMessage msg)
        {
            //Console.WriteLine($"Message:{msg} \nreceived by {Context.Self.Path}\n");
            Console.WriteLine($"Prefix:{_prefix} - Message:{msg.Value} " +
                              $"\nreceived by {Context.Self.Path}" +
                              $"\nsender {Sender.Path}\n");
        }
    }
}
