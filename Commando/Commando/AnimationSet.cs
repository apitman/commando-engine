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
    /// <summary>
    /// AnimationSets is used to encapsulate all the animations of a single entity into one
    /// object.  It provides the ability to choose and run an animation at will.
    /// </summary>
    class AnimationSet
    {
        
        protected List<GameTexture> animations_;

        protected int nextFrame_;

        protected int curAnimation_;

        /// <summary>
        /// Create a default AnimationSet.  ****Currently this should never be used!****
        /// </summary>
        public AnimationSet()
        {
        }

        /// <summary>
        /// Create an AnimationSet from the list of animations.
        /// </summary>
        /// <param name="animations">List of GameTextures, each of which is an animation</param>
        public AnimationSet(List<GameTexture> animations)
        {
            animations_ = new List<GameTexture>(animations);
            nextFrame_ = 0;
            curAnimation_ = 0;
        }

        /// <summary>
        /// Start an animation.
        /// </summary>
        /// <param name="animationNumber">The number of the animation to start</param>
        public void startAnimation(int animationNumber)
        {
            curAnimation_ = animationNumber;
            nextFrame_ = 0;
        }

        /// <summary>
        /// Set the frame which will be drawn next.
        /// </summary>
        /// <param name="nextFrame">The number of the frame</param>
        public void setNextFrame(int nextFrame)
        {
            nextFrame_ = nextFrame;
        }

        /// <summary>
        /// Get the next frame to be drawn.
        /// </summary>
        /// <returns>The number of the next frame</returns>
        public int getNextFrame()
        {
            return nextFrame_;
        }

        /// <summary>
        /// Get the current animation.
        /// </summary>
        /// <returns>The number of the current animation</returns>
        public int getCurrentAnimation()
        {
            return curAnimation_;
        }

        /// <summary>
        /// Draw the next frame of the current animation at the specified position with the 
        /// specified rotation and depth.
        /// </summary>
        /// <param name="position">Position relative to the top left corner</param>
        /// <param name="rotation">Rotation of the frame in radians</param>
        /// <param name="depth">Drawing depth of the frame</param>
        public void drawNextFrame(Vector2 position, float rotation, float depth)
        {
            animations_[curAnimation_].drawImage(nextFrame_, position, rotation, depth);
            nextFrame_ = (nextFrame_ + 1) % animations_[curAnimation_].getNumberOfImages();
        }
    }
}
