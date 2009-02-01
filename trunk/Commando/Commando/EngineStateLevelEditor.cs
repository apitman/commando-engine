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
using Commando.levels;
using Commando.objects;

namespace Commando
{
    /// <summary>
    /// The main class for the level editor
    /// </summary>
    public class EngineStateLevelEditor : EngineStateInterface
    {
        const int SCREEN_SIZE_X = 375;
        const int SCREEN_SIZE_Y = 375;
        const int NUM_TILES = 23;

        protected int[,] intTiles_ = new int[,] {{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                                        {0,7,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,8,0,0,0,0,0,0,0},
                                        {0,2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,4,0,0,0,0,0,0,0},
                                        {0,2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,4,0,0,0,0,0,0,0},
                                        {0,2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,4,0,0,0,0,0,0,0},
                                        {0,2,1,1,1,10,11,12,10,11,12,1,1,1,1,1,1,19,3,3,3,3,3,8,0},
                                        {0,2,1,1,1,13,14,15,13,14,15,1,1,1,1,1,1,1,1,1,1,1,1,4,0},
                                        {0,2,1,1,1,16,17,18,16,17,18,1,1,1,1,1,1,1,1,1,1,1,1,4,0},
                                        {0,2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,4,0},
                                        {0,2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,4,0},
                                        {0,2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,4,0},
                                        {0,2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,4,0},
                                        {0,2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,4,0},
                                        {0,2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,4,0},
                                        {0,2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,4,0},
                                        {0,2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,4,0},
                                        {0,2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,20,5,5,5,5,5,9,0},
                                        {0,2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,4,0,0,0,0,0,0,0},
                                        {0,2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,4,0,0,0,0,0,0,0},
                                        {0,2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,4,0,0,0,0,0,0,0},
                                        {0,6,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,9,0,0,0,0,0,0,0},
                                        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0}};
        protected EngineStateInterface returnState_;
        protected List<TileObject> tiles_;
        protected Engine engine_;
        protected int cursorPosX_;
        protected int cursorPosY_;
        protected TileObject displayTile_;
        protected int curTileIndex_;

        /// <summary>
        /// The constructor takes an EngineStateInterface to return to when level editing is done
        /// </summary>
        public EngineStateLevelEditor(Engine engine, EngineStateInterface returnState)
        {
            engine_ = engine;
            engine_.setScreenSize(SCREEN_SIZE_X, SCREEN_SIZE_Y);
            returnState_ = returnState;
            tiles_ = Tiler.getTiles(intTiles_);
            cursorPosX_ = 0;
            cursorPosY_ = 0;
            curTileIndex_ = 0;
        }

        /// <summary>
        /// Determines how to update the level editor based on user input
        /// </summary>
        /// <param name="gameTime">The GameTime parameter</param>
        /// <returns>Returns itself if user wants to stay in this mode. Returns EngineStateMenu otherwise.</returns>
        public EngineStateInterface update(GameTime gameTime)
        {
            InputSet inputs = engine_.getInputs();

            if (inputs.getCancelButton())
            {
                inputs.setToggle(InputsEnum.CANCEL_BUTTON);
                return returnState_;
            }

            if (inputs.getLeftDirectionalX() < 0)
            {
                inputs.setToggle(InputsEnum.LEFT_DIRECTIONAL);
                cursorPosX_--;
            }
            else if (inputs.getLeftDirectionalX() > 0)
            {
                inputs.setToggle(InputsEnum.LEFT_DIRECTIONAL);
                cursorPosX_++;
            }

            if (inputs.getLeftDirectionalY() < 0)
            {
                inputs.setToggle(InputsEnum.LEFT_DIRECTIONAL);
                cursorPosY_--;
            }
            else if (inputs.getLeftDirectionalY() > 0)
            {
                inputs.setToggle(InputsEnum.LEFT_DIRECTIONAL);
                cursorPosY_++;
            }

            if (inputs.getButton1())
            {
                inputs.setToggle(InputsEnum.BUTTON_1);
                displayTile_ = new TileObject(TextureMap.getInstance().getTexture("Tile_" + curTileIndex_), new Vector2((float)cursorPosX_ * Tiler.tileSideLength_, (float)cursorPosY_ * Tiler.tileSideLength_), Vector2.Zero, 0.0f);
            }

            return this;
        }

        /// <summary>
        /// Draws the level that is currently being edited
        /// </summary>
        public void draw()
        {
            engine_.GraphicsDevice.Clear(Color.BurlyWood);

            foreach (TileObject tO in tiles_)
            {
                tO.draw(new GameTime());
            }

            displayTile_.draw(new GameTime());
        }
    }
}
