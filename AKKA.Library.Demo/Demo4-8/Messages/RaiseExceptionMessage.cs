using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKKA.Library.Demo
{
    [ToString]
    public class RaiseExceptionMessage : AkkaMessageBase
    {
        private readonly Exception _ex;
        public Exception Value { get { return _ex; } }
        public RaiseExceptionMessage(Exception ex)
        {
            _ex = ex;
        }
    }
}
