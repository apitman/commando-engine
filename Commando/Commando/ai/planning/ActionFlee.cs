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
using Commando.objects;
using Commando.levels;
using Microsoft.Xna.Framework;
using Commando.graphics;

namespace Commando.ai.planning
{
    class ActionFlee : Action
    {
        internal const float COST = 10.0f;
        internal const int THRESHOLD = 60;

        protected int counter = 0;

        internal ActionFlee(NonPlayableCharacterAbstract character)
            : base(character)
        {

        }

        internal override bool testPreConditions(SearchNode node)
        {
            return true;
        }

        internal override SearchNode unifyRegressive(ref SearchNode node)
        {
            SearchNode parent = node.getPredecessor();
            parent.action = new ActionFlee(character_);
            parent.cost += COST;
            parent.setInt(Variable.TargetHealth, node.values[Variable.TargetHealth].i + 1);

            return parent;
        }

        internal override void register(Dictionary<int, List<Action>> actionMap)
        {
            actionMap[Variable.TargetHealth].Add(this);
        }

        internal override bool initialize()
        {
            return true;
        }

        internal override bool update()
        {
            counter++;
            if (counter >= THRESHOLD)
                return true;

            Belief bestTarget = character_.AI_.Memory_.getFirstBelief(BeliefType.BestTarget);
            if (bestTarget == null)
                return true;
            //CharacterAbstract enemy = bestTarget.handle_ as CharacterAbstract;
            Vector2 opposite = bestTarget.position_ - character_.getPosition();
            opposite = -opposite;
            opposite.Normalize();

            (character_.getActuator() as DefaultActuator).moveTo(character_.getPosition() + opposite * 5);

            return false;
        }
    }
}
