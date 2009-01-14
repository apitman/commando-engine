using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Commando.controls
{
    public class InputSet
    {
        protected int[] stickState_ = new int[(int)InputsEnum.DUMMY_LAST];
        protected bool[] toggleState_ = new bool[(int)InputsEnum.DUMMY_LAST];

        public float leftDirectionalX;
        public float leftDirectionalY;

        public float rightDirectionalX;
        public float rightDirectionalY;

        public bool confirmButton;
        public bool cancelButton;

        protected bool button1;
        protected bool button2;
        protected bool button3;
        protected bool button4;

        public bool leftTrigger;
        public bool rightTrigger;

        public bool leftBumper;
        public bool rightBumper;

        public InputSet()
        {
            stickState_ = new int[(int)InputsEnum.DUMMY_LAST];
            toggleState_ = new bool[(int)InputsEnum.DUMMY_LAST];
        }

        # region GETTERS AND SETTERS

        public bool getButton1()
        {
            return button1;
        }

        public bool getButton2()
        {
            return button2;
        }

        public bool getButton3()
        {
            return button3;
        }

        public bool getButton4()
        {
            return button4;
        }

        #endregion

        #region SETTERS

        public void setButton1(bool value)
        {
            if (stickState_[(int)InputsEnum.BUTTON_1] > 0 ||
                toggleState_[(int)InputsEnum.BUTTON_1])
            {
                if (stickState_[(int)InputsEnum.BUTTON_1] > 0)
                {
                    stickState_[(int)InputsEnum.BUTTON_1]--;
                }
                if (button1 == false)
                {
                    toggleState_[(int)InputsEnum.BUTTON_1] = false;
                }
                button1 = false;
                return;
            }
            button1 = value;
        }

        public void setButton2(bool value)
        {
            if (stickState_[(int)InputsEnum.BUTTON_2] > 0 ||
                    toggleState_[(int)InputsEnum.BUTTON_2])
            {
                if (stickState_[(int)InputsEnum.BUTTON_2] > 0)
                {
                    stickState_[(int)InputsEnum.BUTTON_2]--;
                }
                if (button2 == false)
                {
                    toggleState_[(int)InputsEnum.BUTTON_2] = false;
                }
                button2 = false;
                return;
            }
            button2 = value;
        }

        public void setButton3(bool value)
        {
            if (stickState_[(int)InputsEnum.BUTTON_3] > 0 ||
                    toggleState_[(int)InputsEnum.BUTTON_3])
            {
                if (stickState_[(int)InputsEnum.BUTTON_3] > 0)
                {
                    stickState_[(int)InputsEnum.BUTTON_3]--;
                }
                if (button3 == false)
                {
                    toggleState_[(int)InputsEnum.BUTTON_3] = false;
                }
                button3 = false;
                return;
            }
            button3 = value;
        }

        public void setButton4(bool value)
        {
            if (stickState_[(int)InputsEnum.BUTTON_4] > 0 ||
                    toggleState_[(int)InputsEnum.BUTTON_4])
            {
                if (stickState_[(int)InputsEnum.BUTTON_4] > 0)
                {
                    stickState_[(int)InputsEnum.BUTTON_4]--;
                }
                if (button4 == false)
                {
                    toggleState_[(int)InputsEnum.BUTTON_4] = false;
                }
                button4 = false;
                return;
            }
            button4 = value;
        }

        #endregion
    }

    public enum InputsEnum
    {
        LEFT_DIRECTIONAL,
        RIGHT_DIRECTIONAL,
        CONFIRM_BUTTON,
        CANCEL_BUTTON,
        BUTTON_1,
        BUTTON_2,
        BUTTON_3,
        BUTTON_4,
        LEFT_TRIGGER,
        RIGHT_TRIGGER,
        LEFT_BUMPER,
        RIGHT_BUMPER,
        
        DUMMY_LAST
    }
}
