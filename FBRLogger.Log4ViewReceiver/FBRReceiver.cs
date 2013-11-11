#region Copyright PROSA GmbH

// <copyright file="SampleReceiver.cs" company="PROSA GmbH">
//   Copyright © 2013 by PROSA GmbH, All rights reserved.
//   The information contained herein is confidential, proprietary to PROSA GmbH, 
//   and considered a trade secret. Use of this information by anyone other than 
//   authorized employees of PROSA GmbH is granted only under a written nondisclosure
//   agreement, expressly prescribing the the scope and manner of such use.
// </copyright>

#endregion

using System;
using System.Threading;
using Prosa.Log4View.Level;
using Prosa.Log4View.Message;
using Prosa.Log4View.Receiver;

namespace Silentor.FBRLogger.Log4ViewReceiver
{
    internal class FBRReceiver : LogReceiver
    {
        private readonly int _listenPort;
        private readonly LogMessageReceiver _fbrReceiver;

        public FBRReceiver(IReceiverFactory factory, FBRReceiverConfig config) : base(factory, config) 
        {
            _listenPort = config.ListenPort;
            _fbrReceiver = new LogMessageReceiver(_listenPort);

            // Important: This tells Log4View, that this receiver is ready to run.
            IsInitiated = true;
        }

        public override void Start() 
        {
            ReceiveMessages = true;
            _fbrReceiver.MessageReceived += FbrReceiverOnMessageReceived;
            _fbrReceiver.Start();
        }

        private static readonly LogLevel[] FBRLevel2Log4ViewLevel =
        {
            Levels.Trace, 
            Levels.Debug, 
            Levels.Info, 
            Levels.Warn, 
            Levels.Error, 
            Levels.Fatal, 
        };

        private void FbrReceiverOnMessageReceived(LogMessageReceiver logMessageReceiver, LogMessage logMessage, string senderHost)
        {
            if (ReceiveMessages)
            {
                var msg = new Prosa.Log4View.Message.LogMessage(this)
                {
                    Message = logMessage.Message, 
                    Level = FBRLevel2Log4ViewLevel[(int)logMessage.Level], 
                    Logger = logMessage.Logger,
                    Exception = logMessage.Exception,
                    Thread = logMessage.Thread.ToString(),
                    StackTrace = logMessage.Stack,
                    LogSource = logMessage.Stack,
                    OriginalTime = logMessage.TimeStamp,
                    Key = logMessage.Counter,
                    Host = senderHost

                };
                var messages = new MessageBlock { msg };

                AddNewMessages(messages);
            }
        }

        public override void Dispose() 
        {
            base.Dispose();
            _fbrReceiver.Dispose();
        }
    }
}