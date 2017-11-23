using System;

namespace AKKA.Library.Demo
{
    public interface ILogger
    {
        void SetFileName(string path, string fileName);
        void FatalException(string msg, Exception ex);
        void ErrorException(string msg, Exception ex);
        void WarnException(string msg, Exception ex);
        void InfoException(string msg, Exception ex);
        void DebugException(string msg, Exception ex);
        void VerboseException(string msg, Exception ex);

        void FatalFormat(string fmt, params object[] parms);
        void ErrorFormat(string fmt, params object[] parms);
        void WarnFormat(string fmt, params object[] parms);
        void InfoFormat(string fmt, params object[] parms);
        void DebugFormat(string fmt, params object[] parms);
        void VerboseFormat(string fmt, params object[] parms);

        void Fatal(params object[] fmt);
        void Error(params object[] fmt);
        void Warn(params object[] fmt);
        void Info(params object[] fmt);
        void Debug(params object[] fmt);
        void Verbose(params object[] fmt);
    }
}
