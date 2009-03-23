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
using Commando.graphics;
using Microsoft.Xna.Framework;
using Commando.levels;

namespace Commando.objects
{
    public abstract class Projectile : MovableObjectAbstract, CollisionObjectInterface
    {
        protected List<DrawableObjectAbstract> drawPipeline_;

        protected GameTexture texture_;

        protected CollisionDetectorInterface collisionDetector_;

        protected ConvexPolygonInterface boundsPolygon_;

        protected float radius_;

        protected CollisionObjectInterface collidedInto_ = null;

        protected Height height_;

        public Projectile(List<DrawableObjectAbstract> pipeline, GameTexture texture, CollisionDetectorInterface detector, ConvexPolygonInterface bounds, float radius, Height height, Vector2 velocity, Vector2 position, Vector2 direction, float depth) :
            base(pipeline, velocity, position, direction, depth)
        {
            texture_ = texture;
            if (detector != null)
            {
                detector.register(this);
                collisionDetector_ = detector;
            }
            boundsPolygon_ = bounds;
            radius_ = radius;
            height_ = height;
        }

        public override void update(GameTime gameTime)
        {
            Vector2 velocity = velocity_;
            bool colHappened = collisionDetector_.checkCollisions(this, ref velocity, ref direction_);
            position_.X += velocity_.X;
            position_.Y += velocity_.Y;
            if (((!height_.blocksHigh_) && (!height_.blocksLow_)) || (colHappened && collidedInto_ != null && !objectChangesHeight(collidedInto_)))
            {
                handleCollision();
            }
        }

        public override void draw(GameTime gameTime)
        {
            texture_.drawImage(0, position_, getRotationAngle(), depth_);
        }

        public abstract void handleCollision();

        public abstract bool objectChangesHeight(CollisionObjectInterface obj);

        public void setCollisionDetector(CollisionDetectorInterface detector)
        {
            if (detector != null)
            {
                detector.register(this);
                collisionDetector_ = detector;
            }
        }

        public float getRadius()
        {
            return radius_;
        }

        public ConvexPolygonInterface getBounds(HeightEnum height)
        {
            return boundsPolygon_;
        }

        public Height getHeight()
        {
            return height_;
        }

        public override void die()
        {
            base.die();
            if (collisionDetector_ != null)
            {
                collisionDetector_.remove(this);
            }
        }

        public virtual void collidedWith(CollisionObjectInterface obj)
        {
            collidedInto_ = obj;
        }

        public virtual void collidedInto(CollisionObjectInterface obj)
        {
            collidedInto_ = obj;
        }

        public virtual Vector2 checkCollisionWith(CollisionObjectInterface obj, CollisionDetectorInterface detector, HeightEnum height, float radDistance, Vector2 velocity)
        {
            Vector2 translate = detector.checkCollision(obj.getBounds(height), getBounds(height), radDistance, velocity);
            if (translate != Vector2.Zero)
            {
                obj.collidedInto(this);
                collidedWith(obj);
                //return translate;
            }
            if (objectChangesHeight(obj))
            {
                if (height == HeightEnum.HIGH)
                {
                    height_.blocksHigh_ = false;
                }
                else
                {
                    height_.blocksLow_ = false;
                }
            }
            return Vector2.Zero;
        }

        public virtual Vector2 checkCollisionInto(CollisionObjectInterface obj, CollisionDetectorInterface detector, Height height, float radDistance, Vector2 translate)
        {
            if (objectChangesHeight(obj))
            {
                if (height.blocksHigh_)
                {
                    height_.blocksHigh_ = false;
                }
                if(height.blocksLow_)
                {
                    height_.blocksLow_ = false;
                }
            }
            return translate;
        }
    }
}
