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
    class AnimationSet
    {
        
        protected List<GameTexture> animations_;

        protected uint nextFrame_;

        protected uint curAnimation_;

        public AnimationSet(List<GameTexture> animations)
        {
            animations_ = new List<GameTexture>(animations);
            nextFrame_ = 0;
            curAnimation_ = 0;
        }

        public void startAnimation(uint animationNumber)
        {
            curAnimation_ = animationNumber;
            nextFrame_ = 0;
        }

        public void setNextFrame(uint nextFrame)
        {
            nextFrame_ = nextFrame;
        }

        public uint getNextFrame()
        {
            return nextFrame_;
        }

        public uint getCurrentAnimation()
        {
            return curAnimation_;
        }

        public void drawNextFrame(Vector2 position, float rotation, float depth)
        {
            animations_[curAnimation_].drawImage(nextFrame_, position, rotation, depth);
            nextFrame_ = (nextFrame_ + 1) % animations_[curAnimation_].getImageDimensions();
        }
    }
}
