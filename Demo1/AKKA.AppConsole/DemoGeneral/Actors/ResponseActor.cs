using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;
using System.Threading;

namespace AKKA.AppConsole
{
    public class ResponseActor : UntypedActorBase
    {
        public override string Alias => "ResponseActor";

        public ResponseActor(Guid id)
        {
            _id = id;
        }

        public static Props CreateProps(Guid id)
        {
            return Props.Create(() => new ResponseActor(id));
        }


        protected override void OnReceive(object message)
        {
            base.OnReceive(message);
            switch (message)
            {
                case RequestMessage msg:
                    HandleRequestMessage(msg);
                    break;
                default:
                    Unhandled(message);
                    break;
            }
        }

        private async void HandleRequestMessage(RequestMessage msg)
        {
            await Task.Delay(3000);
            Sender?.Tell(new ResponseMessage() { Number = -msg.Number });
            Console.WriteLine("Response");
        }

        //private async Task<ResponseMessage> Test()
        //{
        //    await Task.Delay(5000);
        //    return new ResponseMessage();
        //}
        protected override void ActorInitialize()

        {

        }
    }
}
