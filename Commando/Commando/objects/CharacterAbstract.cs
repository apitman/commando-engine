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

namespace Commando
{
    abstract class CharacterAbstract : AnimatedObjectAbstract
    {

        protected CharacterHealth health_;

        protected CharacterAmmo ammo_;

        protected CharacterWeapon weapon_;

        protected string name_;

        public CharacterAbstract() :
            base()
        {
        }

        public CharacterAbstract(CharacterHealth health, CharacterAmmo ammo, CharacterWeapon weapon, string name) :
            base()
        {
        }

        public CharacterAbstract(CharacterHealth health, CharacterAmmo ammo, CharacterWeapon weapon, string name, AnimationSet animations, float frameLengthModifier, Vector2 velocity, Vector2 position, Vector2 direction, float depth) :
            base(animations, frameLengthModifier, velocity, position, direction, depth)
        {
        }

        protected CharacterHealth getHealth()
        {
            return health_;
        }

        protected CharacterAmmo getAmmo()
        {
            return ammo_;
        }

        protected CharacterWeapon getWeapon()
        {
            return weapon_;
        }

        protected void setHealth(CharacterHealth health)
        {
            health_ = health;
        }

        protected void setAmmo(CharacterAmmo ammo)
        {
            ammo_ = ammo;
        }

        protected void setWeapon(CharacterWeapon weapon)
        {
            weapon_ = weapon;
        }


    }
}
