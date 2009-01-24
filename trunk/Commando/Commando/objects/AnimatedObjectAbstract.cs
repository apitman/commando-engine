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
    /// AnimatedObjects are MovableObjects that are animated.  The contain an AnimationSet,
    /// which consists of all the animations for this object.  They provide a system for 
    /// updating the current animation based on the movement of the object.
    /// </summary>
    abstract class AnimatedObjectAbstract : MovableObjectAbstract
    {

        protected AnimationSet animations_;

        protected int currentAnimation_;

        protected int currentFrame_;

        protected float frameLengthModifier_;

        protected Vector2 moved_;

        /// <summary>
        /// Create a default AnimatedObject.
        /// </summary>
        public AnimatedObjectAbstract() :
            this(new AnimationSet(), 1.0f)
        {
            
        }

        /// <summary>
        /// Create an AnimatedObject with the specified animations and frameLengthModifier
        /// </summary>
        /// <param name="animations">AnimationSet containing all animations for this object</param>
        /// <param name="frameLengthModifier">Float representing the ratio of frames in an animation to movement along the screen</param>
        public AnimatedObjectAbstract(AnimationSet animations, float frameLengthModifier) :
            base()
        {
            animations_ = animations;
            frameLengthModifier_ = frameLengthModifier;
            currentAnimation_ = 0;
            currentFrame_ = 0;
            moved_ = Vector2.Zero;
        }

        /// <summary>
        /// Create an AnimatedObject with the specified animations, frameLengthModifier,
        /// velocity, position, direction, and depth.
        /// </summary>
        /// <param name="animations">AnimationSet containing all animations for this object</param>
        /// <param name="frameLengthModifier">Float representing the ratio of frames in an animation to movement along the screen</param>
        /// <param name="velocity">Vector of velocity, representing both direction of movement and magnitude</param>
        /// <param name="position">Position of object relative to the top left corner</param>
        /// <param name="direction">Vector representing the direction of the object</param>
        /// <param name="depth">Depth the object is to be drawn to</param>
        public AnimatedObjectAbstract(AnimationSet animations, float frameLengthModifier, Vector2 velocity, Vector2 position, Vector2 direction, float depth) :
            base(velocity, position, direction, depth)
        {
            animations_ = animations;
            frameLengthModifier_ = frameLengthModifier;
            currentAnimation_ = 0;
            currentFrame_ = 0;
            moved_ = Vector2.Zero;
        }

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
        /// Set the object's current animation.
        /// </summary>
        /// <param name="currentAnimation">The number of the current animation.</param>
        public void setCurrentAnimation(int currentAnimation)
        {
            currentAnimation_ = currentAnimation;
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
