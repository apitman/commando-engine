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

namespace Commando
{
    /// <summary>
    /// A state of play which runs when the user takes the game
    /// window out of focus; essentially halts gameplay.
    /// </summary>
    class EngineStateOutofFocus : EngineStateInterface
    {
        protected EngineStateInterface outOfFocusState_;

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
            if (engine_.IsActive)
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
        }

        #endregion
    }
}
