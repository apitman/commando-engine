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
    class GameFont
    {
        //Texture image for this GameTexture
        protected SpriteFont font_;

        protected SpriteBatch spriteBatch_;

        public GameFont()
        {

        }

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

        public void drawString(string text, Vector2 pos, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth)
        {
            spriteBatch_.DrawString(font_, text, pos, color, rotation, origin, scale, effects, layerDepth);
        }
    }
}
