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
using System.Text;
using Microsoft.Xna.Framework;

namespace Commando
{
    /// <summary>
    /// MovableObjects are DrawableObjects which also have the capability of moving across
    /// the screen.  The object keeps track of their velocity.
    /// </summary>
    public abstract class MovableObjectAbstract : DrawableObjectAbstract
    {
        protected Vector2 velocity_;

        /// <summary>
        /// Hidden default constructor.
        /// </summary>
        protected MovableObjectAbstract() { }

        /// <summary>
        /// Create a MovableObject at the specifed position, direction, and depth.
        /// </summary>
        /// <param name="pipeline">List of objects from which the object should be drawn.</param>
        /// <param name="position">Position of object relative to the top left corner</param>
        /// <param name="direction">Vector representing the direction of the object</param>
        /// <param name="depth">Depth the object is to be drawn to</param>
        public MovableObjectAbstract(List<DrawableObjectAbstract> pipeline, Vector2 position, Vector2 direction, float depth) :
            base(pipeline, position, direction, depth)
        {
            velocity_ = Vector2.Zero;
        }

        /// <summary>
        /// Create a MovableObject at the specifed position, direction, and depth with the 
        /// specified velocity.
        /// </summary>
        /// <param name="pipeline">List of objects from which the object should be drawn.</param>
        /// <param name="velocity">Vector of velocity, representing both direction of movement and magnitude</param>
        /// <param name="position">Position of object relative to the top left corner</param>
        /// <param name="direction">Vector representing the direction of the object</param>
        /// <param name="depth">Depth the object is to be drawn to</param>
        public MovableObjectAbstract(List<DrawableObjectAbstract> pipeline, Vector2 velocity, Vector2 position, Vector2 direction, float depth) :
            base(pipeline, position, direction, depth)
        {
            velocity_ = velocity;
        }

        /// <summary>
        /// Get the object's current velocity.
        /// </summary>
        /// <returns>Vector of velocity, representing both direction of movement and magnitude</returns>
        public Vector2 getVelocity()
        {
            return velocity_;
        }

        /// <summary>
        /// Set the object's current velocity.
        /// </summary>
        /// <param name="velocity">Vector of velocity, representing both direction of movement and magnitude</param>
        public void setVelocity(Vector2 velocity)
        {
            velocity_ = velocity;
        }
    }
}
