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
using Microsoft.Xna.Framework;
using Commando.graphics;

namespace Commando.ai.planning
{
    class ActionPatrol : Action
    {
        internal const float COST = 5.0f;
        internal const int THRESHOLD = 70;

        protected int counter = 0;

        internal ActionPatrol(NonPlayableCharacterAbstract character)
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
            parent.action = new ActionPatrol(character_);
            parent.cost += COST;
            parent.setBool(Variable.HasPatrolled, false);
            return parent;
        }

        internal override void register(Dictionary<int, List<Action>> actionMap)
        {
            actionMap[Variable.HasPatrolled].Add(this);
        }

        internal override bool update()
        {
            counter++;
            if (counter >= THRESHOLD)
            {
                counter = 0;
                (character_.getActuator() as DefaultActuator).lookAt(-character_.getDirection());
            }
            Vector2 direction = character_.getDirection();
            direction.Normalize();
            Vector2 target = character_.getPosition() + direction * 5;
            (character_.getActuator() as DefaultActuator).moveTo(target);
            return false;
        }

        internal override bool initialize()
        {
            return true;
        }
    }
}
