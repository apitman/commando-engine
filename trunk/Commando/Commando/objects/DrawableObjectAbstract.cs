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
        }

        public DrawableObjectAbstract(Vector2 position, Vector2 direction, float depth)
        {
        }

        public abstract void update(GameTime gameTime);

        public abstract void draw(GameTime gameTime);

        public Vector2 getPosition()
        {

            return Vector2.Zero;
        }

        public Vector2 getDirection()
        {

            return Vector2.Zero;
        }

        public Vector2 getDepth()
        {

            return 0.0f;
        }

        public void setPosition(Vector2 pos)
        {
        }

        public void setDirection(Vector2 dir)
        {
        }

        public void setDepth(float dep)
        {
        }

        protected float getRotationAngle()
        {

            return 0.0f;
        }
    }
}
