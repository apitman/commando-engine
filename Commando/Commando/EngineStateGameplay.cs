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
        const string HEALTH_BAR_OUTLINE_TEX_NAME = "healthBarOutline";
        const string HEALTH_BAR_FILL_TEX_NAME = "healthBarFiller";
        const string WEAPON_TEX_NAME = "pistol";
        const string HEALTH_TEXT = "Health";
        const string AMMO_TEXT = "%i/20 bullets";
        const string AMMO_REPLACE_TEXT = "%i";
        const string SAVE_PATH = "user level.xml";
        const float HUD_DRAW_DEPTH = Constants.DEPTH_HUD;
        const float FONT_DRAW_DEPTH = Constants.DEPTH_HUD_TEXT;

        #region HUD POSITIONING CALCULATIONS AND CONSTANTS
        protected int HUD_BAR_DRAW_X
        {
            get
            {
                Rectangle r = engine_.GraphicsDevice.Viewport.TitleSafeArea;
                return (r.X);
            }

            set
            {
                throw new NotImplementedException();
            }
        }
        protected int HUD_BAR_DRAW_Y
        {
            get
            {
                Rectangle r = engine_.GraphicsDevice.Viewport.TitleSafeArea;
                return (r.Y + r.Height - HUD_BAR_HEIGHT);
            }

            set
            {
                throw new NotImplementedException();
            }
        }
        protected int HUD_BAR_WIDTH
        {
            get
            {
                Rectangle r = engine_.GraphicsDevice.Viewport.TitleSafeArea;
                return (r.Width);
            }

            set
            {
                throw new NotImplementedException();
            }
        }
        const int HUD_BAR_HEIGHT = 45;

        protected Vector2 HEALTH_BAR_POSITION
        {
            get
            {
                return new Vector2(HUD_BAR_DRAW_X + 100.0f, HUD_BAR_DRAW_Y + 20.0f);
            }

            set
            {
                throw new NotImplementedException();
            }
        }
        protected Vector2 HEALTH_TEXT_POSITION
        {
            get
            {
                return new Vector2(HUD_BAR_DRAW_X + 100.0f, HUD_BAR_DRAW_Y + 20.0f);
            }

            set
            {
                throw new NotImplementedException();
            }
        }
        protected Vector2 WEAPON_ICON_POSITION
        {
            get
            {
                return new Vector2(HUD_BAR_DRAW_X + HUD_BAR_WIDTH - 250.0f, HUD_BAR_DRAW_Y + 15.0f);
            }

            set
            {
                throw new NotImplementedException();
            }
        }
        protected Vector2 AMMO_TEXT_POSITION
        {
            get
            {
                return new Vector2(HUD_BAR_DRAW_X + HUD_BAR_WIDTH - 150.0f, HUD_BAR_DRAW_Y + 20.0f);
            }

            set
            {
                throw new NotImplementedException();
            }
        }
        const float HEALTH_TEXT_OFFSET_X = -27.0f;
        const float HEALTH_TEXT_OFFSET_Y = -12.0f;
        #endregion

        //Jared's test stuff
        //protected MainPlayer player_;
        protected ActuatedMainPlayer player_;
        protected List<CharacterAbstract> enemyList_ = new List<CharacterAbstract>();
        protected CollisionDetectorInterface collisionDetector_;
        protected List<DrawableObjectAbstract> drawPipeline_;
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

        protected Level myLevel_;

        /// <summary>
        /// Constructs a state of gameplay
        /// </summary>
        /// <param name="engine">Reference to the engine running the state</param>
        public EngineStateGameplay(Engine engine)
        {
            SoundEngine.getInstance().Music.Stop(Microsoft.Xna.Framework.Audio.AudioStopOptions.AsAuthored);

            // Perform initializations of variables
            engine_ = engine;
            //engine_.setScreenSize(SCREEN_SIZE_X, SCREEN_SIZE_Y);

            GlobalHelper.getInstance().setGameplayState(this);

            loadLevel("user level.xml");
        }

        public void loadLevel(string filename)
        {
            // Cleanup singletons used by prior EngineStateGameplay
            WorldState.reset();

            GlobalHelper.getInstance().getCurrentCamera().setScreenWidth((float)engine_.graphics_.PreferredBackBufferWidth);
            GlobalHelper.getInstance().getCurrentCamera().setScreenHeight((float)engine_.graphics_.PreferredBackBufferHeight);

            drawPipeline_ = new List<DrawableObjectAbstract>();
            
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

            // Load the level and create bounding boxes
            myLevel_ = new Level(new Tileset(), null);
            myLevel_.getLevelFromFile(filename, drawPipeline_);
            //player_ = (ActuatedMainPlayer)myLevel_.getPlayer(); // AMP: I don't like this in the Level class
            player_ = new ActuatedMainPlayer(drawPipeline_, collisionDetector_, myLevel_.getPlayerStartLocation(), new Vector2(1.0f, 0.0f));
            GlobalHelper.getInstance().getCurrentCamera().setCenter(player_.getPosition().X, player_.getPosition().Y);

            boxesToBeAdded = new bool[myLevel_.getHeight(), myLevel_.getWidth()];
            tilesForGrid = new Tile[myLevel_.getHeight(), myLevel_.getWidth()];
            for (int i = 0; i < myLevel_.getHeight(); i++)
            {
                for (int j = 0; j < myLevel_.getWidth(); j++)
                {
                    if (myLevel_.getTiles()[i, j].getTileNumber() != 1)
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
            boxesToBeAddedForReal = Tiler.mergeBoxes(tilesForGrid);
            GlobalHelper.getInstance().setCurrentLevelTileGrid(new TileGrid(tilesForGrid));

            //collisionDetector_ = new CollisionDetector(polygons);
            collisionDetector_ = new SeparatingAxisCollisionDetector();

            foreach (BoxObject boxOb in boxesToBeAddedForReal)
            {
                //boxOb.getBounds().rotate(new Vector2(1.0f, 0.0f), boxOb.getPosition());
                collisionDetector_.register(boxOb);
            }
            //END Jared's test stuff

            healthBarPos_ = HEALTH_BAR_POSITION;
            weaponIconPos_ = WEAPON_ICON_POSITION;
            healthTextPos_ = new Vector2(healthBarPos_.X + HEALTH_TEXT_OFFSET_X, healthBarPos_.Y + HEALTH_TEXT_OFFSET_Y);
            ammoTextPos_ = AMMO_TEXT_POSITION;
            healthBar_ = new HeadsUpDisplayObject(drawPipeline_, TextureMap.getInstance().getTexture(HEALTH_BAR_FILL_TEX_NAME), healthBarPos_, Vector2.Zero, HUD_DRAW_DEPTH);
            weapon_ = new HeadsUpDisplayWeapon(drawPipeline_, TextureMap.getInstance().getTexture(WEAPON_TEX_NAME), weaponIconPos_, Vector2.Zero, HUD_DRAW_DEPTH);
            ammo_ = new HeadsUpDisplayText(ammoTextPos_, FONT_DRAW_DEPTH, FontEnum.Kootenay);
            player_.getHealth().addObserver(healthBar_);
            player_.getWeapon().addObserver(weapon_);
            player_.getAmmo().addObserver(ammo_);
            player_.setCollisionDetector(collisionDetector_);
            for (int i = 0; i < myLevel_.getEnemies().Count; i++)
            {
                myLevel_.getEnemies()[i].setCollisionDetector(collisionDetector_);
            }
            for (int i = 0; i < myLevel_.getItems().Count; i++)
            {
                myLevel_.getItems()[i].setCollisionDetector(collisionDetector_);
            }
            //LevelTransitionObject transition = new LevelTransitionObject("user level.xml", collisionDetector_, tileBox, Vector2.Zero, 20f, new Height(true, true), drawPipeline_, TextureMap.fetchTexture("Tile_0"), new Vector2(150f, 200f), new Vector2(1f, 0f), Constants.DEPTH_LOW);

            WorldState.EnemyList_ = (List<CharacterAbstract>)myLevel_.getEnemies();
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

            if (player_.isDead())
            {
                return new EngineStateGameOver(engine_);
            }
            else
            {
                return this;
            }
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
            TextureMap.getInstance().getTexture("blank").drawImageWithDimAbsolute(0, new Rectangle(HUD_BAR_DRAW_X, HUD_BAR_DRAW_Y, HUD_BAR_WIDTH, HUD_BAR_HEIGHT), HUD_DRAW_DEPTH - 0.01f, Color.Silver);
            FontMap.getInstance().getFont(FontEnum.Kootenay).drawString(HEALTH_TEXT, healthTextPos_, Color.Black, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, FONT_DRAW_DEPTH);
            //FontMap.getInstance().getFont(FontEnum.Kootenay).drawString(AMMO_TEXT, ammoTextPos_, Color.Black, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, FONT_DRAW_DEPTH);
            ammo_.drawString(AMMO_TEXT, AMMO_REPLACE_TEXT, Color.Black, 0.0f);
            /* end section*/
        }

        #endregion
    }
}
