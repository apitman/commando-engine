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
        protected static readonly ConvexPolygon BOUNDS;

        static Grenade()
        {
            List<Vector2> points = new List<Vector2>();
            points.Add(new Vector2(3f, 3f));
            points.Add(new Vector2(-3f, 3f));
            points.Add(new Vector2(-3f, -3f));
            points.Add(new Vector2(3f, -3f));
            BOUNDS = new ConvexPolygon(points, Vector2.Zero);
        }

        public Grenade(List<DrawableObjectAbstract> pipeline, GameTexture texture, CollisionDetectorInterface detector, Vector2 direction, Vector2 position, Vector2 directionFacing)
            : base(pipeline, texture, detector, BOUNDS, RADIUS, HEIGHT, CommonFunctions.normalizeNonmutating(direction) * SPEED, position, directionFacing, DEPTH)
        {

        }

        public override void update(GameTime gameTime)
        {
            base.update(gameTime);
        }

        public override void handleCollision()
        {
            explode();
        }

        public abstract void explode();

        public override bool objectChangesHeight(CollisionObjectInterface obj)
        {
            return false;
        }
    }
}
