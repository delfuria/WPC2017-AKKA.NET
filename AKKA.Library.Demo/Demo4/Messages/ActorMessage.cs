using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKKA.Demo.Library
{
    [ToString]
    public class ActorMessage :AkkaMessageBase
    {
        private readonly int _value;
        public int Value { get { return _value; } }
        public ActorMessage(int value)
        {
            _value = value;
        }
    }
}
