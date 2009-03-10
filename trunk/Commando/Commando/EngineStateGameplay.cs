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
using Commando.controls;
using Commando.levels;
using Commando.objects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Commando.collisiondetection;
using Commando.ai;

namespace Commando
{
    /// <summary>
    /// The state of the engine when the player is moving around
    /// in levels, fighting enemies, etc.
    /// </summary>
    public class EngineStateGameplay : EngineStateInterface
    {
        const int SCREEN_SIZE_X = 375;
        const int SCREEN_SIZE_Y = 375;
        const float HEALTH_BAR_POS_X = 100.0f;
        const float HEALTH_BAR_POS_Y = 350.0f;
        const float WEAPON_ICON_POS_X = 200.0f;
        const float WEAPON_ICON_POS_Y = 350.0f;
        const float HEALTH_TEXT_OFFSET_X = -27.0f;
        const float HEALTH_TEXT_OFFSET_Y = -12.0f;
        const float AMMO_TEXT_POS_X = 300.0f;
        const float AMMO_TEXT_POS_Y = 350.0f;
        const string HEALTH_BAR_OUTLINE_TEX_NAME = "healthBarOutline";
        const string HEALTH_BAR_FILL_TEX_NAME = "healthBarFiller";
        const string WEAPON_TEX_NAME = "pistol";
        const string HEALTH_TEXT = "Health";
        const string AMMO_TEXT = "%i/20 bullets";
        const string AMMO_REPLACE_TEXT = "%i";
        const string SAVE_PATH = "user level.xml";
        const float HUD_DRAW_DEPTH = Constants.DEPTH_HUD;
        const float FONT_DRAW_DEPTH = Constants.DEPTH_HUD_TEXT;
        const int HUD_BAR_DRAW_Y = SCREEN_SIZE_Y - HUD_BAR_HEIGHT;
        const int HUD_BAR_DRAW_X = 0;
        const int HUD_BAR_HEIGHT = 45;

        //Jared's test stuff
        //protected MainPlayer player_;
        protected ActuatedMainPlayer player_;
        protected List<CharacterAbstract> enemyList_ = new List<CharacterAbstract>();
        protected CollisionDetectorInterface collisionDetector_;
        protected List<DrawableObjectAbstract> drawPipeline_ = new List<DrawableObjectAbstract>();
        //END Jared's test stuff

        protected Engine engine_;
        protected HeadsUpDisplayObject healthBar_;
        protected HeadsUpDisplayWeapon weapon_;
        protected HeadsUpDisplayText ammo_;
        protected List<TileObject> tiles_;
        protected Vector2 healthBarPos_;
        protected Vector2 weaponIconPos_;
        protected Vector2 healthTextPos_;
        protected Vector2 ammoTextPos_;


        /// <summary>
        /// Constructs a state of gameplay
        /// </summary>
        /// <param name="engine">Reference to the engine running the state</param>
        public EngineStateGameplay(Engine engine)
        {
            // Cleanup singletons used by prior EngineStateGameplay
            Commando.ai.WorldState.reset();

            // Perform initializations of variables
            engine_ = engine;
            engine_.setScreenSize(SCREEN_SIZE_X, SCREEN_SIZE_Y);

            //Jared's test stuff
            player_ = new ActuatedMainPlayer(drawPipeline_);
            GlobalHelper.getInstance().getCurrentCamera().setPosition(0,0);
            GlobalHelper.getInstance().getCurrentCamera().setScreenWidth((float)SCREEN_SIZE_X);
            GlobalHelper.getInstance().getCurrentCamera().setScreenHeight((float)SCREEN_SIZE_Y);
            //List<BoxObject> boxesToBeAdded = new List<BoxObject>();
            Tile[,] tilesForGrid;
            List<Vector2> tileBox = new List<Vector2>();
            tileBox.Add(new Vector2(-7.5f, -7.5f));
            tileBox.Add(new Vector2(7.5f, -7.5f));
            tileBox.Add(new Vector2(7.5f, 7.5f));
            tileBox.Add(new Vector2(-7.5f, 7.5f));
            int[,] tiles = new int[,]   {{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
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
            // Load the user's level from XML
            //BoxObject[,] boxesToBeAdded;
            bool[,] boxesToBeAdded;
            List<BoxObject> boxesToBeAddedForReal = new List<BoxObject>();
            try
            {
                System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
                doc.Load(SAVE_PATH);

                // First load the tiles
                System.Xml.XmlElement ele = (System.Xml.XmlElement)doc.GetElementsByTagName("level")[0];
                int[,] loadedTiles = new int[Convert.ToInt32(ele.GetAttribute("numTilesTall")), Convert.ToInt32(ele.GetAttribute("numTilesWide"))];
                boxesToBeAdded = new bool[Convert.ToInt32(ele.GetAttribute("numTilesTall")), Convert.ToInt32(ele.GetAttribute("numTilesWide"))];
                tilesForGrid = new Tile[Convert.ToInt32(ele.GetAttribute("numTilesTall")), Convert.ToInt32(ele.GetAttribute("numTilesWide"))];
                System.Xml.XmlNodeList tList = doc.GetElementsByTagName("tile");
                for (int i = 0; i < Convert.ToInt32(ele.GetAttribute("numTilesTall")); i++)
                {
                    for (int j = 0; j < Convert.ToInt32(ele.GetAttribute("numTilesWide")); j++)
                    {
                        System.Xml.XmlElement ele2 = (System.Xml.XmlElement)tList[j + i * Convert.ToInt32(ele.GetAttribute("numTilesWide"))];
                        loadedTiles[i, j] = Convert.ToInt32(ele2.GetAttribute("index"));
                        if (loadedTiles[i, j] != 1)
                        {
                            //boxesToBeAdded[i, j] = new BoxObject(tileBox, new Vector2((float)j * 15f + 7.5f, (float)i * 15f + 7.5f));
                            boxesToBeAdded[i, j] = true;
                            tilesForGrid[i, j].highDistance_ = 0;
                            tilesForGrid[i, j].lowDistance_ = 0;
                        }
                        else
                        {
                            //boxesToBeAdded[i, j] = null;
                            boxesToBeAdded[i, j] = false;
                            tilesForGrid[i, j].highDistance_ = 1;
                            tilesForGrid[i, j].lowDistance_ = 1;
                        }
                    }
                }
                tiles_ = Tiler.getTiles(drawPipeline_, loadedTiles);

                boxesToBeAddedForReal = Tiler.mergeBoxes(tilesForGrid);
                GlobalHelper.getInstance().setCurrentLevelTileGrid(new TileGrid(tilesForGrid));

                // Now load the enemies
                tList = doc.GetElementsByTagName("enemy");
                for (int i = 0; i < Convert.ToInt32(tList.Count); i++)
                {
                    System.Xml.XmlElement ele2 = (System.Xml.XmlElement)tList[i];
                    DummyEnemy dum = new DummyEnemy(drawPipeline_, new Vector2((float)Convert.ToInt32(ele2.GetAttribute("posX")), (float)Convert.ToInt32(ele2.GetAttribute("posY"))));
                    enemyList_.Add(dum);
                }
            }
            catch (Exception)
            {
                int numTilesTall = 22;
                int numTilesWide = 25;
                boxesToBeAdded = new bool[numTilesTall, numTilesWide];
                tilesForGrid = new Tile[numTilesTall, numTilesWide];
                for (int i = 0; i < numTilesTall; i++)
                {
                    for (int j = 0; j < numTilesWide; j++)
                    {
                        if (tiles[i, j] != 1)
                        {
                            boxesToBeAdded[i, j] = true;
                            tilesForGrid[i, j].highDistance_ = 0;
                            tilesForGrid[i, j].lowDistance_ = 0;
                        }
                        else
                        {
                            boxesToBeAdded[i, j] = false;
                            tilesForGrid[i, j].highDistance_ = 1;
                            tilesForGrid[i, j].lowDistance_ = 1;
                        }
                    }
                }
                tiles_ = Tiler.getTiles(drawPipeline_, tiles);

                boxesToBeAddedForReal = Tiler.mergeBoxes(tilesForGrid);
                GlobalHelper.getInstance().setCurrentLevelTileGrid(new TileGrid(tilesForGrid));

                // Now add an enemy
                DummyEnemy dumE = new DummyEnemy(drawPipeline_, new Vector2(250.0f, 250.0f));
                enemyList_.Add(dumE);
            }
            // Done loading level from XML
            
            
            //collisionDetector_ = new CollisionDetector(polygons);
            collisionDetector_ = new SeparatingAxisCollisionDetector();

            foreach (BoxObject boxOb in boxesToBeAddedForReal)
            {
                //boxOb.getBounds().rotate(new Vector2(1.0f, 0.0f), boxOb.getPosition());
                collisionDetector_.register(boxOb);
            }
            //END Jared's test stuff

            healthBarPos_ = new Vector2(HEALTH_BAR_POS_X, HEALTH_BAR_POS_Y);
            weaponIconPos_ = new Vector2(WEAPON_ICON_POS_X, WEAPON_ICON_POS_Y);
            healthTextPos_ = new Vector2(HEALTH_BAR_POS_X + HEALTH_TEXT_OFFSET_X, HEALTH_BAR_POS_Y + HEALTH_TEXT_OFFSET_Y);
            ammoTextPos_ = new Vector2(AMMO_TEXT_POS_X, AMMO_TEXT_POS_Y);
            healthBar_ = new HeadsUpDisplayObject(drawPipeline_, TextureMap.getInstance().getTexture(HEALTH_BAR_FILL_TEX_NAME), healthBarPos_, Vector2.Zero, HUD_DRAW_DEPTH);
            weapon_ = new HeadsUpDisplayWeapon(drawPipeline_,  TextureMap.getInstance().getTexture(WEAPON_TEX_NAME), weaponIconPos_, Vector2.Zero, HUD_DRAW_DEPTH);
            ammo_ = new HeadsUpDisplayText(ammoTextPos_, FONT_DRAW_DEPTH, FontEnum.Kootenay);
            player_.getHealth().addObserver(healthBar_);
            player_.getWeapon().addObserver(weapon_);
            player_.getAmmo().addObserver(ammo_);
            player_.setCollisionDetector(collisionDetector_);
            for (int i = 0; i < enemyList_.Count; i++)
            {
                enemyList_[i].setCollisionDetector(collisionDetector_);
            }

            WorldState.EnemyList_ = (List<CharacterAbstract>)enemyList_;
            WorldState.MainPlayer_ = player_;
        }

        #region EngineStateInterface Members

        /// <summary>
        /// Handles input and moves all characters and objects forward one
        /// frame in time
        /// </summary>
        /// <param name="gameTime">GameTime parameter</param>
        /// <returns>The state of the game for the next frame</returns>
        public EngineStateInterface update(GameTime gameTime)
        {
            InputSet inputs = engine_.getInputs();

            // Check whether to enter pause screen
            if (inputs.getButton(InputsEnum.CONFIRM_BUTTON) || inputs.getButton(InputsEnum.CANCEL_BUTTON))
            {
                inputs.setToggle(InputsEnum.CONFIRM_BUTTON);
                inputs.setToggle(InputsEnum.CANCEL_BUTTON);
                return new EngineStatePause(engine_, this);
            }

            // Enter debug mode at this breakpoint to diagnose problems
            #if !XBOX
            if (Keyboard.GetState().IsKeyDown(Keys.F12))
            {
                System.Console.WriteLine("Debugging Started Here");
            }
            #endif

            // Pass input set to player
            player_.setInputSet(inputs);

            // Update all of the objects in the drawing pipeline
            for (int i = drawPipeline_.Count - 1; i >= 0; i--)
            {
                drawPipeline_[i].update(null);
                if (drawPipeline_[i].isDead())
                {
                    drawPipeline_.RemoveAt(i);
                }
            }
            // TODO
            // may want to maintain a separate pipeline for objects whose
            //  update function doesn't do anything (aka tiles)

            return this;
        }

        /// <summary>
        /// Draws the character, levels, etc.
        /// </summary>
        public void draw()
        {
            engine_.GraphicsDevice.Clear(Color.DarkOrange);

            // Draw Debug Lines
            if (Settings.getInstance().DebugMode_)
                (collisionDetector_ as SeparatingAxisCollisionDetector).draw();
            
            // Draw Laser Pointer at Cursor Position if using a mouse
            #if !XBOX
            if (Settings.getInstance().UsingMouse_)
            {
                MouseState ms = Mouse.GetState();
                Vector2 mpos = new Vector2(ms.X, ms.Y) - new Vector2(2.5f,2.5f);
                TextureDrawer td = TextureMap.fetchTexture("laserpointer")
                    .getDrawer(mpos, Constants.DEPTH_LASER);
                td.Color = Color.Green;
                td.CoordinateType = CoordinateTypeEnum.ABSOLUTE;
                td.draw();
            }
            #endif

            // Draw all the DrawableObjectAbstracts in our pipeline
            for (int i = drawPipeline_.Count - 1; i >= 0; i--)
            {
                drawPipeline_[i].draw(null);
            }

            // TODO
            // Clean up this section -
            //  Most likely, the HUD should be a single object with a .draw()
            //  or the individual pieces should be in the pipeline.
            /* begin section */
            healthBar_.draw(new GameTime());
            TextureMap.getInstance().getTexture(HEALTH_BAR_OUTLINE_TEX_NAME).drawImageAbsolute(0, healthBarPos_, 0.0f, HUD_DRAW_DEPTH);
            weapon_.draw(new GameTime());
            TextureMap.getInstance().getTexture("blank").drawImageWithDimAbsolute(0, new Rectangle(HUD_BAR_DRAW_X, HUD_BAR_DRAW_Y, SCREEN_SIZE_X, HUD_BAR_HEIGHT), HUD_DRAW_DEPTH - 0.01f, Color.Silver);
            FontMap.getInstance().getFont(FontEnum.Kootenay).drawString(HEALTH_TEXT, healthTextPos_, Color.Black, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, FONT_DRAW_DEPTH);
            //FontMap.getInstance().getFont(FontEnum.Kootenay).drawString(AMMO_TEXT, ammoTextPos_, Color.Black, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, FONT_DRAW_DEPTH);
            ammo_.drawString(AMMO_TEXT, AMMO_REPLACE_TEXT, Color.Black, 0.0f);
            /* end section*/
        }

        #endregion
    }
}
