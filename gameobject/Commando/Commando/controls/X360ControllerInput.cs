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
using Microsoft.Xna.Framework.Input;

namespace Commando.controls
{
    /// <summary>
    /// Implementation of a ControllerInputInterface for a user with an
    /// Xbox 360 Gamepad, either connected to a PC or a console.
    /// </summary>
    class X360ControllerInput : ControllerInputInterface
    {
        protected Engine engine_;
        protected PlayerIndex player_;
        protected InputSet inputs_;

        // Key mapping
        // ------------------
        // Currently both directionals are hardcoded to the thumbsticks.
        protected const Buttons CONFIRM = Buttons.Start;
        protected const Buttons CANCEL = Buttons.Back;
        protected const Buttons BUTTON_1 = Buttons.A;
        protected const Buttons BUTTON_2 = Buttons.B;
        protected const Buttons BUTTON_3 = Buttons.X;
        protected const Buttons BUTTON_4 = Buttons.Y;
        protected const Buttons LEFT_TRIGGER = Buttons.LeftTrigger;
        protected const Buttons RIGHT_TRIGGER = Buttons.RightTrigger;
        protected const Buttons LEFT_BUMPER = Buttons.LeftShoulder;
        protected const Buttons RIGHT_BUMPER = Buttons.RightShoulder;

        /// <summary>
        /// Default constructor; assigns itself to Player One's input device.
        /// </summary>
        /// <param name="engine">The engine using the controller.</param>
        public X360ControllerInput(Engine engine)
        {
            engine_ = engine;
            player_ = PlayerIndex.One;
            inputs_ = InputSet.getInstance();
        }

        /// <summary>
        /// Constructor which allows specification of a player.
        /// </summary>
        /// <param name="engine">The engine using the controller.</param>
        /// <param name="player">The player whose input should be read.</param>
        public X360ControllerInput(Engine engine, PlayerIndex player)
        {
            engine_ = engine;
            player_ = player;
            inputs_ = InputSet.getInstance(player);
        }

        #region ControllerInputInterface Members

        /// <summary>
        /// Returns the InputSet which the device is populating.
        /// </summary>
        /// <returns>An InputSet with the current frame's input.</returns>
        public InputSet getInputSet()
        {
            return inputs_;
        }

        /// <summary>
        /// Should be called EXACTLY once per frame to read inputs from
        /// the device and populate the InputSet accordingly.
        /// </summary>
        public void updateInputSet()
        {
            GamePadState gps = GamePad.GetState(player_);

            inputs_.setLeftDirectional(gps.ThumbSticks.Left.X,
                                        gps.ThumbSticks.Left.Y);
            inputs_.setRightDirectional(gps.ThumbSticks.Right.X,
                                        -gps.ThumbSticks.Right.Y);

            inputs_.setConfirmButton(gps.IsButtonDown(CONFIRM));
            inputs_.setCancelButton(gps.IsButtonDown(CANCEL));

            inputs_.setButton1(gps.IsButtonDown(BUTTON_1));
            inputs_.setButton2(gps.IsButtonDown(BUTTON_2));
            inputs_.setButton3(gps.IsButtonDown(BUTTON_3));
            inputs_.setButton4(gps.IsButtonDown(BUTTON_4));

            inputs_.setLeftTrigger(gps.IsButtonDown(LEFT_TRIGGER));
            inputs_.setRightTrigger(gps.IsButtonDown(RIGHT_TRIGGER));
            inputs_.setLeftBumper(gps.IsButtonDown(LEFT_BUMPER));
            inputs_.setRightBumper(gps.IsButtonDown(RIGHT_BUMPER));

        }

        #endregion
    }
}
