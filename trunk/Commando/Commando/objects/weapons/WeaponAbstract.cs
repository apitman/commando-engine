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
using Commando.graphics;
using Commando.objects;
using Microsoft.Xna.Framework;
using Commando.collisiondetection;
using Commando.ai;
using Commando.levels;

namespace Commando
{
    public abstract class WeaponAbstract
    {
        protected List<DrawableObjectAbstract> drawPipeline_;
        
        protected CharacterAbstract character_;

        protected GameTexture texture_;

        /// <summary>
        /// Position of back-end of gun.
        /// </summary>
        protected Vector2 position_;

        /// <summary>
        /// Rotation of weapon.
        /// </summary>
        protected Vector2 rotation_;

        /// <summary>
        /// Position of back end of gun when the character is facing right; used to calculate
        /// position of gun relative to character.
        /// </summary>
        protected Vector2 gunHandle_;

        /// <summary>
        /// Distance from back end of gun to front end of gun.
        /// </summary>
        protected float gunLength_;

        /// <summary>
        /// Amount of time before the gun can fire again.
        /// </summary>
        protected int refireCounter_;

        protected int audialStimulusId_;

        public WeaponAbstract(List<DrawableObjectAbstract> pipeline, CharacterAbstract character, GameTexture animation, Vector2 gunHandle)
        {
            drawPipeline_ = pipeline;
            character_ = character;
            texture_ = animation;
            gunHandle_ = gunHandle;
            position_ = Vector2.Zero;
            rotation_ = Vector2.Zero;
            gunLength_ = animation.getImageDimensions()[0].Width;
            refireCounter_ = 0;
            audialStimulusId_ = StimulusIDGenerator.getNext();
        }

        public void setDrawPipeline(List<DrawableObjectAbstract> pipeline)
        {
            drawPipeline_ = pipeline;
        }

        public abstract void shoot(CollisionDetectorInterface detector);

        public virtual void update()
        {
            Vector2 charPos = character_.getPosition();
            Vector2 newPos = Vector2.Zero;
            rotation_ = character_.getDirection();
            float angle = CommonFunctions.getAngle(rotation_);
            float cosA = (float)Math.Cos(angle);
            float sinA = (float)Math.Sin(angle);
            newPos.X = (gunHandle_.X) * cosA - (gunHandle_.Y) * sinA + charPos.X;
            newPos.Y = (gunHandle_.X) * sinA + (gunHandle_.Y) * cosA + charPos.Y;
            position_ = newPos;
            if (refireCounter_ > 0)
                refireCounter_--;
        }

        public virtual void draw()
        {
            rotation_.Normalize();
            rotation_ *= gunLength_ / 2f;

            // TODO make this depth dependent on character depth
            texture_.drawImage(0, position_ + rotation_, CommonFunctions.getAngle(rotation_), 0.6f);
        }
    }
}
