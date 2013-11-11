using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;

namespace Silentor.FBRLogger
{
    /// <summary>
    /// Sends FBR log messages to remote log viewer by UDP
    /// </summary>
    public class LogMessageSender : IDisposable
    {
        private readonly LogMessage.Formatter _formatter;
        private readonly UdpClient _socket;
        private readonly MemoryStream _dataBuf;

        private readonly Object _locker = new object();

        /// <summary>
        /// Create sender with host name and port of remote log viewer
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        public LogMessageSender(string host, int port)
        {
            _socket = new UdpClient(host, port);
            _dataBuf = new MemoryStream(1024);
            _formatter = new LogMessage.Formatter(_dataBuf);
        }

        /// <summary>
        /// Send message to remote log viewer. Thread-safe
        /// </summary>
        /// <param name="message"></param>
        public void Send(LogMessage message)
        {
            lock (_locker)
            {
                _dataBuf.Position = 0;
                var dgramLength = _formatter.Serialize(message);
                _socket.Send(_dataBuf.GetBuffer(), dgramLength);
            }
        }

        /// <summary>
        /// Dispose log messages sender
        /// </summary>
        public void Dispose()
        {
            lock (_locker)
            {
                _socket.Close();
                _dataBuf.Close();
            }
        }
    }
}