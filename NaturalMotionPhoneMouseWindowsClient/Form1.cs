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
    public partial class Form1 : Form
    {
        private NaturalMotionMouseBluetoothClient BTClient;
        private Thread connectionNotificationThread;

        public Form1()
        {
            InitializeComponent();
        }

        private void ConnectButton_Click(object sender, EventArgs e)
        {
            BTClient = new NaturalMotionMouseBluetoothClient();

            BTClient.StartBluetoothServer();

            ConnectButton.Enabled = false;
            KillButton.Enabled = true;
            ProgressBar.Style = ProgressBarStyle.Marquee;

            connectionNotificationThread = new Thread(() => {
                while (true)
                {
                    if (BTClient.Connected)
                    {
                        if (ProgressBar.InvokeRequired)
                        {
                            this.Invoke(new MethodInvoker(() => {
                                ProgressBar.Style = ProgressBarStyle.Continuous;
                                ProgressBar.Value = 100;
                            }));
                            return;
                        }
                    }
                    Thread.Sleep(1000);
                }
            });
            connectionNotificationThread.Start();

        }

        private void KillButton_Click(object sender, EventArgs e)
        {
            KillButton.Enabled = false;
            ConnectButton.Enabled = true;

            ProgressBar.Value = 0;
            ProgressBar.Style = ProgressBarStyle.Continuous;

            BTClient.KillServer();
            if (connectionNotificationThread.IsAlive)
            {
                connectionNotificationThread.Abort();
            }
        }
    }
}
