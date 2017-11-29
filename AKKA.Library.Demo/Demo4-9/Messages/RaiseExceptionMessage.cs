using System;

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