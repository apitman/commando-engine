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
    abstract class AnimatedObjectAbstract : MovableObjectAbstract
    {

        protected AnimationSet animations_;

        protected uint currentAnimation_;

        protected uint currentFrame_;

        protected float frameLengthModifier_;

        protected Vector2 moved_;

        public AnimatedObjectAbstract() :
            base()
        {
        }

        public AnimatedObjectAbstract(AnimationSet animations, float frameLengthModifier) :
            base()
        {
        }

        public AnimatedObjectAbstract(AnimationSet animations, float frameLengthModifier, Vector2 velocity, Vector2 position, Vector2 direction, float depth) :
            base(velocity, position, direction, depth)
        {
        }

        public uint getCurrentFrame()
        {

            return 0;
        }

        public uint getCurrentAnimation()
        {

            return 0;
        }

        public AnimationSet getAnimations()
        {

            return null;
        }

        public float getFrameLengthModifier()
        {

            return 0.0f;
        }

        public void setCurrentFrame(uint currentFrame)
        {
        }

        public void setCurrentAnimation(uint currentAnimation)
        {
        }

        public void setAnimations(AnimationSet animations)
        {
        }

        public void setFrameLengthModifier(float frameLengthModifier)
        {
        }

        //Uses the moved 
        protected void updateFrameNumber()
        {
        }
    }
}
