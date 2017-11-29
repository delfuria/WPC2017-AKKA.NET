namespace AKKA.Library.Demo
{
    public class EnvelopeMessage<TMessage>
    {
        public EnvelopeMessage(TMessage message, long messageId)
        {
            Message = message;
            MessageId = messageId;
        }

        public readonly TMessage Message;

        public readonly long MessageId;
    }

}
