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
using Commando.levels;
using Commando.collisiondetection;

namespace Commando.objects.weapons
{
    public class Missile : Projectile
    {
        protected const string TEXTURE_NAME = "missile";
        protected const float RADIUS = 12f;
        protected const float SPEED = 2.0f;
        protected const float DEPTH = 0.5f;
        protected const int DAMAGE = 20;
        protected static readonly Height HEIGHT = new Height(true, false);
        protected const float TURNSPEED = .1f;

        private static readonly List<Vector2> BOUNDS_POINTS;

        protected CharacterAbstract target_;

        protected int damage_;

        static Missile()
        {
            BOUNDS_POINTS = new List<Vector2>();
            BOUNDS_POINTS.Add(new Vector2(-8f, -4f));
            BOUNDS_POINTS.Add(new Vector2(9f, -3f));
            BOUNDS_POINTS.Add(new Vector2(12f, 0f));
            BOUNDS_POINTS.Add(new Vector2(9f, 3f));
            BOUNDS_POINTS.Add(new Vector2(-8f, 4f));
        }

        public Missile(List<DrawableObjectAbstract> pipeline, CollisionDetectorInterface detector, CharacterAbstract target)
            : base(pipeline, TextureMap.fetchTexture(TEXTURE_NAME), detector, null, RADIUS, HEIGHT, Vector2.Zero, Vector2.Zero, Vector2.Zero, DEPTH)
        {
            damage_ = DAMAGE;
            boundsPolygon_ = new ConvexPolygon(BOUNDS_POINTS, Vector2.Zero);
            target_ = target;
        }

        public override void update(GameTime gameTime)
        {
            setTargetPosition(target_.getPosition());
            base.update(gameTime);
            // nothing needed here, Projectile works fine and will pass
            //  collisions to our handleCollision() method
        }

        public override void handleCollision()
        {
            if (collidedInto_ != null && collidedInto_ is CharacterAbstract)
            {
                (collidedInto_ as CharacterAbstract).damage(damage_, this);
            }
            ShrapnelGenerator.createShrapnel(pipeline_, position_,
                Microsoft.Xna.Framework.Graphics.Color.Yellow,
                Constants.DEPTH_DEBUG_LINES);
            SoundEngine.getInstance().playCue("firework-explosion-1");
            die();
        }

        public override void collidedInto(CollisionObjectInterface obj)
        {
            if (obj is Bullet || obj is ItemAbstract || obj is CoverObject)
            {
                return;
            }
            collidedInto_ = obj;
        }

        public override bool objectChangesHeight(CollisionObjectInterface obj)
        {
            //return false;
            return !((obj is Bullet) || (obj is CharacterAbstract) || (obj is ItemAbstract) || (obj is LevelTransitionObject));
        }

        public void setTargetPosition(Vector2 position)
        {
            Vector2 newDirection = position - position_;
            float rotationDirectional = (float)Math.Atan2(newDirection.Y, newDirection.X);
            float rotAngle = CommonFunctions.getAngle(direction_);

            float rotDiff = MathHelper.WrapAngle(rotAngle - rotationDirectional);
            if (Math.Abs(rotDiff) <= TURNSPEED || Math.Abs(rotDiff) >= MathHelper.TwoPi - TURNSPEED)
            {
                direction_ = newDirection;
            }
            else if (rotDiff < 0f && rotDiff > -MathHelper.Pi)
            {
                rotAngle += TURNSPEED;
                direction_.X = (float)Math.Cos((double)rotAngle);
                direction_.Y = (float)Math.Sin((double)rotAngle);
            }
            else
            {
                rotAngle -= TURNSPEED;
                direction_.X = (float)Math.Cos((double)rotAngle);
                direction_.Y = (float)Math.Sin((double)rotAngle);
            }
            direction_.Normalize();
            velocity_ = direction_ * SPEED;
        }
    }
}
