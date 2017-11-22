using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKKA.Demo.Library
{
    [ToString]
    public class AkkaMessageBase
    {
        private readonly DateTime _timeStamp = DateTime.Now.ToUniversalTime();
        public DateTime TimeStamp { get { return _timeStamp; } }

    }
}
