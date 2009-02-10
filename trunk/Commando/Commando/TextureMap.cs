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
using Microsoft.Xna.Framework.Content;

namespace Commando
{
    /// <summary>
    /// A Singleton class which holds all the textures for the game and maps them with their
    /// names.
    /// </summary>
    class TextureMap
    {
        protected Dictionary<string, GameTexture> textures_;

        protected static TextureMap textureMapInstance_ = null;

        protected ContentManager content_;

        /// <summary>
        /// Private constructor so that only one can be created.
        /// </summary>
        private TextureMap()
        {
            textures_ = new Dictionary<string, GameTexture>();
        }

        /// <summary>
        /// Get the Singleton instance of this class.
        /// </summary>
        /// <returns></returns>
        public static TextureMap getInstance()
        {
            if (textureMapInstance_ == null)
            {
                textureMapInstance_ = new TextureMap();
            }
            return textureMapInstance_;
        }

        /// <summary>
        /// Get the ContentManager.
        /// </summary>
        /// <returns>The ContentManager for the game.</returns>
        public ContentManager getContent()
        {
            return content_;
        }

        /// <summary>
        /// Set the ContentManager.
        /// </summary>
        /// <param name="content">The ContentManager for the game.</param>
        public void setContent(ContentManager content)
        {
            content_ = content;
        }

        /// <summary>
        /// Load all the textures for the game.
        /// </summary>
        /// <param name="filename">Filename of the texture document</param>
        /// <param name="spriteBatch">SpriteBatch for the game</param>
        /// <param name="graphics">GraphicsDevice for the game</param>
        public void loadTextures(string filename, SpriteBatch spriteBatch, GraphicsDevice graphics)
        {
            //TODO: Eventually, create automatic scripted loading of textures
            //      For now, just create the load for each texture in the function
            textures_.Add("Woger_Ru", new GameTexture("Giant_A", spriteBatch, graphics));
            textures_.Add("TitleScreen", new GameTexture("TitleScreen", spriteBatch, graphics));
            textures_.Add("SamplePlayer", new GameTexture("Sprites\\SamplePlayer", spriteBatch, graphics));
            textures_.Add("SamplePlayer_Small", new GameTexture("Sprites\\SamplePlayer_Small", spriteBatch, graphics));
            textures_.Add("SamplePlayer_XSmall", new GameTexture("Sprites\\SamplePlayer_XSmall", spriteBatch, graphics));
            textures_.Add("testMenu", new GameTexture("Sprites\\testmenu", spriteBatch, graphics));
            textures_.Add("MenuStartReg", new GameTexture("Sprites\\menuStartReg", spriteBatch, graphics));
            textures_.Add("MenuStartSelected", new GameTexture("Sprites\\menuStartDown", spriteBatch, graphics));
            textures_.Add("healthBarOutline", new GameTexture("healthBarOutline", spriteBatch, graphics));
            textures_.Add("healthBarFiller", new GameTexture("healthBarFiller", spriteBatch, graphics));
            textures_.Add("pistol", new GameTexture("pistol", spriteBatch, graphics));
            textures_.Add("Tile_0", new GameTexture("Tiles\\Blank", spriteBatch, graphics));
            textures_.Add("Tile_1", new GameTexture("Tiles\\Floor_Tile", spriteBatch, graphics));
            textures_.Add("Tile_2", new GameTexture("Tiles\\Wall_Left", spriteBatch, graphics));
            textures_.Add("Tile_3", new GameTexture("Tiles\\Wall_Top", spriteBatch, graphics));
            textures_.Add("Tile_4", new GameTexture("Tiles\\Wall_Right", spriteBatch, graphics));
            textures_.Add("Tile_5", new GameTexture("Tiles\\Wall_Bottom", spriteBatch, graphics));
            textures_.Add("Tile_6", new GameTexture("Tiles\\Wall_Corner_Bottom_Left", spriteBatch, graphics));
            textures_.Add("Tile_7", new GameTexture("Tiles\\Wall_Corner_Top_Left", spriteBatch, graphics));
            textures_.Add("Tile_8", new GameTexture("Tiles\\Wall_Corner_Top_Right", spriteBatch, graphics));
            textures_.Add("Tile_9", new GameTexture("Tiles\\Wall_Corner_Bottom_Right", spriteBatch, graphics));
            textures_.Add("Tile_10", new GameTexture("Tiles\\Crate_0_0", spriteBatch, graphics));
            textures_.Add("Tile_11", new GameTexture("Tiles\\Crate_0_1", spriteBatch, graphics));
            textures_.Add("Tile_12", new GameTexture("Tiles\\Crate_0_2", spriteBatch, graphics));
            textures_.Add("Tile_13", new GameTexture("Tiles\\Crate_1_0", spriteBatch, graphics));
            textures_.Add("Tile_14", new GameTexture("Tiles\\Crate_1_1", spriteBatch, graphics));
            textures_.Add("Tile_15", new GameTexture("Tiles\\Crate_1_2", spriteBatch, graphics));
            textures_.Add("Tile_16", new GameTexture("Tiles\\Crate_2_0", spriteBatch, graphics));
            textures_.Add("Tile_17", new GameTexture("Tiles\\Crate_2_1", spriteBatch, graphics));
            textures_.Add("Tile_18", new GameTexture("Tiles\\Crate_2_2", spriteBatch, graphics));
            textures_.Add("Tile_19", new GameTexture("Tiles\\Wall_Corner_I_Bottom_Left", spriteBatch, graphics));
            textures_.Add("Tile_20", new GameTexture("Tiles\\Wall_Corner_I_Top_Left", spriteBatch, graphics));
            textures_.Add("Tile_21", new GameTexture("Tiles\\Wall_Corner_I_Top_Right", spriteBatch, graphics));
            textures_.Add("Tile_22", new GameTexture("Tiles\\Wall_Corner_I_Bottom_Right", spriteBatch, graphics));
            textures_.Add("TileHighlight", new GameTexture("Tiles\\TileHighlight", spriteBatch, graphics));
            KeyValuePair<string, GameTexture> basicEnemyWalk = GameTexture.loadTextureFromFile(".\\Content\\SpriteXML\\basic_enemy_walk.xml", spriteBatch, graphics);
            textures_.Add(basicEnemyWalk.Key, basicEnemyWalk.Value);
            KeyValuePair<string, GameTexture> playerWalk = GameTexture.loadTextureFromFile(".\\Content\\SpriteXML\\PlayerWalk.xml", spriteBatch, graphics);
            textures_.Add(playerWalk.Key, playerWalk.Value);
        }

        /// <summary>
        /// Get a texture from the map.
        /// </summary>
        /// <param name="textureName">Name of the texture</param>
        /// <returns>GameTexture with the name textureName</returns>
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
