using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;

namespace AKKA.Demo.Library
{
    public class SimpleSubActor1 : UntypedActorBase
    {
        private Guid _randomId;
        private int _value;
        public override string Alias => "SimpleSubActor1";

        public SimpleSubActor1(Guid id)
        {
            _randomId = Guid.NewGuid();
            _id = id;
        }

        public static Props CreateProps(Guid id = new Guid())
        {
            //if (id == Guid.Empty)
            //    id = Guid.NewGuid();
            return Props.Create(() => new SimpleSubActor1(id));
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
            Console.WriteLine($"randomID {_randomId}");
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
