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
    class Shotgun : PlayerWeapon
    {
        protected const string WEAPON_TEXTURE_NAME = "Pistol";

        protected const int TIME_TO_REFIRE = 20;
        protected const float SHOTGUN_SOUND_RADIUS = 250.0f;

        public Shotgun(List<DrawableObjectAbstract> pipeline, CharacterAbstract character, Vector2 gunHandle)
            : base(pipeline, character, TextureMap.fetchTexture(WEAPON_TEXTURE_NAME), gunHandle)
        {
            SOUND_RADIUS = SHOTGUN_SOUND_RADIUS;
        }

        public override void shoot(Commando.collisiondetection.CollisionDetectorInterface detector)
        {
            if (refireCounter_ == 0 && character_.getAmmo().getValue() > 0)
            {
                rotation_.Normalize();
                Vector2 rotation2 = CommonFunctions.rotate(rotation_, -10 * Math.PI / 180f);
                Vector2 rotation3 = CommonFunctions.rotate(rotation_, 10 * Math.PI / 180f);
                rotation2.Normalize();
                rotation3.Normalize();
                Vector2 bulletPos = position_ + rotation_ * 15f;
                Vector2 bulletPos2 = position_ + rotation2 * 15f;
                Vector2 bulletPos3 = position_ + rotation3 * 15f;
                Bullet bullet = new Bullet(drawPipeline_, detector, bulletPos, rotation_);
                Bullet bullet2 = new Bullet(drawPipeline_, detector, bulletPos2, rotation2);
                Bullet bullet3 = new Bullet(drawPipeline_, detector, bulletPos3, rotation3);
                refireCounter_ = TIME_TO_REFIRE;
                character_.getAmmo().update(character_.getAmmo().getValue() - 1);

                weaponFired_ = true;
            }
        }
    }
}
