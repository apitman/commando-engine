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

        // stickState_ is an array of ints for each input device in InputsEnum
        //  where each int represents the number of frames left for which that
        //  particular device will pretend to be released regardless of its
        //  actual state
        protected int[] stickState_ = new int[(int)InputsEnum.LENGTH];

        // toggleState_ is an array of bools for each input device in
        //  InputsEnum where each bool represents whether that device must be
        //  released before it can register normally again
        protected bool[] toggleState_ = new bool[(int)InputsEnum.LENGTH];

        protected float leftDirectionalX;
        protected float leftDirectionalY;

        protected float rightDirectionalX;
        protected float rightDirectionalY;

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
            stickState_ = new int[(int)InputsEnum.LENGTH];
            toggleState_ = new bool[(int)InputsEnum.LENGTH];
        }

        # region GETTERS

        public float getLeftDirectionalX()
        {
            return leftDirectionalX;
        }

        public float getLeftDirectionalY()
        {
            return leftDirectionalY;
        }

        public float getRightDirectionalX()
        {
            return rightDirectionalX;
        }

        public float getRightDirectionalY()
        {
            return rightDirectionalY;
        }

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

        public void setLeftDirectional(float x, float y)
        {
            if (stickState_[(int)InputsEnum.LEFT_DIRECTIONAL] > 0 ||
                toggleState_[(int)InputsEnum.LEFT_DIRECTIONAL])
            {
                if (stickState_[(int)InputsEnum.LEFT_DIRECTIONAL] > 0)
                {
                    stickState_[(int)InputsEnum.LEFT_DIRECTIONAL]--;
                }
                if (x == 0 && y == 0)
                {
                    toggleState_[(int)InputsEnum.LEFT_DIRECTIONAL] = false;
                }
                leftDirectionalX = 0;
                leftDirectionalY = 0;
                return;
            }
            leftDirectionalX = x;
            leftDirectionalY = y;
        }

        public void setRightDirectional(float x, float y)
        {
            if (stickState_[(int)InputsEnum.RIGHT_DIRECTIONAL] > 0 ||
                toggleState_[(int)InputsEnum.RIGHT_DIRECTIONAL])
            {
                if (stickState_[(int)InputsEnum.RIGHT_DIRECTIONAL] > 0)
                {
                    stickState_[(int)InputsEnum.RIGHT_DIRECTIONAL]--;
                }
                if (x == 0 && y == 0)
                {
                    toggleState_[(int)InputsEnum.RIGHT_DIRECTIONAL] = false;
                }
                rightDirectionalX = 0;
                rightDirectionalY = 0;
                return;
            }
            rightDirectionalX = x;
            rightDirectionalY = y;
        }

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


        // This function will cause the button 'toStick' to
        // act released for 'numFrames' frames.
        public void setStick(InputsEnum toStick, int numFrames)
        {
            if (numFrames >= 0)
            {
                stickState_[(int)toStick] = numFrames;
            }
            else
            {
                stickState_[(int)toStick] = 0;
            }
        }

        // This function will cause the button 'toToggle' to
        // act released until physically released and then used.
        public void setToggle(InputsEnum toToggle, bool value)
        {
            toggleState_[(int)toToggle] = value;
        }

        // Causes all buttons to act as if they were in the
        // released state for 'numFrames' frames
        public void setAllSticks(int numFrames)
        {
            for (int i = 0; i < (int)InputsEnum.LENGTH; i++)
            {
                stickState_[i] = numFrames;
            }
        }

        // Causes all buttons to act as if they were released
        // until they are actually released.  This could be
        // useful for changing EngineStates and not having
        // buttons being held down trigger in the next state.
        public void setAllToggles()
        {
            for (int i = 0; i < (int)InputsEnum.LENGTH; i++)
            {
                toggleState_[i] = true;
            }
        }

        // Clears all buttons of their stick values so that
        // there is no delay for their usage.
        public void clearSticks()
        {
            for (int i = 0; i < (int)InputsEnum.LENGTH; i++)
            {
                stickState_[i] = 0;
            }
        }

        // Clears all buttons of their toggle values so that
        // they do not have to be released before use.
        public void clearToggles()
        {
            for (int i = 0; i < (int)InputsEnum.LENGTH; i++)
            {
                toggleState_[i] = false;
            }
        }

        // Sets all buttons to the released state
        public void clearInputs()
        {
            leftDirectionalX = 0;
            leftDirectionalY = 0;
            rightDirectionalX = 0;
            rightDirectionalY = 0;
            confirmButton = false;
            cancelButton = false;
            button1 = false;
            button2 = false;
            button3 = false;
            button4 = false;
            leftTrigger = false;
            rightTrigger = false;
            leftBumper = false;
            rightBumper = false;
        }

    }

    // This enumeration is used when setting and clearing toggles and sticks
    // The last item, LENGTH, should never be used in these conditions, but
    //  is a replacement since C# enumerations do not have a .length attrib,
    //  and is useful for iterating over arrays which contain one element
    //  for each item in the enumeration.  This item MUST remain last.
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
        
        LENGTH
    }
}
