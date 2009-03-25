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

namespace Commando.objects.weapons
{
    internal class FragGrenade : Grenade
    {
        protected const string TEXTURE_KEY = "FragGrenade";
        protected const float RANGE_RADIUS = 30;
        protected const int NUM_PIECES = 8;

        internal FragGrenade(List<DrawableObjectAbstract> pipeline, CollisionDetectorInterface detector, Vector2 direction, Vector2 position, Vector2 directionFacing)
            : base(pipeline, TextureMap.fetchTexture(TEXTURE_KEY), detector, direction, position, directionFacing)
        {

        }

        public override void explode()
        {
            double fractionAngle = MathHelper.TwoPi / NUM_PIECES;

            for (int i = 0; i < NUM_PIECES; i++)
            {
                double rotation = fractionAngle * i;
                Vector2 dir = CommonFunctions.getVector(rotation);
                BulletLimitedRange bullet =
                    new BulletLimitedRange(pipeline_, collisionDetector_, position_, dir, RANGE_RADIUS);
            }
            die();
        }
    }
}
