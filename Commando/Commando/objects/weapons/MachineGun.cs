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
using Microsoft.Xna.Framework;
using Commando.controls;
using Commando.graphics;

namespace Commando.objects.weapons
{
    class MachineGun : PlayerWeapon
    {
        protected const string WEAPON_TEXTURE_NAME = "MachineGun";
        protected const float DRAW_OFFSET = 5f;

        protected const AmmoTypeEnum AMMO_TYPE = AmmoTypeEnum.ROUNDS;
        protected const int TIME_TO_REFIRE = 5;
        protected const float MACHINE_GUN_SOUND_RADIUS = 250.0f;
        protected static readonly GameTexture BULLET_TEXTURE;
        internal const int CLIP_SIZE = 30;

        internal const float MAX_BASE_INACCURACY = 0.01f;
        internal const float INACCURACY_PER_RECOIL = 0.002f;
        internal const int RECOIL_PER_SHOT_FIRED = 12;
        internal const int MAX_RECOIL = 50;

        static MachineGun()
        {
            BULLET_TEXTURE = TextureMap.fetchTexture("BulletSmall");
        }

        public MachineGun(List<DrawableObjectAbstract> pipeline, CharacterAbstract character, Vector2 gunHandle)
            : base(pipeline, character, TextureMap.fetchTexture(WEAPON_TEXTURE_NAME), gunHandle, AMMO_TYPE, CLIP_SIZE)
        {
            SOUND_RADIUS = MACHINE_GUN_SOUND_RADIUS;
            drawOffset_ = DRAW_OFFSET;
            gunTip_ = gunLength_ / 2f + DRAW_OFFSET;
            CurrentAmmo_ = CLIP_SIZE;
        }

        public override void shoot()
        {
            if (refireCounter_ == 0 && CurrentAmmo_ > 0)
            {
                rotation_.Normalize();
                Vector2 bulletPos = position_ + rotation_ * gunTip_;
                float inaccuracy = MAX_BASE_INACCURACY + INACCURACY_PER_RECOIL * recoil_;
                Bullet bullet = new Bullet(drawPipeline_, collisionDetector_, bulletPos, adjustForInaccuracy(rotation_,inaccuracy));
                refireCounter_ = TIME_TO_REFIRE;
                CurrentAmmo_--;
                character_.getAmmo().update(CurrentAmmo_);

                weaponFired_ = true;
                recoil_ += RECOIL_PER_SHOT_FIRED;
                if (recoil_ > MAX_RECOIL)
                    recoil_ = MAX_RECOIL;

                SoundEngine.getInstance().playCue("gunshot");
            }
            else if (refireCounter_ == 0)
            {
                InputSet.getInstance().setToggle(Commando.controls.InputsEnum.RIGHT_TRIGGER);
                if (character_ is ActuatedMainPlayer)
                {
                    character_.getActuator().perform("reload", new ActionParameters());
                }
                else
                {
                    character_.reload();
                }
            }
        }
    }
}
