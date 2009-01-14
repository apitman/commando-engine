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
using Microsoft.Xna.Framework.Input;

namespace Commando.controls
{
    class PCControllerInput : ControllerInputInterface
    {

        protected int previousMouseX_;
        protected int previousMouseY_;

        #region ControllerInputInterface Members

        public InputSet getInputSet()
        {
            InputSet inputs = new InputSet();

            inputs.leftDirectionalX = 0;
            inputs.rightDirectionalX = 0;
            inputs.leftDirectionalY = 0;
            inputs.rightDirectionalY = 0;

            if (Keyboard.GetState().IsKeyDown(Keys.W) &&
                Keyboard.GetState().IsKeyDown(Keys.S))
            {
                inputs.leftDirectionalY = 0;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                inputs.leftDirectionalY = -1;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                inputs.leftDirectionalY = 1;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.A) &&
                Keyboard.GetState().IsKeyDown(Keys.D))
            {
                inputs.leftDirectionalX = 0;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                inputs.leftDirectionalX = -1;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                inputs.leftDirectionalX = 1;
            }

            inputs.rightDirectionalX = Mouse.GetState().X - previousMouseX_;
            inputs.rightDirectionalY = Mouse.GetState().Y + previousMouseY_;
            previousMouseX_ = Mouse.GetState().X;
            previousMouseY_ = Mouse.GetState().Y;

            inputs.confirmButton = Keyboard.GetState().IsKeyDown(Keys.Enter);
            inputs.cancelButton = Keyboard.GetState().IsKeyDown(Keys.Escape);

            inputs.button1 = Keyboard.GetState().IsKeyDown(Keys.Space);
            inputs.button2 = Keyboard.GetState().IsKeyDown(Keys.LeftShift);
            inputs.button3 = Keyboard.GetState().IsKeyDown(Keys.X);
            inputs.button4 = Keyboard.GetState().IsKeyDown(Keys.C);

            inputs.leftTrigger = Mouse.GetState().LeftButton == ButtonState.Pressed;
            inputs.rightTrigger = Mouse.GetState().RightButton == ButtonState.Pressed;

            return inputs;
        }

        #endregion
    }
}
