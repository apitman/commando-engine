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
    internal class ActionTypeTakeCover : ActionType
    {
        protected const float COST = 1.0f;

        internal ActionTypeTakeCover(NonPlayableCharacterAbstract character)
            : base(character)
        {

        }

        internal override bool testPreConditions(SearchNode node)
        {
            // TODO
            // if the position AFTER taking cover - think backwards! - is known,
            //  we can only take cover there if that position has cover, otherwise
            //  we just check that we know about nearby cover
            return (character_.AI_.Memory_.getFirstBelief(BeliefType.AvailableCover) != null);
        }

        internal override SearchNode unifyRegressive(ref SearchNode node)
        {
            // TODO
            // If the current location is resolved and doesn't have cover, we
            // actually resolve with a Goto instead

            TileIndex coverLocation =
                character_.AI_.Memory_.getFirstBelief(BeliefType.AvailableCover).data_.tile1;

            SearchNode parent = node.getPredecessor();
            parent.action = new ActionTakeCover(character_, ref coverLocation);
            parent.cost += COST;
            parent.setBool(Variable.Cover, false);

            // TODO ...?
            // if the position AFTER taking cover was known, that's where we were
            //  at the predecessor, which is handled by the clone operation

            // if it wasn't known, we need to make a best guess as to where we had
            //  to be in order to take cover
            parent.setPosition(Variable.Location, ref coverLocation);

            return parent;
        }

        internal override void register(Dictionary<int, List<ActionType>> actionMap)
        {
            actionMap[Variable.Cover].Add(this);
        }
    }

    internal class ActionTakeCover : Action
    {
        internal CoverObject cover_;
        internal TileIndex coverLocation_;

        internal ActionTakeCover(NonPlayableCharacterAbstract character, ref TileIndex coverLocation)
            : base(character)
        {
            coverLocation_ = coverLocation;
        }

        /// <summary>
        /// Send signal to actuator to attach to a piece of cover.
        /// </summary>
        /// <returns>Returns true if successful.</returns>
        internal override bool initialize()
        {
            (character_.getActuator() as DefaultActuator).cover(cover_);
            return true;
        }

        /// <summary>
        /// Poll actuator to see if it is done attaching to cover.
        /// </summary>
        /// <returns>Returns true once attached.</returns>
        internal override ActionStatus update()
        {
            if ((character_.getActuator() as DefaultActuator).isFinished())
                return ActionStatus.SUCCESS;
            return ActionStatus.IN_PROGRESS;
        }

        internal override void reserve()
        {
            base.reserve();
            Belief bestCover = character_.AI_.Memory_.getFirstBelief(BeliefType.AvailableCover);
            cover_ = (bestCover.handle_ as CoverObject);
            ReservationTable.reserveResource(cover_, character_);
        }

        internal override void unreserve()
        {
            base.unreserve();
            ReservationTable.freeResource(cover_, character_);
        }
    }
}
