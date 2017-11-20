using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKKA.Demo.Library
{
    public class SimpleMessage
    {
        public string Value { get; private set; }
        public SimpleMessage(string value)
        {
            Value = value;
        }
    }
}
