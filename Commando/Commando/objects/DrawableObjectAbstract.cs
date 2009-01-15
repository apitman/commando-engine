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

namespace Commando
{
    abstract class DrawableObjectAbstract
    {

        protected Vector2 position_;

        protected Vector2 direction_;

        protected float depth_;

        public DrawableObjectAbstract()
        {
            position_ = Vector2.Zero;
            position_ = new Vector2(1.0f, 0.0f);
            depth_ = 0.5f;
        }

        public DrawableObjectAbstract(Vector2 position, Vector2 direction, float depth)
        {
            position_ = position;
            direction_ = direction;
            depth_ = depth;
        }

        public abstract void update(GameTime gameTime);

        public abstract void draw(GameTime gameTime);

        public Vector2 getPosition()
        {
            return position_;
        }

        public Vector2 getDirection()
        {
            return direction_;
        }

        public float getDepth()
        {
            return depth_;
        }

        public void setPosition(Vector2 pos)
        {
            position_ = pos;
        }

        public void setDirection(Vector2 dir)
        {
            direction_ = dir;
        }

        public void setDepth(float dep)
        {
            depth_ = dep;
        }

        protected float getRotationAngle()
        {
            return (float)Math.Atan2((double)position_.Y, (double)direction_.X);
        }
    }
}
