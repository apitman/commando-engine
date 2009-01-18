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
using Microsoft.Xna.Framework.Graphics;

namespace Commando
{
    class FontMap
    {
        protected Dictionary<string, GameFont> fonts_;

        protected static FontMap fontMapInstance_ = null;

        private FontMap()
        {
            fonts_ = new Dictionary<string, GameFont>();
        }

        public static FontMap getInstance()
        {
            if (fontMapInstance_ == null)
            {
                fontMapInstance_ = new FontMap();
            }
            return fontMapInstance_;
        }

        public void loadFonts(string filename, SpriteBatch spriteBatch, Engine engine)
        {
            //TODO: Eventually, create automatic scripted loading of fonts
            //      For now, just create the load for each font in this function
            fonts_.Add("Kootenay", new GameFont("SpriteFonts/Kootenay", spriteBatch, engine));
        }

        public GameFont getFont(string fontName)
        {
            if (!fonts_.ContainsKey(fontName))
            {
                return null;
            }
            return fonts_[fontName];
        }
    }
}
