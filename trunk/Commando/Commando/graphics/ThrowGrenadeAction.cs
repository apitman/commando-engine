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
using Commando.objects.weapons;
using Microsoft.Xna.Framework;

namespace Commando.graphics
{
    public class ThrowGrenadeAction : CharacterActionInterface
    {
        private const int PRIORITY = 17;

        protected int priority_;

        protected CharacterAbstract character_;

        protected Vector2 throwOffset_;

        protected Grenade grenade_;

        protected bool finished_;

        protected AnimationInterface animation_;

        public ThrowGrenadeAction(CharacterAbstract character, AnimationInterface animation, Vector2 throwOffset)
        {
            character_ = character;
            animation_ = animation;
            throwOffset_ = throwOffset;
            priority_ = PRIORITY;
            finished_ = true;
        }

        public void throwGrenade(Grenade grenade)
        {
            grenade_ = grenade;
        }

        public void update()
        {
            Vector2 position = character_.getPosition();
            Vector2 direction = character_.getDirection();
            float angle = CommonFunctions.getAngle(direction);
            float cosA = (float)Math.Cos(angle);
            float sinA = (float)Math.Sin(angle);
            Vector2 newPos = Vector2.Zero;
            newPos.X = (throwOffset_.X) * cosA - (throwOffset_.Y) * sinA + position.X;
            newPos.Y = (throwOffset_.X) * sinA + (throwOffset_.Y) * cosA + position.Y;
            grenade_.setPosition(newPos);
            grenade_.setDirection(direction);
            finished_ = true;
        }

        public void draw()
        {
            animation_.draw();
        }

        public void draw(Microsoft.Xna.Framework.Graphics.Color color)
        {
            animation_.draw(color);
        }

        public bool isFinished()
        {
            return finished_;
        }

        public CharacterActionInterface interrupt(CharacterActionInterface newAction)
        {
            if (newAction == this || (!finished_ && newAction.getPriority() <= priority_))
            {
                return this;
            }
            newAction.start();
            return newAction;
        }

        public int getPriority()
        {
            return priority_;
        }

        public void setCharacter(CharacterAbstract character)
        {
            character_ = character;
        }

        public void start()
        {
            finished_ = false;
            animation_.reset();
            animation_.setPosition(character_.getPosition());
            animation_.setRotation(character_.getDirection());
        }
    }
}
