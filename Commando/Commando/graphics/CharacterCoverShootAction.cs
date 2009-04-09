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
using Commando.levels;

namespace Commando.graphics
{
    public class CharacterCoverShootAction : CharacterActionInterface
    {
        private const int PRIORITY = 12;

        protected int priority_;

        protected CharacterAbstract character_;

        protected ActuatorInterface actuator_;

        protected AnimationInterface[] animation_;

        protected int frameToShoot_;

        protected int currentFrame_;

        protected RangedWeaponAbstract weapon_;

        protected bool finished_;

        protected bool holding_;

        protected Height oldHeight_;

        protected string actionLevel_;

        public CharacterCoverShootAction(CharacterAbstract character,
                                            AnimationInterface[] animation,
                                            int frameToShoot,
                                            string actionLevel)
        {
            character_ = character;
            animation_ = animation;
            frameToShoot_ = frameToShoot;
            priority_ = PRIORITY;
            actionLevel_ = actionLevel;
            actuator_ = null;
        }
        
        public void update()
        {
            int animSet = 0;
            if (animation_.GetLength(0) > 1)
            {
                animSet = character_.getActuator().getCurrentAnimationSet();
                
            }
            animation_[animSet].updateFrameNumber(currentFrame_);
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
                if (currentFrame_ >= animation_[animSet].getNumFrames())
                {
                    finished_ = true;
                    character_.setHeight(oldHeight_);
                }
            }
            Vector2 direction = character_.getDirection();
            Vector2 position = character_.getPosition();
            if (animation_.GetLength(0) > 1)
            {animSet = character_.getActuator().getCurrentAnimationSet();
                animation_[animSet].update(position, direction);
            }
            else
            {
                animation_[0].update(position, direction);
            }
            holding_ = false;
        }

        public void draw()
        {
            if (animation_.GetLength(0) > 1)
            {
                int animSet = character_.getActuator().getCurrentAnimationSet();
                animation_[animSet].draw();
            }
            else
            {
                animation_[0].draw();
            }
        }

        public void draw(Color color)
        {
            if (animation_.GetLength(0) > 1)
            {
                int animSet = character_.getActuator().getCurrentAnimationSet();
                animation_[animSet].draw(color);
            }
            else
            {
                animation_[0].draw(color);
            }
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
            if (actuator_ == null)
            {
                actuator_ = character_.getActuator();
            }
            finished_ = false;
            holding_ = false;
            currentFrame_ = 0;
            foreach (AnimationInterface anim in animation_)
            {
                anim.reset();
                anim.setPosition(character_.getPosition());
                anim.setRotation(character_.getDirection());
            }
            oldHeight_ = character_.getHeight();
            character_.setHeight(new Height(true, true));
            weapon_ = character_.Weapon_;
        }

        public string getActionLevel()
        {
            return actionLevel_;
        }

        public void setParameters(ActionParameters parameters)
        {
            
        }

        public Commando.collisiondetection.ConvexPolygonInterface getBounds(HeightEnum height)
        {
            if (animation_.GetLength(0) > 1)
            {
                int animSet = character_.getActuator().getCurrentAnimationSet();
                return animation_[animSet].getBounds();
            }
            return animation_[0].getBounds();
        }
    }
}
