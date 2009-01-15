﻿/*
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
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Commando.controls
{
    class PCControllerInput : ControllerInputInterface
    {
        protected InputSet inputs_;
        protected int previousMouseX_;
        protected int previousMouseY_;

        public PCControllerInput()
        {
            inputs_ = new InputSet();
        }

        #region ControllerInputInterface Members

        public InputSet getInputSet()
        {
            return inputs_;
        }

        public void updateInputSet()
        {

            KeyboardState ks = Keyboard.GetState();
            MouseState ms = Mouse.GetState();

            float leftX = 0;
            float leftY = 0;

            if (ks.IsKeyDown(Keys.W))
            {
                leftY += 1.0f;
            }
            if (ks.IsKeyDown(Keys.S))
            {
                leftY += -1.0f;
            }

            if (ks.IsKeyDown(Keys.D))
            {
                leftX += 1.0f;
            }
            if (ks.IsKeyDown(Keys.A))
            {
                leftX += -1.0f;
            }

            inputs_.setLeftDirectional(leftX, leftY);

            inputs_.setRightDirectional(ms.X - previousMouseX_,
                                        ms.Y + previousMouseY_);
            previousMouseX_ = ms.X;
            previousMouseY_ = ms.Y;

            inputs_.setConfirmButton(ks.IsKeyDown(Keys.Enter));
            inputs_.setCancelButton(ks.IsKeyDown(Keys.Escape));

            inputs_.setButton1(ks.IsKeyDown(Keys.Space));
            inputs_.setButton2(ks.IsKeyDown(Keys.LeftShift));
            inputs_.setButton3(ks.IsKeyDown(Keys.X));
            inputs_.setButton4(ks.IsKeyDown(Keys.C));

            inputs_.setLeftTrigger(ms.LeftButton == ButtonState.Pressed);
            inputs_.setRightTrigger(ms.RightButton == ButtonState.Pressed);

            inputs_.setLeftBumper(ks.IsKeyDown(Keys.Z));
            inputs_.setRightBumper(ks.IsKeyDown(Keys.V));
        }

        #endregion
    }
}
