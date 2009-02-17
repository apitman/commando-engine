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
using Commando.graphics;
using Microsoft.Xna.Framework;

namespace Commando.objects
{
    public class Projectile : MovableObjectAbstract/*, CollisionObjectInterface*/
    {
        protected GameTexture texture_;

        public Projectile() :
            base()
        {
            texture_ = null;
        }

        public Projectile(GameTexture texture) :
            base()
        {
            texture_ = texture;
        }

        public Projectile(GameTexture texture, Vector2 velocity, Vector2 position, Vector2 direction, float depth) :
            base(velocity, position, direction, depth)
        {
            texture_ = texture;
        }

        public override void update(GameTime gameTime)
        {
            position_ += velocity_;
            if (position_.X > 400 || position_.X < 0 ||
                position_.Y > 400 || position_.Y < 0)
            {
                die();
            }
        }

        public override void draw(GameTime gameTime)
        {
            texture_.drawImage(0, position_, getRotationAngle(), depth_);
        }
    }
}
