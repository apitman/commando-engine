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
using Commando.collisiondetection;
using Commando.levels;
using Microsoft.Xna.Framework;

namespace Commando.objects
{
    /// <summary>
    /// Controls the flow of transitioning from a Gameplay state into another
    /// state, such as between levels, into story, dialogue, etc..
    /// </summary>
    public abstract class TransitionObjectAbstract : LevelObjectAbstract, CollisionObjectInterface
    {
        // --- Member Variables ---

        protected ConvexPolygonInterface bounds_;
        protected Height height_;
        protected float radius_;

        protected CollisionDetectorInterface detector_;

        // --- Abstract Functions ---

        /// <summary>
        /// Callback performed by EngineStateGameplay once that state's
        /// transition object is set to this.  Determines what state will
        /// be run by engine next.
        /// </summary>
        /// <returns>Next state to be run by Engine.</returns>
        public abstract EngineStateInterface go();

        // --- Implemented Functions ---

        public TransitionObjectAbstract(List<DrawableObjectAbstract> pipeline, CollisionDetectorInterface detector, GameTexture image, Vector2 position, Vector2 direction, float depth, float radius, Height height)
            : base(pipeline, image, position, direction, depth)
        {
            if (detector != null)
            {
                detector.register(this);
            }
            detector_ = detector;
            radius_ = radius;
            height_ = height;
        }

        public ConvexPolygonInterface getBounds(HeightEnum height)
        {
            return bounds_;
        }

        public float getRadius()
        {
            return radius_;
        }

        public virtual Vector2 checkCollisionWith(CollisionObjectInterface obj, CollisionDetectorInterface detector, HeightEnum height, float radDistance, Vector2 velocity)
        {
            if (detector.checkCollision(obj.getBounds(height), getBounds(height), radDistance, velocity) != Vector2.Zero)
            {
                handleCollision(obj);
            }
            return Vector2.Zero;
        }

        public virtual Vector2 checkCollisionInto(CollisionObjectInterface obj, CollisionDetectorInterface detector, Height height, float radDistance, Vector2 translate)
        {
            handleCollision(obj);
            return Vector2.Zero;
        }

        public void handleCollision(CollisionObjectInterface obj)
        {
            if (obj is PlayableCharacterAbstract)
            {
                GlobalHelper.getInstance().getGameplayState().setTransition(this);
            }
        }

        public void collidedWith(CollisionObjectInterface obj)
        {
            handleCollision(obj);
        }

        public void collidedInto(CollisionObjectInterface obj)
        {
            handleCollision(obj);
        }

        public Height getHeight()
        {
            return height_;
        }

        public override void draw(GameTime gameTime)
        {
            image_.drawImage(0, position_, CommonFunctions.getAngle(direction_), depth_);
        }

        public override void die()
        {
            base.die();
            if (detector_ != null)
            {
                detector_.remove(this);
            }
        }

        public override void setCollisionDetector(CollisionDetectorInterface collisionDetector)
        {
            if (detector_ != null)
            {
                detector_.remove(this);
            }
            detector_ = collisionDetector;
            if (detector_ != null)
            {
                detector_.register(this);
            }
        }
    }
}
