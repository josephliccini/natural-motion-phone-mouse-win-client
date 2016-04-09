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
        private MenuItem menuItem1;
        private ContextMenu contextMenu1;

        public Form1()
        {
            InitializeComponent();

            this.menuItem1 = new MenuItem();
            this.menuItem1.Text = "Exit";
            this.menuItem1.Click += new EventHandler(this.menuItem1_Click);

            this.contextMenu1 = new ContextMenu(new MenuItem[] { menuItem1 });
            this.notifyIcon1.ContextMenu = this.contextMenu1;

            BTClient = new NaturalMotionMouseBluetoothClient();
            BTClient.RegisterConnectionObserver(this);
            ConnectButton_Click(null, null);
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

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (FormWindowState.Minimized == this.WindowState)
            {
                notifyIcon1.Visible = true;
                this.Hide();
            }
            else if (FormWindowState.Normal == this.WindowState)
            {
                notifyIcon1.Visible = false;
            }
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Show();
            WindowState = FormWindowState.Normal;
        }

        private void menuItem1_Click(object Sender, EventArgs e)
        {
            this.Close();
        }
    }
}
