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
            var msg = new LogMessage("Test.Logger", "Test message", LogMessage.LogLevel.Fatal);
            var result = new List<LogMessage>();
            var sender = new LogMessageSender("127.0.0.1", 9999);
            var receiver = new LogMessageReceiver(9999);
            receiver.Start();

            receiver.MessageReceived += (thisReceiver, receiverMsg, host) => result.Add(receiverMsg);

            //Act
            for (var i = 0; i < 10; i++)
                sender.Send(msg);

            //Assert
            for (var i = 0; i < 10; i++)
            {
                Thread.Sleep(100);
                if (result.Count == 10) break;
            }

            Assert.That(result.Count,Is.EqualTo(10));
            Assert.That(result.All(m => m.Level == LogMessage.LogLevel.Fatal));
            Assert.That(result.All(m => m.Logger == "Test.Logger"));
            Assert.That(result.All(m => m.Message == "Test message"));

            sender.Dispose();
            receiver.Dispose();
        }
    }
}
