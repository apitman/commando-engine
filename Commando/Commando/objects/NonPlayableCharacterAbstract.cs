﻿/*
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
using Commando.collisiondetection;
using Commando.ai.planning;

namespace Commando.objects
{
    public abstract class NonPlayableCharacterAbstract : CharacterAbstract
    {
        internal AI AI_ { get; set; }

        internal override int Allegiance_
        {
            get
            {
                return base.Allegiance_;
            }
            set
            {
                base.Allegiance_ = value;
                TeamPlannerManager.register(AI_);
            }
        }

        /// <summary>
        /// Create a default NonPlayableCharacter
        /// </summary>
        public NonPlayableCharacterAbstract() :
            base()
        {
            
        }

        /// <summary>
        /// Create a NonPlayableCharacter with the specified health, ammo, and weapon objects, plus the given name
        /// </summary>
        /// <param name="health">CharacterStatusElement for health</param>
        /// <param name="ammo">CharacterStatusElement for ammo</param>
        /// <param name="weapon">CharacterStatusElement for the current weapon</param>
        /// <param name="name">The character's name</param>
        /// <param name="detector">Collision detector with which the object should register.</param>
        public NonPlayableCharacterAbstract(CharacterHealth health, CharacterAmmo ammo, CharacterWeapon weapon, string name, CollisionDetectorInterface detector) :
            base(health, ammo, weapon, name, detector)
        {
            
        }

        /// <summary>
        /// Create a NonPlayableCharacter with the specified health, ammo, and weapon objects, plus the given name.
        /// Also, specify the AnimationSet, frameLengthModifier, velocity, position, direction,
        /// and depth of the character.
        /// </summary>
        /// <param name="pipeline">List of objects from which the object should be drawn.</param>
        /// <param name="health">CharacterStatusElement for health</param>
        /// <param name="ammo">CharacterStatusElement for ammo</param>
        /// <param name="weapon">CharacterStatusElement for the current weapon</param>
        /// <param name="name">The character's name</param>
        /// <param name="detector">Collision detector with which this object should register.</param>
        /// <param name="animations">AnimationSet containing all animations for this object</param>
        /// <param name="frameLengthModifier">Float representing the ratio of frames in an animation to movement along the screen</param>
        /// <param name="velocity">Vector of velocity, representing both direction of movement and magnitude</param>
        /// <param name="position">Position of object relative to the top left corner</param>
        /// <param name="direction">Vector representing the direction of the object</param>
        /// <param name="depth">Depth the object is to be drawn to</param>
        public NonPlayableCharacterAbstract(List<DrawableObjectAbstract> pipeline, CharacterHealth health, CharacterAmmo ammo, CharacterWeapon weapon, string name, CollisionDetectorInterface detector, AnimationSet animations, float frameLengthModifier, Vector2 velocity, Vector2 position, Vector2 direction, float depth) :
            base(pipeline, health, ammo, weapon, name, detector, animations, frameLengthModifier, velocity, position, direction, depth)
        {
            
        }

        public override void die()
        {
            AI_.die();
            base.die();
        }
    }
}
