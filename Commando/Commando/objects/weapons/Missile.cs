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
        protected const float SPEED = 12.0f;
        protected const float DEPTH = 0.5f;
        protected const int DAMAGE = 20;
        protected static readonly Height HEIGHT = new Height(true, false);

        private static readonly List<Vector2> BOUNDS_POINTS;


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

        public Missile(List<DrawableObjectAbstract> pipeline, CollisionDetectorInterface detector, Vector2 position, Vector2 direction)
            : base(pipeline, TextureMap.fetchTexture(TEXTURE_NAME), detector, null, RADIUS, HEIGHT, direction * SPEED, position, direction, DEPTH)
        {
            damage_ = DAMAGE;
            boundsPolygon_ = new ConvexPolygon(BOUNDS_POINTS, Vector2.Zero);
        }

        public override void update(GameTime gameTime)
        {
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
            if (obj is ItemAbstract || obj is CoverObject)
            {
                return;
            }
            collidedInto_ = obj;
        }

        public override bool objectChangesHeight(CollisionObjectInterface obj)
        {
            //return false;
            return !((obj is CharacterAbstract) || (obj is ItemAbstract) || (obj is LevelTransitionObject));
        }
    }
}
