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
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Commando.controls;
using Commando.levels;
using Commando.objects;
using Microsoft.Xna.Framework.Storage;
using System.IO;

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
        const int NUM_TILES = 23;
        const int NUM_PALLETTES = 3;
        const int NUM_MISC = 3;
        const int MAX_MOUSE_X = 345;
        const int MAX_MOUSE_Y = 300;
        const int MIN_MOUSE_X = 30;
        const int MIN_MOUSE_Y = 30;
        const int MIN_CURSOR_X = 2;
        const int MIN_CURSOR_Y = 2;
        const int MAX_NUM_ENEMIES = 3;
        const string DUMMY_ENEMY = "dummyEnemy";
       
        const float DISP_TILE_DEPTH = 0.1f;
        public const int SCREEN_SIZE_X = 375;
        public const int SCREEN_SIZE_Y = 375;
        const int HUD_BAR_HEIGHT = 45;
        const int HUD_BAR_DRAW_Y = SCREEN_SIZE_Y - HUD_BAR_HEIGHT;
        const int HUD_BAR_DRAW_X = 0;

        List<DrawableObjectAbstract> drawPipeline_ = new List<DrawableObjectAbstract>();

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
        protected bool isObjSelected_;
        protected int typeSelected_;
        protected int selectedIndex_;
        protected int numTilesWide_ = 25;
        protected int numTilesTall_ = 22;
        protected int maxCursorX;
        protected int maxCursorY;

        public Level myLevel_;
        public string currentFilepath_;

        /// <summary>
        /// The constructor takes an EngineStateInterface to return to when level editing is done
        /// </summary>
        public EngineStateLevelEditor(Engine engine, EngineStateInterface returnState, string filepath, StorageContainer container)
        {
            currentFilepath_ = filepath;

            engine_ = engine;
            engine_.setScreenSize(SCREEN_SIZE_X, SCREEN_SIZE_Y);
            engine_.IsMouseVisible = true;
            returnState_ = returnState;
            enemyIndex_ = 0;
            isObjSelected_ = false;
            typeSelected_ = 0;
            selectedIndex_ = 0;
            myObjects_ = new List<objectRepresentation>(MAX_NUM_ENEMIES);

            myLevel_ = new Level(new Tileset(), null);
            myLevel_.getLevelFromFile(filepath, drawPipeline_);
            if (container != null)
            {
                container.Dispose();
            }

            maxCursorX = numTilesWide_ - 3;
            maxCursorY = numTilesTall_ - 3;
            cursorPosX_ = SCREEN_SIZE_X / 30;
            cursorPosY_ = SCREEN_SIZE_Y / 30;

            // Initialize the camera
            GlobalHelper.getInstance().getCurrentCamera().setScreenWidth((float)SCREEN_SIZE_X);
            GlobalHelper.getInstance().getCurrentCamera().setScreenHeight((float)SCREEN_SIZE_Y);
            GlobalHelper.getInstance().getCurrentCamera().setCenter((float)cursorPosX_ * Tiler.tileSideLength_ - 7.5f, (float)cursorPosY_ * Tiler.tileSideLength_ - 7.5f);

            curTileIndex_ = 0;
            displayTile_ = new TileObject(curTileIndex_, drawPipeline_, TextureMap.getInstance().getTexture("Tile_" + curTileIndex_), new Vector2((float)cursorPosX_ * Tiler.tileSideLength_, (float)cursorPosY_ * Tiler.tileSideLength_), Vector2.Zero, DISP_TILE_DEPTH);
        }

        /// <summary>
        /// Determines how to update the level editor based on user input
        /// </summary>
        /// <param name="gameTime">The GameTime parameter</param>
        /// <returns>Returns itself if user wants to stay in this mode. Returns EngineStateMenu otherwise.</returns>
        public EngineStateInterface update(GameTime gameTime)
        {
            InputSet inputs = InputSet.getInstance();


            for (int i = 0; i < myLevel_.getEnemies().Count; i++)
            {
                if (myLevel_.getEnemies()[i].getDirection() == Vector2.Zero)
                {
                    // I don't think this line should ever get run, but tell
                    // Andrew if the code ever stops on this breakpoint.
                    // The direction vector should be initialized properly elsewhere.
                    myLevel_.getEnemies()[i].setDirection(new Vector2(1.0f, 0.0f));
                }
            }
            // END

            // Check the inputs
            if (inputs.getLeftDirectionalY() < 0)
            {
                inputs.setToggle(InputsEnum.LEFT_DIRECTIONAL);
                if (cursorPosY_ < Constants.MAX_NUM_TILES_Y)
                {
                    cursorPosY_++;
                }
                GlobalHelper.getInstance().getCurrentCamera().setCenter((float)cursorPosX_ * Tiler.tileSideLength_ - 7.5f, (float)cursorPosY_ * Tiler.tileSideLength_ - 7.5f);
            }
            else if (inputs.getLeftDirectionalY() > 0)
            {
                inputs.setToggle(InputsEnum.LEFT_DIRECTIONAL);
                if (cursorPosY_ > Constants.MIN_NUM_TILES_Y)
                {
                    cursorPosY_--;
                }
                GlobalHelper.getInstance().getCurrentCamera().setCenter((float)cursorPosX_ * Tiler.tileSideLength_ - 7.5f, (float)cursorPosY_ * Tiler.tileSideLength_ - 7.5f);
            }
            else if (inputs.getLeftDirectionalX() > 0)
            {
                inputs.setToggle(InputsEnum.LEFT_DIRECTIONAL);
                if (cursorPosX_ < Constants.MAX_NUM_TILES_X)
                {
                    cursorPosX_++;
                }
                GlobalHelper.getInstance().getCurrentCamera().setCenter((float)cursorPosX_ * Tiler.tileSideLength_ - 7.5f, (float)cursorPosY_ * Tiler.tileSideLength_ - 7.5f);
            }
            else if (inputs.getLeftDirectionalX() < 0)
            {
                inputs.setToggle(InputsEnum.LEFT_DIRECTIONAL);
                if (cursorPosX_ > Constants.MIN_NUM_TILES_X)
                {
                    cursorPosX_--;
                }
                GlobalHelper.getInstance().getCurrentCamera().setCenter((float)cursorPosX_ * Tiler.tileSideLength_ - 7.5f, (float)cursorPosY_ * Tiler.tileSideLength_ - 7.5f);
            }
            else if (inputs.getButton(InputsEnum.CANCEL_BUTTON))
            {
                inputs.setToggle(InputsEnum.CANCEL_BUTTON);
                engine_.initializeScreen();
                return new EngineStateLevelSave(engine_, this);
            }
            else if (inputs.getButton(InputsEnum.BUTTON_4))
            {
                engine_.initializeScreen();
                return new EngineStateEditorControls(engine_, this, SCREEN_SIZE_X, SCREEN_SIZE_Y);
            }
            else if (inputs.getButton(InputsEnum.LEFT_BUMPER) && isObjSelected_ == false)
            {
                curTileIndex_ = 0;
                inputs.setToggle(InputsEnum.LEFT_BUMPER);
                curPallette_--;
                if (curPallette_ < 0)
                {
                    curPallette_ = NUM_PALLETTES - 1;
                }
            }
            else if (inputs.getButton(InputsEnum.LEFT_BUMPER) && isObjSelected_ == true)
            {
                inputs.setToggle(InputsEnum.LEFT_BUMPER);
                switch (typeSelected_)
                {

                    case 1:
                        {
                            CharacterAbstract currEnemy = myLevel_.getEnemies()[selectedIndex_];
                            currEnemy.setDirection(CommonFunctions.rotate(currEnemy.getDirection(), (double)(-1 * (Math.PI / 4))));
                            // TODO: AMP Fix it so we don't have to do this next line of code
                            currEnemy.getActuator().update(); // Makes it so the enemies are drawn in the correct position
                            break;
                        }
                    case 2:
                        {
                            //LevelObjectAbstract currItem = myLevel_.getItems()[selectedIndex_];
                            //currItem.setDirection(CommonFunctions.rotate(currItem.getDirection(), (double) (Math.PI/ 4)));
                            // END
                            break;
                        }
                }
            }




            else if (inputs.getButton(InputsEnum.RIGHT_BUMPER) && isObjSelected_ == false)
            {
                curTileIndex_ = 0;
                inputs.setToggle(InputsEnum.RIGHT_BUMPER);
                curPallette_++;
                curPallette_ = curPallette_ % NUM_PALLETTES;
            }

            else if (inputs.getButton(InputsEnum.RIGHT_BUMPER) && isObjSelected_ == true)
            {
             
                inputs.setToggle(InputsEnum.RIGHT_BUMPER);
                switch (typeSelected_)
                {

                    case 1:
                        {
                            CharacterAbstract currEnemy = myLevel_.getEnemies()[selectedIndex_];
                            currEnemy.setDirection(CommonFunctions.rotate(currEnemy.getDirection(), (double)(Math.PI / 4)));
                            // TODO: AMP Fix it so we don't have to do this next line of code
                            currEnemy.getActuator().update(); // Makes it so the enemies are drawn in the correct position
                            break;
                        }
                    case 2:
                        {
                            //LevelObjectAbstract currItem = myLevel_.getItems()[selectedIndex_];
                            //currItem.setDirection(CommonFunctions.rotate(currItem.getDirection(), (double) (Math.PI/ 4)));
                            // END
                            break;
                        }
                }
            }

            else if (inputs.getButton(InputsEnum.CONFIRM_BUTTON))
            {
                inputs.setToggle(InputsEnum.CONFIRM_BUTTON);
                switch (curPallette_)
                {
                    case (int)pallette_.tile:
                        {
                            // Edit the Level
                            myLevel_.changeTile(curTileIndex_, cursorPosX_, cursorPosY_, drawPipeline_);
                            break;
                        }
                    case (int)pallette_.enemy:
                        {
                            //to do, add enemy on cursor position
                            Vector2 pos = new Vector2((float)cursorPosX_ * Tiler.tileSideLength_, (float)cursorPosY_ * Tiler.tileSideLength_);
                            DummyEnemy dumE = new DummyEnemy(drawPipeline_, pos);
                            // TODO: AMP Fix it so we don't have to do this next line of code
                            dumE.getActuator().update(); // Makes it so the enemies are drawn in the correct position

                            myLevel_.getEnemies().Add(dumE);

                            break;
                        }
                    case (int)pallette_.misc:
                        {
                            Vector2 mousePos = new Vector2((float)cursorPosX_ * Tiler.tileSideLength_, (float)cursorPosY_ * Tiler.tileSideLength_);
                            if (mousePos.X <= Constants.MAX_NUM_TILES_X * Tiler.tileSideLength_
                                   && mousePos.X > Constants.MIN_NUM_TILES_X * Tiler.tileSideLength_
                                   && mousePos.Y <= Constants.MAX_NUM_TILES_Y * Tiler.tileSideLength_
                                   && mousePos.Y > Constants.MIN_NUM_TILES_Y * Tiler.tileSideLength_)
                            {
                                
                                switch (curTileIndex_)
                                {
                                    case 0:
                                        {
                                            AmmoBox myAmmo = new AmmoBox(null, drawPipeline_, mousePos, new Vector2(1.0f, 0.0f), 0.2f);
                                            myLevel_.getItems().Add(myAmmo);
                                            break;
                                        }
                                    case 1:
                                        {
                                            HealthBox myHealth = new HealthBox(null, drawPipeline_, mousePos, new Vector2(1.0f, 0.0f), 0.2f);
                                            myLevel_.getItems().Add(myHealth);
                                            break;
                                        }
                                    case 2:
                                        {
                                            myLevel_.setPlayerStartLocation(mousePos);

                                            break;
                                        }
                                }
                                
                            }
                            break;
                        }
                }
            }
            else if (inputs.getButton(InputsEnum.RIGHT_TRIGGER) && isObjSelected_ == false)
            {
                if (curPallette_ != (int)pallette_.tile)
                {
                    inputs.setToggle(InputsEnum.RIGHT_TRIGGER);
                }

                Vector2 rightD = new Vector2(inputs.getRightDirectionalX(), inputs.getRightDirectionalY());
                Vector2 camOffset = new Vector2(GlobalHelper.getInstance().getCurrentCamera().getPosition().X, GlobalHelper.getInstance().getCurrentCamera().getPosition().Y);
                Vector2 mousePos;
                if (Settings.getInstance().IsUsingMouse_)
                {
                    mousePos = new Vector2(rightD.X + camOffset.X, rightD.Y + camOffset.Y);
                }
                else
                {

                    mousePos = new Vector2((float)cursorPosX_ * Tiler.tileSideLength_, (float)cursorPosY_ * Tiler.tileSideLength_);
                }

                switch (curPallette_)
                {
                    case (int)pallette_.tile:
                        {

                            if (mousePos.X <= Constants.MAX_NUM_TILES_X * Tiler.tileSideLength_
                            && mousePos.X > Constants.MIN_NUM_TILES_X * Tiler.tileSideLength_
                            && mousePos.Y <= Constants.MAX_NUM_TILES_Y * Tiler.tileSideLength_
                            && mousePos.Y > Constants.MIN_NUM_TILES_Y * Tiler.tileSideLength_
                            && rightD.Y < HUD_BAR_DRAW_Y)
                            {
                                int myX = (int)(mousePos.X) / 15;
                                int myY = (int)(mousePos.Y) / 15;

                                // Edit the Level
                                myLevel_.changeTile(curTileIndex_, myX, myY, drawPipeline_);
                            }
                            break;
                        }
                    case (int)pallette_.enemy:
                        {
                            if (mousePos.X <= Constants.MAX_NUM_TILES_X * Tiler.tileSideLength_
                               && mousePos.X > Constants.MIN_NUM_TILES_X * Tiler.tileSideLength_
                               && mousePos.Y <= Constants.MAX_NUM_TILES_Y * Tiler.tileSideLength_
                               && mousePos.Y > Constants.MIN_NUM_TILES_Y * Tiler.tileSideLength_
                               && rightD.Y < HUD_BAR_DRAW_Y)
                            {
                                // ERIC
                                //Vector2 defRotation = new Vector2(1.0f, 0.0f);
                                //objectRepresentation newObject = new objectRepresentation(DUMMY_ENEMY, rightD, defRotation, 0.2f);

                                // AMP
                                DummyEnemy dumE = new DummyEnemy(drawPipeline_, mousePos);
                                // TODO: AMP Fix it so we don't have to do this next line of code
                                dumE.getActuator().update(); // Makes it so the enemies are drawn in the correct position

                                myLevel_.getEnemies().Add(dumE);
                                // END
                            }
                            break;
                        }
                    case (int)pallette_.misc:
                        {
                            if (mousePos.X <= Constants.MAX_NUM_TILES_X * Tiler.tileSideLength_
                                   && mousePos.X > Constants.MIN_NUM_TILES_X * Tiler.tileSideLength_
                                   && mousePos.Y <= Constants.MAX_NUM_TILES_Y * Tiler.tileSideLength_
                                   && mousePos.Y > Constants.MIN_NUM_TILES_Y * Tiler.tileSideLength_)
                            {
                                switch (curTileIndex_)
                                {
                                    case 0:
                                        {
                                            AmmoBox myAmmo = new AmmoBox(null, drawPipeline_, mousePos, new Vector2(1.0f, 0.0f), 0.2f);
                                            myLevel_.getItems().Add(myAmmo);
                                            break;
                                        }
                                    case 1:
                                        {
                                            HealthBox myHealth = new HealthBox(null, drawPipeline_, mousePos, new Vector2(1.0f, 0.0f), 0.2f);
                                            myLevel_.getItems().Add(myHealth);
                                            break;
                                        }
                                    case 2:
                                        {
                                            myLevel_.setPlayerStartLocation(mousePos);

                                            break;
                                        }
                                }
                            }
                            break;
                        }
                }
            }
            else if (inputs.getButton(InputsEnum.RIGHT_TRIGGER) && isObjSelected_)
            {
                Vector2 rightD = new Vector2(inputs.getRightDirectionalX(), inputs.getRightDirectionalY());
                Vector2 camOffset = new Vector2(GlobalHelper.getInstance().getCurrentCamera().getPosition().X, GlobalHelper.getInstance().getCurrentCamera().getPosition().Y);
                Vector2 mousePos;
                if (Settings.getInstance().IsUsingMouse_)
                {
                    mousePos = new Vector2(rightD.X + camOffset.X, rightD.Y + camOffset.Y);
                }
                else
                {
                    mousePos = new Vector2((float)cursorPosX_ * Tiler.tileSideLength_, (float)cursorPosY_ * Tiler.tileSideLength_);
                }
                if (mousePos.X <= Constants.MAX_NUM_TILES_X * Tiler.tileSideLength_
                    && mousePos.X > Constants.MIN_NUM_TILES_X * Tiler.tileSideLength_
                    && mousePos.Y <= Constants.MAX_NUM_TILES_Y * Tiler.tileSideLength_
                    && mousePos.Y > Constants.MIN_NUM_TILES_Y * Tiler.tileSideLength_
                    && rightD.Y < HUD_BAR_DRAW_Y)
                {
                    switch (typeSelected_)
                    {
                        case 1:
                            {
                                myLevel_.getEnemies()[selectedIndex_].setPosition(mousePos);
                                // TODO: AMP Fix it so we don't have to do this next line of code
                                myLevel_.getEnemies()[selectedIndex_].getActuator().update(); // Makes it so the enemies are drawn in the correct position
                                break;
                            }
                        case 2:
                            {
                                myLevel_.getItems()[selectedIndex_].setPosition(mousePos);

                                break;
                            }
                    }
                }
                // END
            }
            else if (inputs.getButton(InputsEnum.BUTTON_3) && isObjSelected_)
            {
                // ERIC
                //myObjects_.RemoveAt(selectedIndex_);
                // AMP
                drawPipeline_.Remove(myLevel_.getEnemies()[selectedIndex_]);
                myLevel_.getEnemies().RemoveAt(selectedIndex_);
                // END
                isObjSelected_ = false;
                selectedIndex_ = 0;

            }
            else if (inputs.getButton(InputsEnum.LEFT_TRIGGER))
            {
                inputs.setToggle(InputsEnum.LEFT_TRIGGER);
                pickTile();
                if (isObjSelected_)
                {
                    isObjSelected_ = false;
                    selectedIndex_ = 0;
                    typeSelected_ = 0;
                }
                else
                {
                    bool foundObj = false;
                    Vector2 rightD = new Vector2(inputs.getRightDirectionalX(), inputs.getRightDirectionalY());

                    Vector2 camOffset = new Vector2(GlobalHelper.getInstance().getCurrentCamera().getPosition().X, GlobalHelper.getInstance().getCurrentCamera().getPosition().Y);
                    Vector2 mousePos;
                    if (Settings.getInstance().IsUsingMouse_)
                    {
                        mousePos = new Vector2(rightD.X + camOffset.X, rightD.Y + camOffset.Y);
                    }
                    else
                    {
                        mousePos = new Vector2((float)cursorPosX_ * Tiler.tileSideLength_, (float)cursorPosY_ * Tiler.tileSideLength_);
                    }
                    List<CharacterAbstract> myEnemies = myLevel_.getEnemies();
                    for (int i = 0; i < myEnemies.Count && foundObj == false; i++)
                    {
                        if (mousePos.Y >= myEnemies[i].getPosition().Y - Tiler.tileSideLength_ && mousePos.Y < myEnemies[i].getPosition().Y + Tiler.tileSideLength_
                            && mousePos.X >= myEnemies[i].getPosition().X - Tiler.tileSideLength_ && mousePos.X < myEnemies[i].getPosition().X + Tiler.tileSideLength_)
                        {
                            typeSelected_ = 1;
                            selectedIndex_ = i;
                            isObjSelected_ = true;
                            foundObj = true;
                        }
                    }
                    if (foundObj == false)
                    {
                        List<LevelObjectAbstract> myItems = myLevel_.getItems();
                        for (int i = 0; i < myItems.Count && foundObj == false; i++)
                        {
                            if (mousePos.Y >= myItems[i].getPosition().Y - Tiler.tileSideLength_ && mousePos.Y < myItems[i].getPosition().Y + Tiler.tileSideLength_
                                  && mousePos.X >= myItems[i].getPosition().X - Tiler.tileSideLength_ && mousePos.X < myItems[i].getPosition().X + Tiler.tileSideLength_)
                            {
                                typeSelected_ = 2;
                                selectedIndex_ = i;
                                isObjSelected_ = true;
                                foundObj = true;
                            }
                        }
                    }
                    // END
                }


            }
            else if (inputs.getButton(InputsEnum.BUTTON_1))
            {
                switch (curPallette_)
                {
                    case (int)pallette_.tile:
                        {
                            inputs.setToggle(InputsEnum.BUTTON_1);
                            curTileIndex_ = (curTileIndex_ + 1) % NUM_TILES;
                            break;
                        }
                    case (int)pallette_.enemy:
                        {
                            break;
                        }
                    case (int)pallette_.misc:
                        {
                            inputs.setToggle(InputsEnum.BUTTON_1);
                            curTileIndex_ = (curTileIndex_ + 1) % NUM_MISC;
                            break;
                        }
                }
            }

            else if (inputs.getButton(InputsEnum.BUTTON_2))
            {
                inputs.setToggle(InputsEnum.BUTTON_2);


                switch (curPallette_)
                {
                    case (int)pallette_.tile:
                        {
                            inputs.setToggle(InputsEnum.BUTTON_1);
                            if (curTileIndex_ == 0)
                            {
                                curTileIndex_ = NUM_TILES;
                            }
                            curTileIndex_ = curTileIndex_ - 1;
                            break;
                        }
                    case (int)pallette_.enemy:
                        {
                            break;
                        }
                    case (int)pallette_.misc:
                        {
                            inputs.setToggle(InputsEnum.BUTTON_1);
                            if (curTileIndex_ == 0)
                            {
                                curTileIndex_ = NUM_MISC;
                            }
                            curTileIndex_ = curTileIndex_ - 1;
                            break;
                        }
                }



            }

            drawPipeline_.Remove(displayTile_);
            displayTile_ = new TileObject(curTileIndex_, drawPipeline_, TextureMap.getInstance().getTexture("Tile_" + curTileIndex_), new Vector2((float)cursorPosX_ * Tiler.tileSideLength_, (float)cursorPosY_ * Tiler.tileSideLength_), Vector2.Zero, DISP_TILE_DEPTH);

            return this;
        }
        

        public void moveObject(ref objectRepresentation or, Vector2 pos)
        {
            or.objPos_ = pos;
        }
        public void pickTile()
        {
            InputSet inputs = InputSet.getInstance();
            Vector2 camOffset = new Vector2(GlobalHelper.getInstance().getCurrentCamera().getPosition().X, GlobalHelper.getInstance().getCurrentCamera().getPosition().Y);
            Vector2 rightD = new Vector2(inputs.getRightDirectionalX(), inputs.getRightDirectionalY());
            Vector2 mousePos = new Vector2(rightD.X + camOffset.X, rightD.Y + camOffset.Y);
            switch (curPallette_)
            {
                case (int)pallette_.tile:
                    {
                        bool foundTile = false;
                        for (int i = 0; i < NUM_TILES && foundTile == false; i++)
                        {
                            
                            Vector2 tilePos = new Vector2(((10.0f + i * 20.0f) % 365.0f + camOffset.X), (((((10 + i * 20) / 365) * 20.0f) + 335.0f) + camOffset.Y));
                            if (mousePos.Y >= tilePos.Y - Tiler.tileSideLength_ && mousePos.Y < tilePos.Y + Tiler.tileSideLength_
                                  && mousePos.X >= tilePos.X - Tiler.tileSideLength_ && mousePos.X < tilePos.X + Tiler.tileSideLength_)
                                {
                                    curTileIndex_ = i;
                                    foundTile = true;
                                }

                        }
                        break;
                       }
                case (int)pallette_.misc:
                    {
                        bool foundTile = false;
                        for (int i = 0; i < NUM_TILES && foundTile == false; i++)
                        {

                            Vector2 tilePos = new Vector2(((10.0f + i * 35.0f) % 365.0f + camOffset.X), (((((10 + i * 35.0f) / 365) * 20.0f) + 335.0f) + camOffset.Y));
                            if (mousePos.Y >= tilePos.Y - Tiler.tileSideLength_ && mousePos.Y < tilePos.Y + Tiler.tileSideLength_
                                  && mousePos.X >= tilePos.X - Tiler.tileSideLength_ && mousePos.X < tilePos.X + Tiler.tileSideLength_)
                            {
                                curTileIndex_ = i;
                                foundTile = true;
                            }

                        }
                        break;
                    }
            }
        }


        
        /// <summary>
        /// Draws the level that is currently being edited
        /// </summary>
        public void draw()
        {
            InputSet inputs = InputSet.getInstance();
            engine_.GraphicsDevice.Clear(Color.Firebrick);

            // Draw all the DrawableObjectAbstracts in our pipeline
            for (int i = drawPipeline_.Count - 1; i >= 0; i--)
            {
                drawPipeline_[i].draw(null);
            }
            string STR_HELP_TEXT = "PRESS " + inputs.getControlName(InputsEnum.BUTTON_4) + " FOR CONTROLS";
            FontMap.getInstance().getFont(FontEnum.PescaderoBold).drawStringCentered(STR_HELP_TEXT,
                new Vector2(120.0f, 15.0f),
                Color.Green,
                0.0f,
                0.9f);


            displayTile_.draw(new GameTime());

            TextureMap.getInstance().getTexture("TileHighlight").drawImage(0, new Vector2((cursorPosX_ *Tiler.tileSideLength_ - 1 ), (cursorPosY_ * Tiler.tileSideLength_ - 1)), 0.2f);

            TextureMap.fetchTexture("PlayerStartPos").drawImage(0, myLevel_.getPlayerStartLocation(), 0.3f);
            
            
            if (isObjSelected_)
            {
                Vector2 highlightPos = new Vector2();
                switch (typeSelected_)
                {
                    case 1:
                        {
                            highlightPos = myLevel_.getEnemies()[selectedIndex_].getPosition();
                            break;
                        }
                    case 2:
                        {
                            highlightPos = myLevel_.getItems()[selectedIndex_].getPosition();
                            break;
                        }
                }  
                        TextureMap.getInstance().getTexture("TileHighlight").drawImage(0, highlightPos, 0.2f);
            }
            
            
     
            // Draw the palette

            Vector2 camOffset = new Vector2(GlobalHelper.getInstance().getCurrentCamera().getPosition().X, GlobalHelper.getInstance().getCurrentCamera().getPosition().Y);

            TextureMap.getInstance().getTexture("blank").drawImageWithDimAbsolute(0, new Rectangle(HUD_BAR_DRAW_X, HUD_BAR_DRAW_Y, SCREEN_SIZE_X, HUD_BAR_HEIGHT), Constants.DEPTH_HUD - 0.01f, Color.Azure);


            
            switch (curPallette_)
            {
                
                case (int)pallette_.tile:
                    for (int i = 0; i < NUM_TILES; i++)
                    {

                        TextureMap.getInstance().getTexture("Tile_" + i).drawImage(0, new Vector2(((10.0f + i * 20.0f) % 365.0f + camOffset.X), ((((10 + i * 20) / 365) * 20.0f) + 335.0f) + camOffset.Y), 0.2f);

                        TextureMap.getInstance().getTexture("Tile_" + i).drawImageAbsolute(0, new Vector2((10.0f + i * 20.0f) % 365.0f, (((10 + i * 20) / 365) * 20.0f) + 335.0f), Constants.DEPTH_HUD);

                        if(i == curTileIndex_)


                            TextureMap.getInstance().getTexture("TileHighlight").drawImageAbsolute(0, new Vector2((10.0f + i * 20.0f) % 365.0f, (((10 + i * 20) / 365) * 20.0f) + 335.0f), Constants.DEPTH_HUD);

                    }
                    break;
                case (int)pallette_.enemy:
                    {

                        TextureMap.getInstance().getTexture("basic_enemy_walk").drawImage(0, new Vector2(10.0f + camOffset.X, (((10* 20) / 365) * 20.0f) + 335.0f + camOffset.Y), 0.2f);

                        TextureMap.getInstance().getTexture("basic_enemy_walk").drawImageAbsolute(0, new Vector2(10.0f, (((10 * 20) / 365) * 20.0f) + 335.0f), Constants.DEPTH_HUD);

                        break;
                    }
                case (int)pallette_.misc:
                    {
                        int i = 0;
                        TextureMap.fetchTexture("AmmoBox").drawImageAbsolute(0, new Vector2(10.0f + 35.0f * i , (((10 * 20) / 365) * 20.0f) + 335.0f ), Constants.DEPTH_HUD);
                        i++;
                        TextureMap.fetchTexture("HealthBox").drawImageAbsolute(0, new Vector2(10.0f + 35.0f * i , (((10 * 20) / 365) * 20.0f) + 335.0f ), Constants.DEPTH_HUD);
                        i++;
                        TextureMap.fetchTexture("PlayerStartPos").drawImageAbsolute(0, new Vector2(10.0f + 35.0f * i, (((10 * 20) / 365) * 20.0f) + 335.0f ), Constants.DEPTH_HUD);
                        TextureMap.fetchTexture("TileHighlight").drawImageAbsolute(0, new Vector2(10.0f + 35.0f * curTileIndex_, (((10 * 20) / 365) * 20.0f) + 335.0f ), Constants.DEPTH_HUD);
                        break;
                    }
            }
        }
    }
}
