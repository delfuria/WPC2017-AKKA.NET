using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKKA.AppConsole1
{
    public class RequestMessage : AkkaMessageBase
    {
        public int Number { get; set; }
    }
}
