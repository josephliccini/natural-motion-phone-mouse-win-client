using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsInput;

namespace NaturalMotionPhoneMouseWindowsClient
{
    class MouseDelegate
    {
        private InputSimulator Sim;

        public MouseDelegate()
        {
            this.Sim = new InputSimulator();
        }

        public void TranslateMouseCursorBy(MouseMotionDelta mouseDelta)
        {
            Sim.Mouse.MoveMouseBy((int) mouseDelta.DisplacementX, (int) mouseDelta.DisplacementY);
        }

        public void DoMouseButtonAction(MouseButtonAction action)
        {
            switch(action.MouseActionType)
            {
                case "Left_Press":
                    Sim.Mouse.LeftButtonDown();
                    break;
                case "Right_Press":
                    Sim.Mouse.RightButtonDown();
                    break;
                case "Left_Release":
                    Sim.Mouse.LeftButtonUp();
                    break;
                case "Right_Release":
                    Sim.Mouse.RightButtonUp();
                    break;
            }

        }
    }
}
