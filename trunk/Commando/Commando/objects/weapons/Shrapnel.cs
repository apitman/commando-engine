﻿/*
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
    class Shrapnel : MovableObjectAbstract
    {
        protected GameTexture image_;
        protected Color color_;
        //protected float depth_;

        protected List<Vector2> piecePositions_ = new List<Vector2>();
        protected List<Vector2> pieceVelocities_ = new List<Vector2>();
        protected List<int> pieceLifespans_ = new List<int>();

        protected int timeAlive_;
        protected int timeDeath_;

        protected const int DEFAULT_COUNT_MIN = 3;
        protected const int DEFAULT_COUNT_MAX = 7;
        protected const int DEFAULT_COUNT_RANGE = DEFAULT_COUNT_MAX - DEFAULT_COUNT_MIN;

        protected const int DEFAULT_VELOCITY_MIN = -3;
        protected const int DEFAULT_VELOCITY_MAX = 3;
        protected const int DEFAULT_VELOCITY_RANGE = DEFAULT_VELOCITY_MAX - DEFAULT_VELOCITY_MIN;

        protected const int DEFAULT_LIFE_MIN = 4;
        protected const int DEFAULT_LIFE_MAX = 10;
        protected const int DEFAULT_LIFE_RANGE = DEFAULT_LIFE_MAX - DEFAULT_LIFE_MIN;

        public Shrapnel(List<DrawableObjectAbstract> pipeline, Vector2 pos, Color color, float depth)
        {
            pipeline.Add(this);
            image_ = TextureMap.fetchTexture("Pixel");
            color_ = color;
            depth_ = depth;

            // use a random seed based on position, not time, otherwise all the bullets
            //  in a frame will have the same shrapnel generated (or so it seems)
            Random r = new Random((int)pos.X + (int)pos.Y);
            int count = r.Next(DEFAULT_COUNT_RANGE) + DEFAULT_COUNT_MIN;
            int biggestlife = int.MinValue;
            for (int i = 0; i < count; i++)
            {
                piecePositions_.Add(pos);
                Vector2 v =
                    new Vector2(r.Next(DEFAULT_VELOCITY_RANGE) + DEFAULT_VELOCITY_MIN,
                                r.Next(DEFAULT_VELOCITY_RANGE) + DEFAULT_VELOCITY_MIN);
                pieceVelocities_.Add(v);
                int life = r.Next(DEFAULT_LIFE_RANGE) + DEFAULT_LIFE_MIN;
                if (life > biggestlife)
                    biggestlife = life;
                pieceLifespans_.Add(life);
            }
            timeAlive_ = 0;
            timeDeath_ = biggestlife;
        }

        public override void update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            for (int i = 0; i < piecePositions_.Count; i++)
            {
                piecePositions_[i] += pieceVelocities_[i];
            }
            timeAlive_++;
            if (timeAlive_ > timeDeath_)
                this.die();
        }

        public override void draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            for (int i = 0; i < piecePositions_.Count; i++)
            {
                if (pieceLifespans_[i] >= timeAlive_)
                    image_.drawImage(0, piecePositions_[i], depth_);
            }
        }
    }
}
