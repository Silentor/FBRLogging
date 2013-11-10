using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Silentor.FBRLogger;
using NUnit.Framework;
namespace Silentor.FBRLogger.Tests
{
    [TestFixture()]
    public class LogMessageSenderTests
    {
        [Test()]
        public void SendAndReceiveTest()
        {
            //Arrange
            var msg = new LogMessage("Test.Logger", "Test message", LogMessage.LogLevel.Fatal, true, new Exception());
            var msg2 = new LogMessage("1", "2");     //to satisfy compiler
            var sender = new LogMessageSender("127.0.0.1", 9999);
            var receiver = new LogMessageReceiver(9999);
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

        [Test()]
        public void DisposeTest()
        {
            //Arrange
            var sender = new LogMessageSender("127.0.0.1", 9999);
            sender.Dispose();

            //Act
            //Assert
            Assert.That(() => sender.Send(new LogMessage("a", "b")), Throws.Exception);
        }
    }
}
