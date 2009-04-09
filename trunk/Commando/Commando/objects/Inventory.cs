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
using Commando.objects.weapons;

namespace Commando.objects
{
    public class Inventory
    {
        /// <summary>
        /// Stores the number of bullets of each type that exist in the player's inventory
        /// </summary>
        public Dictionary<AmmoTypeEnum, int> Ammo_ { get; set; }

        /// <summary>
        /// Stores the weapons that the player is currently holding
        /// </summary>
        public Queue<RangedWeaponAbstract> Weapons_ { get; set; }

        public Inventory()
        {
            Ammo_ = new Dictionary<AmmoTypeEnum, int>();
            Ammo_.Add(AmmoTypeEnum.BUCKSHOT, 0);
            Ammo_.Add(AmmoTypeEnum.BULLETS, 0);
            Ammo_.Add(AmmoTypeEnum.ROUNDS, 0);
            Weapons_ = new Queue<RangedWeaponAbstract>();
        }

        /// <summary>
        /// Places a weapon into the inventory
        /// </summary>
        /// <param name="weapon">The weapon to place in the inventory</param>
        public void addWeapon(RangedWeaponAbstract weapon)
        {
            Weapons_.Enqueue(weapon);
        }

        /// <summary>
        /// Switches currently active weapons by returning the next weapon to use
        /// </summary>
        /// <param name="weaponInHand">Pass in the current weapon to the inventory</param>
        /// <returns>The next weapon that will become the current weapon</returns>
        public RangedWeaponAbstract switchWeapon(RangedWeaponAbstract weaponInHand)
        {
            Weapons_.Enqueue(weaponInHand);
            return Weapons_.Dequeue();
        }

        /// <summary>
        /// Adds ammo to the inventory
        /// </summary>
        /// <param name="type">The type of ammo to add to the inventory</param>
        /// <param name="amount">The number of rounds to add</param>
        public void addAmmo(AmmoTypeEnum type, int amount)
        {
            Ammo_[type] += amount;
        }

        public void reload(RangedWeaponAbstract weaponToReload)
        {
            // TODO
        }

    }
}
