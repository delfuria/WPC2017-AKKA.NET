using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.Pattern;
using Akka.Routing;

namespace AKKA.Library.Demo
{
    public class SimpleActor : UntypedActorBase
    {
        public override string Alias => "SimpleActor";

        public int State { get; private set; }
        public SimpleActor()
        {
        }

        public static Props CreateProps()
        {
            return Props.Create(() => new SimpleActor());
        }

        protected override void OnReceive(object message)
        {
            base.OnReceive(message);
            switch (message)
            {
                case SimpleMessage msg:
                    HandleSimpleMessage(msg);
                    break;
                case ActorMessage msg:
                    HandleActorMessage(msg);
                    break;
                case ChangeStateMessage msg:
                    HandleChangeStateMessage(msg);
                    break;
                case ExceptionMessage msg:
                    HandleExceptionMessage(msg);
                    break;
                case RaiseExceptionMessage msg:
                    HandleRaiseExceptionMessage(msg);
                    break;
                //case BackOfficeRaiseExceptionMessage msg:
                //    HandleBackOfficeRaiseExceptionMessage(msg);
                //    break;
                case RequestMessage msg:
                    HandleRequestMessage(msg);
                    break;
            }
        }

        private void HandleChangeStateMessage(ChangeStateMessage msg)
        {
            State = msg.Value;
        }

        private void HandleExceptionMessage(ExceptionMessage msg)
        {
            throw new Exception();
        }
        protected override SupervisorStrategy SupervisorStrategy()
        {

            return new OneForOneStrategy(
                maxNrOfRetries: 2,
                withinTimeRange: TimeSpan.FromMinutes(2),
                localOnlyDecider: ex =>
                {
                    logger.Info($"Exception raised:{ex.ToString()}");

                    switch (ex)
                    {
                        case FormatException fe:
                            return Directive.Resume;
                        case NullReferenceException nre:
                            return Directive.Restart;
                        case Exception e:
                            return Directive.Stop;
                        default:
                            return Directive.Escalate;
                    }
                });
        }

        private void HandleSimpleMessage(SimpleMessage msg)
        {
            Console.WriteLine($"SimpleMessage received {msg.Value}");
            Console.WriteLine($"path {Self.Path}");
            Console.WriteLine($"sender {Sender.Path}");
        }

        private void HandleRaiseExceptionMessage(RaiseExceptionMessage msg)
        {
            IActorRef sub1 = ActorsSystem.Actors["SimpleSubActor1"];
            sub1.Forward(msg);
        }

        private void HandleActorMessage(ActorMessage msg)
        {
            var sub1 = Context.ActorSelection("akka://AKKA-NET/user//SimpleActor/SimpleSubActor1").ResolveOne(TimeSpan.Zero).Result;
            sub1.Tell(msg);

        }
        private async Task HandleRequestMessage(RequestMessage msg)
        {
            Console.WriteLine("before Ask");

            //var selector = Context.ActorSelection("../*");
            //var res = selector.ResolveOne(TimeSpan.FromMilliseconds(10)).Result;
            var actor = Context.ActorSelection("akka://AKKA-NET/user/ResponseActor").ResolveOne(TimeSpan.Zero).Result;
            //actor.Tell(new RequestMessage());
            //var resp = await actor.Ask(new RequestMessage());//.PipeTo(Self);
            try

            {
                var resp = await actor.Ask(new RequestMessage() { Number = msg.Number });//, TimeSpan.FromSeconds(2));//.PipeTo(Self);
                Console.WriteLine(((ResponseMessage)resp).Number);
            }
            catch (Exception)
            {
                Console.WriteLine("Eccezione timeout");
            }
            Console.WriteLine("Ask");

        }


        protected override void ActorInitialize()
        {
            //var childProps = SimpleSubActor1.CreateProps();

            //var supervisor = BackoffSupervisor.Props(
            //    Backoff.OnFailure(
            //            childProps: childProps,
            //            childName: "SimpleSubActor1",
            //            minBackoff: TimeSpan.FromSeconds(10),
            //            maxBackoff: TimeSpan.FromSeconds(60),
            //            randomFactor: 0.2)
            //        .WithAutoReset(TimeSpan.FromSeconds(160)));

            //Context.ActorOf(supervisor, "SimpleSubActor1:Supervisor");

            var sub1 = Context.ActorOf(SimpleSubActor1.CreateProps(), "SimpleSubActor1");
            var sub2 = Context.ActorOf(SimpleSubActor2.CreateProps(), "SimpleSubActor2");
        }
    }
}
