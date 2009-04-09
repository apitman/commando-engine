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
using Commando.controls;
using Commando.graphics;

namespace Commando.objects.weapons
{
    class Shotgun : PlayerWeapon
    {
        protected const string WEAPON_TEXTURE_NAME = "Shotgun";

        protected const AmmoTypeEnum AMMO_TYPE = AmmoTypeEnum.BUCKSHOT;
        protected const int TIME_TO_REFIRE = 20;
        protected const float SHOTGUN_SOUND_RADIUS = 250.0f;
        internal const int CLIP_SIZE = 6;
        protected const int NUM_SHOTS = 8;

        public Shotgun(List<DrawableObjectAbstract> pipeline, CharacterAbstract character, Vector2 gunHandle)
            : base(pipeline, character, TextureMap.fetchTexture(WEAPON_TEXTURE_NAME), gunHandle, AMMO_TYPE, CLIP_SIZE)
        {
            SOUND_RADIUS = SHOTGUN_SOUND_RADIUS;
            CurrentAmmo_ = CLIP_SIZE;
        }

        public override void shoot()
        {
            if (refireCounter_ == 0 && CurrentAmmo_ > 0)
            {
                Random rand = RandomManager.get();
                rotation_.Normalize();
                
                Vector2 bulletPos = position_ + rotation_ * gunLength_;
                for (int i = 0; i < NUM_SHOTS; i++)
                {
                    Vector2 tempPos = bulletPos;
                    tempPos.X += ((float)rand.NextDouble() - 1f) * 2f;
                    tempPos.Y += ((float)rand.NextDouble() - 1f) * 2f;
                    Bullet bullet = new SmallBullet(drawPipeline_, 
                                                    collisionDetector_, 
                                                    tempPos, 
                                                    CommonFunctions.rotate(rotation_, ((float)rand.NextDouble() - 0.5f) * 20f * Math.PI / 180f));
                }
                refireCounter_ = TIME_TO_REFIRE;
                CurrentAmmo_--;
                character_.getAmmo().update(CurrentAmmo_);

                InputSet.getInstance().setToggle(Commando.controls.InputsEnum.RIGHT_TRIGGER);

                weaponFired_ = true;

                SoundEngine.getInstance().playCue("bang_2");
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
