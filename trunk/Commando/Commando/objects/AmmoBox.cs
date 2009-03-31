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
using Commando.levels;

namespace Commando.objects
{
    class AmmoBox : ItemAbstract
    {
        protected static readonly List<Vector2> BOUNDSPOINTS;

        protected static readonly Vector2 CENTER = Vector2.Zero;

        protected const float RADIUS = 17.0f;

        protected static readonly Height HEIGHT = new Height(true, false);

        protected const string TEXTURENAME = "AmmoBoxPistol";

        protected bool toDie_ = false;

        //List<Vector2> points, Vector2 center, float radius, Height height, List<DrawableObjectAbstract> pipeline, GameTexture image, Vector2 position, Vector2 direction, float depth
        static AmmoBox()
        {
            BOUNDSPOINTS = new List<Vector2>();
            BOUNDSPOINTS.Add(new Vector2(-15f, -7.5f));
            BOUNDSPOINTS.Add(new Vector2(15f, -7.5f));
            BOUNDSPOINTS.Add(new Vector2(15f, 7.5f));
            BOUNDSPOINTS.Add(new Vector2(-15f, 7.5f));
        }

        public AmmoBox(CollisionDetectorInterface detector, List<DrawableObjectAbstract> pipeline, Vector2 position, Vector2 direction, float depth)
            : base(detector, BOUNDSPOINTS, CENTER, RADIUS, HEIGHT, pipeline, TextureMap.fetchTexture(TEXTURENAME), position, direction, depth)
        {

        }

        public override void handleCollision(CollisionObjectInterface obj)
        {
            if (obj is CharacterAbstract)
            {
                //CharacterAmmo ammo = (obj as CharacterAbstract).getAmmo();
                (obj as CharacterAbstract).Inventory_.Ammo_[Commando.objects.weapons.AmmoTypeEnum.BUCKSHOT] += Commando.objects.weapons.Shotgun.CLIP_SIZE;
                (obj as CharacterAbstract).Inventory_.Ammo_[Commando.objects.weapons.AmmoTypeEnum.BULLETS] += Commando.objects.weapons.Pistol.CLIP_SIZE;
                (obj as CharacterAbstract).Inventory_.Ammo_[Commando.objects.weapons.AmmoTypeEnum.ROUNDS] += Commando.objects.weapons.MachineGun.CLIP_SIZE;
                //int ammoCount = ammo.getValue();
                //ammoCount += 5;
                //if (ammoCount > 20)
                //{
                //    ammoCount = 20;
                //}
                //ammo.update(ammoCount);
                toDie_ = true;
                hasBeenPickedUp_ = true;
            }
        }

        public override void collidedInto(CollisionObjectInterface obj)
        {
            //
        }

        public override void collidedWith(CollisionObjectInterface obj)
        {
            //
        }

        public override void update(GameTime gameTime)
        {
            base.update(gameTime);
            if (toDie_)
            {
                die();
            }
        }
    }
}
