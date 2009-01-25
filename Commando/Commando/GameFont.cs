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
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace Commando
{
    /// <summary>
    /// Encapsulates SpriteFonts to make them easier to use
    /// </summary>
    class GameFont
    {
        // The low-level font object
        protected SpriteFont font_;

        // A SpriteBatch is necessary to draw the font
        protected SpriteBatch spriteBatch_;

        /// <summary>
        /// The private constructor prevents anyone from using the default constructor
        /// </summary>
        private GameFont()
        {

        }

        /// <summary>
        /// This is the constructor that should almost always be used
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="spriteBatch"></param>
        /// <param name="engine"></param>
        public GameFont(string filename, SpriteBatch spriteBatch, Engine engine)
        {
            //TODO: add functionality to read from a file to get imageDims

            spriteBatch_ = spriteBatch;

            font_ = engine.Content.Load<SpriteFont>(filename);
        }

        public GameFont(GameFont gFont)
        {
            spriteBatch_ = gFont.spriteBatch_;

            font_ = gFont.font_;
        }

        public SpriteFont getFont()
        {
            return font_;
        }

        public void setFont(SpriteFont sFont)
        {
            font_ = sFont;
        }

        public SpriteBatch getSpriteBatch()
        {
            return spriteBatch_;
        }

        public void setSpriteBatch(SpriteBatch sBatch)
        {
            spriteBatch_ = sBatch;
        }

        public void drawString(string text, Vector2 pos, Color color)
        {
            spriteBatch_.DrawString(font_, text, pos, color);
        }

        public void drawString(string text, Vector2 pos, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth)
        {
            spriteBatch_.DrawString(font_, text, pos, color, rotation, origin, scale, effects, layerDepth);
        }

        public void drawStringCentered(string text, Vector2 pos, Color color, float rotation, float layerDepth)
        {
            Vector2 origin = font_.MeasureString(text);
            spriteBatch_.DrawString(font_, text, pos, color, rotation, new Vector2(origin.X / 2, origin.Y / 2), 1.0f, SpriteEffects.None, layerDepth);
        }

        public void drawStringCentered(string text, Vector2 pos, Color color, float rotation, float scale, SpriteEffects effects, float layerDepth)
        {
            Vector2 origin = font_.MeasureString(text);
            spriteBatch_.DrawString(font_, text, pos, color, rotation, new Vector2(origin.X / 2, origin.Y / 2), scale, effects, layerDepth);
        }
    }
}
