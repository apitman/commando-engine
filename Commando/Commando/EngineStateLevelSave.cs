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
using Microsoft.Xna.Framework.Input;

namespace Commando
{
    public class EngineStateLevelSave : EngineStateInterface
    {
        protected const string MESSAGE = "Please enter a filename to save as (no extension):";
        protected const FontEnum MESSAGE_FONT = FontEnum.Kootenay;
        protected Vector2 MESSAGE_POSITION
        {
            get
            {
                Rectangle r = engine_.GraphicsDevice.Viewport.TitleSafeArea;
                return new Vector2(r.Width / 2 + r.X, r.Height / 2 + r.Y);
            }

            set
            {
                throw new NotImplementedException();
            }
        }
        protected static readonly Color MESSAGE_COLOR = Color.White;
        protected const float MESSAGE_DEPTH = Constants.DEPTH_MENU_TEXT;
        protected static readonly Color FILENAME_COLOR = Color.Green;
        protected static readonly Vector2 FILENAME_OFFSET = new Vector2(0, 50);

        protected const int STICKY_TIME = 5;

        protected Engine engine_;
        protected EngineStateLevelEditor state_;

        protected GameFont mainMessage_;
        protected string currentFilename_ = "";
        protected Dictionary<Keys, int> stickies_ = new Dictionary<Keys,int>();

        public EngineStateLevelSave(Engine engine/*, EngineStateLevelEditor state*/)
        {
            engine_ = engine;
            //state_ = state;
            mainMessage_ = FontMap.getInstance().getFont(MESSAGE_FONT);
            stickies_[Keys.Enter] = STICKY_TIME * 10;

        }

        public EngineStateInterface update(GameTime gameTime)
        {

#if !XBOX
            KeyboardState ks = Keyboard.GetState();
            if (ks.IsKeyDown(Keys.Enter) && safeKeyTest(Keys.Enter))
            {
                // TODO Actually save level
                return new EngineStateMenu(engine_);
            }

            if (ks.IsKeyDown(Keys.Back) && safeKeyTest(Keys.Back))
            {
                stickies_[Keys.Back] = STICKY_TIME;
                if (currentFilename_.Length > 0)
                {
                    currentFilename_ = currentFilename_.Remove(currentFilename_.Length - 1);
                }
                return this;
            }

            Keys[] keys = ks.GetPressedKeys();
            for (int i = 0; i < keys.Length; i++)
            {
                if (!safeKeyTest(keys[i]))
                {
                    // do nothing
                    continue;
                }
                if (keys[i].ToString().Length > 1)
                {
                    continue;
                }
                if (ks.IsKeyDown(Keys.LeftShift) || ks.IsKeyDown(Keys.RightShift))
                {
                    currentFilename_ += keys[i].ToString().ToUpper();
                }
                else
                {
                    currentFilename_ += keys[i].ToString().ToLower();
                }
                stickies_[keys[i]] = STICKY_TIME;
            }

            Dictionary<Keys, int>.KeyCollection keyCollection = stickies_.Keys;
            List<Keys> stuckKeys = new List<Keys>(keyCollection);
            for (int i = 0; i < stickies_.Keys.Count; i++)
            {
                if (stickies_[stuckKeys[i]] >= 0)
                    stickies_[stuckKeys[i]] -= 1;
            }

            return this;
#else

#endif

        }

        public void draw()
        {
            engine_.GraphicsDevice.Clear(Color.Black);

            mainMessage_.drawStringCentered(MESSAGE, MESSAGE_POSITION, MESSAGE_COLOR, 0, MESSAGE_DEPTH);

            Vector2 fileNamePos = MESSAGE_POSITION + FILENAME_OFFSET;
            mainMessage_.drawStringCentered(currentFilename_, fileNamePos, FILENAME_COLOR, 0, MESSAGE_DEPTH);
        }

        private bool safeKeyTest(Keys key)
        {
            return (!(stickies_.ContainsKey(key) && stickies_[key] > 0));
        }

    }
}
