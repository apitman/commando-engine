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
    public struct objectRepresentation
    {
        string objName_;
        Vector2 objPos_;
        Vector2 objRotation_;
        float objDepth_;
        objectRepresentation(string objName,Vector2 objPos,Vector2 objRotation, float objDepth)
        {
            objName_ = objName;
            objPos_ = objPos;
            objRotation_ = objRotation;
            objDepth_ = objDepth;
        }
    }
    public class EngineStateLevelEditor : EngineStateInterface
    {
        const int SCREEN_SIZE_X = 375;
        const int SCREEN_SIZE_Y = 375;
        const int NUM_TILES = 23;
        const int NUM_TILES_PER_ROW = 25;
        const int NUM_TILES_PER_COL = 22;
        const int NUM_PALLETTES = 3;
        const int MaxX = 345;
        const int MinX = 30;
        const int MaxY = 300;
        const int MinY = 30;

        const float DISP_TILE_DEPTH = 0.1f;

        protected int[,] defaultTiles_ = new int[,] {{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
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
        protected int returnScreenSizeX_;
        protected int returnScreenSizeY_;
        protected List<TileObject> tiles_;
        protected Engine engine_;
        protected int cursorPosX_;
        protected int cursorPosY_;
        protected TileObject displayTile_;
        protected int curTileIndex_;
        protected List<objectRepresentation> myObjects_;
        protected List<String>enemyNameList;
        protected List<String>objectNameList;
        protected enum pallette_{tile, enemy, misc};
        protected int curPallette_;


        /// <summary>
        /// The constructor takes an EngineStateInterface to return to when level editing is done
        /// </summary>
        public EngineStateLevelEditor(Engine engine, EngineStateInterface returnState, int returnScreenSizeX, int returnScreenSizeY)
        {
            engine_ = engine;
            engine_.setScreenSize(SCREEN_SIZE_X, SCREEN_SIZE_Y);
            engine_.IsMouseVisible = true;
            returnState_ = returnState;
            returnScreenSizeX_ = returnScreenSizeX;
            returnScreenSizeY_ = returnScreenSizeY;
            tiles_ = Tiler.getTiles(defaultTiles_);
            cursorPosX_ = 0;
            cursorPosY_ = 0;
            curTileIndex_ = 0;
            displayTile_ = new TileObject(TextureMap.getInstance().getTexture("Tile_" + curTileIndex_), new Vector2((float)cursorPosX_ * Tiler.tileSideLength_, (float)cursorPosY_ * Tiler.tileSideLength_), Vector2.Zero, DISP_TILE_DEPTH);
        }

        /// <summary>
        /// Determines how to update the level editor based on user input
        /// </summary>
        /// <param name="gameTime">The GameTime parameter</param>
        /// <returns>Returns itself if user wants to stay in this mode. Returns EngineStateMenu otherwise.</returns>
        public EngineStateInterface update(GameTime gameTime)
        {
            InputSet inputs = engine_.getInputs();

            if (inputs.getButton(InputsEnum.CANCEL_BUTTON))
            {
                inputs.setToggle(InputsEnum.CANCEL_BUTTON);
                engine_.setScreenSize(returnScreenSizeX_, returnScreenSizeY_);
                return returnState_;
            }

            if (inputs.getLeftDirectionalX() < 0)
            {
                inputs.setToggle(InputsEnum.LEFT_DIRECTIONAL);
                if (cursorPosX_ > 0)
                {
                    cursorPosX_--;
                }
            }
            else if (inputs.getLeftDirectionalX() > 0)
            {
                inputs.setToggle(InputsEnum.LEFT_DIRECTIONAL);
                if (cursorPosX_ < NUM_TILES_PER_ROW - 1)
                {
                    cursorPosX_++;
                }
            }

            if (inputs.getLeftDirectionalY() < 0)
            {
                inputs.setToggle(InputsEnum.LEFT_DIRECTIONAL);
                if (cursorPosY_ < NUM_TILES_PER_COL - 1)
                {
                    cursorPosY_++;
                }
            }
            else if (inputs.getLeftDirectionalY() > 0)
            {
                inputs.setToggle(InputsEnum.LEFT_DIRECTIONAL);
                if (cursorPosY_ > 0)
                {
                    cursorPosY_--;
                }
            }
            if (inputs.getLeftBumper())
            {
                inputs.setToggle(InputsEnum.LEFT_BUMPER);
                curPallette_--;
                if (curPallette_ < 0)
                {
                    curPallette_ = NUM_PALLETTES - 1;
                }
            }
            if (inputs.getRightBumper())
            {
                inputs.setToggle(InputsEnum.RIGHT_BUMPER);
                curPallette_++;
                curPallette_ = curPallette_ % NUM_PALLETTES;
            }
            if (inputs.getLeftTrigger())
            {
                inputs.setToggle(InputsEnum.LEFT_TRIGGER);
                //tiles_[cursorPosX_ + cursorPosY_ * NUM_TILES_PER_ROW] = new TileObject(TextureMap.getInstance().getTexture("Tile_" + curTileIndex_), new Vector2((float)cursorPosX_ * Tiler.tileSideLength_, (float)cursorPosY_ * Tiler.tileSideLength_), Vector2.Zero, 0.0f);
                Vector2 rightD = new Vector2(inputs.getRightDirectionalX(), inputs.getRightDirectionalY());
                if (rightD.X < MaxX && rightD.X > MinX && rightD.Y < MaxY && rightD.Y > MinY)
                {
                    objectRepresentation newObject = new objectRepresentation("test", rightD, Vector2.Zero, 1.0f);
                    myObjects_.Add(newObject);
                }
            }

            if (inputs.getButton(InputsEnum.BUTTON_1))
            {
                inputs.setToggle(InputsEnum.BUTTON_1);
                curTileIndex_ = (curTileIndex_ + 1) % NUM_TILES;
            }

            if (inputs.getButton(InputsEnum.BUTTON_2))
            {
                inputs.setToggle(InputsEnum.BUTTON_2);
                if (curTileIndex_ == 0)
                {
                    curTileIndex_ = NUM_TILES;
                }
                curTileIndex_ = curTileIndex_ - 1;
            }

            displayTile_ = new TileObject(TextureMap.getInstance().getTexture("Tile_" + curTileIndex_), new Vector2((float)cursorPosX_ * Tiler.tileSideLength_, (float)cursorPosY_ * Tiler.tileSideLength_), Vector2.Zero, DISP_TILE_DEPTH);

            return this;
        }

        /// <summary>
        /// Draws the level that is currently being edited
        /// </summary>
        public void draw()
        {
            engine_.GraphicsDevice.Clear(Color.Firebrick);

            foreach (TileObject tO in tiles_)
            {
                tO.draw(new GameTime());
            }

            displayTile_.draw(new GameTime());

            // Draw the highlight
            TextureMap.getInstance().getTexture("TileHighlight").drawImage(0, new Vector2((float)cursorPosX_ * Tiler.tileSideLength_ - 1.0f, (float)cursorPosY_ * Tiler.tileSideLength_ - 1.0f), 0.15f);
            //Draw the drawn objects
            for (int i = 0; i < myObjects_.Count(); i++)
            {
                
                objectRepresentation currObject = myObjects_[i];
                switch (currObject.objName_)
                {
                    cond"test":
                {

                }
                }
                
            }

            // Draw the palette
            switch (curPallette_)
            {
                case (int)pallette_.tile:
                    for (int i = 0; i < NUM_TILES; i++)
                    {
                        TextureMap.getInstance().getTexture("Tile_" + i).drawImage(0, new Vector2((10.0f + i * 20.0f) % 365.0f, (((10 + i * 20) / 365) * 20.0f) + 335.0f), 0.2f);
                    }
                    break;
                case (int)pallette_.enemy:
                    {
                        break;
                    }
                case (int)pallette_.misc:
                    {
                        break;
                    }
            }
        }
    }
}
