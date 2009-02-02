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

namespace Commando.objects
{
    public class BasicLoopAnimation : AnimationInterface
    {
        protected GameObject owner_;

        AnimationSet animations_;

        int currentAnimation_;

        int currentFrame_;

        protected float frameLengthModifier_;

        protected Vector2 moved_;

        public BasicLoopAnimation(GameObject owner, AnimationSet animations, float frameLengthModifier)
        {
            owner_ = owner;
            animations_ = animations;
            frameLengthModifier_ = frameLengthModifier;
            currentAnimation_ = 0;
            currentFrame_ = 0;
            moved_ = Vector2.Zero;
        }

        #region AnimationInterface Members

        public void draw()
        {
            
        }

        public void setCurrentAnimation(int animation)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region ComponentInterface Members

        public void update()
        {
            throw new NotImplementedException();
        }

        public GameObject getOwner()
        {
            return owner_;
        }

        #endregion

        /// <summary>
        /// Get the current frame of the current animation.
        /// </summary>
        /// <returns>Current frame's frame number.</returns>
        public int getCurrentFrame()
        {
            return currentFrame_;
        }

        /// <summary>
        /// Get the current animation.
        /// </summary>
        /// <returns>Current animation's number</returns>
        public int getCurrentAnimation()
        {
            return currentAnimation_;
        }

        /// <summary>
        /// Get this object's animation set.
        /// </summary>
        /// <returns>AnimationSet of this object.</returns>
        public AnimationSet getAnimations()
        {
            return animations_;
        }

        /// <summary>
        /// Get this object's frameLengthModifier.
        /// </summary>
        /// <returns>Float representing the ratio of frames in an animation to movement along the screen</returns>
        public float getFrameLengthModifier()
        {
            return frameLengthModifier_;
        }

        /// <summary>
        /// Set the object's current frame.
        /// </summary>
        /// <param name="currentFrame">Current frame number of this object's current animation</param>
        public void setCurrentFrame(int currentFrame)
        {
            currentFrame_ = currentFrame;
        }

        /// <summary>
        /// Set the object's AnimationSet.
        /// </summary>
        /// <param name="animations">The object's new AnimationSet</param>
        public void setAnimations(AnimationSet animations)
        {
            animations_ = animations;
        }

        /// <summary>
        /// Set the object's frameLengthModifier.
        /// </summary>
        /// <param name="frameLengthModifier">Float representing the ratio of frames in an animation to movement along the screen</param>
        public void setFrameLengthModifier(float frameLengthModifier)
        {
            frameLengthModifier_ = frameLengthModifier;
        }

        /// <summary>
        /// Use the moved_ Vector and the frameLengthModifier_ to set the currentFrame to the correct value.
        /// </summary>
        protected void updateFrameNumber()
        {
            float magnitude = moved_.Length();
            magnitude /= frameLengthModifier_;
            if (magnitude >= 1.0f)
            {
                currentFrame_ += (int)magnitude;
                Vector2 temp = moved_;
                temp.Normalize();
                moved_ -= temp * frameLengthModifier_;
            }
        }
    }
}
