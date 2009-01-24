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
    /// <summary>
    /// CharacterAbstract inherits from AnimatedObjectAbstract and is an ancestor of all 
    /// playable and non-playable characters.
    /// </summary>
    abstract class CharacterAbstract : AnimatedObjectAbstract
    {

        protected CharacterHealth health_;

        protected CharacterAmmo ammo_;

        protected CharacterWeapon weapon_;

        protected string name_;

        /// <summary>
        /// Create a default Character
        /// </summary>
        public CharacterAbstract() :
            this(new CharacterHealth(), new CharacterAmmo(), new CharacterWeapon(), "")
        {
        }

        /// <summary>
        /// Create a Character with the specified health, ammo, and weapon objects, plus the given name
        /// </summary>
        /// <param name="health">CharacterStatusElement for health</param>
        /// <param name="ammo">CharacterStatusElement for ammo</param>
        /// <param name="weapon">CharacterStatusElement for the current weapon</param>
        /// <param name="name">The character's name</param>
        public CharacterAbstract(CharacterHealth health, CharacterAmmo ammo, CharacterWeapon weapon, string name) :
            base()
        {
            health_ = health;
            ammo_ = ammo;
            weapon_ = weapon;
            name_ = name;
        }

        /// <summary>
        /// Create a Character with the specified health, ammo, and weapon objects, plus the given name.
        /// Also, specify the AnimationSet, frameLengthModifier, velocity, position, direction,
        /// and depth of the character.
        /// </summary>
        /// <param name="health">CharacterStatusElement for health</param>
        /// <param name="ammo">CharacterStatusElement for ammo</param>
        /// <param name="weapon">CharacterStatusElement for the current weapon</param>
        /// <param name="name">The character's name</param>
        /// <param name="animations">AnimationSet containing all animations for this object</param>
        /// <param name="frameLengthModifier">Float representing the ratio of frames in an animation to movement along the screen</param>
        /// <param name="velocity">Vector of velocity, representing both direction of movement and magnitude</param>
        /// <param name="position">Position of object relative to the top left corner</param>
        /// <param name="direction">Vector representing the direction of the object</param>
        /// <param name="depth">Depth the object is to be drawn to</param>
        public CharacterAbstract(CharacterHealth health, CharacterAmmo ammo, CharacterWeapon weapon, string name, AnimationSet animations, float frameLengthModifier, Vector2 velocity, Vector2 position, Vector2 direction, float depth) :
            base(animations, frameLengthModifier, velocity, position, direction, depth)
        {
            health_ = health;
            ammo_ = ammo;
            weapon_ = weapon;
            name_ = name;
        }

        public CharacterHealth getHealth()
        {
            return health_;
        }

        public CharacterAmmo getAmmo()
        {
            return ammo_;
        }

        public CharacterWeapon getWeapon()
        {
            return weapon_;
        }

        public string getName()
        {
            return name_;
        }

        public void setHealth(CharacterHealth health)
        {
            health_ = health;
        }

        public void setAmmo(CharacterAmmo ammo)
        {
            ammo_ = ammo;
        }

        public void setWeapon(CharacterWeapon weapon)
        {
            weapon_ = weapon;
        }

        public void setName(string name)
        {
            name_ = name;
        }
    }
}
