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

using Commando.objects;

namespace Commando.controls
{
    /// <summary>
    /// Implementation of ControllerInputInterface for a regular PC user
    /// with keyboard and mouse.
    /// </summary>
    class PCControllerInput : ControllerInputInterface
    {
        protected Engine engine_;
        protected InputSet inputs_;

        // Key mapping
        // ------------------
        // Currently the Left and Right triggers are hardcoded to the mouse
        // buttons, and the right directional is hardcoded to mouse movement.
        protected const Keys LEFT_DIR_UP = Keys.W;
        protected const Keys LEFT_DIR_DOWN = Keys.S;
        protected const Keys LEFT_DIR_LEFT = Keys.A;
        protected const Keys LEFT_DIR_RIGHT = Keys.D;
        protected const Keys CONFIRM = Keys.Enter;
        protected const Keys CANCEL = Keys.Escape;
        protected const Keys BUTTON_1 = Keys.Space;
        protected const Keys BUTTON_2 = Keys.LeftShift;
        protected const Keys BUTTON_3 = Keys.X;
        protected const Keys BUTTON_4 = Keys.C;
        protected const Keys LEFT_BUMPER = Keys.Q;
        protected const Keys RIGHT_BUMPER = Keys.E;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="engine">The engine using the controller.</param>
        public PCControllerInput(Engine engine)
        {
            engine_ = engine;
            inputs_ = InputSet.getInstance();
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

            KeyboardState ks = Keyboard.GetState();
            MouseState ms = Mouse.GetState();

            float leftX = 0;
            float leftY = 0;

            if (ks.IsKeyDown(LEFT_DIR_UP))
            {
                leftY += 1.0f;
            }
            if (ks.IsKeyDown(LEFT_DIR_DOWN))
            {
                leftY += -1.0f;
            }

            if (ks.IsKeyDown(LEFT_DIR_RIGHT))
            {
                leftX += 1.0f;
            }
            if (ks.IsKeyDown(LEFT_DIR_LEFT))
            {
                leftX += -1.0f;
            }

            inputs_.setLeftDirectional(leftX, leftY);

            // Calculate RightDirectional from the mouse, which in gameplay is relative
            //  to the player's character
            GameObject player = PlayerHelper.Player_;
            if (player != null)
            {
                float playerCenterX = PlayerHelper.Player_.getPosition().X;
                float playerCenterY = PlayerHelper.Player_.getPosition().Y;

                Vector2 rightDirectional =
                    new Vector2(ms.X - playerCenterX, ms.Y - playerCenterY);
                rightDirectional.Normalize();

                inputs_.setRightDirectional(rightDirectional.X,
                                            rightDirectional.Y);
            }
            else
            {
                inputs_.setRightDirectional(ms.X, ms.Y);
            }

            inputs_.setConfirmButton(ks.IsKeyDown(CONFIRM));
            inputs_.setCancelButton(ks.IsKeyDown(CANCEL));

            inputs_.setButton1(ks.IsKeyDown(BUTTON_1));
            inputs_.setButton2(ks.IsKeyDown(BUTTON_2));
            inputs_.setButton3(ks.IsKeyDown(BUTTON_3));
            inputs_.setButton4(ks.IsKeyDown(BUTTON_4));

            inputs_.setLeftTrigger(ms.LeftButton == ButtonState.Pressed);
            inputs_.setRightTrigger(ms.RightButton == ButtonState.Pressed);

            inputs_.setLeftBumper(ks.IsKeyDown(LEFT_BUMPER));
            inputs_.setRightBumper(ks.IsKeyDown(RIGHT_BUMPER));
        }

        #endregion
    }
}
