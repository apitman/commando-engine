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
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Commando.controls;

namespace Commando
{
    public class EngineStateGameOver : EngineStateInterface
    {
        protected static readonly Color BACKGROUND_COLOR = Color.Black;

        protected static readonly Color GAME_OVER_COLOR = Color.White;
        protected Vector2 GAME_OVER_POSITION
        {
            get
            {
                Rectangle r = engine_.GraphicsDevice.Viewport.TitleSafeArea;
                return new Vector2(r.X + r.Width / 2.0f, r.Y + r.Height / 2.0f - 50.0f);
            }
            set
            {
                throw new NotImplementedException();
            }
        }
        protected FontEnum GAME_OVER_FONT = FontEnum.Kootenay48;
        protected float GAME_OVER_ROTATION = 0f;
        protected float GAME_OVER_DEPTH = Constants.DEPTH_MENU_TEXT;

        protected const int LENGTH_OF_GAME_OVER = 2;
        protected const string STR_GAME_OVER_TEXT = "GAME OVER";

        protected Engine engine_;
        protected TimeSpan duration_;

        public EngineStateGameOver(Engine engine)
        {
            engine_ = engine;
            duration_ = new TimeSpan();
        }

        public EngineStateInterface update(GameTime gameTime)
        {
            duration_ = duration_.Add(gameTime.ElapsedGameTime);

            if (duration_.Seconds >= LENGTH_OF_GAME_OVER)
            {
                return new EngineStateMenu(engine_);
            }

            if (InputSet.getInstance().getButton(InputsEnum.CONFIRM_BUTTON))
            {
                InputSet.getInstance().setToggle(InputsEnum.CONFIRM_BUTTON);
                return new EngineStateMenu(engine_);
            }

            if (InputSet.getInstance().getButton(InputsEnum.CANCEL_BUTTON))
            {
                InputSet.getInstance().setToggle(InputsEnum.CANCEL_BUTTON);
                return new EngineStateMenu(engine_);
            }

            return this;
        }

        public void draw()
        {
            engine_.GraphicsDevice.Clear(BACKGROUND_COLOR);
            FontMap.getInstance().getFont(GAME_OVER_FONT).drawStringCentered(STR_GAME_OVER_TEXT,
                GAME_OVER_POSITION,
                GAME_OVER_COLOR,
                GAME_OVER_ROTATION,
                GAME_OVER_DEPTH);
        }
    }
}
