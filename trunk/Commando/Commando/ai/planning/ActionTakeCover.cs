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

namespace Commando.ai.planning
{
    internal class ActionTakeCover : Action
    {
        protected const float COST = 1.0f;

        internal TileIndex coverLocation_;

        internal ActionTakeCover(NonPlayableCharacterAbstract character, ref TileIndex coverLocation)
            : base(character)
        {
            coverLocation_ = coverLocation;
        }

        internal override bool testPreConditions(SearchNode node)
        {
            // TODO
            // if the position AFTER taking cover - think backwards! - is known,
            //  we can only take cover there if that position has cover, otherwise
            //  we just check that we know about nearby cover
            return character_.AI_.Memory_.getFirstBelief(BeliefType.BestCover) != null;
        }

        internal override SearchNode unifyRegressive(ref SearchNode node)
        {
            // TODO
            // If the current location is resolved and doesn't have cover, we
            // actually resolve with a Goto instead

            TileIndex coverLocation =
                character_.AI_.Memory_.getFirstBelief(BeliefType.BestCover).data_.t;

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

        internal override void register(Dictionary<int, List<Action>> actionMap)
        {
            actionMap[Variable.Cover].Add(this);
        }
    }
}
