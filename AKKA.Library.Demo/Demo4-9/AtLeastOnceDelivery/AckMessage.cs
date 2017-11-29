namespace AKKA.Library.Demo
{

    public class AckMessage
    {
        public AckMessage(long messageId)
        {
            MessageId = messageId;
        }

        public readonly long MessageId;
    }
}
