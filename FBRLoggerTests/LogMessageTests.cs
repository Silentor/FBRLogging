using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Silentor.FBRLogger.Tests
{
    [TestFixture()]
    public class LogMessageTests
    {
        [Test()]
        public void SequenceMessagesTest()
        {
            //Arrange
            //Act
            var msg1 = new LogMessage("Test.Logger1", "Msg");
            var msg2 = new LogMessage("Test.Logger2", "Msg", LogMessage.LogLevel.Warning);

            //Assert
            Assert.That(msg1.Message, Is.EqualTo(msg2.Message));
            Assert.That(msg1.Logger, Is.Not.EqualTo(msg2.Logger));
            Assert.That(msg1.Level, Is.Not.EqualTo(msg2.Level));
            Assert.That(msg1.Level, Is.EqualTo(LogMessage.LogLevel.Log));
            Assert.That(msg1.Counter, Is.LessThan(msg2.Counter));
        }
    }
}
