using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Silentor.FBRLogger;
using NUnit.Framework;
namespace Silentor.FBRLogger.Tests
{
    [TestFixture()]
    public class LogMessageReceiverTests
    {
        [Test()]
        public void ReceivePackTest()
        {
            //Arrange
            var result = new List<LogMessage>();
            var sender = new LogMessageSender("127.0.0.2", 9997);
            var receiver = new LogMessageReceiver(9997);

            receiver.MessageReceived += (thisReceiver, receiverMsg, host) => 
                result.Add(receiverMsg);

            //Act
            for (var i = 0; i < 10; i++)
                sender.Send(new LogMessage("Test.Logger", "Message " + (i + 1), LogMessage.LogLevel.Trace, false));

            receiver.Start();

            for (var i = 0; i < 10; i++)
            {
                Thread.Sleep(100);
                if (result.Count == 10) break;
            }

            //Assert
            Assert.That(result.Count, Is.EqualTo(10));
            Assert.That(result.All(m => m.Level == LogMessage.LogLevel.Trace));
            Assert.That(result.All(m => m.Logger == "Test.Logger"));
            Assert.That(result.All(m => m.Message.StartsWith("Message")));

            sender.Dispose();
            receiver.Dispose();
        }
    }
}
