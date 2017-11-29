namespace AKKA.Library.Demo
{
    [ToString]
    public class ChangeStateMessage : AkkaMessageBase
    {
        private readonly int _value;
        public int Value { get { return _value; } }

        public ChangeStateMessage(int value)
        {
            _value = value;
        }
    }
}