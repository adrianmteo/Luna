using System;

namespace Luna.Utils.Logger
{
    public class ConsoleLogger : AbstractLogger
    {
        internal override void WriteMessage(string message)
        {
            Console.WriteLine(message);
        }
    }
}
