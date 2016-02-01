using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;

namespace NaturalMotionPhoneMouseWindowsClient
{
    public partial class Form1 : Form, ConnectionObserver
    {
        private NaturalMotionMouseBluetoothClient BTClient;

        public Form1()
        {
            InitializeComponent();
            BTClient = new NaturalMotionMouseBluetoothClient();
            BTClient.RegisterConnectionObserver(this);
        }

        public void HandleConnectionChange(bool connectionChange)
        {
            if (ProgressBar.InvokeRequired)
            {
                this.Invoke(new MethodInvoker(() => {
                    ProgressBar.Style = ProgressBarStyle.Continuous;
                    if (connectionChange == true) { // YES Connection
                        ProgressBar.Value = 100;
                    }
                    if (connectionChange == false) { // NO Connection
                        ProgressBar.Value = 0;
                        KillButton.Enabled = false;
                        ConnectButton.Enabled = true;
                    }
                }));
            }
        }

        private void ConnectButton_Click(object sender, EventArgs e)
        {
            BTClient.StartBluetoothServer();

            ConnectButton.Enabled = false;
            KillButton.Enabled = true;
            ProgressBar.Style = ProgressBarStyle.Marquee;
         }

        private void KillButton_Click(object sender, EventArgs e)
        {
            BTClient.KillServer();
        }
    }
}
