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
using Microsoft.Xna.Framework;
using Commando.levels;

namespace Commando.objects
{
    abstract class ItemAbstract : LevelObjectAbstract, CollisionObjectInterface
    {
        protected float radius_;

        protected ConvexPolygonInterface bounds_;

        protected Height height_;

        protected CollisionDetectorInterface detector_;

        public ItemAbstract(CollisionDetectorInterface detector, List<Vector2> points, Vector2 center, float radius, Height height, List<DrawableObjectAbstract> pipeline, GameTexture image, Vector2 position, Vector2 direction, float depth)
            : base(pipeline, image, position, direction, depth)
        {
            bounds_ = new ConvexPolygon(points, center);
            bounds_.rotate(direction_, position_);
            radius_ = radius;
            height_ = height;
            if (detector != null)
            {
                detector.register(this);
            }
            detector_ = detector;
        }

        public float getRadius()
        {
            return radius_;
        }

        public ConvexPolygonInterface getBounds(HeightEnum height)
        {
            return bounds_;
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

        public abstract void handleCollision(CollisionObjectInterface obj);

        public abstract void collidedWith(CollisionObjectInterface obj);

        public abstract void collidedInto(CollisionObjectInterface obj);

        public Height getHeight()
        {
            return height_;
        }

        public override void die()
        {
            base.die();
            if (detector_ != null)
            {
                detector_.remove(this);
            }
        }
    }
}
