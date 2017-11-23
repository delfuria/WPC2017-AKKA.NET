using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKKA.Library.Demo
{
    public class ResponseActor : UntypedActorBase
    {
        public override string Alias => "ResponseActor";

        public ResponseActor()
        {
        }

        public static Props CreateProps()
        {
            return Props.Create(() => new ResponseActor());
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
            var replyNumber = -msg.Number;
            var sender = Context.Sender;
            await Task.Delay(3000);
            sender?.Tell(new ResponseMessage() { Number = replyNumber });
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
