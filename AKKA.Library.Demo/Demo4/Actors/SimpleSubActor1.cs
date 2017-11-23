using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;

namespace AKKA.Library.Demo
{
    public class SimpleSubActor1 : UntypedActorBase
    {
        private int _value;
        public override string Alias => "SimpleSubActor1";

        public SimpleSubActor1()
        {
        }

        public static Props CreateProps()
        {
            return Props.Create(() => new SimpleSubActor1());
        }

        protected override void OnReceive(object message)
        {
            base.OnReceive(message);
            switch (message)
            {
                case ActorMessage msg:
                    HandleActorMessage(msg);
                    break;
                case RaiseExceptionMessage msg:
                    HandleRaiseExceptionMessage(msg);
                    break;
            }
        }

        private void HandleActorMessage(ActorMessage msg)
        {
            Console.WriteLine($"The previous state was {_value}");
            _value += msg.Value;
            Console.WriteLine($"The current state is now {_value}");
            Console.WriteLine($"path {Self.Path}");
            Console.WriteLine($"randomID {_id}");
            Console.WriteLine($"sender {Sender.Path}\n" );
        }

        private void HandleRaiseExceptionMessage(RaiseExceptionMessage msg)
        {
            //throw new FormatException();
            throw new NullReferenceException();
            throw new Exception();
        }

        protected override void ActorInitialize()
        {
            
        }
    }
}
