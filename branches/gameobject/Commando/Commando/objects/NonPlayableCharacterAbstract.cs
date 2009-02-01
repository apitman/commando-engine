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

namespace Commando.objects
{
    public abstract class NonPlayableCharacterAbstract : CharacterAbstract
    {
        protected AIInterface AI_ { get; private set; }
        
        /// <summary>
        /// Create a default NonPlayableCharacter
        /// </summary>
        public NonPlayableCharacterAbstract() :
            base()
        {
            //AI_ = new DummyAI(this);
        }

        /// <summary>
        /// Create a NonPlayableCharacter with the specified health, ammo, and weapon objects, plus the given name
        /// </summary>
        /// <param name="health">CharacterStatusElement for health</param>
        /// <param name="ammo">CharacterStatusElement for ammo</param>
        /// <param name="weapon">CharacterStatusElement for the current weapon</param>
        /// <param name="name">The character's name</param>
        public NonPlayableCharacterAbstract(CharacterHealth health, CharacterAmmo ammo, CharacterWeapon weapon, string name) :
            base(health, ammo, weapon, name)
        {
            //AI_ = new AI(this);
        }

        /// <summary>
        /// Create a NonPlayableCharacter with the specified health, ammo, and weapon objects, plus the given name.
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
        public NonPlayableCharacterAbstract(CharacterHealth health, CharacterAmmo ammo, CharacterWeapon weapon, string name, AnimationSet animations, float frameLengthModifier, Vector2 velocity, Vector2 position, Vector2 direction, float depth) :
            base(health, ammo, weapon, name, animations, frameLengthModifier, velocity, position, direction, depth)
        {
            //AI_ = new AI(this);
        }

        public abstract void moveTo(Vector2 position);

        public abstract void lookAt(Vector2 location);

    }
}
