namespace AKKA.Library.Demo
{
    public class ReliableDeliveryEnvelope<TMessage>
    {
        public ReliableDeliveryEnvelope(TMessage message, long messageId)
        {
            Message = message;
            MessageId = messageId;
        }

        public readonly TMessage Message;

        public readonly long MessageId;
    }

}
