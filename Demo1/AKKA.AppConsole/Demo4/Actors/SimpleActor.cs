using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.Routing;

namespace AKKA.Demo.Library
{
    public class SimpleActor : UntypedActorBase
    {
        public override string Alias => "SimpleActor";

        public SimpleActor(Guid id)
        {
            _id = id;
        }

        public static Props CreateProps(Guid id = new Guid())
        {
            //if (id == Guid.Empty)
            //    id = Guid.NewGuid();
            return Props.Create(() => new SimpleActor(id));
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

        //protected override SupervisorStrategy SupervisorStrategy()
        //{

        //    return new OneForOneStrategy(
        //        maxNrOfRetries: 2,
        //        withinTimeRange: TimeSpan.FromMinutes(2),
        //        localOnlyDecider: ex =>
        //        {
        //            logger.Info($"Exception raised:{ex.ToString()}");

        //            switch (ex)
        //            {
        //                case FormatException fe:
        //                    return Directive.Resume;
        //                case NullReferenceException nre:
        //                    return Directive.Restart;
        //                case Exception e:
        //                    return Directive.Stop;
        //                default:
        //                    return Directive.Escalate;
        //            }
        //        });
        //}

        private void HandleRaiseExceptionMessage(RaiseExceptionMessage msg)
        {
            IActorRef sub1 = ActorsSystem.Actors[new ActorReference("SimpleSubActor1", _id)];
            sub1.Tell(msg);
        }

        private void HandleActorMessage(ActorMessage msg)
        {
            IActorRef sub1 = ActorsSystem.Actors[new ActorReference("SimpleSubActor1", _id)];
            sub1.Tell(msg);

        }

        protected override void ActorInitialize()
        {
            Context.ActorOf(SimpleSubActor1.CreateProps(_id), "SimpleSubActor1");
            Context.ActorOf(SimpleSubActor2.CreateProps(_id), "SimpleSubActor2");
        }
    }
}
