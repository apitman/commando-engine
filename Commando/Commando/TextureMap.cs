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
    class TextureMap
    {
        protected Dictionary<string, GameTexture> textures_;

        protected static TextureMap textureMapInstance_ = null;

        private TextureMap()
        {
            textures_ = new Dictionary<string, GameTexture>();
        }

        public static TextureMap getInstance()
        {
            if (textureMapInstance_ == null)
            {
                textureMapInstance_ = new TextureMap();
            }
            return textureMapInstance_;
        }

        public void loadTextures(string filename, SpriteBatch spriteBatch, GraphicsDevice graphics)
        {
            //TODO: Eventually, create automatic scripted loading of textures
            //      For now, just create the load for each texture in the function
            textures_.Add("Woger_Ru", new GameTexture(".//Content//Roger.png", spriteBatch, graphics));
        }

        public GameTexture getTexture(string textureName)
        {
            if(!textures_.ContainsKey(textureName))
            {
                return null;
            }
            return textures_[textureName];
        }
    }
}
