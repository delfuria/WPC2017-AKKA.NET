using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;

namespace AKKA.Library.Demo
{
    public class SenderUntypedActor : UntypedActor
    {
        private IActorRef _firstUntyped;
        public IActorRef FirstUntyped
        {
            get
            {
                if (_firstUntyped == null)
                    _firstUntyped = Context.ActorSelection($"/user/FirstUntypedActor")
                                            .ResolveOne(TimeSpan.FromSeconds(2))
                                            .Result;

                return _firstUntyped;
            }
            private set { _firstUntyped = value; }
        }

        public SenderUntypedActor()

        {
        }

        public SenderUntypedActor(IActorRef firstUntyped)
        {
            FirstUntyped = firstUntyped;
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
                              $"\nsender {Sender.Path}" +
                              $"\nforwarded to {FirstUntyped.Path}" +
                              $"\n");
            FirstUntyped.Tell(msg);

        }
    }
}
