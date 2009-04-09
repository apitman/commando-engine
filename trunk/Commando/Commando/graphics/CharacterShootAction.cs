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

namespace Commando.graphics
{
    public class CharacterShootAction : CharacterActionInterface
    {
        private const int PRIORITY = 10;

        protected int priority_;

        protected string actionLevel_;

        protected AnimationInterface[] animation_;

        protected CharacterAbstract character_;

        protected bool finished_;

        protected int shootFrame_;

        protected RangedWeaponAbstract weapon_;

        public CharacterShootAction(CharacterAbstract character,
                                    AnimationInterface[] animation,
                                    string actionLevel,
                                    int shootFrame)
        {
            character_ = character;
            animation_ = animation;
            actionLevel_ = actionLevel;
            shootFrame_ = shootFrame;
            finished_ = true;
        }

        public void update()
        {
            
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

        public void draw(Microsoft.Xna.Framework.Graphics.Color color)
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
            if (newAction == this || (!finished_ && newAction.getPriority() <= priority_))
            {
                return this;
            }
            newAction.start();
            return newAction;
        }

        public int getPriority()
        {
            return PRIORITY;
        }

        public void setCharacter(CharacterAbstract character)
        {
            character_ = character;
        }

        public void start()
        {
            
        }

        public string getActionLevel()
        {
            return actionLevel_;
        }

        public void setParameters(ActionParameters parameters)
        {
            weapon_ = character_.Weapon_;
            if (shootFrame_ == 0)
            {
                weapon_.shoot();
            }
        }

        public Commando.collisiondetection.ConvexPolygonInterface getBounds(Commando.levels.HeightEnum height)
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
