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
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using Commando.controls;

namespace Commando
{
    class EngineStatePause : EngineStateInterface
    {

        protected Engine engine_;
        protected EngineStateInterface savedState_;

        public EngineStatePause(Engine engine, EngineStateInterface savedState)
        {
            engine_ = engine;
            savedState_ = savedState;
        }

        #region EngineStateInterface Members

        public EngineStateInterface update(GameTime gameTime)
        {
            InputSet inputs = engine_.getInputs();

            if (inputs.getConfirmButton())
            {
                return savedState_;
            }

            if (inputs.getCancelButton())
            {
                inputs.setToggle(InputsEnum.CANCEL_BUTTON);
                return savedState_;
            }

            return this;
        }

        public void draw()
        {
            engine_.GraphicsDevice.Clear(Color.DarkOliveGreen);
            string pauseText = "Game is paused";
            GameFont pauseFont = FontMap.getInstance().getFont(FontEnum.Kootenay);
            Vector2 origin = pauseFont.getFont().MeasureString(pauseText);
            pauseFont.drawString(pauseText,
                new Vector2(engine_.GraphicsDevice.Viewport.Width / 2,
                    engine_.GraphicsDevice.Viewport.Height / 2),
                Color.Black,
                0.0f,
                new Vector2(pauseFont.getFont().MeasureString(pauseText).X / 2,
                    pauseFont.getFont().MeasureString(pauseText).Y / 2),
                1.0f,
                SpriteEffects.None,
                1.0f);
        }

        #endregion
    }
}
