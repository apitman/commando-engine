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

namespace Commando.graphics
{
    public class CharacterCoverShootAction : ShootActionInterface
    {
        private const int PRIORITY = 12;

        protected int priority_;

        protected CharacterAbstract character_;

        protected AnimationInterface animation_;

        protected int frameToShoot_;

        protected int currentFrame_;

        protected RangedWeaponAbstract weapon_;

        protected bool finished_;

        protected bool holding_;

        public CharacterCoverShootAction(CharacterAbstract character, AnimationInterface animation, int frameToShoot)
        {
            character_ = character;
            animation_ = animation;
            frameToShoot_ = frameToShoot;
            priority_ = PRIORITY;
        }
        
        public void shoot(RangedWeaponAbstract weapon)
        {
            weapon_ = weapon;
        }
        public void update()
        {
            animation_.updateFrameNumber(currentFrame_);
            if (currentFrame_ == frameToShoot_)
            {
                weapon_.shoot();
                if (!holding_)
                {
                    currentFrame_++;
                }
            }
            else
            {
                currentFrame_++;
                if (currentFrame_ >= animation_.getNumFrames())
                {
                    finished_ = true;
                }
            }
            Vector2 direction = character_.getDirection();
            Vector2 position = character_.getPosition();
            animation_.update(position, direction);
            holding_ = false;
        }

        public void draw()
        {
            animation_.draw();
        }

        public void draw(Color color)
        {
            animation_.draw(color);
        }

        public bool isFinished()
        {
            return finished_;
        }

        public CharacterActionInterface interrupt(CharacterActionInterface newAction)
        {
            if (newAction == this)
            {
                holding_ = true;
                return this;
            }
            if (newAction.getPriority() <= priority_)
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
            holding_ = false;
            currentFrame_ = 0;
            animation_.reset();
            animation_.setPosition(character_.getPosition());
            animation_.setRotation(character_.getDirection());
        }
    }
}
