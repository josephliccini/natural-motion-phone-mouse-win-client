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
            var mouseDelegate = new MouseDelegate();

            Console.WriteLine("Waiting...");

            var client = btListener.AcceptBluetoothClient();
            var peerStream = client.GetStream();
            var reader = new StreamReader(peerStream);

            while (!reader.EndOfStream)
            {
                var message = reader.ReadLine();
                Console.WriteLine("Data: " + message);

                var jsonObject = JObject.Parse(message);

                var messageType = jsonObject.Value<string>("messageType");

                switch(messageType)
                {
                    case "MouseTranslation":
                        var mouseDelta = displacementTranslator.TranslateData(jsonObject);
                        mouseDelegate.TranslateMouseCursorBy(mouseDelta);
                        break;
                    case "MouseButtonAction":
                        var mouseAction = new MouseButtonAction
                        {
                            MouseActionType = jsonObject.Value<string>("mouseActionType")
                        };
                        mouseDelegate.DoMouseButtonAction(mouseAction);
                        break;
                    case "MouseWheelDelta":
                        var mouseWheelDelta = new MouseWheelDelta
                        {
                            MouseWheelActionType = jsonObject.Value<string>("mouseWheelActionType"),
                            Amount = jsonObject.Value<double>("amount")
                        };
                        mouseDelegate.MoveMouseWheel(mouseWheelDelta);
                        break;
                }

            }

            reader.Close();
            btListener.Stop();
        }

    }
}
