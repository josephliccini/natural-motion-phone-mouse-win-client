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
        private bool Connected = false;

        private List<ConnectionObserver> observers = new List<ConnectionObserver>();

        public NaturalMotionMouseBluetoothClient()
        {
            btListener = new BluetoothListener(guid);
            btListener.ServiceName = "NaturalMotionMouse";
            btListener.Authenticate = false;
        }

        public void RegisterConnectionObserver(ConnectionObserver o)
        {
            observers.Add(o);
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
                NotifyObservers();

                var peerStream = client.GetStream();

                SendAckMessage(peerStream);

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
                Connected = false;
                reader.Close();
                btListener.Stop();
                NotifyObservers();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                if (reader != null) reader.Close();
                btListener.Stop();
                Connected = false;
                NotifyObservers();
            }

        }

        public void KillServer()
        {
            btListener.Stop();
            backgroundThread.Abort();
        }

        private void NotifyObservers()
        {
            observers.ForEach(o => o.HandleConnectionChange(Connected));
        }

        private void SendAckMessage(Stream stream)
        {
            var writer = new StreamWriter(stream);

            JObject ackObj = JObject.FromObject(new
            {
                message = "ConnectionAck"
            });

            writer.Write(ackObj.ToString().Replace("\r\n", "") + "\n");
            writer.Flush();
        }
    }
}
