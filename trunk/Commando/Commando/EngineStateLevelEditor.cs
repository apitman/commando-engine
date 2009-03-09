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

    public struct objectRepresentation
    {
        public string objName_;
        public Vector2 objPos_;
        public Vector2 objRotation_;
        public float objDepth_;
        public objectRepresentation(string objName, Vector2 objPos, Vector2 objRotation, float objDepth)
        {
            objName_ = objName;
            objPos_ = objPos;
            objRotation_ = objRotation;
            objDepth_ = objDepth;
        }
    }

    /// <summary>
    /// The main class for the level editor
    /// </summary>
    public class EngineStateLevelEditor : EngineStateInterface
    {
        const int SCREEN_SIZE_X = 375;
        const int SCREEN_SIZE_Y = 375;
        const int NUM_TILES = 23;
        const int NUM_TILES_PER_ROW = 25;
        const int NUM_TILES_PER_COL = 22;
        const int NUM_PALLETTES = 3;
        const int MAX_MOUSE_X = 345;
        const int MAX_MOUSE_Y = 300;
        const int MIN_MOUSE_X = 30;
        const int MIN_MOUSE_Y = 30;
        const int MAX_CURSOR_X = NUM_TILES_PER_ROW - 3;
        const int MAX_CURSOR_Y = NUM_TILES_PER_COL - 3;
        const int MIN_CURSOR_X = 2;
        const int MIN_CURSOR_Y = 2;
        const int MAX_NUM_ENEMIES = 3;
        const string DUMMY_ENEMY = "dummyEnemy";
        const string SAVE_PATH = "user level.xml";
        const float DISP_TILE_DEPTH = 0.1f;

        List<DrawableObjectAbstract> drawPipeline_;

        protected int[,] defaultTiles_ = new int[,] {{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                                                    {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                                                    {0,0,7,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,8,0,0},
                                                    {0,0,2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,4,0,0},
                                                    {0,0,2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,4,0,0},
                                                    {0,0,2,1,1,10,11,12,10,11,12,1,1,1,1,1,1,1,1,1,1,1,4,0,0},
                                                    {0,0,2,1,1,13,14,15,13,14,15,1,1,1,1,1,1,1,1,1,1,1,4,0,0},
                                                    {0,0,2,1,1,16,17,18,16,17,18,1,1,1,1,1,1,1,1,1,1,1,4,0,0},
                                                    {0,0,2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,4,0,0},
                                                    {0,0,2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,4,0,0},
                                                    {0,0,2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,4,0,0},
                                                    {0,0,2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,4,0,0},
                                                    {0,0,2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,4,0,0},
                                                    {0,0,2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,4,0,0},
                                                    {0,0,2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,4,0,0},
                                                    {0,0,2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,4,0,0},
                                                    {0,0,2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,4,0,0},
                                                    {0,0,2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,4,0,0},
                                                    {0,0,2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,4,0,0},
                                                    {0,0,6,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,9,0,0},
                                                    {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
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
        protected int enemyIndex_;


        /// <summary>
        /// The constructor takes an EngineStateInterface to return to when level editing is done
        /// </summary>
        public EngineStateLevelEditor(Engine engine, EngineStateInterface returnState, int returnScreenSizeX, int returnScreenSizeY)
        {
            PlayerHelper.Player_ = null; // Necessary to have mouse input if the player has entered EngineStateGameplay before entering the level editor
            engine_ = engine;
            engine_.setScreenSize(SCREEN_SIZE_X, SCREEN_SIZE_Y);
            engine_.IsMouseVisible = true;
            returnState_ = returnState;
            returnScreenSizeX_ = returnScreenSizeX;
            returnScreenSizeY_ = returnScreenSizeY;
            enemyIndex_ = 0;
            myObjects_ = new List<objectRepresentation>(MAX_NUM_ENEMIES);

            // Load the user's level from XML
            try
            {
                System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
                doc.Load(SAVE_PATH);
                
                // First load the tiles
                System.Xml.XmlElement ele = (System.Xml.XmlElement) doc.GetElementsByTagName("level")[0];
                int[,] loadedTiles = new int[Convert.ToInt32(ele.GetAttribute("numTilesTall")), Convert.ToInt32(ele.GetAttribute("numTilesWide"))];
                System.Xml.XmlNodeList tList = doc.GetElementsByTagName("tile");
                for (int i = 0; i < Convert.ToInt32(ele.GetAttribute("numTilesTall")); i++)
                {
                    for (int j = 0; j < Convert.ToInt32(ele.GetAttribute("numTilesWide")); j++)
                    {
                        System.Xml.XmlElement ele2 = (System.Xml.XmlElement) tList[j + i * Convert.ToInt32(ele.GetAttribute("numTilesWide"))];
                        loadedTiles[i, j] = Convert.ToInt32(ele2.GetAttribute("index"));
                    }
                }
                tiles_ = Tiler.getTiles(drawPipeline_, loadedTiles);

                // Now load the enemies
                tList = doc.GetElementsByTagName("enemy");
                for (int i = 0; i < Convert.ToInt32(tList.Count); i++)
                {
                    System.Xml.XmlElement ele2 = (System.Xml.XmlElement) tList[i];
                    objectRepresentation newObject = new objectRepresentation(ele2.GetAttribute("name"), new Vector2((float)Convert.ToInt32(ele2.GetAttribute("posX")), (float)Convert.ToInt32(ele2.GetAttribute("posY"))), Vector2.Zero, 0.2f);
                    myObjects_.Add(newObject);
                }
            }
            catch (Exception)
            {
                tiles_ = Tiler.getTiles(drawPipeline_, defaultTiles_);
            }

            cursorPosX_ = 2;
            cursorPosY_ = 2;
            curTileIndex_ = 0;
            displayTile_ = new TileObject(drawPipeline_, TextureMap.getInstance().getTexture("Tile_" + curTileIndex_), new Vector2((float)cursorPosX_ * Tiler.tileSideLength_, (float)cursorPosY_ * Tiler.tileSideLength_), Vector2.Zero, DISP_TILE_DEPTH);
        }

        /// <summary>
        /// Determines how to update the level editor based on user input
        /// </summary>
        /// <param name="gameTime">The GameTime parameter</param>
        /// <returns>Returns itself if user wants to stay in this mode. Returns EngineStateMenu otherwise.</returns>
        public EngineStateInterface update(GameTime gameTime)
        {
            InputSet inputs = engine_.getInputs();

            // Prepare to output to XML
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            try
            {
                doc.Load(SAVE_PATH);
            }
            catch (Exception)
            {
                System.Xml.XmlElement ele = doc.CreateElement("level");
                ele.SetAttribute("numTilesWide", NUM_TILES_PER_ROW.ToString());
                ele.SetAttribute("numTilesTall", NUM_TILES_PER_COL.ToString());
                ele.SetAttribute("screenSizeX", SCREEN_SIZE_X.ToString());
                ele.SetAttribute("screenSizeY", SCREEN_SIZE_Y.ToString());
                doc.AppendChild(ele);

                ele = doc.CreateElement("tiles");
                System.Xml.XmlElement ele2;
                foreach (int i in defaultTiles_)
                {
                    ele2 = doc.CreateElement("tile");
                    ele2.SetAttribute("index", i.ToString());
                    ele.AppendChild(ele2);
                }
                doc.GetElementsByTagName("level")[0].AppendChild(ele);

                ele = doc.CreateElement("enemies");
                doc.GetElementsByTagName("level")[0].AppendChild(ele);
            }

            // Check the inputs
            if (inputs.getLeftDirectionalY() < 0)
            {
                inputs.setToggle(InputsEnum.LEFT_DIRECTIONAL);
                if (cursorPosY_ < MAX_CURSOR_Y)
                    cursorPosY_++;
            }
            else if (inputs.getLeftDirectionalY() > 0)
            {
                inputs.setToggle(InputsEnum.LEFT_DIRECTIONAL);
                if (cursorPosY_ > MIN_CURSOR_Y)
                    cursorPosY_--;
            }
            else if (inputs.getLeftDirectionalX() > 0)
            {
                inputs.setToggle(InputsEnum.LEFT_DIRECTIONAL);
                if (cursorPosX_ < MAX_CURSOR_X)
                    cursorPosX_++;
            }
            else if (inputs.getLeftDirectionalX() < 0)
            {
                inputs.setToggle(InputsEnum.LEFT_DIRECTIONAL);
                if (cursorPosX_ > MIN_CURSOR_X)
                    cursorPosX_--;
            }
            else if (inputs.getButton(InputsEnum.CANCEL_BUTTON))
            {
                inputs.setToggle(InputsEnum.CANCEL_BUTTON);
                engine_.setScreenSize(returnScreenSizeX_, returnScreenSizeY_);
                return returnState_;
            }

            else if (inputs.getButton(InputsEnum.LEFT_BUMPER))
            {
                curTileIndex_ = 0;
                inputs.setToggle(InputsEnum.LEFT_BUMPER);
                curPallette_--;
                if (curPallette_ < 0)
                {
                    curPallette_ = NUM_PALLETTES - 1;
                }
            }
            else if (inputs.getButton(InputsEnum.RIGHT_BUMPER))
            {
                curTileIndex_ = 0;
                inputs.setToggle(InputsEnum.RIGHT_BUMPER);
                curPallette_++;
                curPallette_ = curPallette_ % NUM_PALLETTES;
            }
            else if (inputs.getButton(InputsEnum.CONFIRM_BUTTON))
            {
                inputs.setToggle(InputsEnum.CONFIRM_BUTTON);
                switch (curPallette_)
                {
                    case (int)pallette_.tile:
                        {
                            tiles_[cursorPosX_ + cursorPosY_ * NUM_TILES_PER_ROW] = new TileObject(drawPipeline_, TextureMap.getInstance().getTexture("Tile_" + curTileIndex_), new Vector2((float)cursorPosX_ * Tiler.tileSideLength_, (float)cursorPosY_ * Tiler.tileSideLength_), Vector2.Zero, 0.0f);
                            
                            // Edit the XML tile
                            System.Xml.XmlElement tileElement = (System.Xml.XmlElement)doc.GetElementsByTagName("tile")[cursorPosX_ + cursorPosY_ * NUM_TILES_PER_ROW];
                            tileElement.SetAttribute("index", curTileIndex_.ToString());
                            break;
                        }
                    case (int)pallette_.enemy:
                        {
                            
                         //to do, add enemy on cursor position

                            break;
                        }
                    case (int)pallette_.misc:
                        {

                            break;
                        }
                }
            }
            else if (inputs.getButton(InputsEnum.RIGHT_TRIGGER))
            {
                if (curPallette_ != (int)pallette_.tile)
                {
                    inputs.setToggle(InputsEnum.RIGHT_TRIGGER);
                    
                   
                }
                
                Vector2 rightD = new Vector2(inputs.getRightDirectionalX(), inputs.getRightDirectionalY());

                switch(curPallette_)
                {
                    case (int)pallette_.tile:
                        {
                            if (rightD.X < MAX_MOUSE_X && rightD.X > MIN_MOUSE_X && rightD.Y < MAX_MOUSE_Y && rightD.Y > MIN_MOUSE_Y)
                            {
                                int myX = (int)rightD.X / 15;
                                int myY = (int)rightD.Y / 15;
                                tiles_[myX + myY * NUM_TILES_PER_ROW] = new TileObject(drawPipeline_, TextureMap.getInstance().getTexture("Tile_" + curTileIndex_), new Vector2((float)myX * Tiler.tileSideLength_, (float)myY * Tiler.tileSideLength_), Vector2.Zero, 0.0f);

                                // Edit the XML tile
                                System.Xml.XmlElement tileElement = (System.Xml.XmlElement) doc.GetElementsByTagName("tile")[myX + myY * NUM_TILES_PER_ROW];
                                tileElement.SetAttribute("index", curTileIndex_.ToString());
                            }
                            break;
                        }
                    case (int)pallette_.enemy:
                        {
                            if (rightD.X < MAX_MOUSE_X && rightD.X > MIN_MOUSE_X && rightD.Y < MAX_MOUSE_Y && rightD.Y > MIN_MOUSE_Y)
                            {
                                objectRepresentation newObject = new objectRepresentation(DUMMY_ENEMY, rightD, Vector2.Zero, 0.2f);
                                if (myObjects_.Count < MAX_NUM_ENEMIES)
                                {
                                    myObjects_.Add(newObject);
                                }
                                else
                                {
                                    myObjects_[enemyIndex_] = newObject;
                                    enemyIndex_++;
                                    enemyIndex_ = enemyIndex_ % MAX_NUM_ENEMIES;
                                }

                                 // Add an XML enemy
                                if (doc.GetElementsByTagName("enemy").Count < MAX_NUM_ENEMIES)
                                {
                                    System.Xml.XmlElement enemiesElement = (System.Xml.XmlElement) doc.GetElementsByTagName("enemies")[0];
                                    System.Xml.XmlElement enemyElement = doc.CreateElement("enemy");
                                    enemyElement.SetAttribute("name", DUMMY_ENEMY);
                                    enemyElement.SetAttribute("posX", rightD.X.ToString());
                                    enemyElement.SetAttribute("posY", rightD.Y.ToString());
                                    enemiesElement.AppendChild(enemyElement);
                                }
                                else
                                {
                                    System.Xml.XmlElement enemyElement = (System.Xml.XmlElement) doc.GetElementsByTagName("enemy")[(enemyIndex_ + MAX_NUM_ENEMIES - 1) % MAX_NUM_ENEMIES];
                                    enemyElement.SetAttribute("name", DUMMY_ENEMY);
                                    enemyElement.SetAttribute("posX", rightD.X.ToString());
                                    enemyElement.SetAttribute("posY", rightD.Y.ToString());
                                }
                            }


                            break;
                        }
                }
            }

            else if (inputs.getButton(InputsEnum.BUTTON_1))
            {
                inputs.setToggle(InputsEnum.BUTTON_1);
                curTileIndex_ = (curTileIndex_ + 1) % NUM_TILES;
            }

            else if (inputs.getButton(InputsEnum.BUTTON_2))
            {
                inputs.setToggle(InputsEnum.BUTTON_2);
                if (curTileIndex_ == 0)
                {
                    curTileIndex_ = NUM_TILES;
                }
                curTileIndex_ = curTileIndex_ - 1;
            }

            // Finish Outputting to XML
            doc.Save(SAVE_PATH);

            displayTile_ = new TileObject(drawPipeline_, TextureMap.getInstance().getTexture("Tile_" + curTileIndex_), new Vector2((float)cursorPosX_ * Tiler.tileSideLength_, (float)cursorPosY_ * Tiler.tileSideLength_), Vector2.Zero, DISP_TILE_DEPTH);

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

            TextureMap.getInstance().getTexture("TileHighlight").drawImage(0, new Vector2((cursorPosX_ *Tiler.tileSideLength_ - 1 ), (cursorPosY_ * Tiler.tileSideLength_ - 1)), 0.2f);


           // Draw  objects (ie enemies/misc)
            for (int i = 0; i < myObjects_.Count(); i++)
            {
                
                objectRepresentation currObject = myObjects_[i];
                
                if (currObject.objName_ == DUMMY_ENEMY)
                {
                    TextureMap.getInstance().getTexture("basic_enemy_walk").drawImage(0, currObject.objPos_, currObject.objDepth_);
                }
                
                
            }

            // Draw the palette
            switch (curPallette_)
            {
                case (int)pallette_.tile:
                    for (int i = 0; i < NUM_TILES; i++)
                    {
                        TextureMap.getInstance().getTexture("Tile_" + i).drawImage(0, new Vector2((10.0f + i * 20.0f) % 365.0f, (((10 + i * 20) / 365) * 20.0f) + 335.0f), 0.2f);
                        if(i == curTileIndex_)
                            TextureMap.getInstance().getTexture("TileHighlight").drawImage(0, new Vector2((10.0f + i * 20.0f) % 365.0f, (((10 + i * 20) / 365) * 20.0f) + 335.0f), 0.2f);
                    }
                    break;
                case (int)pallette_.enemy:
                    {
                        TextureMap.getInstance().getTexture("basic_enemy_walk").drawImage(0, new Vector2(10.0f , (((10* 20) / 365) * 20.0f) + 335.0f), 0.2f);
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
