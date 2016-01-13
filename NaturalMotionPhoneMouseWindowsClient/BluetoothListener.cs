using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InTheHand.Net.Bluetooth;
using InTheHand.Net.Sockets;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using Newtonsoft.Json.Linq;
using WindowsInput;

namespace NaturalMotionPhoneMouseWindowsClient
{
    class NaturalMotionMouseBluetoothClient
    {
        private BluetoothListener btListener;

        public NaturalMotionMouseBluetoothClient()
        {
        }

        public void StartBluetoothServer()
        {
            if (!BluetoothRadio.IsSupported)
            {
                Console.WriteLine("Bluetooth doesn't work!");
                return;
            }

            BluetoothRadio.PrimaryRadio.Mode = RadioMode.Discoverable;

            var guid = new Guid("{11f4ad42-73f7-4bb4-a5c9-998cbf22b6fb}");

            btListener = new BluetoothListener(guid);
            btListener.ServiceName = "NaturalMotionMouse";
            btListener.Authenticate = false;
            btListener.Start();
            Thread thHR = new Thread(new ThreadStart(receiving));
            thHR.Start();
        }

        private void receiving()
        {
            var displacementTranslator = new TestDataDisplacementTranslator();

            var sim = new InputSimulator();

            Console.WriteLine("Waiting...");

            var client = btListener.AcceptBluetoothClient();
            var peerStream = client.GetStream();
            var reader = new StreamReader(peerStream);

            while (!reader.EndOfStream)
            {
                var message = reader.ReadLine();
                Console.WriteLine("Data: " + message);

                var displacementJson = JObject.Parse(message);
                var mouseDelta = displacementTranslator.TranslateData(displacementJson);
                sim.Mouse.MoveMouseBy((int) mouseDelta.DisplacementX, (int) mouseDelta.DisplacementY);
            }

            reader.Close();
            btListener.Stop();
        }

    }
}
