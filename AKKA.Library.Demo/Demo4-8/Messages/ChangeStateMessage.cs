using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
