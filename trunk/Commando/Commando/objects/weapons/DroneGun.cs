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
using Commando.collisiondetection;
using Commando.controls;

namespace Commando.objects.weapons
{
    public class DroneGun : RangedWeaponAbstract
    {
        protected const AmmoTypeEnum AMMO_TYPE = AmmoTypeEnum.BULLETS;
        protected const int CLIP_SIZE = 10;
        protected const int TIME_TO_REFIRE = 10;

        public DroneGun(List<DrawableObjectAbstract> pipeline, CharacterAbstract character, Vector2 gunHandle)
            : base(pipeline, character, TextureMap.fetchTexture("Pistol"), gunHandle, AMMO_TYPE, CLIP_SIZE)
        {
            this.CurrentAmmo_ = CLIP_SIZE;
        }

        public override void shoot()
        {
            if (refireCounter_ == 0 && CurrentAmmo_ > 0)
            {
                rotation_.Normalize();
                Vector2 pos = position_ + rotation_ * 15f;
                Bullet bullet = new Bullet(drawPipeline_, collisionDetector_, pos, rotation_);
                refireCounter_ = TIME_TO_REFIRE;
                CurrentAmmo_--;
                character_.getAmmo().update(CurrentAmmo_);
            }
            else if (refireCounter_ == 0)
            {
                character_.reload();
            }
        }

        public override void draw()
        {
            // do nothing - no graphic for the DroneGun
        }
    }
}
