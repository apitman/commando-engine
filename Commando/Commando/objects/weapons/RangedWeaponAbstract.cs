﻿/*
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
using Commando.objects.weapons;
using Commando.graphics.multithreading;
using Microsoft.Xna.Framework.Graphics;

namespace Commando
{
    public abstract class RangedWeaponAbstract
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

        protected float gunTip_;

        /// <summary>
        /// Amount of time before the gun can fire again.
        /// </summary>
        protected int refireCounter_ = 0;

        protected int recoil_ = 0;

        internal int CurrentAmmo_ { get; set; }
        internal int ClipSize_ { get; set; }
        internal AmmoTypeEnum AmmoType_ { get; set; }

        protected float drawOffset_;

        protected int audialStimulusId_;

        protected bool pistol_ = false;

        protected CollisionDetectorInterface collisionDetector_;

        public RangedWeaponAbstract(List<DrawableObjectAbstract> pipeline,
            CharacterAbstract character, GameTexture animation, Vector2 gunHandle,
            AmmoTypeEnum ammoType, int clipSize)
        {
            drawPipeline_ = pipeline;
            character_ = character;
            texture_ = animation;
            gunHandle_ = gunHandle;
            position_ = Vector2.Zero;
            rotation_ = Vector2.Zero;
            gunLength_ = animation.getImageDimensions()[0].Width;
            audialStimulusId_ = StimulusIDGenerator.getNext();
            drawOffset_ = gunLength_ / 2f;
            gunTip_ = gunLength_;

            AmmoType_ = ammoType;
            ClipSize_ = clipSize;
            collisionDetector_ = character.getCollisionDetector();
        }

        public void setDrawPipeline(List<DrawableObjectAbstract> pipeline)
        {
            drawPipeline_ = pipeline;
        }

        public abstract void shoot();

        public virtual void update()
        {
            if (character_.getGunHandle(pistol_) != Vector2.Zero)
            {
                gunHandle_ = character_.getGunHandle(pistol_);
            }
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
            if (recoil_ > 0)
                recoil_--;
        }

        public virtual void draw()
        {
            rotation_.Normalize();
            rotation_ *= drawOffset_;

            // TODO make this depth dependent on character depth
            //texture_.drawImage(0, position_ + rotation_, CommonFunctions.getAngle(rotation_), 0.6f);
            DrawStack stack = DrawBuffer.getInstance().getUpdateStack();
            TextureDrawer td = stack.getNext();
            td.Texture = texture_;
            td.ImageIndex = 0;
            td.Position = position_ + rotation_;
            td.Dest = false;
            td.CoordinateType = CoordinateTypeEnum.RELATIVE;
            td.Depth = character_.getDepth() - 0.005f;
            td.Centered = true;
            td.Color = Color.White;
            td.Effects = SpriteEffects.None;
            td.Direction = rotation_;
            td.Scale = 1.0f;
            stack.push();
        }

        public void setCollisionDetector(CollisionDetectorInterface detector)
        {
            collisionDetector_ = detector;
        }

        protected Vector2 adjustForInaccuracy(Vector2 rotation, float maxInaccuracy)
        {
            return
                CommonFunctions.rotate(
                    rotation,
                    RandomManager.nextNormalDistValue(0, maxInaccuracy)
                );
        }
    }
}
