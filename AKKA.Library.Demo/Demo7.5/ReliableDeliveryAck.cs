namespace AKKA.Library.Demo
{

    public class ReliableDeliveryAck
    {
        public ReliableDeliveryAck(long messageId)
        {
            MessageId = messageId;
        }

        public readonly long MessageId;
    }
}
