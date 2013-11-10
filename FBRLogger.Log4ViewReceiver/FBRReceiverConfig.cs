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
using System.Runtime.Serialization;
using Prosa.Log4View.Configuration;

namespace Silentor.FBRLogger.Log4ViewReceiver
{
    [DataContract, Serializable]
    internal class FBRReceiverConfig : ReceiverConfig
    {
        public FBRReceiverConfig() : base("FBRLogger Receiver") { }

        [DataMember]
        public int ListenPort { get; set; }

        public override string ReceiverTypeId {
            get { return FBRReceiverFactory.TypeId; }
        }

        public override string Description {
            get { return "Fast Binary Remote receiver listen for UDP messages at port " + ListenPort; }
        }
    }
}