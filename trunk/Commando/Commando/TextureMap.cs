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
            textures_.Add("Woger_Ru", new GameTexture(".//Content//Giant_A.png", spriteBatch, graphics));
            textures_.Add("SamplePlayer", new GameTexture(".//Content//Sprites//SamplePlayer.png", spriteBatch, graphics));
            textures_.Add("SamplePlayer_Small", new GameTexture(".//Content//Sprites//SamplePlayer_Small.png", spriteBatch, graphics));
            textures_.Add("SamplePlayer_XSmall", new GameTexture(".//Content//Sprites//SamplePlayer_XSmall.png", spriteBatch, graphics));
            textures_.Add("testMenu", new GameTexture(".//Content//Sprites//testmenu.PNG", spriteBatch, graphics));
            textures_.Add("MenuStartReg", new GameTexture(".//Content//Sprites//menuStartReg.png", spriteBatch, graphics));
            textures_.Add("MenuStartSelected", new GameTexture(".//Content//Sprites//menuStartDown.png", spriteBatch, graphics));
            textures_.Add("healthBarOutline", new GameTexture(".//Content//healthBarOutline.png", spriteBatch, graphics));
            textures_.Add("healthBarFiller", new GameTexture(".//Content//healthBarFiller.png", spriteBatch, graphics));
            textures_.Add("pistol", new GameTexture(".//Content//pistol.png", spriteBatch, graphics));
            textures_.Add("Tile_0", new GameTexture(".//Content//Tiles//Blank.bmp", spriteBatch, graphics));
            textures_.Add("Tile_1", new GameTexture(".//Content//Tiles//Floor_Tile.bmp", spriteBatch, graphics));
            textures_.Add("Tile_2", new GameTexture(".//Content//Tiles//Wall_Left.bmp", spriteBatch, graphics));
            textures_.Add("Tile_3", new GameTexture(".//Content//Tiles//Wall_Top.bmp", spriteBatch, graphics));
            textures_.Add("Tile_4", new GameTexture(".//Content//Tiles//Wall_Right.bmp", spriteBatch, graphics));
            textures_.Add("Tile_5", new GameTexture(".//Content//Tiles//Wall_Bottom.bmp", spriteBatch, graphics));
            textures_.Add("Tile_6", new GameTexture(".//Content//Tiles//Wall_Corner_Bottom_Left.bmp", spriteBatch, graphics));
            textures_.Add("Tile_7", new GameTexture(".//Content//Tiles//Wall_Corner_Top_Left.bmp", spriteBatch, graphics));
            textures_.Add("Tile_8", new GameTexture(".//Content//Tiles//Wall_Corner_Top_Right.bmp", spriteBatch, graphics));
            textures_.Add("Tile_9", new GameTexture(".//Content//Tiles//Wall_Corner_Bottom_Right.bmp", spriteBatch, graphics));
            textures_.Add("Tile_10", new GameTexture(".//Content//Tiles//Crate_0_0.bmp", spriteBatch, graphics));
            textures_.Add("Tile_11", new GameTexture(".//Content//Tiles//Crate_0_1.bmp", spriteBatch, graphics));
            textures_.Add("Tile_12", new GameTexture(".//Content//Tiles//Crate_0_2.bmp", spriteBatch, graphics));
            textures_.Add("Tile_13", new GameTexture(".//Content//Tiles//Crate_1_0.bmp", spriteBatch, graphics));
            textures_.Add("Tile_14", new GameTexture(".//Content//Tiles//Crate_1_1.bmp", spriteBatch, graphics));
            textures_.Add("Tile_15", new GameTexture(".//Content//Tiles//Crate_1_2.bmp", spriteBatch, graphics));
            textures_.Add("Tile_16", new GameTexture(".//Content//Tiles//Crate_2_0.bmp", spriteBatch, graphics));
            textures_.Add("Tile_17", new GameTexture(".//Content//Tiles//Crate_2_1.bmp", spriteBatch, graphics));
            textures_.Add("Tile_18", new GameTexture(".//Content//Tiles//Crate_2_2.bmp", spriteBatch, graphics));
            textures_.Add("Tile_19", new GameTexture(".//Content//Tiles//Wall_Corner_I_Bottom_Left.bmp", spriteBatch, graphics));
            textures_.Add("Tile_20", new GameTexture(".//Content//Tiles//Wall_Corner_I_Top_Left.bmp", spriteBatch, graphics));
            textures_.Add("Tile_21", new GameTexture(".//Content//Tiles//Wall_Corner_I_Top_Right.bmp", spriteBatch, graphics));
            textures_.Add("Tile_22", new GameTexture(".//Content//Tiles//Wall_Corner_I_Bottom_Right.bmp", spriteBatch, graphics));

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
