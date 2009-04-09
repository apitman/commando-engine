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
using Microsoft.Xna.Framework.Storage;
using Commando.objects.enemies;
using Commando.ai.planning;

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
        const string AMMO_TEXT = "%i/";
        const string AMMO_REPLACE_TEXT = "%i";
        const string SAVE_PATH = "user level.xml";
        const float HUD_DRAW_DEPTH = Constants.DEPTH_HUD;
        const float FONT_DRAW_DEPTH = Constants.DEPTH_HUD_TEXT;
        const string NEXT_LEVEL = "C:\\Documents and Settings\\segalj\\My Documents\\SavedGames\\CommandoXbox\\AllPlayers\\levels\\defaultlevel.commandolevel";

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

        protected TransitionObjectAbstract transition_;

        protected Level myLevel_;

        /// <summary>
        /// Constructs a state of gameplay
        /// </summary>
        /// <param name="engine">Reference to the engine running the state</param>
        /// <param name="filepath">Path to level file which should be loaded.</param>
        public EngineStateGameplay(Engine engine, string filepath)
            : this(engine, Level.getLevelFromFile(filepath, engine))
        {
            
        }

        /// <summary>
        /// Constructs a state of gameplay using the provided level
        /// </summary>
        public EngineStateGameplay(Engine engine, Level level)
        {
            engine_ = engine;

            GlobalHelper.getInstance().setGameplayState(this);

            GlobalHelper.getInstance().getCurrentCamera().setScreenWidth((float)engine_.graphics_.PreferredBackBufferWidth);
            GlobalHelper.getInstance().getCurrentCamera().setScreenHeight((float)engine_.graphics_.PreferredBackBufferHeight);

            loadLevel(level);

            SoundEngine.getInstance().Music = SoundEngine.getInstance().playCue("epic");
        }

        public void setTransition(TransitionObjectAbstract next)
        {
            transition_ = next;
        }

        public void loadLevel(Level level)
        {
            myLevel_ = level;

            drawPipeline_ = myLevel_.Pipeline_;
            collisionDetector_ = myLevel_.CollisionDetector_;

            if (player_ != null && myLevel_.getPlayerStartLocation() != null && myLevel_.getPlayerStartLocation() != Vector2.Zero)
            {
                player_.setPosition(myLevel_.getPlayerStartLocation());
                player_.setDrawPipeline(drawPipeline_);
            }
            else if (myLevel_.getPlayerStartLocation() != null && myLevel_.getPlayerStartLocation() != Vector2.Zero)
            {
                player_ = new ActuatedMainPlayer(drawPipeline_, collisionDetector_, myLevel_.getPlayerStartLocation(), new Vector2(1.0f, 0.0f));
                myLevel_.setPlayer(player_);
            }
            else
            {
                player_ = null;
            }

            // Initialize mouse and/or camera
            if (player_ == null)
            {
                engine_.IsMouseVisible = true;
            }
            else
            {
                GlobalHelper.getInstance().getCurrentCamera().setCenter(player_.getPosition().X, player_.getPosition().Y);
                engine_.IsMouseVisible = false;
            }

            WorldState.reset();
            myLevel_.initializeForGameplay();

            // Initialize player and HUD
            if (player_ != null)
            {
                healthBarPos_ = HEALTH_BAR_POSITION;
                weaponIconPos_ = WEAPON_ICON_POSITION;
                healthTextPos_ = new Vector2(healthBarPos_.X + HEALTH_TEXT_OFFSET_X, healthBarPos_.Y + HEALTH_TEXT_OFFSET_Y);
                ammoTextPos_ = AMMO_TEXT_POSITION;
                healthBar_ = new HeadsUpDisplayObject(drawPipeline_, TextureMap.getInstance().getTexture(HEALTH_BAR_FILL_TEX_NAME), healthBarPos_, Vector2.Zero, HUD_DRAW_DEPTH);
                weapon_ = new HeadsUpDisplayWeapon(drawPipeline_, TextureMap.getInstance().getTexture(WEAPON_TEX_NAME), weaponIconPos_, Vector2.Zero, HUD_DRAW_DEPTH);
                ammo_ = new HeadsUpDisplayText(ammoTextPos_, FONT_DRAW_DEPTH, FontEnum.Kootenay14, player_.Weapon_.CurrentAmmo_);
                player_.getHealth().addObserver(healthBar_);
                player_.getWeapon().addObserver(weapon_);
                player_.getAmmo().addObserver(ammo_);
                player_.setCollisionDetector(collisionDetector_);
            }
        }

        /// <summary>
        /// Handles input and moves all characters and objects forward one
        /// frame in time
        /// </summary>
        /// <param name="gameTime">GameTime parameter</param>
        /// <returns>The state of the game for the next frame</returns>
        public EngineStateInterface update(GameTime gameTime)
        {
            if (transition_ != null)
            {
                return transition_.go();
            }

            InputSet inputs = InputSet.getInstance();

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
            if (player_ != null)
            {
                player_.setInputSet(inputs);
            }

            // Update enemy team AI
            TeamPlannerManager.update();

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

            WorldState.refresh();

            adjustCamera(inputs);

            if (player_ != null && player_.isDead())
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
            engine_.GraphicsDevice.Clear(Color.Black);

            // Draw Debug Lines
            if (Settings.getInstance().IsInDebugMode_)
                (collisionDetector_ as SeparatingAxisCollisionDetector).draw();
            
            // Draw Laser Pointer at Cursor Position if using a mouse
            #if !XBOX
            if (Settings.getInstance().IsUsingMouse_)
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
            if (player_ != null)
            {
                healthBar_.draw(new GameTime());
                TextureMap.getInstance().getTexture(HEALTH_BAR_OUTLINE_TEX_NAME).drawImageAbsolute(0, healthBarPos_, 0.0f, HUD_DRAW_DEPTH);
                weapon_.draw(new GameTime());
                TextureMap.getInstance().getTexture("blank").drawImageWithDimAbsolute(0, new Rectangle(HUD_BAR_DRAW_X, HUD_BAR_DRAW_Y, HUD_BAR_WIDTH, HUD_BAR_HEIGHT), HUD_DRAW_DEPTH - 0.01f, Color.Silver);
                FontMap.getInstance().getFont(FontEnum.Kootenay14).drawString(HEALTH_TEXT, healthTextPos_, Color.Black, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, FONT_DRAW_DEPTH);
                //FontMap.getInstance().getFont(FontEnum.Kootenay14).drawString(AMMO_TEXT, ammoTextPos_, Color.Black, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, FONT_DRAW_DEPTH);
                string realAmmoText = AMMO_TEXT + player_.Inventory_.Ammo_[player_.Weapon_.AmmoType_].ToString() + " " + player_.Weapon_.AmmoType_.ToString();
                ammo_.drawString(realAmmoText, AMMO_REPLACE_TEXT, Color.Black, 0.0f);
            }
            /* end section*/
        }

        /// <summary>
        /// Moves the Gameplay camera based upon either player or mouse position
        /// </summary>
        /// <param name="inputs">This frame's controller input</param>
        protected void adjustCamera(InputSet inputs)
        {
            if (player_ == null)
            {
                Vector2 moveVector = Vector2.Zero;
                Vector2 cameraPosition = new Vector2(inputs.getRightDirectionalX(), inputs.getRightDirectionalY());
                if (cameraPosition.X < 100.0f)
                {
                    moveVector.X = -3f;
                }
                else if (cameraPosition.X > engine_.GraphicsDevice.Viewport.Width - 100.0f)
                {
                    moveVector.X = 3f;
                }
                if (cameraPosition.Y < 100.0f)
                {
                    moveVector.Y = -3f;
                }
                else if (cameraPosition.Y > engine_.GraphicsDevice.Viewport.Height - 100.0f)
                {
                    moveVector.Y = 3f;
                }
                GlobalHelper.getInstance().getCurrentCamera().move(moveVector.X, moveVector.Y);
            }
            else
            {
                GlobalHelper.getInstance().getCurrentCamera().setCenter(player_.getPosition().X, player_.getPosition().Y);
            }
        }
    }
}
