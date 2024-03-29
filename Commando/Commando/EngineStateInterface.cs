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

using Microsoft.Xna.Framework;

namespace Commando
{
    /// <summary>
    /// Implemented by all the different major states of the game, such as
    /// the main menu, playing through levels, pausing, credits, cutscenes,
    /// etc.
    /// </summary>
    public interface EngineStateInterface
    {
        /// <summary>
        /// Perform one frame of update as designated by the state
        /// </summary>
        /// <param name="gameTime">Gametime Parameter</param>
        /// <returns>The state of the game for the next frame</returns>
        EngineStateInterface update(GameTime gameTime);

        /// <summary>
        /// Draw the current state of the frame
        /// </summary>
        void draw();
    }
}
