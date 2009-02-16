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
using Microsoft.Xna.Framework;

namespace Commando
{
    /// <summary>
    /// All objects which move, but are not animated inherit from this abstract class.
    /// </summary>
    abstract class NonAnimatedMovableObjectAbstract : MovableObjectAbstract
    {
        protected GameTexture texture_;

        protected int curImage_;

        /// <summary>
        /// Create a default NonAnimatedMovableObject.
        /// </summary>
        public NonAnimatedMovableObjectAbstract() :
            base()
        {
        }

        /// <summary>
        /// Create a NonAnimatedMovableObject with the specified texture and current image.
        /// </summary>
        /// <param name="texture">GameTexture of this object</param>
        /// <param name="curImage">The number of the current image</param>
        public NonAnimatedMovableObjectAbstract(GameTexture texture, int curImage) :
            base()
        {
        }

        /// <summary>
        /// Create a NonAnimatedMovableObject with the specified texture, current image,
        /// position, direction, and depth with the specified velocity.
        /// </summary>
        /// <param name="texture">GameTexture of this object</param>
        /// <param name="curImage">The number of the current image</param>
        /// <param name="velocity">Vector of velocity, representing both direction of movement and magnitude</param>
        /// <param name="position">Position of object relative to the top left corner</param>
        /// <param name="direction">Vector representing the direction of the object</param>
        /// <param name="depth">Depth the object is to be drawn to</param>
        public NonAnimatedMovableObjectAbstract(GameTexture texture, int curImage, Vector2 velocity, Vector2 position, Vector2 direction, float depth) :
            base(velocity, position, direction, depth)
        {
        }
    }
}