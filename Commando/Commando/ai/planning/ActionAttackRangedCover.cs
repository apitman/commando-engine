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

namespace Commando.ai.planning
{
    internal class ActionAttackRangedCover : Action
    {
        protected internal const int COST = 1;

        internal ActionAttackRangedCover(NonPlayableCharacterAbstract character)
            : base(character)
        {

        }

        internal override bool testPreConditions(SearchNode node)
        {
            return
                node.boolPasses(Variable.Cover, true) && 
                node.boolPasses(Variable.Weapon, true) &&
                node.boolPasses(Variable.Ammo, true);
        }

        internal override SearchNode unifyRegressive(ref SearchNode node)
        {
            SearchNode parent = node.getPredecessor();
            parent.action = new ActionAttackRangedCover(character_);
            parent.cost += COST;
            parent.setInt(Variable.TargetHealth, node.values[Variable.TargetHealth].i + 1);
            parent.setBool(Variable.Weapon, true);
            parent.setBool(Variable.Ammo, true);
            parent.setBool(Variable.Cover, true);
            return parent;
        }

        internal override void reserve()
        {
            throw new NotImplementedException();
        }

        internal override void register(Dictionary<int, List<Action>> actionMap)
        {
            actionMap[Variable.TargetHealth].Add(this);
        }
    }
}
