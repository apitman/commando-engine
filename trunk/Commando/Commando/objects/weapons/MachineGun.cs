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

namespace Commando.objects.weapons
{
    class MachineGun : PlayerWeapon
    {
        protected const string WEAPON_TEXTURE_NAME = "MachineGun";
        protected const float DRAW_OFFSET = 5f;

        protected const int TIME_TO_REFIRE = 5;
        protected const float MACHINE_GUN_SOUND_RADIUS = 250.0f;
        protected static readonly GameTexture BULLET_TEXTURE;

        static MachineGun()
        {
            BULLET_TEXTURE = TextureMap.fetchTexture("BulletSmall");
        }

        public MachineGun(List<DrawableObjectAbstract> pipeline, CharacterAbstract character, Vector2 gunHandle)
            : base(pipeline, character, TextureMap.fetchTexture(WEAPON_TEXTURE_NAME), gunHandle)
        {
            SOUND_RADIUS = MACHINE_GUN_SOUND_RADIUS;
            drawOffset_ = DRAW_OFFSET;
            gunTip_ = gunLength_ / 2f + DRAW_OFFSET;
        }

        public override void shoot(Commando.collisiondetection.CollisionDetectorInterface detector)
        {
            if (refireCounter_ == 0 && character_.getAmmo().getValue() > 0)
            {
                rotation_.Normalize();
                Vector2 bulletPos = position_ + rotation_ * gunTip_;
                Bullet bullet = new Bullet(drawPipeline_, detector, bulletPos, rotation_);
                refireCounter_ = TIME_TO_REFIRE;
                character_.getAmmo().update(character_.getAmmo().getValue() - 1);

                weaponFired_ = true;
            }
        }
    }
}
