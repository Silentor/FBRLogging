using System;
using System.Diagnostics;

namespace Silentor.FBRLogger
{
    /// <summary>
    /// Log message (FBR format)
    /// </summary>
    public partial struct LogMessage
    {
        private static uint _counter = 0;

        /// <summary>
        /// Preferred constructor for manual log message creation
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="message"></param>
        /// <param name="level"></param>
        /// <param name="includeStack"></param>
        /// <param name="exception"></param>
        public LogMessage(string logger, string message, LogLevel level = LogLevel.Log, bool includeStack = true, Exception exception = null)
            : this(_counter++, logger, message, level, includeStack ? new StackTrace(1, true).ToString() : string.Empty, exception)
        {
        }

        /// <summary>
        /// Preferred constructor for convertation from some another log message format
        /// </summary>
        /// <param name="counter"></param>
        /// <param name="logger"></param>
        /// <param name="message"></param>
        /// <param name="level"></param>
        /// <param name="stack"></param>
        /// <param name="exception"></param>
        public LogMessage(uint counter, string logger, string message, LogLevel level, string stack, Exception exception) : this()
        {
            Counter = counter;
            TimeStamp = DateTime.Now;
            Level = level;
            Logger = logger;
            Message = message;
            Thread = System.Threading.Thread.CurrentThread.ManagedThreadId;
            Stack = stack;
            Exception = exception != null ? exception.ToString() : String.Empty;
        }

        /// <summary>
        /// Preferred constructor for convertation from some another log message format
        /// </summary>
        /// <param name="counter"></param>
        /// <param name="logger"></param>
        /// <param name="message"></param>
        /// <param name="level"></param>
        /// <param name="stack"></param>
        /// <param name="exception"></param>
        public LogMessage(string logger, string message, LogLevel level, string stack, Exception exception)
            : this(_counter++, logger, message, level, stack, exception)
        {
        }

        /// <summary>
        /// Deserializing constructor
        /// </summary>
        /// <param name="counter"></param>
        /// <param name="dateTime"></param>
        /// <param name="logger"></param>
        /// <param name="message"></param>
        /// <param name="level"></param>
        /// <param name="threadId"></param>
        /// <param name="stack"></param>
        /// <param name="exception"></param>
        private LogMessage(uint counter, long dateTime, string logger, string message, LogLevel level, int threadId, string stack, string exception)
            : this()
        {
            Counter = counter;
            TimeStamp = DateTime.FromBinary(dateTime);
            Level = level;
            Logger = logger;
            Message = message;
            Thread = threadId;
            Stack = stack;
            Exception = exception;
        }

        public enum LogLevel {Trace, Debug, Log, Warning, Error, Fatal}

        //Required fileds
        public uint Counter { get; private set; }
        public string Logger { get; private set; }
        public string Message { get; private set; }
        public LogLevel Level { get; private set; }
        public DateTime TimeStamp { get; private set; }
        public int Thread { get; private set; }

        //Optional Fields
        public string Stack { get; private set; }
        public string Exception { get; private set; }

        public static readonly LogLevel[] LogLevels =
        {
            LogLevel.Trace,
            LogLevel.Debug,
            LogLevel.Log,
            LogLevel.Warning,
            LogLevel.Error,
            LogLevel.Fatal,
        };

    }
}
