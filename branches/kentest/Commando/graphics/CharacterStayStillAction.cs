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

namespace Commando.graphics
{
    public class CharacterStayStillAction : CharacterActionInterface
    {
        private const int PRIORITY = 0;

        protected CharacterAbstract character_;

        protected int priority_;

        protected AnimationInterface animation_;

        public CharacterStayStillAction(CharacterAbstract character, AnimationInterface animation)
        {
            priority_ = PRIORITY;
            character_ = character;
            animation_ = animation;
        }

        public void update()
        {
            animation_.setPosition(character_.getPosition());
            animation_.setRotation(character_.getDirection());
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
            return true;
        }

        public CharacterActionInterface interrupt(CharacterActionInterface newAction)
        {
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
            animation_.reset();
            animation_.setPosition(character_.getPosition());
            animation_.setRotation(character_.getDirection());
        }
    }
}
