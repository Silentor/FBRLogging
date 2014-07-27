using System;
using System.IO;

namespace Silentor.FBRLogger
{
    public partial struct LogMessage
    {
        /// <summary>
        /// Read/write log message to stream
        /// </summary>
        public class Formatter : IDisposable
        {
            /// <summary>
            /// Underlying stream for reader or/and writer
            /// </summary>
            public Stream Stream
            {
                get { return _writer.BaseStream; }
            }

            /// <summary>
            /// Stream oriented
            /// </summary>
            /// <param name="stream"></param>
            public Formatter(Stream stream)
            {
                _writer = new BinaryWriter(stream);
                _reader = new BinaryReader(stream);
            }

            /// <summary>
            /// Serialize message to this <see cref="Stream"/>
            /// </summary>
            /// <param name="message"></param>
            /// <returns>Lenght of serialized message</returns>
            public int Serialize(LogMessage message)
            {
                if(_writer == null) throw new InvalidOperationException("Serialization stream is not provided!");

                var startPos = _writer.BaseStream.Position;

                //Required felds
                _writer.Write(message.Counter);
                _writer.Write(message.TimeStamp.ToBinary());
                _writer.Write(message.Logger ?? String.Empty);
                _writer.Write(message.Message ?? String.Empty);
                _writer.Write((byte) message.Level);
                _writer.Write(message.Thread);

                //Optional fields
                _writer.Write(message.Stack ?? String.Empty);
                _writer.Write(message.Exception ?? String.Empty);

                return (int) (_writer.BaseStream.Position - startPos);
            }

            /// <summary>
            /// Deserialize from this <see cref="Stream"/>
            /// </summary>
            /// <returns></returns>
            public LogMessage Deserialize()
            {
                if (_reader == null) throw new InvalidOperationException("Serialization stream is not provided!");

                var newMsg = new LogMessage(
                    _reader.ReadInt32(),               //Counter
                    _reader.ReadInt64(),                //DateTime
                    _reader.ReadString(),               //Logger name
                    _reader.ReadString(),               //Message
                    (LogLevel)_reader.ReadByte(),       //Log level
                    _reader.ReadInt32(),                //Thread id
                    _reader.ReadString(),               //Stacktrace
                    _reader.ReadString());              //Exception message
                return newMsg;
            }

            public void Dispose()
            {
                _writer.Close();
                _reader.Close();
            }

            private readonly BinaryWriter _writer;
            private readonly BinaryReader _reader;
        }
    }
}