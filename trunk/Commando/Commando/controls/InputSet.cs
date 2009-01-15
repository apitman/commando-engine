/*
 ***************************************************************************
 * Copyright 2009 Eric Barnes, Ken Hartsook, Andrew Pitman, & Jared Segal  *
 *                                                                         *
 * Licensed under the Apache License, Version 2.0 (the "License");         *
 * you may not use this file except in compliance with the License.        *
 * You may obtain a copy of the License at                                 *
 *                                                                         *
 * http://www.apache.org/licenses/LICENSE-2.0                              *
 *                                                                         *
 * Unless required by applicable law or agreed to in writing, software     *
 * distributed under the License is distributed on an "AS IS" BASIS,       *
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.*
 * See the License for the specific language governing permissions and     *
 * limitations under the License.                                          *
 ***************************************************************************
*/

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

        protected bool confirmButton;
        protected bool cancelButton;

        protected bool button1;
        protected bool button2;
        protected bool button3;
        protected bool button4;

        protected bool leftTrigger;
        protected bool rightTrigger;

        protected bool leftBumper;
        protected bool rightBumper;

        public InputSet()
        {
            stickState_ = new int[(int)InputsEnum.DUMMY_LAST];
            toggleState_ = new bool[(int)InputsEnum.DUMMY_LAST];
        }

        # region GETTERS

        public bool getConfirmButton()
        {
            return confirmButton;
        }

        public bool getCancelButton()
        {
            return cancelButton;
        }

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

        public bool getLeftTrigger()
        {
            return leftTrigger;
        }

        public bool getRightTrigger()
        {
            return rightTrigger;
        }

        public bool getLeftBumper()
        {
            return leftBumper;
        }

        public bool getRightBumper()
        {
            return rightBumper;
        }

        #endregion

        #region SETTERS

        public void setConfirmButton(bool value)
        {
            if (stickState_[(int)InputsEnum.CONFIRM_BUTTON] > 0 ||
                toggleState_[(int)InputsEnum.CONFIRM_BUTTON])
            {
                if (stickState_[(int)InputsEnum.CONFIRM_BUTTON] > 0)
                {
                    stickState_[(int)InputsEnum.CONFIRM_BUTTON]--;
                }
                if (value == false)
                {
                    toggleState_[(int)InputsEnum.CONFIRM_BUTTON] = false;
                }
                confirmButton = false;
                return;
            }
            confirmButton = value;
        }

        public void setCancelButton(bool value)
        {
            if (stickState_[(int)InputsEnum.CANCEL_BUTTON] > 0 ||
                toggleState_[(int)InputsEnum.CANCEL_BUTTON])
            {
                if (stickState_[(int)InputsEnum.CANCEL_BUTTON] > 0)
                {
                    stickState_[(int)InputsEnum.CANCEL_BUTTON]--;
                }
                if (value == false)
                {
                    toggleState_[(int)InputsEnum.CANCEL_BUTTON] = false;
                }
                cancelButton = false;
                return;
            }
            cancelButton = value;
        }

        public void setButton1(bool value)
        {
            if (stickState_[(int)InputsEnum.BUTTON_1] > 0 ||
                toggleState_[(int)InputsEnum.BUTTON_1])
            {
                if (stickState_[(int)InputsEnum.BUTTON_1] > 0)
                {
                    stickState_[(int)InputsEnum.BUTTON_1]--;
                }
                if (value == false)
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
                if (value == false)
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
                if (value == false)
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
                if (value == false)
                {
                    toggleState_[(int)InputsEnum.BUTTON_4] = false;
                }
                button4 = false;
                return;
            }
            button4 = value;
        }

        public void setLeftTrigger(bool value)
        {
            if (stickState_[(int)InputsEnum.LEFT_TRIGGER] > 0 ||
                toggleState_[(int)InputsEnum.LEFT_TRIGGER])
            {
                if (stickState_[(int)InputsEnum.LEFT_TRIGGER] > 0)
                {
                    stickState_[(int)InputsEnum.LEFT_TRIGGER]--;
                }
                if (value == false)
                {
                    toggleState_[(int)InputsEnum.LEFT_TRIGGER] = false;
                }
                leftTrigger = false;
                return;
            }
            leftTrigger = value;
        }

        public void setRightTrigger(bool value)
        {
            if (stickState_[(int)InputsEnum.RIGHT_TRIGGER] > 0 ||
                toggleState_[(int)InputsEnum.RIGHT_TRIGGER])
            {
                if (stickState_[(int)InputsEnum.RIGHT_TRIGGER] > 0)
                {
                    stickState_[(int)InputsEnum.RIGHT_TRIGGER]--;
                }
                if (value == false)
                {
                    toggleState_[(int)InputsEnum.RIGHT_TRIGGER] = false;
                }
                rightTrigger = false;
                return;
            }
            rightTrigger = value;
        }

        public void setLeftBumper(bool value)
        {
            if (stickState_[(int)InputsEnum.LEFT_BUMPER] > 0 ||
                toggleState_[(int)InputsEnum.LEFT_BUMPER])
            {
                if (stickState_[(int)InputsEnum.LEFT_BUMPER] > 0)
                {
                    stickState_[(int)InputsEnum.LEFT_BUMPER]--;
                }
                if (value == false)
                {
                    toggleState_[(int)InputsEnum.LEFT_BUMPER] = false;
                }
                leftBumper = false;
                return;
            }
            leftBumper = value;
        }

        public void setRightBumper(bool value)
        {
            if (stickState_[(int)InputsEnum.RIGHT_BUMPER] > 0 ||
                toggleState_[(int)InputsEnum.RIGHT_BUMPER])
            {
                if (stickState_[(int)InputsEnum.RIGHT_BUMPER] > 0)
                {
                    stickState_[(int)InputsEnum.RIGHT_BUMPER]--;
                }
                if (value == false)
                {
                    toggleState_[(int)InputsEnum.RIGHT_BUMPER] = false;
                }
                rightBumper = false;
                return;
            }
            rightBumper = value;
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
