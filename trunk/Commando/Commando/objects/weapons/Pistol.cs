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

using System.Collections.Generic;
using Commando.ai;
using Commando.collisiondetection;
using Commando.levels;
using Microsoft.Xna.Framework;

namespace Commando.objects.weapons
{
    class Pistol : PlayerWeapon
    {
        protected const string WEAPON_TEXTURE_NAME = "Pistol";

        protected const int TIME_TO_REFIRE = 10;
        protected const float PISTOL_SOUND_RADIUS = 150.0f;

        public Pistol(List<DrawableObjectAbstract> pipeline, CharacterAbstract character, Vector2 gunHandle)
            : base(pipeline, character, TextureMap.fetchTexture(WEAPON_TEXTURE_NAME), gunHandle)
        {
            laserImage_ = TextureMap.fetchTexture(TARGET_TEXTURE_NAME);
            SOUND_RADIUS = PISTOL_SOUND_RADIUS;
        }

        public override void shoot(CollisionDetectorInterface detector)
        {
            if (refireCounter_ == 0 && character_.getAmmo().getValue() > 0)
            {
                rotation_.Normalize();
                Vector2 bulletPos = position_ + rotation_ * 15f;
                Bullet bullet = new Bullet(drawPipeline_, detector, bulletPos, rotation_);
                refireCounter_ = TIME_TO_REFIRE;
                character_.getAmmo().update(character_.getAmmo().getValue() - 1);

                weaponFired_ = true;
            }
        }

        public override void update()
        {
            base.update();
        }

        public override void draw()
        {
            base.draw();
        }
    }
}