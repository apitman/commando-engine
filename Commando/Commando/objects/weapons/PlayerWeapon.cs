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
using Commando.ai;
using Commando.levels;
using Microsoft.Xna.Framework.Graphics;
using Commando.graphics.multithreading;

namespace Commando.objects.weapons
{
    /// <summary>
    /// Weapon which is usable by the player; controls laser pointer and
    /// producing suspicious noises.
    /// </summary>
    abstract class PlayerWeapon : RangedWeaponAbstract
    {
        protected const string TARGET_TEXTURE_NAME = "laserpointer";

        protected GameTexture laserImage_;
        protected Vector2 laserTarget_;

        protected bool weaponFired_;

        protected static float SOUND_RADIUS;
        protected static readonly Vector2 LASER_OFFSET = new Vector2(2.5f, 2.5f);

        public PlayerWeapon(List<DrawableObjectAbstract> pipeline, CharacterAbstract character,
                GameTexture animation, Vector2 gunHandle, AmmoTypeEnum ammoType, int clipSize)
            : base(pipeline, character, animation, gunHandle, ammoType, clipSize)
        {
            laserImage_ = TextureMap.fetchTexture(TARGET_TEXTURE_NAME);
            weaponFired_ = false;
        }

        public override void update()
        {
            base.update();
            laserTarget_
                = Raycaster.roughCollision(position_, rotation_, new Height(false, true)) - LASER_OFFSET;

            WorldState.Audial_.Remove(audialStimulusId_);
            if (weaponFired_)
            {
                weaponFired_ = false;
                WorldState.Audial_.Add(
                    audialStimulusId_,
                    new Stimulus(StimulusSource.CharacterAbstract, this.character_.Allegiance_, StimulusType.Position, SOUND_RADIUS, this.position_, this)
                );
            }
        }

        public override void draw()
        {
            base.draw();
            //if (!Settings.getInstance().UsingMouse_)
            //TextureDrawer td =
            //    laserImage_.getDrawer(laserTarget_, Constants.DEPTH_LASER);
            //td.Color = Color.Red;
            //td.draw();
            DrawStack stack = DrawBuffer.getInstance().getUpdateStack();
            TextureDrawer td = stack.getNext();
            td.Texture = laserImage_;
            td.ImageIndex = 0;
            td.Position = laserTarget_;
            td.Dest = false;
            td.CoordinateType = CoordinateTypeEnum.RELATIVE;
            td.Depth = Constants.DEPTH_LASER;
            td.Centered = true;
            td.Color = Color.Red;
            td.Effects = SpriteEffects.None;
            td.Rotation = 0.0f;
            td.Scale = 1.0f;
            stack.push();
        }
    }
}
