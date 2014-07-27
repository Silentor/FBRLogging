using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Silentor.FBRLogger;
using NUnit.Framework;
namespace Silentor.FBRLogger.Tests
{
    [TestFixture]
    public class LogMessageSenderTests
    {
        [Test]
        public void SendAndReceiveTest()
        {
            //Arrange
            var msg = new LogMessage("Test.Logger", "Test message", LogMessage.LogLevel.Fatal, true, new Exception());
            var msg2 = new LogMessage("1", "2");     //to satisfy compiler
            var sender = new LogMessageSender("127.0.0.1", 9995);
            var receiver = new LogMessageReceiver(9995);
            receiver.Start();

            receiver.MessageReceived += (thisReceiver, receiverMsg, host) =>
            {
                msg2 = receiverMsg;
            };

            //Act
            sender.Send(msg);

            //Assert
            Assert.That(() => msg2, Has.
                Property("Logger").EqualTo("Test.Logger").And.
                Property("Thread").EqualTo(msg.Thread).And.
                Property("Message").EqualTo("Test message").And.
                Property("Stack").EqualTo(msg.Stack).And.
                Property("Exception").EqualTo(msg.Exception).And.
                Property("Counter").EqualTo(msg.Counter).And.
                Property("Level").EqualTo(msg.Level).And.
                Property("TimeStamp").EqualTo(msg.TimeStamp).
                After(1000));

            sender.Dispose();
            receiver.Dispose();
        }

        [Test]
        public void ConcurrentSenderBenchmark()
        {
            var timer = Stopwatch.StartNew();

            var tasks = new Task[5];
            for(var i = 0; i < 5; i++)
            {
                tasks[i] = Task.Run(async () =>
                {
                    var sender = new LogMessageSender("127.0.0.1", 9995);
                    var msg = new LogMessage("Test.Logger", "Test message", LogMessage.LogLevel.Fatal, true,
                        new Exception());
                    var rnd = new Random();

                    for (int j = 0; j < 10000; j++)
                    {
                        sender.Send(msg);
                        //await Task.Delay(rnd.Next(1, 50));
                    }
                });
            }

            Task.WaitAll(tasks);
            Trace.TraceInformation("Time: {0}", timer.ElapsedMilliseconds);
        }

        [Test()]
        public void DisposeTest()
        {
            //Arrange
            var sender = new LogMessageSender("127.0.0.1", 9996);
            sender.Dispose();

            //Act
            //Assert
            Assert.That(() => sender.Send(new LogMessage("a", "b")), Throws.Exception);
        }
    }
}
