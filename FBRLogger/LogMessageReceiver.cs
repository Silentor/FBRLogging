using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading;

namespace Silentor.FBRLogger
{
    /// <summary>
    /// Receives FBR log messages by UDP
    /// </summary>
    public class LogMessageReceiver : IDisposable
    {
        private readonly LogMessage.Formatter _formatter;
        private readonly UdpClient _socket;
        private readonly MemoryStream _dataBuf;
        private Thread _recvWorker;
        private readonly AutoResetEvent _terminateThread;

        /// <summary>
        /// Create receiver with listen port
        /// </summary>
        /// <param name="port"></param>
        public LogMessageReceiver(int port)
        {
            _socket = new UdpClient(port);

            _dataBuf = new MemoryStream(1024);
            _formatter = new LogMessage.Formatter(_dataBuf);

            _terminateThread = new AutoResetEvent(false);
        }

        /// <summary>
        /// Start receiving messages
        /// </summary>
        public void Start()
        {
            _recvWorker = new Thread(ReceiveMessagesLoop) { Name = ("ReceiveMessagesLoop"), IsBackground = true };
            _recvWorker.Start();
        }

        private void ReceiveMessagesLoop()
        {
            var e = new IPEndPoint(IPAddress.Any, 0);

            while (!_terminateThread.WaitOne(500))
            {
                while (_socket.Available > 0)
                {
                    LogMessage fbrMessage;

                    try
                    {
                        var buf = _socket.Receive(ref e);
                        _dataBuf.Position = 0;
                        _dataBuf.Write(buf, 0, buf.Length);
                        _dataBuf.Position = 0;
                        fbrMessage = _formatter.Deserialize();
                    }
                    catch (Exception ex)
                    {
                        fbrMessage = new LogMessage("Silentor.FBRLogger", "Error receiving data", LogMessage.LogLevel.Error, true, ex);
                    }

                    DoMessageReceived(fbrMessage, e.Address.ToString());
                }
            }
        }

        /// <summary>
        /// Stop receiving data and dispose receiver
        /// </summary>
        public void Dispose()
        {
            _terminateThread.Set();
            if (!_recvWorker.Join(200))
                _recvWorker.Abort();

            _socket.Close();
            _formatter.Dispose();
            _terminateThread.Close();
        }

        /// <summary>
        /// Fires when received message
        /// </summary>
        public event Action<LogMessageReceiver, LogMessage, string> MessageReceived;

        private void DoMessageReceived(LogMessage message, string host)
        {
            if (MessageReceived != null)
                MessageReceived(this, message, host);
        }
    }
}