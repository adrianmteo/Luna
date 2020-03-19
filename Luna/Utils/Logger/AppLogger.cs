using System.Diagnostics;

namespace Luna.Utils.Logger
{
    public static class AppLogger
    {
        public static ILogger GetLoggerForCurrentClass()
        {
            ILogger logger = new MultiLogger(
                new FileLogger("application.log") { MinimumLevel = LogLevel.Info },
                new FileLogger("error.log") { MinimumLevel = LogLevel.Error },
                new ConsoleLogger() { MaximumLevel = LogLevel.Error }
            );

            StackTrace trace = new StackTrace();

            if (trace.FrameCount > 1)
            {
                logger.Tag = trace.GetFrame(1).GetMethod().DeclaringType.Name;
            }

            return logger;
        }
    }
}
