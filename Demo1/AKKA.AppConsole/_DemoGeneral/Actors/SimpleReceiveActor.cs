using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;

namespace AKKA.AppConsole1
{
    public class SimpleReceiveActor : ReceiveActorBase
    {
        public override string Alias => "ResponseActor";

        public SimpleReceiveActor()
        {
            ReceiveAsync<RequestMessage>(async (t) =>
            {
                  await HandleRequestMessage(t);
            });

        }

        public static Props CreateProps()
        {
            return Props.Create(() => new SimpleReceiveActor());
        }

        private async Task<ResponseMessage> HandleRequestMessage(RequestMessage msg)
        {
            return await Test();
        }

        private async Task<ResponseMessage> Test()
        {
            await Task.Delay(5000);
            return new ResponseMessage();
        }
        protected override void ActorInitialize()
        {

        }
    }
}
