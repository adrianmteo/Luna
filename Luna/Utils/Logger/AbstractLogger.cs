using System;

namespace Luna.Utils.Logger
{
    public static class LogMessageToken
    {
        public static readonly string Date = "{date}";
        public static readonly string Level = "{level}";
        public static readonly string Message = "{message}";
        public static readonly string Tag = "{tag}";
    }

    public abstract class AbstractLogger : ILogger
    {
        public string Tag { get; set; }
        public string MessageFormat { get; set; }
            = $"[{LogMessageToken.Date}] ({LogMessageToken.Level}) {LogMessageToken.Tag}: {LogMessageToken.Message}";

        public string DateFormat { get; set; }

        public LogLevel MinimumLevel { get; set; } = LogLevel.Debug;

        public LogLevel MaximumLevel { get; set; } = LogLevel.Severe;

        public void Debug(string format, params object[] args)
        {
            Log(LogLevel.Debug, format, args);
        }

        public void Info(string format, params object[] args)
        {
            Log(LogLevel.Info, format, args);
        }

        public void Warning(string format, params object[] args)
        {
            Log(LogLevel.Warning, format, args);
        }

        public void Error(string format, params object[] args)
        {
            Log(LogLevel.Error, format, args);
        }

        public void Exception(Exception ex)
        {
            Log(LogLevel.Severe, ex.ToString());
        }

        public void Log(LogLevel level, string format, params object[] args)
        {
            if (level >= MinimumLevel && level <= MaximumLevel)
            {
                string date = DateTime.Now.ToString(DateFormat);
                string log = string.Format(format, args);

                string[] lines = log.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

                foreach (string line in lines)
                {
                    string message = MessageFormat
                        .Replace(LogMessageToken.Date, date)
                        .Replace(LogMessageToken.Level, level.ToString().ToUpper())
                        .Replace(LogMessageToken.Message, line)
                        .Replace(LogMessageToken.Tag, Tag);

                    WriteMessage(message);
                }
            }
        }

        internal virtual void WriteMessage(string message)
        {
            throw new NotImplementedException();
        }
    }
}
