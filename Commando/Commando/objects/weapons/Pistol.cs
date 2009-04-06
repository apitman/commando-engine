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
using Commando.controls;

namespace Commando.objects.weapons
{
    class Pistol : PlayerWeapon
    {
        protected const string WEAPON_TEXTURE_NAME = "Pistol";

        protected const AmmoTypeEnum AMMO_TYPE = AmmoTypeEnum.BULLETS;
        protected const int TIME_TO_REFIRE = 10;
        protected const float PISTOL_SOUND_RADIUS = 150.0f;

        internal const int CLIP_SIZE = 20;

        internal const float MAX_INACCURACY = 0.035f;

        public Pistol(List<DrawableObjectAbstract> pipeline, CharacterAbstract character, Vector2 gunHandle)
            : base(pipeline, character, TextureMap.fetchTexture(WEAPON_TEXTURE_NAME), gunHandle, AMMO_TYPE, CLIP_SIZE)
        {
            laserImage_ = TextureMap.fetchTexture(TARGET_TEXTURE_NAME);
            SOUND_RADIUS = PISTOL_SOUND_RADIUS;
            CurrentAmmo_ = CLIP_SIZE;
            pistol_ = true;
        }

        public override void shoot()
        {
            if (refireCounter_ == 0 && CurrentAmmo_ > 0)
            {
                rotation_.Normalize();
                Vector2 bulletPos = position_ + rotation_ * 15f;
                Bullet bullet = new Bullet(drawPipeline_, collisionDetector_, bulletPos, adjustForInaccuracy(rotation_, MAX_INACCURACY));
                refireCounter_ = TIME_TO_REFIRE;
                CurrentAmmo_--;
                character_.getAmmo().update(CurrentAmmo_);

                InputSet.getInstance().setToggle(Commando.controls.InputsEnum.RIGHT_TRIGGER);

                weaponFired_ = true;
            }
            else if (refireCounter_ == 0)
            {
                InputSet.getInstance().setToggle(Commando.controls.InputsEnum.RIGHT_TRIGGER);
                character_.reload();
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
