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
using System.Diagnostics;
using Commando.ai;
using Commando.graphics.multithreading;

namespace Commando
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Engine : Microsoft.Xna.Framework.Game
    {
        public GraphicsDeviceManager graphics_;
        public SpriteBatch spriteBatch_;
        EngineStateInterface engineState_;
        protected ControllerInputInterface controls_;
        public ControllerInputInterface Controls_ 
        {
            get
            {
                return controls_;
            }
            set
            {
                controls_ = value;
                if (UpdateThread_ != null)
                {
                    UpdateThread_.Controls_ = controls_;
                }
            }
        }
        public UpdateThread UpdateThread_ { get; set; }
        public RenderThread RenderThread_ { get; set; }
        public DrawBuffer DrawBuffer_ { get; set; }

        internal bool UpdateGraphicsFlag_ { get; set; }

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
            UpdateGraphicsFlag_ = true;
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
            this.engineState_ = new EngineStateSplash(this);

            int tiles = (int)((GraphicsDevice.Viewport.Height / 15) * (GraphicsDevice.Viewport.Width / 15) * 1.2);
            tiles += 350;

            DrawBuffer.initialize(tiles, spriteBatch_);
            DrawBuffer_ = DrawBuffer.getInstance();
            UpdateThread_ = new UpdateThread(this, engineState_);
            RenderThread_ = new RenderThread();
            UpdateThread_.Controls_ = Controls_;
            UpdateThread_.startThread();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch_ = new SpriteBatch(GraphicsDevice);

            //TextureMap.getInstance().setContent(Content);
            //TextureMap.getInstance().loadTextures(TEXTUREMAPXML, this);
            //FontMap.getInstance().loadFonts("", spriteBatch_, this);
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
            /*
#if DEBUG
            Stopwatch clock = new Stopwatch();
            clock.Start();
#endif

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

#if DEBUG
            FPSMonitor.getInstance().update(gameTime);

            clock.Stop();
            PerformanceLogger.addValue(MetricType.UPDATE, clock.ElapsedMilliseconds);

            PerformanceLogger.addValue(MetricType.RAYCAST, Raycaster.clock.ElapsedMilliseconds);
            Raycaster.clock.Reset();

            PerformanceLogger.addValue(MetricType.PATHFIND, AStarPathfinder.clock.ElapsedMilliseconds);
            AStarPathfinder.clock.Reset();
            */

            if (UpdateGraphicsFlag_)
                graphics_.ApplyChanges();

            if (Controls_ != null)
                Controls_.updateInputSet();

            base.Update(gameTime);
//#endif
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            /*
#if DEBUG
            Stopwatch clock = new Stopwatch();
            clock.Start();
#endif

            // TODO: Add your drawing code here
            spriteBatch_.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.FrontToBack, SaveStateMode.None);
            engineState_.draw();
#if DEBUG
            FPSMonitor.getInstance().draw(gameTime);
#endif
            spriteBatch_.End();

            base.Draw(gameTime);

#if DEBUG
            clock.Stop();
            PerformanceLogger.addValue(MetricType.DRAW, clock.ElapsedMilliseconds);
#endif
             */
            spriteBatch_.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.FrontToBack, SaveStateMode.None);

            DrawBuffer_.globalStartFrame(gameTime);
            graphics_.GraphicsDevice.Clear(DrawBuffer_.getRenderStack().ScreenClearColor_);

            RenderThread_.tick();
            base.Draw(gameTime);

            DrawBuffer_.globalSynchronize();
            spriteBatch_.End();

        }

        /// <summary>
        /// Event handler for when the game tries to close.  Overridden to push
        /// settings to a file and clean up audio resources.
        /// </summary>
        /// <param name="sender">Object initiating the event.</param>
        /// <param name="args">Arguments.</param>
        protected override void OnExiting(object sender, EventArgs args)
        {
            DrawBuffer_.cleanUp();
            if (UpdateThread_.RunningThread != null)
            {
                UpdateThread_.RunningThread.Abort();
            }

            base.OnExiting(sender, args);
            SoundEngine.cleanup();
            Settings.getInstance().saveSettingsToFile();
#if DEBUG
            //PerformanceLogger.printMetric(MetricType.RAYCAST);
            //PerformanceLogger.printMetric(MetricType.PATHFIND);
            //PerformanceLogger.printMetric(MetricType.DRAW);
#if XBOX
            throw new PerformanceMonitorException(PerformanceLogger.printMetric(MetricType.UPDATE));
#else
            System.Console.WriteLine(PerformanceLogger.printMetric(MetricType.UPDATE));
#endif
#endif
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
