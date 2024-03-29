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
using Commando.objects;
using Commando.levels;
using Microsoft.Xna.Framework;
using Commando.graphics;

namespace Commando.ai.planning
{
    class ActionTypeFlee : ActionType
    {
        internal const float COST = 200.0f;

        internal ActionTypeFlee(NonPlayableCharacterAbstract character)
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
            parent.setBool(Variable.FarFromTarget, false);

            return parent;
        }

        internal override void register(Dictionary<int, List<ActionType>> actionMap)
        {
            actionMap[Variable.TargetHealth].Add(this);
            actionMap[Variable.FarFromTarget].Add(this);
        }
    }

    internal class ActionFlee : Action
    {
        internal const int THRESHOLD = 60;

        protected int counter = 0;

        internal ActionFlee(NonPlayableCharacterAbstract character)
            : base(character)
        {

        }

        internal override bool initialize()
        {
            return true;
        }

        internal override ActionStatus update()
        {
            counter++;
            if (counter >= THRESHOLD)
            {
                return ActionStatus.SUCCESS;
            }

            Belief bestTarget = character_.AI_.Memory_.getFirstBelief(BeliefType.BestTarget);
            if (bestTarget == null)
            {
                return ActionStatus.SUCCESS;
            }
            //CharacterAbstract enemy = bestTarget.handle_ as CharacterAbstract;
            Vector2 opposite = bestTarget.position_ - character_.getPosition();
            opposite = -opposite;
            opposite.Normalize();

            //(character_.getActuator() as DefaultActuator).moveTo(character_.getPosition() + opposite * 5);
            character_.getActuator().perform("moveTo", new ActionParameters(character_.getPosition() + opposite * 5));

            return ActionStatus.IN_PROGRESS;
        }
    }
}
