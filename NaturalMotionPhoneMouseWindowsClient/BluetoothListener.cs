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
        private readonly Guid guid = new Guid("{11f4ad42-73f7-4bb4-a5c9-998cbf22b6fb}");
        private Thread backgroundThread;
        public bool Connected = false;

        public NaturalMotionMouseBluetoothClient()
        {
            btListener = new BluetoothListener(guid);
            btListener.ServiceName = "NaturalMotionMouse";
            btListener.Authenticate = false;
        }

        public void StartBluetoothServer()
        {
            if (!BluetoothRadio.IsSupported)
            {
                Console.WriteLine("Bluetooth doesn't work!");
                return;
            }

            BluetoothRadio.PrimaryRadio.Mode = RadioMode.Discoverable;

            btListener.Start();

            backgroundThread = new Thread(new ThreadStart(receiving));
            backgroundThread.Start();
        }

        private void receiving()
        {
            StreamReader reader = null;
            try
            {
                var displacementTranslator = new TestDataDisplacementTranslator();
                var mouseDelegate = new MouseDelegate();

                var client = btListener.AcceptBluetoothClient();

                Connected = true;

                var peerStream = client.GetStream();
                reader = new StreamReader(peerStream);

                while (!reader.EndOfStream)
                {
                    var message = reader.ReadLine();

                    Console.WriteLine(message);

                    var jsonObject = JObject.Parse(message);

                    var messageType = jsonObject.Value<string>("messageType");

                    switch(messageType)
                    {
                        case "MouseSensitivity":
                            var mouseSensitivity = new MouseSensitivityMessage
                            {
                                Amount = jsonObject.Value<double>("amount")  
                            };
                            displacementTranslator.MultiplicativeConstant = mouseSensitivity.Amount;
                            break;
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
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                if (reader != null) reader.Close();
                btListener.Stop();
                Connected = false;
            }

        }

        public void KillServer()
        {
            this.btListener.Stop();
            this.backgroundThread.Abort();
        }

    }
}
