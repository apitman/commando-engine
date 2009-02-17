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

namespace Commando.objects
{
    public class Projectile : MovableObjectAbstract, CollisionObjectInterface
    {
        protected GameTexture texture_;

        protected CollisionDetectorInterface collisionDetector_;

        protected ConvexPolygonInterface boundsPolygon_;

        protected float radius_;

        public Projectile() :
            this(null, null, null, 0f)
        {
        }

        public Projectile(GameTexture texture, CollisionDetectorInterface detector, ConvexPolygonInterface bounds, float radius) :
            base()
        {
            texture_ = texture;
            if (detector != null)
            {
                detector.register(this);
                collisionDetector_ = detector;
            }
            boundsPolygon_ = bounds;
            radius_ = radius;
        }

        public Projectile(GameTexture texture, CollisionDetectorInterface detector, ConvexPolygonInterface bounds, float radius, Vector2 velocity, Vector2 position, Vector2 direction, float depth) :
            base(velocity, position, direction, depth)
        {
            texture_ = texture;
            if (detector != null)
            {
                detector.register(this);
                collisionDetector_ = detector;
            }
            boundsPolygon_ = bounds;
            radius_ = radius;
        }

        public override void update(GameTime gameTime)
        {
            Vector2 newPosition = position_+ velocity_;
            if (newPosition.X > 400 || newPosition.X < 0 ||
                newPosition.Y > 330 || newPosition.Y < 0 ||
                newPosition != collisionDetector_.checkCollisions(this, newPosition))
            {
                die();
            }
            position_ = newPosition;
        }

        public override void draw(GameTime gameTime)
        {
            texture_.drawImage(0, position_, getRotationAngle(), depth_);
        }

        public void setCollisionDetector(CollisionDetectorInterface detector)
        {
            if (detector != null)
            {
                detector.register(this);
                collisionDetector_ = detector;
            }
        }

        public Vector2 getPosition()
        {
            return position_;
        }

        public Vector2 getDirection()
        {
            return direction_;
        }

        public float getRadius()
        {
            return radius_;
        }

        public ConvexPolygonInterface getBounds()
        {
            return boundsPolygon_;
        }

        public override void die()
        {
            base.die();
            if (collisionDetector_ != null)
            {
                collisionDetector_.remove(this);
            }
        }
    }
}
