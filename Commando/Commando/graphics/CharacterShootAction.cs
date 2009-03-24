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

namespace Commando.graphics
{
    public class CharacterShootAction : ShootActionInterface
    {
        private const int PRIORITY = 0;

        public CharacterShootAction()
        {
        }

        public void shoot(RangedWeaponAbstract weapon)
        {
            weapon.shoot();
        }

        public void update()
        {
            
        }

        public void draw()
        {
            
        }

        public void draw(Microsoft.Xna.Framework.Graphics.Color color)
        {
            
        }

        public bool isFinished()
        {
            return true;
        }

        public CharacterActionInterface interrupt(CharacterActionInterface newAction)
        {
            return newAction;
        }

        public int getPriority()
        {
            return PRIORITY;
        }

        public void setCharacter(CharacterAbstract character)
        {
            
        }

        public void start()
        {
            
        }
    }
}
