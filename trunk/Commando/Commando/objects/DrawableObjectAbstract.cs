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
using Commando.ai;

namespace Commando
{
    /// <summary>
    /// DrawableObjects are any object which can be drawn to the screen and requires both
    /// an update and a draw function.  This class keeps track of the position, direction,
    /// and drawing depth of the object.  Almost all things drawn to the screen inherit from
    /// this class, except for those that are completely static (i.e. menu backgrounds).
    /// </summary>
    public abstract class DrawableObjectAbstract
    {
        public List<DrawableObjectAbstract> pipeline_;

        protected Vector2 position_;

        protected Vector2 direction_;

        protected float depth_;

        protected bool isDead_;

        // TODO Temporary block?
        /// <summary>
        /// Contains the ID for the visual stimulus this object owns
        /// in the WorldState
        /// </summary>
        protected int visualStimulusId_;

        /// <summary>
        /// Hidden default constructor.
        /// </summary>
        protected DrawableObjectAbstract() { }

        /// <summary>
        /// Creates a DrawableObject with the specified position, direction, and depth
        /// </summary>
        /// <param name="pipeline">Drawing pipeline in which to register the object</param>
        /// <param name="position">Position of object relative to the top left corner</param>
        /// <param name="direction">Vector representing the direction of the object</param>
        /// <param name="depth">Depth the object is to be drawn to</param>
        public DrawableObjectAbstract(List<DrawableObjectAbstract> pipeline, Vector2 position, Vector2 direction, float depth)
        {
            pipeline_ = pipeline;
            if (pipeline_ != null)
            {
                pipeline_.Add(this);
            }

            position_ = position;
            direction_ = direction;
            depth_ = depth;

            visualStimulusId_ = StimulusIDGenerator.getNext();

            isDead_ = false;
        }

        /// <summary>
        /// Function which causes an object to update its status for the current frame
        /// </summary>
        /// <param name="gameTime">The time since the last update</param>
        public abstract void update(GameTime gameTime);

        /// <summary>
        /// Draws the current object to the screen based on its current status
        /// </summary>
        /// <param name="gameTime"></param>
        public abstract void draw(GameTime gameTime);

        /// <summary>
        /// Get the object's current position
        /// </summary>
        /// <returns>Position of object relative to top left corner of the screen</returns>
        public Vector2 getPosition()
        {
            return position_;
        }

        /// <summary>
        /// Get the object's current direction
        /// </summary>
        /// <returns>Direction of the object in Vector form</returns>
        public Vector2 getDirection()
        {
            return direction_;
        }

        /// <summary>
        /// Get the object's current depth
        /// </summary>
        /// <returns>Object's current drawing depth (between 0 and 1)</returns>
        public float getDepth()
        {
            return depth_;
        }

        /// <summary>
        /// Set the object's current postion
        /// </summary>
        /// <param name="myPos">Object's position relative to the top left corner of the screen</param>
        public void setPosition(Vector2 pos)
        {
            position_ = pos;
        }

        /// <summary>
        /// Set the object's current direction
        /// </summary>
        /// <param name="dir">Direction of the object in Vector form</param>
        public void setDirection(Vector2 dir)
        {
            direction_ = dir;
        }

        /// <summary>
        /// Set the object's current depth.  It is floored at 0 and has a ceiling at 1
        /// </summary>
        /// <param name="dep">Object's current drawing depth (between 0 and 1)</param>
        public void setDepth(float dep)
        {
            if (dep < 0.0f)
                dep = 0.0f;
            if (dep > 1.0f)
                dep = 1.0f;
            depth_ = dep;
        }

        public virtual bool isDead()
        {
            return isDead_;
        }

        public virtual void die()
        {
            isDead_ = true;
        }

        /// <summary>
        /// Convert the direction vector into a float representing the angle of the object
        /// </summary>
        /// <returns>Angle of the rotation in radians</returns>
        public float getRotationAngle()
        {
            return (float)Math.Atan2((double)direction_.Y, (double)direction_.X);
        }
    }
}
