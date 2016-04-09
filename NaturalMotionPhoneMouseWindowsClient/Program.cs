using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NaturalMotionPhoneMouseWindowsClient
{
    static class Program
    {
        private static Mutex m_Mutex;

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            bool createdNew;
            m_Mutex = new Mutex(true, "MyApplicationMutex", out createdNew);
            if (createdNew)
                Application.Run(new Form1());
            else
                MessageBox.Show("The application is already running.", Application.ProductName,
                  MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
    }
}
