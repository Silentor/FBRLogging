 #region Copyright PROSA GmbH
// /////////////////////////////////////////////////////////////////////////////// 
// // Copyright © 2008 by PROSA GmbH, All rights reserved. 
// // 
// // The information contained herein is confidential, proprietary to PROSA GmbH, 
// // and considered a trade secret. Use of this information by anyone other than 
// // authorized employees of PROSA GmbH is granted only under a written nondisclosure
// // agreement, expressly prescribing the the scope and manner of such use.
// //
// /////////////////////////////////////////////////////////////////////////////// 
#endregion

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Drawing;
using Prosa.Log4View.Configuration;
using Prosa.Log4View.Frameworks;
using Prosa.Log4View.Receiver;
using Silentor.FBRLogger.Log4ViewReceiver.Properties;

namespace Silentor.FBRLogger.Log4ViewReceiver
{
    [Export(typeof (IReceiverFactory))]
    public class FBRReceiverFactory : IReceiverFactory
    {
        internal const string TypeId = "FBRLogger.Receiver";

        #region IReceiverFactory Members
        public string ReceiverTypeId {
            get { return TypeId; }
        }

        public string Name {
            get { return "FBR Receiver"; }
        }

        public Bitmap SmallBitmap {
            get { return Resources.SampleReceiver16; }
        }

        public Bitmap LargeBitmap {
            get { return Resources.SampleReceiver128; }
        }

        public IEnumerable<Type> ConfigType {
            get { return new[] { typeof(FBRReceiverConfig) }; }
        }

        public string HelpKeyword {
            get { return ""; }
        }

        public ILogReceiver CreateReceiver(ReceiverConfig config) {
            return new FBRReceiver(this, (FBRReceiverConfig)config);
        }

        public ReceiverConfig CreateReceiverDefinition() {
            return new FBRReceiverConfig();
        }

        public IReceiverControl CreateReceiverControl(ReceiverConfig receiver, IReceiverForm receiverForm) {
            return new FBRConfigControl((FBRReceiverConfig)receiver, receiverForm);
        }

        public ReceiverConfig CreateReceiverConfig(SourceConfig source, Log4ViewAppenderNode appender) {
            return null;
        }
        #endregion
    }
}