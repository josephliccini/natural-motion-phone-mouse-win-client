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
                case "Middle_Press":
                    Sim.Mouse.MiddleButtonClick();
                    break;
                case "XButton1_Press":
                case "XButton2_Press":
                    break;
                case "XButton1_Release":
                    Sim.Mouse.XButtonClick(1);
                    break;
                case "XButton2_Release":
                    Sim.Mouse.XButtonClick(2);
                    break;
            }

        }

        public void MoveMouseWheel(MouseWheelDelta mouseWheelDelta)
        {
            switch (mouseWheelDelta.MouseWheelActionType)
            {
                case "MouseWheelUp":
                    Sim.Mouse.VerticalScroll((int) mouseWheelDelta.Amount);
                    break;
                case "MouseWheelDown":
                    Sim.Mouse.VerticalScroll(-1 * ((int) mouseWheelDelta.Amount));
                    break;
            }
        }
    }
}
