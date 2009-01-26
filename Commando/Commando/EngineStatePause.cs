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

using Commando.controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Commando
{
    /// <summary>
    /// A state of play which waits for the user to return playing;
    /// might implement a menu in later functionality
    /// </summary>
    public class EngineStatePause : EngineStateInterface
    {

        protected Engine engine_;
        protected EngineStateInterface savedState_;

        /// <summary>
        /// Creates a pause state which waits for the user to resume play
        /// </summary>
        /// <param name="engine">Reference to the engine running the state</param>
        /// <param name="savedState">The state of play to return to once the user unpauses</param>
        public EngineStatePause(Engine engine, EngineStateInterface savedState)
        {
            engine_ = engine;
            savedState_ = savedState;
        }

        #region EngineStateInterface Members

        /// <summary>
        /// Handles user input to determine whether to switch out of pause
        /// </summary>
        /// <param name="gameTime">GameTime parameter</param>
        /// <returns>The engine state to be in next frame</returns>
        public EngineStateInterface update(GameTime gameTime)
        {
            InputSet inputs = engine_.getInputs();

            if (inputs.getConfirmButton())
            {
                inputs.setToggle(InputsEnum.CONFIRM_BUTTON);
                return savedState_;
            }

            if (inputs.getCancelButton())
            {
                engine_.Exit();
            }

            return this;
        }

        /// <summary>
        /// Draws the pause screen
        /// </summary>
        public void draw()
        {
            engine_.GraphicsDevice.Clear(Color.Black);
            string pauseText = "Press Enter to Continue\nor Escape to Quit";
            GameFont pauseFont = FontMap.getInstance().getFont(FontEnum.Pericles);
            Vector2 origin = pauseFont.getFont().MeasureString(pauseText);
            pauseFont.drawString(pauseText,
                new Vector2(engine_.GraphicsDevice.Viewport.Width / 2,
                    engine_.GraphicsDevice.Viewport.Height / 2),
                Color.White,
                0.0f,
                new Vector2(origin.X / 2,
                    origin.Y / 2),
                1.0f,
                SpriteEffects.None,
                1.0f);
        }

        #endregion
    }
}
