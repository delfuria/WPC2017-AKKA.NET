using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.Persistence;

namespace AKKA.Library.Demo
{
    public class SimpleDeliveryActor : AtLeastOnceDeliveryActor
    {
        public override string PersistenceId => "SimpleDeliveryActor";
        private readonly IActorRef _destinationActor;
        private ICancelable _recurringMessageSend;

        public SimpleDeliveryActor(IActorRef destinationActor)
        {
            _destinationActor = destinationActor;
        }

        protected override bool ReceiveRecover(object message)
        {
            switch (message)
            {
                case AckMessage msg:
                    Handler(msg);
                    break;
                case WriteMessage msg:
                    Handler(msg);
                    break;

            }
            return false;
        }

        protected override bool ReceiveCommand(object message)
        {
            switch (message)
            {
                case AckMessage msg:
                    Persist(new AckMessage(msg.MessageId), Handler);
                    return false;
                    break;
                case WriteMessage msg:
                    Persist(msg, Handler);
                    return true;
                    break;

            }
            return false;
        }

        protected override void PreStart()
        {
            //_recurringMessageSend = Context.System.Scheduler.ScheduleTellRepeatedlyCancelable(TimeSpan.FromSeconds(1),
            //    TimeSpan.FromSeconds(10), Self, new WriteMessage("SomeText"), Self);

            base.PreStart();
        }

        protected override void PostStop()
        {
            _recurringMessageSend?.Cancel();
            base.PostStop();
        }


        private void Handler(WriteMessage message)
        {
            Deliver(_destinationActor.Path, deliveryId => new EnvelopeMessage<WriteMessage>(message, deliveryId));
        }
        private void Handler(AckMessage confirmed)
        {
            ConfirmDelivery(confirmed.MessageId);
        }
    }
}
