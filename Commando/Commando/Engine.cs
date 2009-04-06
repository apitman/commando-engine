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
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using Commando.controls;

namespace Commando
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Engine : Microsoft.Xna.Framework.Game
    {
        public GraphicsDeviceManager graphics_;
        SpriteBatch spriteBatch_;
        EngineStateInterface engineState_;
        public ControllerInputInterface Controls_ { get; set; }

        const float GLOBALSPEEDMULTIPLIER = 2.5F;
        const int FRAMERATE = 30;
        const string TEXTUREMAPXML = ".\\Content\\XML\\LoadScripts\\TextureLoader.xml";

        const int SCREEN_MIN_WIDTH = 800;
        const int SCREEN_MIN_HEIGHT = 600;

        public Engine()
        {
            graphics_ = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            this.IsFixedTimeStep = true;
            this.TargetElapsedTime = new TimeSpan(0, 0, 0, 0, (1000 / FRAMERATE));
           
        }

        public ContentManager getContent()
        {
            return Content;
        }

        public void initializeScreen()
        {
            setScreenSize(
                Math.Max(this.GraphicsDevice.DisplayMode.Width, SCREEN_MIN_WIDTH),
                Math.Max(this.GraphicsDevice.DisplayMode.Height, SCREEN_MIN_HEIGHT)
                );
        }

        public void setScreenSize(int x, int y)
        {
            graphics_.IsFullScreen = false;

#if !XBOX
            if (!graphics_.IsFullScreen)
            {
                y -= 100;
            }
#endif

            graphics_.PreferredBackBufferHeight = y;
            graphics_.PreferredBackBufferWidth = x;
            graphics_.ApplyChanges();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
            initializeScreen();
            Settings.initialize(this);

#if XBOX
            Settings.getInstance().IsUsingMouse_ = false;
#else
            if (GamePad.GetState(PlayerIndex.One).IsConnected)
            {
                Settings.getInstance().IsUsingMouse_ = false;
            }
            else
            {
                Settings.getInstance().IsUsingMouse_ = true;
            }
#endif

            try
            {
                // Debugging - Uncomment this line to try PC version as if it
                //  were running with the Redistributable runtime in which
                //  GamerServices is not available
                // Note that this is not a truly accurate test, as there could
                //  be lurking calls to GamerServices outside of a block which
                //  tests Settings.IsGamerServicesAllowed_ prior to using
                // throw new Exception();

                GamerServicesComponent gsc = new GamerServicesComponent(this);
                gsc.Initialize();
                this.Components.Add(gsc);
                Settings.getInstance().IsGamerServicesAllowed_ = true;
            }
            catch
            {
                Settings.getInstance().IsGamerServicesAllowed_ = false;
            }

            // creating EngineStateStart must come AFTER setting the
            //  IsGamerServicesAllowed_ member of Settings
            this.engineState_ = new EngineStateStart(this);
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch_ = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            TextureMap.getInstance().setContent(Content);
            TextureMap.getInstance().loadTextures(TEXTUREMAPXML, spriteBatch_, graphics_.GraphicsDevice);
            FontMap.getInstance().loadFonts("", spriteBatch_, this);

            /*
            MediaPlayerHelper.loadSongs(Content);
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(MediaPlayerHelper.getSong("epic"));
            */
            SoundEngine.getInstance().Music = SoundEngine.getInstance().playCue("epic");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // TODO: Add your update logic here
            if (!IsActive && !(engineState_ is EngineStateOutofFocus)
#if !XBOX
                || mouseOutsideWindow() && !(engineState_ is EngineStateOutofFocus) 
#endif           
               )
            {
                engineState_ = new EngineStateOutofFocus(this, engineState_);
            }

            if (Controls_ != null)
                Controls_.updateInputSet();

            engineState_ = engineState_.update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {

            // TODO: Add your drawing code here
            spriteBatch_.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.FrontToBack, SaveStateMode.None);
            engineState_.draw();
            spriteBatch_.End();

            base.Draw(gameTime);
        }

#if !XBOX
        /// <summary>
        /// Whether or not the mouse is outside the game window.
        /// </summary>
        /// <returns>True is the mouse is outside the game window, false otherwise</returns>
        internal protected bool mouseOutsideWindow()
        {
            MouseState ms = Mouse.GetState();
            if (ms.X < 0 || ms.Y < 0 ||
                ms.X > this.GraphicsDevice.Viewport.X + this.GraphicsDevice.Viewport.Width ||
                ms.Y > this.GraphicsDevice.Viewport.Y + this.GraphicsDevice.Viewport.Height)
            {
                return true;
            }
            return false;
        }
#endif

    }

}
