using System;
using System.Windows.Forms;
using Prosa.Log4View.Configuration;
using Prosa.Log4View.Properties;
using Prosa.Log4View.Receiver;

namespace Silentor.FBRLogger.Log4ViewReceiver 
{
    internal partial class FBRConfigControl : UserControl, IReceiverControl
    {
        private readonly FBRReceiverConfig _receiverConfig;
        private readonly IReceiverForm _receiverForm;

        public FBRConfigControl(FBRReceiverConfig config, IReceiverForm receiverForm)
        {
            InitializeComponent();

            _receiverConfig = config;
            _receiverForm = receiverForm;
        }

        public ReceiverConfig ReceiverConfig {
            get { return _receiverConfig; }
        }

        private void SampleConfigControlLoad(object sender, EventArgs e) {
            tbPort.Text = _receiverConfig.ListenPort.ToString();
            helpProvider.HelpNamespace = Resources.HelpPath;

            IsModified = false;
        }

        public void WriteConfiguration() {
            _receiverConfig.ListenPort = int.Parse(tbPort.Text);
        }

        public bool ReadFilterEnabled {
            get { return false; }
        }

        public bool IsModified { get; private set; }

        public bool IsValid 
        {
            get 
            {
                int stub;
                return tbPort.Text.Trim().Length > 0 && int.TryParse(tbPort.Text, out stub); 
            }
        }

        public void UpdateControls(bool calledFromReceiverForm = false)
        {
           if (!calledFromReceiverForm)
              _receiverForm.UpdateControls();
        }

        private void TbMessageTextValueChanged(object sender, EventArgs e) {
            IsModified = true;
            UpdateControls();
        }
    }
}
