using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsInput;

namespace NaturalMotionPhoneMouseWindowsClient
{
    class InputSimulationTest
    {
        public static void SimulateMouseMotions()
        {
            var sim = new InputSimulator();
            sim.Mouse.MoveMouseBy(200, 200);
        }
    }
}
