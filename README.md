FBRLogging
==========

Fast Binary Remote logging infrastructure. Intended for Unity3d mobile development, where XML log message serialization may be GC heavy.
Log message serialized into custom binary format and sended via UDP to some remote log viewer. NLog target and Log4View receiver supported for now.

Solution structure:

FBRLogger - core project, consist of message serializator/deserializator, message class itself and UDP transmitter and receiver.
FBRLogger.Log4ViewReceiver - plugin for excellent Log4View application. Plugin receives and display FBR messages. Receiver plugins are supported in beta version of Log4View for now.
FBRLogger.NLogTarget - target for sending FBR log messages using NLog logging framework.
FBRLogger.Runner - test application, simply send some FBR messages to given host address and port.
FBRLoggerTests - NUnit tests.
Libs - folder for Log4View executable file, needs for building FBRLogger.Log4ViewReceiver from sources.

Some perfomance testing:



