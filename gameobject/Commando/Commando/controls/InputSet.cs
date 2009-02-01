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

using Microsoft.Xna.Framework;

namespace Commando.controls
{
    /// <summary>
    /// Pseudo-singleton which contains a data structure of values of input
    /// from a user device during a single frame.  Also contains some
    /// functionality for controlling how often this input can change.
    /// </summary>
    public class InputSet
    {
        /// <summary>
        /// An array of instances of InputSets, one for each possible player.
        /// </summary>
        static protected InputSet[] instances_ = null;

        /// <summary>
        /// An array of ints for each input device in InputsEnum;
        ///  each int represents the number of frames left for which that
        ///  particular device will pretend to be released regardless of its
        ///  actual state.
        /// </summary>
        protected int[] stickState_ = new int[(int)InputsEnum.LENGTH];

        /// <summary>
        /// An array of booleans for each input device in InputsEnum; each
        /// bool represents whether that device must be released before it can
        /// register normally again.
        /// </summary>
        protected bool[] toggleState_ = new bool[(int)InputsEnum.LENGTH];

        protected float leftDirectionalX_;
        protected float leftDirectionalY_;

        protected float rightDirectionalX_;
        protected float rightDirectionalY_;

        protected bool confirmButton_;
        protected bool cancelButton_;

        protected bool button1_;
        protected bool button2_;
        protected bool button3_;
        protected bool button4_;

        protected bool leftTrigger_;
        protected bool rightTrigger_;

        protected bool leftBumper_;
        protected bool rightBumper_;

        /// <summary>
        /// Statically creates an InputSet for four different players.
        /// </summary>
        static InputSet()
        {
            instances_ = new InputSet[4];

            // Not expensive to create 4 InputSets, so we aren't
            // worried about lazy instantiation.  If we instantiate
            // them now, and later we use multithreading, we won't
            // have to use sync locks - just pull the InputSet and
            // read from it.
            for (int i = 0; i < 4; i++)
            {
                instances_[i] = new InputSet();
            }
        }

        /// <summary>
        /// Private constructor as per Singleton pattern.
        /// Initializes empty arrays for Sticks and Toggles.
        /// </summary>
        private InputSet()
        {
            stickState_ = new int[(int)InputsEnum.LENGTH];
            toggleState_ = new bool[(int)InputsEnum.LENGTH];
        }

        /// <summary>
        /// Returns Player One's InputSet as per the Singleton pattern.
        /// </summary>
        /// <returns>The InputSet for Player One.</returns>
        static public InputSet getInstance()
        {
            return instances_[0];
        }

        /// <summary>
        /// Returns the InputSet of specific player.
        /// </summary>
        /// <param name="index">The player whose inputs are desired.</param>
        /// <returns>The InputSet for the specified player.</returns>
        static public InputSet getInstance(PlayerIndex index)
        {
            int i = (int)index;
            return instances_[i];
        }

        # region GETTERS

        public float getLeftDirectionalX()
        {
            return leftDirectionalX_;
        }

        public float getLeftDirectionalY()
        {
            return leftDirectionalY_;
        }

        public float getRightDirectionalX()
        {
            return rightDirectionalX_;
        }

        public float getRightDirectionalY()
        {
            return rightDirectionalY_;
        }

        public bool getConfirmButton()
        {
            return confirmButton_;
        }

        public bool getCancelButton()
        {
            return cancelButton_;
        }

        public bool getButton1()
        {
            return button1_;
        }

        public bool getButton2()
        {
            return button2_;
        }

        public bool getButton3()
        {
            return button3_;
        }

        public bool getButton4()
        {
            return button4_;
        }

        public bool getLeftTrigger()
        {
            return leftTrigger_;
        }

        public bool getRightTrigger()
        {
            return rightTrigger_;
        }

        public bool getLeftBumper()
        {
            return leftBumper_;
        }

        public bool getRightBumper()
        {
            return rightBumper_;
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
                leftDirectionalX_ = 0;
                leftDirectionalY_ = 0;
                return;
            }
            leftDirectionalX_ = x;
            leftDirectionalY_ = y;
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
                rightDirectionalX_ = 0;
                rightDirectionalY_ = 0;
                return;
            }
            rightDirectionalX_ = x;
            rightDirectionalY_ = y;
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
                confirmButton_ = false;
                return;
            }
            confirmButton_ = value;
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
                cancelButton_ = false;
                return;
            }
            cancelButton_ = value;
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
                button1_ = false;
                return;
            }
            button1_ = value;
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
                button2_ = false;
                return;
            }
            button2_ = value;
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
                button3_ = false;
                return;
            }
            button3_ = value;
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
                button4_ = false;
                return;
            }
            button4_ = value;
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
                leftTrigger_ = false;
                return;
            }
            leftTrigger_ = value;
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
                rightTrigger_ = false;
                return;
            }
            rightTrigger_ = value;
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
                leftBumper_ = false;
                return;
            }
            leftBumper_ = value;
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
                rightBumper_ = false;
                return;
            }
            rightBumper_ = value;
        }

        #endregion


        /// <summary>
        /// Causes an input unit to act released for a set number of frames.
        /// </summary>
        /// <param name="toStick">The input unit which will act released.</param>
        /// <param name="numFrames">Duration (in frames) before acting normal.</param>
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

        /// <summary>
        /// Causes an input unit to act released until being physically
        /// released and then pressed/moved/clicked/etc again.
        /// </summary>
        /// <param name="toToggle">The input unit which will act released.</param>
        public void setToggle(InputsEnum toToggle)
        {
            toggleState_[(int)toToggle] = true;
        }

        /// <summary>
        /// Clears the toggle requirement for an input unit.  See setToggle().
        /// </summary>
        /// <param name="toToggle">The input unit to no longer wait for a toggle.</param>
        public void clearToggle(InputsEnum toToggle)
        {
            toggleState_[(int)toToggle] = false;
        }

        /// <summary>
        /// Performs a setStick() on all input units.
        /// </summary>
        /// <param name="numFrames">Duration (in frames) before acting normal.</param>
        public void setAllSticks(int numFrames)
        {
            for (int i = 0; i < (int)InputsEnum.LENGTH; i++)
            {
                stickState_[i] = numFrames;
            }
        }

        /// <summary>
        /// Performs a setToggle() on all input units.
        /// </summary>
        public void setAllToggles()
        {
            for (int i = 0; i < (int)InputsEnum.LENGTH; i++)
            {
                toggleState_[i] = true;
            }
        }

        /// <summary>
        /// Returns normal functionality to input units which were put into
        /// a Stick state by setStick() or setAllSticks().  Does not affect
        /// Toggle states.
        /// </summary>
        public void clearSticks()
        {
            for (int i = 0; i < (int)InputsEnum.LENGTH; i++)
            {
                stickState_[i] = 0;
            }
        }

        /// <summary>
        /// Returns normal functionality to input units which were put into
        /// a Toggle state by setToggle() or setAllToggles().  Does not affect
        /// Stick states.
        /// </summary>
        public void clearToggles()
        {
            for (int i = 0; i < (int)InputsEnum.LENGTH; i++)
            {
                toggleState_[i] = false;
            }
        }

        /// <summary>
        /// Sets all buttons to the released state.
        /// </summary>
        public void clearInputs()
        {
            leftDirectionalX_ = 0;
            leftDirectionalY_ = 0;
            rightDirectionalX_ = 0;
            rightDirectionalY_ = 0;
            confirmButton_ = false;
            cancelButton_ = false;
            button1_ = false;
            button2_ = false;
            button3_ = false;
            button4_ = false;
            leftTrigger_ = false;
            rightTrigger_ = false;
            leftBumper_ = false;
            rightBumper_ = false;
        }

        /// <summary>
        /// Completely reinitializes an InputSet
        /// </summary>
        public void reset()
        {
            clearSticks();
            clearToggles();
            clearInputs();
        }

    }

    /// <summary>
    /// This enumeration is used when setting and clearing toggles and sticks
    /// The last item, LENGTH, should never be used in these conditions, but
    ///  is a replacement since C# enumerations do not have a .length attrib,
    ///  and is useful for iterating over arrays which contain one element
    ///  for each item in the enumeration.  This item MUST remain last.
    /// </summary>
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
