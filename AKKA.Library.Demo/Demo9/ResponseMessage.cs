using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKKA.Library.Demo
{
    public class ResponseMessage : AkkaMessageBase
    {
        public int Number { get; set; }
    }
}
