using System;
using System.IO;
using System.Text;
using NLog;
using NLog.Config;
using NLog.Layouts;
using NLog.Targets;

namespace Silentor.FBRLogger.NLogTarget
{
    /// <summary>
    /// Target for sending NLog events to FBR
    /// </summary>
    [Target("FBR")]
    public class FBRTarget : Target
    {
        private LogMessageSender _sender;

        /// <summary>
        /// The name of the remote log viewer host ("localhost" or "192.168.0.1"). Default: localhost
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// Port of the remote log viewer host 
        /// </summary>
        [RequiredParameter]
        public int Port { get; set; }

        protected override void InitializeTarget()
        {
            base.InitializeTarget();

            if (string.IsNullOrEmpty(Host)) Host = "localhost";

            _sender = new LogMessageSender(Host, Port);
        }

        /// <summary>
        /// Write NLog event to FBR target
        /// </summary>
        /// <param name="logEvent"></param>
        protected override void Write(LogEventInfo logEvent)
        {
            var msg = new LogMessage(logEvent.SequenceID, logEvent.LoggerName, logEvent.FormattedMessage,
                LogMessage.LogLevels[logEvent.Level.Ordinal],
                logEvent.HasStackTrace ? logEvent.StackTrace.ToString() : String.Empty, logEvent.Exception);

            _sender.Send(msg);
        }

        protected override void CloseTarget()
        {
            if (_sender != null)
            {
                _sender.Dispose();
                _sender = null;
            }

            base.CloseTarget();
        }
    }
}
