using System;

namespace Luna.Utils.Logger
{
    public enum LogLevel
    {
        Debug = 1,
        Info = 2,
        Warning = 3,
        Error = 4,
        Severe = 5,
    }

    public interface ILogger
    {
        string Tag { get; set; }
        void Debug(string format, params object[] args);
        void Info(string format, params object[] args);
        void Warning(string format, params object[] args);
        void Error(string format, params object[] args);
        void Exception(Exception ex);
        void Log(LogLevel level, string format, params object[] args);
    }
}
