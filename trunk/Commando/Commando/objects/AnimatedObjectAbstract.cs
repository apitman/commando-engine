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

        protected int currentAnimation_;

        protected int currentFrame_;

        protected float frameLengthModifier_;

        protected Vector2 moved_;

        public AnimatedObjectAbstract() :
            this(new AnimationSet(), 1.0f)
        {
            
        }

        public AnimatedObjectAbstract(AnimationSet animations, float frameLengthModifier) :
            base()
        {
            animations_ = animations;
            frameLengthModifier_ = frameLengthModifier;
            currentAnimation_ = 0;
            currentFrame_ = 0;
            moved_ = Vector2.Zero;
        }

        public AnimatedObjectAbstract(AnimationSet animations, float frameLengthModifier, Vector2 velocity, Vector2 position, Vector2 direction, float depth) :
            base(velocity, position, direction, depth)
        {
            animations_ = animations;
            frameLengthModifier_ = frameLengthModifier;
            currentAnimation_ = 0;
            currentFrame_ = 0;
            moved_ = Vector2.Zero;
        }

        public int getCurrentFrame()
        {
            return currentFrame_;
        }

        public int getCurrentAnimation()
        {
            return currentAnimation_;
        }

        public AnimationSet getAnimations()
        {
            return animations_;
        }

        public float getFrameLengthModifier()
        {
            return frameLengthModifier_;
        }

        public void setCurrentFrame(int currentFrame)
        {
            currentFrame_ = currentFrame;
        }

        public void setCurrentAnimation(int currentAnimation)
        {
            currentAnimation_ = currentAnimation;
        }

        public void setAnimations(AnimationSet animations)
        {
            animations_ = animations;
        }

        public void setFrameLengthModifier(float frameLengthModifier)
        {
            frameLengthModifier_ = frameLengthModifier;
        }

        //Uses the moved 
        protected void updateFrameNumber()
        {
            float magnitude = (float)Math.Sqrt(Math.Pow((double)moved_.X, 2.0) + Math.Pow((double)moved_.Y, 2.0));
            magnitude /= frameLengthModifier_;
            if (magnitude > 0)
            {
                currentFrame_ += (int)magnitude;
                moved_ = Vector2.Zero;
            }
        }
    }
}
