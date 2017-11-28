namespace AKKA.Library.Demo
{
    [ToString]
    public class ActorMessage : AkkaMessageBase
    {
        private readonly int _value;
        public int Value { get { return _value; } }

        public ActorMessage(int value)
        {
            _value = value;
        }
    }
}