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
    class ActionTypeInvestigate : ActionType
    {
        internal const float COST = 1.0f;

        internal ActionTypeInvestigate(NonPlayableCharacterAbstract character)
            : base(character)
        {

        }

        internal override bool testPreConditions(SearchNode node)
        {
            return true;
        }

        internal override SearchNode unifyRegressive(ref SearchNode node)
        {
            TileIndex investigateLocation =
                //character_.AI_.Memory_.getFirstBelief(BeliefType.InvestigateTarget).data_.t;
                GlobalHelper.getInstance().getCurrentLevelTileGrid().getTileIndex(
                    character_.AI_.Memory_.getFirstBelief(BeliefType.InvestigateTarget).position_
                    );

            // Create a copy of this search node and set its position to our
            //  investigate location
            SearchNode parent = node.getPredecessor();
            parent.action = new ActionInvestigate(character_);
            parent.cost += COST;
            parent.setPosition(Variable.Location, ref investigateLocation);
            parent.setBool(Variable.HasInvestigated, false);

            return parent;
        }

        internal override void register(Dictionary<int, List<ActionType>> actionMap)
        {
            actionMap[Variable.HasInvestigated].Add(this);
        }
    }

    internal class ActionInvestigate : Action
    {
        internal ActionInvestigate(NonPlayableCharacterAbstract character)
            : base(character)
        {

        }

        internal override bool initialize()
        {
            return true;
        }

        internal override ActionStatus update()
        {
            character_.AI_.Memory_.removeBeliefs(BeliefType.InvestigateTarget);
            character_.AI_.Memory_.removeBeliefs(BeliefType.SuspiciousNoise);
            return ActionStatus.SUCCESS;
        }
    }
}
