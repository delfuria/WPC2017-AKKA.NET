using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;

namespace AKKA.Demo.Library
{
    public class SenderUntypedActor : UntypedActor
    {
        private IActorRef _firstUntyped;
        public SenderUntypedActor(IActorRef firstUntyped)
        {
            _firstUntyped = firstUntyped;
        }

        protected override void OnReceive(object message)
        {
            switch (message)
            {
                case SimpleMessage msg:
                    HandleSimpleMessage(msg);
                    break;
                default:
                    break;
            }
        }

        public static Props Props(IActorRef firstUntyped)
        {
            return Akka.Actor.Props.Create(() => new SenderUntypedActor(firstUntyped));
        }

        private void HandleSimpleMessage(SimpleMessage msg)
        {
            //Console.WriteLine($"Message:{msg} \nreceived by {Context.Self.Path}\n");
            Console.WriteLine($"Message:{msg.Value} " +
                              $"\nreceived by {Context.Self.Path}" +
                              $"\nforwarded to {_firstUntyped.Path}\n");
            _firstUntyped.Forward(msg);
            
        }
    }
}
