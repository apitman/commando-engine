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


using Microsoft.Xna.Framework;
using Commando.graphics;
using Microsoft.Xna.Framework.Graphics;

namespace Commando
{
    /// <summary>
    /// A state of play which runs when the user takes the game
    /// window out of focus; essentially halts gameplay.
    /// </summary>
    class EngineStateOutofFocus : EngineStateInterface
    {
        protected EngineStateInterface outOfFocusState_;

        const float OVERLAY_DEPTH = Constants.DEPTH_OUT_OF_FOCUS_OVERLAY;
        const string MESSAGE = "Game Temporarily Paused\n\nPlease move the mouse back\ninto the game window and make\nsure that it is active.";
        readonly Color MESSAGE_COLOR = Color.White;
        const float MESSAGE_ROTATION = 0.0f;
        const float MESSAGE_DEPTH = Constants.DEPTH_OUT_OF_FOCUS_TEXT;

        protected Engine engine_;

        /// <summary>
        /// Creates an OutofFocus state to encapsulate the state which just
        /// left focus, so that it can be returned to.
        /// </summary>
        /// <param name="engine">A reference to the engine running the state</param>
        /// <param name="outOfFocusState">The state of play to return to once focus is regained</param>
        public EngineStateOutofFocus(Engine engine, EngineStateInterface outOfFocusState)
        {
            engine_ = engine;
            outOfFocusState_ = outOfFocusState;
        }

        #region EngineStateInterface Members

        /// <summary>
        /// Update one frame, which does nothing but determine whether
        /// or not to return to the previous state of gameplay (by checking
        /// whether focus has been regained)
        /// </summary>
        /// <param name="gameTime"></param>
        /// <returns></returns>
        public EngineStateInterface update(GameTime gameTime)
        {
            if (engine_.IsActive
#if !XBOX                
                && !engine_.mouseOutsideWindow()
#endif
                )
            {
                return outOfFocusState_;
            }
            return this;
        }

        /// <summary>
        /// Draw the encapsulated state with a layer of transparent color
        /// over it, and some text explaining to the user what happened -
        /// TODO
        /// </summary>
        public void draw()
        {
            outOfFocusState_.draw();

            Rectangle screen =
                new Rectangle(
                    0,
                    0,
                    engine_.GraphicsDevice.Viewport.Width,
                    engine_.GraphicsDevice.Viewport.Height);

            TextureMap tm = TextureMap.getInstance();
            tm.getTexture("overlay").drawImageWithDim(0, screen, OVERLAY_DEPTH);

            Vector2 fontpos = new Vector2(
                engine_.GraphicsDevice.Viewport.Width / 2,
                engine_.GraphicsDevice.Viewport.Height / 2);

            GameFont gf = FontMap.getInstance().getFont(FontEnum.Pericles);
            gf.drawStringCentered(MESSAGE,
                fontpos,
                MESSAGE_COLOR,
                MESSAGE_ROTATION,
                MESSAGE_DEPTH);
        }

        #endregion
    }
}
