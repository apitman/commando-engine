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
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Commando.controls;

namespace Commando.controls
{
    class X360ControllerInput : ControllerInputInterface
    {
        protected InputSet inputs_;

        private PlayerIndex player_;

        public X360ControllerInput()
        {
            player_ = PlayerIndex.One;
            inputs_ = new InputSet();
        }

        public X360ControllerInput(PlayerIndex player)
        {
            player_ = player;
            inputs_ = new InputSet();
        }

        #region ControllerInputInterface Members

        public InputSet getInputSet()
        {
            GamePadState gps = GamePad.GetState(player_);

            inputs_.leftDirectionalX = gps.ThumbSticks.Left.X;
            inputs_.leftDirectionalY = gps.ThumbSticks.Left.Y;
            inputs_.rightDirectionalX = gps.ThumbSticks.Right.X;
            inputs_.rightDirectionalY = gps.ThumbSticks.Right.Y;

            inputs_.confirmButton = gps.IsButtonDown(Buttons.Start);
            inputs_.cancelButton = gps.IsButtonDown(Buttons.Back);

            inputs_.setButton1(gps.IsButtonDown(Buttons.A));
            inputs_.setButton2(gps.IsButtonDown(Buttons.B));
            inputs_.setButton3(gps.IsButtonDown(Buttons.X));
            inputs_.setButton4(gps.IsButtonDown(Buttons.Y));

            inputs_.leftTrigger = gps.IsButtonDown(Buttons.LeftTrigger);
            inputs_.rightTrigger = gps.IsButtonDown(Buttons.RightTrigger);
            inputs_.leftBumper = gps.IsButtonDown(Buttons.LeftShoulder);
            inputs_.rightBumper = gps.IsButtonDown(Buttons.RightShoulder);

            return inputs_;
        }

        #endregion
    }
}