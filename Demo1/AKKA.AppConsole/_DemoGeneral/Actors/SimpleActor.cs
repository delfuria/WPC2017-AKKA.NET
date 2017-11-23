using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;

namespace AKKA.AppConsole1
{
    public class SimpleActor : UntypedActorBase
    {
        public override string Alias => "SimpleActor";

        public SimpleActor()
        {
            var timeout = TimeSpan.FromSeconds(3);
            this.SetReceiveTimeout(timeout);
        }

        public static Props CreateProps()
        {
            return Props.Create(() => new SimpleActor());
        }


        protected override void PreStart()
        {
            base.PreStart();
            var scheduler = ActorsSystem.Instance.Scheduler;
            scheduler.ScheduleTellRepeatedly(TimeSpan.FromSeconds(5),
                TimeSpan.FromSeconds(1),
                Self,
                new AskMessage(){Number = -5},
                Self);

        }

        protected override void OnReceive(object message)
        {
            base.OnReceive(message);
            switch (message)
            {
                //case ResponseMessage msg:
                //    HandleResponseMessage(msg);
                //    break;
                case AskMessage msg:
                    HandleAskMessage(msg);
                    break;
                case DummyMessage msg:
                    HandleDummyMessage(msg);
                    break;
                case ReceiveTimeout msg:
                    HandleReceiveTimeout(msg);
                    break;
                default:
                    Unhandled(message);
                    break;
            }
        }

        private void HandleReceiveTimeout(ReceiveTimeout msg)
        {
            Console.WriteLine("ReceiveTimeout");
        }

        private void HandleDummyMessage(DummyMessage msg)
        {
            Console.WriteLine("Dummy");
        }

        private async Task HandleAskMessage(AskMessage msg)
        {
            Console.WriteLine("before Ask");

            //var selector = Context.ActorSelection("../*");
            //var res = selector.ResolveOne(TimeSpan.FromMilliseconds(10)).Result;
            var actor = Context.ActorSelection("akka://AKKA-NET/user/ResponseActor").ResolveOne(TimeSpan.Zero).Result;
            //actor.Tell(new RequestMessage());
            //var resp = await actor.Ask(new RequestMessage());//.PipeTo(Self);
            try

            {
                var resp = await actor.Ask(new RequestMessage(){Number = msg.Number});//, TimeSpan.FromSeconds(2));//.PipeTo(Self);
                Console.WriteLine(((ResponseMessage)resp).Number);
            }
            catch (Exception)
            {
                Console.WriteLine("Eccezione timeout");
            }
            Console.WriteLine("Ask");

        }

        //private void HandleResponseMessage(ResponseMessage msg)
        //{
        //    throw new NotImplementedException();
        //}

        protected override void ActorInitialize()
        {
            
        }
    }
}
