using System;
using System.Threading;

namespace Silentor.FBRLogger.Runner
{
    /// <summary>
    /// Sends FBR messages for testing purposes
    /// </summary>
    class Program
    {
        static readonly Random rnd = new Random();

        static void Main(string[] args)
        {
            var levels = Enum.GetValues(typeof (LogMessage.LogLevel)) as LogMessage.LogLevel[];
            var sender = new LogMessageSender("127.0.0.1", 9998);
            
            for (int i = 0; i < 10; i++)
            {
                var level = levels[rnd.Next(levels.Length)];

                try
                {
                    throw new Exception(i.ToString());
                }
                catch (Exception ex)
                {
                    var msg = new LogMessage("Test.Logger", "Test message", level, true, ex);
                    sender.Send(msg);    
                }

                Thread.Sleep(1);
            }
        }
    }
}
