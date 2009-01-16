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
    abstract class PlayableCharacterAbstract : CharacterAbstract
    {

        protected controls.InputSet inputSet_;
        
        public PlayableCharacterAbstract() :
            base()
        {
        }

        public PlayableCharacterAbstract(CharacterHealth health, CharacterAmmo ammo, CharacterWeapon weapon, string name) :
            base(health, ammo, weapon, name)
        {
        }

        public PlayableCharacterAbstract(CharacterHealth health, CharacterAmmo ammo, CharacterWeapon weapon, string name, AnimationSet animations, float frameLengthModifier, Vector2 velocity, Vector2 position, Vector2 direction, float depth) :
            base(health, ammo, weapon, name, animations, frameLengthModifier, velocity, position, direction, depth)
        {
        }

        public void setInputSet(controls.InputSet inputSet)
        {
            inputSet_ = inputSet;
        }
    }
}
