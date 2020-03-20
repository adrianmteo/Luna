using System;

namespace Luna.Utils.Logger
{
    public class MultiLogger : ILogger
    {
        public ILogger[] Loggers { get; set; }

        private string _tag;

        public string Tag
        {
            get
            {
                return _tag;
            }
            set
            {
                _tag = value;

                foreach (ILogger logger in Loggers)
                {
                    logger.Tag = value;
                }
            }
        }

        public MultiLogger(params ILogger[] loggers)
        {
            Loggers = loggers;
        }

        public void Debug(string format, params object[] args)
        {
            foreach (ILogger logger in Loggers)
            {
                logger.Debug(format, args);
            }
        }

        public void Info(string format, params object[] args)
        {
            foreach (ILogger logger in Loggers)
            {
                logger.Info(format, args);
            }
        }

        public void Warning(string format, params object[] args)
        {
            foreach (ILogger logger in Loggers)
            {
                logger.Warning(format, args);
            }
        }

        public void Error(string format, params object[] args)
        {
            foreach (ILogger logger in Loggers)
            {
                logger.Error(format, args);
            }
        }

        public void Exception(Exception ex)
        {
            foreach (ILogger logger in Loggers)
            {
                logger.Exception(ex);
            }
        }

        public void Log(LogLevel level, string format, params object[] args)
        {
            foreach (ILogger logger in Loggers)
            {
                logger.Log(level, format, args);
            }
        }
    }
}
