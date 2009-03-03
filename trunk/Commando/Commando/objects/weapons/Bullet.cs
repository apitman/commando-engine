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

namespace Commando.objects.weapons
{
    class Bullet : Projectile
    {
        protected const string TEXTURE_NAME = "Pistol";
        protected const float RADIUS = 2.5f;
        protected const float SPEED = 15.0f;
        protected const float HEIGHT = 0.5f;
        protected const int DAMAGE = 5;

        protected static readonly ConvexPolygon BOUNDS;

        static Bullet()
        {
            List<Vector2> points = new List<Vector2>();
            points.Add(new Vector2(2f, 2f));
            points.Add(new Vector2(-2f, 2f));
            points.Add(new Vector2(-2f, -2f));
            points.Add(new Vector2(2f, -2f));
            BOUNDS = new ConvexPolygon(points, Vector2.Zero);
        }

        public Bullet(CollisionDetectorInterface detector, Vector2 position, Vector2 direction)
            : base(TextureMap.fetchTexture(TEXTURE_NAME), detector, BOUNDS, RADIUS, direction * SPEED, position, direction, HEIGHT)
        {

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
                (collidedInto_ as CharacterAbstract).damage(DAMAGE, this);
            }
            die();
        }
    }
}
