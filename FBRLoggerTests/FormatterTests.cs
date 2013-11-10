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
    public class FormatterTests
    {
        [Test()]
        public void SerializeDeserializeStreamTest()
        {
            //Arrange
            var stream = new MemoryStream();
            var formatter = new LogMessage.Formatter(stream);
            var msg = new LogMessage("Test.Logger", "Test message", LogMessage.LogLevel.Fatal, true, new Exception());

            //Act
            formatter.Serialize(msg);
            stream.Position = 0;
            var msg2 = formatter.Deserialize();

            //Assert
            Assert.That(msg.Stack, Is.Not.Empty);
            Assert.That(msg.Exception, Is.Not.Empty);

            Assert.That(msg2.Counter, Is.EqualTo(msg.Counter));
            Assert.That(msg2.TimeStamp, Is.EqualTo(msg.TimeStamp));
            Assert.That(msg2.Logger, Is.EqualTo("Test.Logger"));
            Assert.That(msg2.Message, Is.EqualTo("Test message"));
            Assert.That(msg2.Level, Is.EqualTo(LogMessage.LogLevel.Fatal));
            Assert.That(msg2.Thread, Is.EqualTo(msg.Thread));
            Assert.That(msg2.Stack, Is.EqualTo(msg.Stack));
            Assert.That(msg2.Exception, Is.EqualTo(msg.Exception));

            formatter.Dispose();
        }

        [Test()]
        public void DisposeTest()
        {
            //Arrange
            var formatter = new LogMessage.Formatter(new MemoryStream());
            formatter.Dispose();

            //Act
            //Assert
            Assert.That(() => formatter.Serialize(new LogMessage("a", "b")), Throws.Exception);
        }
    }
}
