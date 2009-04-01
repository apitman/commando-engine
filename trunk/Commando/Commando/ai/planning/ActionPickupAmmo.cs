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
using Commando.levels;
using Commando.objects;
using Commando.graphics;
using Microsoft.Xna.Framework;

namespace Commando.ai.planning
{
    internal class ActionPickupAmmoType : ActionType
    {
        protected const float COST = 1.0f;

        internal ActionPickupAmmoType(NonPlayableCharacterAbstract character)
            : base(character)
        {

        }

        internal override bool testPreConditions(SearchNode node)
        {
            return (character_.AI_.Memory_.getFirstBelief(BeliefType.AmmoLoc) != null);
        }

        internal override SearchNode unifyRegressive(ref SearchNode node)
        {
            Belief ammoBelief =
                character_.AI_.Memory_.getFirstBelief(BeliefType.AmmoLoc);

            TileIndex ammoLocation = ammoBelief.data_.tile1;

            SearchNode parent = node.getPredecessor();
            parent.action = new ActionPickupAmmo(character_, ref ammoLocation, ammoBelief.handle_);
            parent.cost += COST;
            parent.unresolve(Variable.Ammo);

            parent.setPosition(Variable.Location, ref ammoLocation);

            return parent;
        }

        internal override void register(Dictionary<int, List<ActionType>> actionMap)
        {
            actionMap[Variable.Ammo].Add(this);
        }
    }

    internal class ActionPickupAmmo : Action
    {
        internal AmmoBox ammo_;
        internal TileIndex ammoLocation_;
        internal Object tempHandle_;

        internal ActionPickupAmmo(NonPlayableCharacterAbstract character, ref TileIndex ammoLocation, Object tempHandle)
            : base(character)
        {
            ammoLocation_ = ammoLocation;
            tempHandle_ = tempHandle;
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <returns></returns>
        internal override bool initialize()
        {
            return true;
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <returns></returns>
        internal override bool update()
        {
            if (ammo_.tryToPickUp(character_, character_.getCollisionDetector()))
            {
                return true;
            }
            (character_.getActuator() as DefaultActuator).moveTo(ammo_.getPosition());
            return false;
        }

        internal override void reserve()
        {
            base.reserve();

            ammo_ = (tempHandle_ as AmmoBox);
            ReservationTable.reserveResource(ammo_, character_);
        }

        internal override void unreserve()
        {
            base.unreserve();
            ReservationTable.freeResource(ammo_, character_);
        }
    }
}
