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
using Commando.objects.weapons;

namespace Commando.objects
{
    class WeaponBox : ItemAbstract
    {
        public WeaponType WeapnType { get; set; }

        protected static readonly List<Vector2> BOUNDSPOINTS;

        protected static readonly Vector2 CENTER = Vector2.Zero;

        protected const float RADIUS = 17.0f;

        protected static readonly Height HEIGHT = new Height(true, false);

        protected const string PISTOLTEXTURE = "PistolIcon";

        protected const string MACHINEGUNTEXTURE = "MachineGunIcon";

        protected const string SHOTGUNTEXTURE = "ShotgunIcon";

        protected bool toDie_ = false;

        //List<Vector2> points, Vector2 center, float radius, Height tileHeight, List<DrawableObjectAbstract> pipeline, GameTexture image, Vector2 position, Vector2 direction, float depth
        static WeaponBox()
        {
            BOUNDSPOINTS = new List<Vector2>();
            BOUNDSPOINTS.Add(new Vector2(-15f, -7.5f));
            BOUNDSPOINTS.Add(new Vector2(15f, -7.5f));
            BOUNDSPOINTS.Add(new Vector2(15f, 7.5f));
            BOUNDSPOINTS.Add(new Vector2(-15f, 7.5f));
        }

        public WeaponBox(CollisionDetectorInterface detector, List<DrawableObjectAbstract> pipeline, Vector2 position, Vector2 direction, float depth, WeaponType type)
            : base(detector, BOUNDSPOINTS, CENTER, RADIUS, HEIGHT, pipeline, TextureMap.fetchTexture(selectTexture(type)), position, direction, depth)
        {
            WeapnType = type;
        }

        public override void handleCollision(CollisionObjectInterface obj)
        {
            if (obj is CharacterAbstract)
            {
                bool weaponWasInInventory = false;
                Inventory inv = (obj as CharacterAbstract).Inventory_;
                switch (WeapnType)
                {
                    case WeaponType.Pistol:
                        foreach (RangedWeaponAbstract wp in inv.Weapons_)
                        {
                            if (wp is Pistol)
                            {
                                wp.CurrentAmmo_ += Pistol.CLIP_SIZE;
                                weaponWasInInventory = true;
                            }
                        }
                        if (!weaponWasInInventory)
                        {
                            inv.Weapons_.Enqueue(new Pistol(pipeline_, (obj as CharacterAbstract), new Vector2(60f - 37.5f, 33.5f - 37.5f)));
                        }
                        break;
                    case WeaponType.Shotgun:
                        foreach (RangedWeaponAbstract wp in inv.Weapons_)
                        {
                            if (wp is Shotgun)
                            {
                                wp.CurrentAmmo_ += Shotgun.CLIP_SIZE;
                                weaponWasInInventory = true;
                            }
                        }
                        if (!weaponWasInInventory)
                        {
                            inv.Weapons_.Enqueue(new Shotgun(pipeline_, (obj as CharacterAbstract), new Vector2(42f - 37.5f, 47f - 37.5f)));
                        }
                        break;
                    case WeaponType.MachineGun:
                        foreach (RangedWeaponAbstract wp in inv.Weapons_)
                        {
                            if (wp is MachineGun)
                            {
                                wp.CurrentAmmo_ += MachineGun.CLIP_SIZE;
                                weaponWasInInventory = true;
                            }
                        }
                        if (!weaponWasInInventory)
                        {
                            inv.Weapons_.Enqueue(new MachineGun(pipeline_, (obj as CharacterAbstract), new Vector2(42f - 37.5f, 47f - 37.5f)));
                        }
                        break;
                }
                (obj as CharacterAbstract).Inventory_ = inv;
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
        public override void  draw(GameTime gameTime)
        {       
             TextureDrawer td = new TextureDrawer(image_, position_, depth_);
             td.Scale = 0.20f;
             td.draw();
 	        
        }
        public enum WeaponType
        {
            Pistol,
            Shotgun,
            MachineGun
        }
        public static string selectTexture(WeaponType WeapnType)
        {
            switch (WeapnType)
            {
                case WeaponType.Pistol:
                    return PISTOLTEXTURE;
                    
                case WeaponType.MachineGun:
                    return MACHINEGUNTEXTURE;
                    
                case WeaponType.Shotgun:
                    return SHOTGUNTEXTURE;
                    
                default:throw(new NotImplementedException("not a weapon"));
            }

        }
    }
}
