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
using Commando.graphics.multithreading;

namespace Commando.objects.weapons
{
    internal class Shrapnel : NonAnimatedMovableObjectAbstract
    {
        protected Color color_;
        protected float size_;

        protected int lifeLeft_;

        public Shrapnel(List<DrawableObjectAbstract> pipeline, Vector2 position, Vector2 velocity, float depth, Color color, int lifeLeft, int size)
            : base(pipeline, TextureMap.fetchTexture("Pixel"), 0, velocity, position, Vector2.Zero, depth)
        {
            color_ = color;
            lifeLeft_ = lifeLeft;
            size_ = size;
        }

        public override void update(GameTime gameTime)
        {
            base.update(gameTime);
            lifeLeft_--;
            if (lifeLeft_ <= 0)
                die();
        }

        public override void draw(GameTime gameTime)
        {
            //texture_.drawImageWithColor(0, position_, direction_, depth_, color_);
            DrawStack stack = DrawBuffer.getInstance().getUpdateStack();
            TextureDrawer td = stack.getNext();
            td.Texture = texture_;
            td.ImageIndex = 0;
            td.Position = position_;
            td.Dest = false;
            td.CoordinateType = CoordinateTypeEnum.RELATIVE;
            td.Depth = depth_;
            td.Centered = true;
            td.Color = color_;
            td.Effects = SpriteEffects.None;
            td.Direction = direction_;
            td.Scale = size_;
            stack.push();
        }
    }

    /// <summary>
    /// Basic implementation of pieces of shrapnel which fly out when projectiles
    /// strike objects; this class encapsulates a set of "particles."
    /// </summary>
    static class ShrapnelGenerator
    {
        static ShrapnelInfo DEFAULT_SHRAPNEL_INFO;

        static ShrapnelGenerator()
        {
            DEFAULT_SHRAPNEL_INFO = new ShrapnelInfo();
            DEFAULT_SHRAPNEL_INFO.COUNT_MIN = 3;
            DEFAULT_SHRAPNEL_INFO.COUNT_MAX = 7;
            DEFAULT_SHRAPNEL_INFO.VELOCITY_MIN = -3;
            DEFAULT_SHRAPNEL_INFO.VELOCITY_MAX = 3;
            DEFAULT_SHRAPNEL_INFO.LIFE_MIN = 4;
            DEFAULT_SHRAPNEL_INFO.LIFE_MAX = 10;
            DEFAULT_SHRAPNEL_INFO.SIZE = 1;
        }

        internal static void createShrapnel(List<DrawableObjectAbstract> pipeline, Vector2 pos, Color color, float depth)
        {
            createShrapnel(pipeline, pos, color, depth, ref DEFAULT_SHRAPNEL_INFO);
        }

        internal static void createShrapnel(List<DrawableObjectAbstract> pipeline, Vector2 pos, Color color, float depth, ref ShrapnelInfo info)
        {
            GameTexture image = TextureMap.fetchTexture("Pixel");

            // use a random seed based on position, not time, otherwise all the bullets
            //  in a frame will have the same shrapnel generated (or so it seems)
            Random r = RandomManager.get();
            int count = r.Next(info.COUNT_RANGE) + info.COUNT_MIN;
            for (int i = 0; i < count; i++)
            {
                Vector2 v =
                    new Vector2(r.Next(info.VELOCITY_RANGE) + info.VELOCITY_MIN,
                                r.Next(info.VELOCITY_RANGE) + info.VELOCITY_MIN);
                int life = r.Next(info.LIFE_RANGE) + info.LIFE_MIN;
                
                Shrapnel s = new Shrapnel(pipeline, pos, v, depth, color, life, info.SIZE);
            }
        }
    }

    struct ShrapnelInfo
    {
        internal int COUNT_MIN;
        internal int COUNT_MAX;
        internal int COUNT_RANGE
        {
            get
            {
                return COUNT_MAX - COUNT_MIN;
            }
        }

        internal int VELOCITY_MIN;
        internal int VELOCITY_MAX;
        internal int VELOCITY_RANGE
        {
            get
            {
                return VELOCITY_MAX - VELOCITY_MIN;
            }
        }

        internal int LIFE_MIN;
        internal int LIFE_MAX;
        internal int LIFE_RANGE
        {
            get
            {
                return LIFE_MAX - LIFE_MIN;
            }
        }

        internal int SIZE;
    }
}
