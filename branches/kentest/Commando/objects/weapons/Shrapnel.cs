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
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Commando.objects.weapons
{
    internal class Shrapnel : NonAnimatedMovableObjectAbstract
    {
        protected Color color_;
        //protected float depth_;

        protected int lifeLeft_;

        public Shrapnel(List<DrawableObjectAbstract> pipeline, Vector2 position, Vector2 velocity, float depth, Color color, int lifeLeft)
            : base(pipeline, TextureMap.fetchTexture("Pixel"), 0, velocity, position, Vector2.Zero, depth)
        {
            color_ = color;
            lifeLeft_ = lifeLeft;
        }

        public override void update(GameTime gameTime)
        {
            //base.update(gameTime);
            lifeLeft_--;
            if (lifeLeft_ <= 0)
                die();
        }

        public override void draw(GameTime gameTime)
        {
            // TODO figure this crap out
            //texture_.drawImageWithColor(0, position_, direction_, depth_, color_);
        }
    }

    /// <summary>
    /// Basic implementation of pieces of shrapnel which fly out when projectiles
    /// strike objects; this class encapsulates a set of "particles."
    /// </summary>
    static class ShrapnelGenerator
    {
        const int DEFAULT_COUNT_MIN = 3;
        const int DEFAULT_COUNT_MAX = 7;
        const int DEFAULT_COUNT_RANGE = DEFAULT_COUNT_MAX - DEFAULT_COUNT_MIN;

        const int DEFAULT_VELOCITY_MIN = -3;
        const int DEFAULT_VELOCITY_MAX = 3;
        const int DEFAULT_VELOCITY_RANGE = DEFAULT_VELOCITY_MAX - DEFAULT_VELOCITY_MIN;

        const int DEFAULT_LIFE_MIN = 4;
        const int DEFAULT_LIFE_MAX = 10;
        const int DEFAULT_LIFE_RANGE = DEFAULT_LIFE_MAX - DEFAULT_LIFE_MIN;

        internal static void createShrapnel(List<DrawableObjectAbstract> pipeline, Vector2 pos, Color color, float depth)
        {
            GameTexture image = TextureMap.fetchTexture("Pixel");

            // use a random seed based on position, not time, otherwise all the bullets
            //  in a frame will have the same shrapnel generated (or so it seems)
            Random r = new Random((int)pos.X + (int)pos.Y);
            int count = r.Next(DEFAULT_COUNT_RANGE) + DEFAULT_COUNT_MIN;
            int biggestlife = int.MinValue;
            for (int i = 0; i < count; i++)
            {
                Vector2 v =
                    new Vector2(r.Next(DEFAULT_VELOCITY_RANGE) + DEFAULT_VELOCITY_MIN,
                                r.Next(DEFAULT_VELOCITY_RANGE) + DEFAULT_VELOCITY_MIN);
                int life = r.Next(DEFAULT_LIFE_RANGE) + DEFAULT_LIFE_MIN;
                if (life > biggestlife)
                    biggestlife = life;
                
                Shrapnel s = new Shrapnel(pipeline, pos, v, depth, color, life);
            }
        }
    }
}
