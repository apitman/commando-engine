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
using Commando.collisiondetection;
using Microsoft.Xna.Framework;
using Commando.ai;
using Commando.levels;

namespace Commando.objects.weapons
{
    class Pistol : WeaponAbstract
    {
        protected const string TEXTURE_NAME = "Pistol";

        protected GameTexture laserImage_;
        protected Vector2 laserTarget_;

        public Pistol(List<DrawableObjectAbstract> pipeline, CharacterAbstract character, Vector2 gunHandle)
            : base(pipeline, character, TextureMap.fetchTexture(TEXTURE_NAME), gunHandle)
        {
            laserImage_ = TextureMap.fetchTexture("laserpointer");
        }

        public override void shoot(CollisionDetectorInterface detector)
        {
            if (refireCounter_ == 0 && character_.getAmmo().getValue() > 0)
            {
                List<Vector2> points = new List<Vector2>();
                points.Add(new Vector2(2f, 2f));
                points.Add(new Vector2(-2f, 2f));
                points.Add(new Vector2(-2f, -2f));
                points.Add(new Vector2(2f, -2f));
                rotation_.Normalize();
                Vector2 pos = position_ + rotation_ * 15f;
                Bullet bullet = new Bullet(detector, pos, rotation_);
                drawPipeline_.Add(bullet);
                refireCounter_ = 10;
                character_.getAmmo().update(character_.getAmmo().getValue() - 1);


                weaponFired_ = true;
            }
        }

        public override void update()
        {
            base.update();

            // TODO Change/fix how this is done, modularize it, etc.
            // Essentially, the player updates his visual location in the WorldState
            // Must remove before adding because Dictionaries don't like duplicate keys
            // Removing a nonexistent key (for first frame) does no harm
            // Also, need to make it so the radius isn't hardcoded - probably all
            //  objects which will have a visual stimulus should have a radius
            WorldState.Audial_.Remove(audialStimulusId_);
            if (weaponFired_)
            {
                weaponFired_ = false;
                WorldState.Audial_.Add(
                    audialStimulusId_,
                    new Stimulus(StimulusSource.CharacterAbstract, StimulusType.Position, 150.0f, character_.getPosition())
                );
            }

            laserTarget_ = Raycaster.roughCollision(position_, rotation_, new Height(false, true));
        }

        public override void draw()
        {
            base.draw();
            if (!Settings.getInstance().UsingMouse_)
                laserImage_.drawImage(0, laserTarget_, Constants.DEPTH_LASER);
        }
    }
}
