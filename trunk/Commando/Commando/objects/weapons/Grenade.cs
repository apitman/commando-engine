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
using Commando.collisiondetection;
using Commando.levels;

namespace Commando.objects.weapons
{
    public abstract class Grenade : Projectile
    {
        protected const float RADIUS = 3;
        protected static readonly Height HEIGHT = new Height(true, true);
        protected const float SPEED = 5.0f;
        protected const float DEPTH = 0.5f;
        private static readonly List<Vector2> BOUNDS_POINTS;
        protected const int FUSE = 90;

        protected int fuseTime_;

        protected bool started_;

        protected int airTime_;

        static Grenade()
        {
            BOUNDS_POINTS = new List<Vector2>();
            BOUNDS_POINTS.Add(new Vector2(1f, 1f));
            BOUNDS_POINTS.Add(new Vector2(-1f, 1f));
            BOUNDS_POINTS.Add(new Vector2(-1f, -1f));
            BOUNDS_POINTS.Add(new Vector2(1f, -1f));
        }

        public Grenade(List<DrawableObjectAbstract> pipeline, GameTexture texture, CollisionDetectorInterface detector, Vector2 direction, Vector2 position, Vector2 directionFacing)
            : base(pipeline, texture, detector, null, RADIUS, HEIGHT, CommonFunctions.normalizeNonmutating(direction) * SPEED, position, directionFacing, DEPTH)
        {
            boundsPolygon_ = new ConvexPolygon(BOUNDS_POINTS, Vector2.Zero);
            fuseTime_ = FUSE;
            started_ = false;
        }

        public override void update(GameTime gameTime)
        {
            if (!started_)
            {
                return;
            }
            if (fuseTime_ == 0)
            {
                handleCollision();
                return;
            }
            Vector2 velocity = velocity_;
            bool colHappened = collisionDetector_.checkCollisions(this, ref velocity, ref direction_);
            position_.X += velocity_.X;
            position_.Y += velocity_.Y;
            fuseTime_--;
        }

        public override void draw(GameTime gameTime)
        {
            if (started_)
            {
                base.draw(gameTime);
            }
        }

        public override void handleCollision()
        {
            explode();
        }

        public abstract void explode();

        public override bool objectChangesHeight(CollisionObjectInterface obj)
        {
            return !((obj is Projectile) || (obj is CharacterAbstract) || (obj is ItemAbstract) || (obj is LevelTransitionObject));
        }

        public override Vector2 checkCollisionInto(CollisionObjectInterface obj, CollisionDetectorInterface detector, Height height, float radDistance, Vector2 translate)
        {
            Vector2 normal = translate;
            //normal.Normalize();
            //velocity_ = velocity_ - velocity_.Length() * (CommonFunctions.dotProduct(velocity_, normal)) * normal;
            velocity_ /= 2f;
            velocity_ = velocity_ - 2 * (CommonFunctions.dotProduct(velocity_, normal) / normal.LengthSquared()) * normal;
            return 2 * translate;
        }

        public void start()
        {
            started_ = true;
        }

        public override void setVelocity(Vector2 velocity)
        {
            velocity_ = CommonFunctions.normalizeNonmutating(velocity) * SPEED;
        }
    }
}
