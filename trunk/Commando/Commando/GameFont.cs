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

namespace Commando
{
    /// <summary>
    /// Encapsulates SpriteFonts to make them easier to use
    /// </summary>
    public class GameFont
    {
        /// <summary>
        /// The low-level font object
        /// </summary>
        protected SpriteFont font_;

        /// <summary>
        /// A SpriteBatch is necessary to draw the font
        /// </summary>
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
        /// <param name="filename">The path to the .spritefont file, without .spritefont at the end</param>
        /// <param name="spriteBatch">The spriteBatch to be used when drawing text</param>
        /// <param name="engine">The main Engine class</param>
        public GameFont(string filename, SpriteBatch spriteBatch, Engine engine)
        {
            spriteBatch_ = spriteBatch;
            font_ = engine.Content.Load<SpriteFont>(filename);
        }

        /// <summary>
        /// A copy constructor
        /// </summary>
        /// <param name="gFont">The GameFont to copy</param>
        public GameFont(GameFont gFont)
        {
            spriteBatch_ = gFont.spriteBatch_;
            font_ = gFont.font_;
        }

        /// <summary>
        /// Public accessor method
        /// </summary>
        /// <returns>Returns the current SpriteFont</returns>
        public SpriteFont getFont()
        {
            return font_;
        }

        /// <summary>
        /// Public setter method
        /// </summary>
        /// <param name="sFont">The SpriteFont you want the GameFont to use</param>
        public void setFont(SpriteFont sFont)
        {
            font_ = sFont;
        }

        /// <summary>
        /// Public accessor method
        /// </summary>
        /// <returns>Returns the SpriteBatch used for drawing strings</returns>
        public SpriteBatch getSpriteBatch()
        {
            return spriteBatch_;
        }

        /// <summary>
        /// Public setter method
        /// </summary>
        /// <param name="sBatch">The SpriteBatch you want the GameFont to use</param>
        public void setSpriteBatch(SpriteBatch sBatch)
        {
            spriteBatch_ = sBatch;
        }

        /// <summary>
        /// Draws a string to the screen with the specified options
        /// </summary>
        /// <param name="text">The text you want to draw</param>
        /// <param name="myPos">The position to start drawing the top-left corner of the text</param>
        /// <param name="color">The color of the drawn text</param>
        public void drawString(string text, Vector2 pos, Color color)
        {
            spriteBatch_.DrawString(font_, text, pos, color);
        }

        /// <summary>
        /// Draws a string to the screen with the specified options
        /// </summary>
        /// <param name="text">The text you want to draw</param>
        /// <param name="myPos">The position to start drawing the top-left corner of the text</param>
        /// <param name="color">The color of the drawn text</param>
        /// <param name="rotation">How far to rotate the text</param>
        /// <param name="origin">The width and height of the text</param>
        /// <param name="scale">How big or small to scale the text</param>
        /// <param name="effects">Which SpriteEffects to add to the text</param>
        /// <param name="layerDepth">The depth at which to draw the text</param>
        public void drawString(string text, Vector2 pos, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth)
        {
            spriteBatch_.DrawString(font_, text, pos, color, rotation, origin, scale, effects, layerDepth);
        }

        /// <summary>
        /// Draws a string to the screen with the specified options
        /// </summary>
        /// <param name="text">The text you want to draw</param>
        /// <param name="myPos">The position to start drawing the top-left corner of the text</param>
        /// <param name="color">The color of the drawn text</param>
        /// <param name="rotation">How far to rotate the text</param>
        /// <param name="layerDepth">The depth at which to draw the text</param>
        public void drawStringCentered(string text, Vector2 pos, Color color, float rotation, float layerDepth)
        {
            Vector2 origin = font_.MeasureString(text);
            spriteBatch_.DrawString(font_, text, pos, color, rotation, new Vector2(origin.X / 2, origin.Y / 2), 1.0f, SpriteEffects.None, layerDepth);
        }

        /// <summary>
        /// Draws a string to the screen with the specified options
        /// </summary>
        /// <param name="text">The text you want to draw</param>
        /// <param name="myPos">The position to start drawing the top-left corner of the text</param>
        /// <param name="color">The color of the drawn text</param>
        /// <param name="rotation">How far to rotate the text</param>
        /// <param name="scale">How big or small to scale the text</param>
        /// <param name="effects">Which SpriteEffects to add to the text</param>
        /// <param name="layerDepth">The depth at which to draw the text</param>
        public void drawStringCentered(string text, Vector2 pos, Color color, float rotation, float scale, SpriteEffects effects, float layerDepth)
        {
            Vector2 origin = font_.MeasureString(text);
            spriteBatch_.DrawString(font_, text, pos, color, rotation, new Vector2(origin.X / 2, origin.Y / 2), scale, effects, layerDepth);
        }
    }
}
