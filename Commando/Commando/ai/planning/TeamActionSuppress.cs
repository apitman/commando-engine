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
    internal class TeamActionTypeSuppress : ActionType
    {
        protected internal const int COST = 3;

        internal TeamActionTypeSuppress(NonPlayableCharacterAbstract character)
            : base(character)
        {
            
        }

        internal override bool testPreConditions(SearchNode node)
        {
            return
                node.taskPasses(Variable.TeamTask, TeamTask.SUPPRESS) &&
                node.boolPasses(Variable.Ammo, true) &&
                node.boolPasses(Variable.Weapon, true);
        }

        internal override SearchNode unifyRegressive(ref SearchNode node)
        {
            Belief target = character_.AI_.Memory_.getFirstBelief(BeliefType.TeamInfo);

            SearchNode parent = node.getPredecessor();
            parent.action = new TeamActionSuppress(character_, target.handle_);
            parent.cost += COST;
            parent.setBool(Variable.Weapon, true);
            parent.setBool(Variable.Ammo, true);
            parent.setTask(TeamTask.CLEAR);
            return parent;
        }

        internal override void register(Dictionary<int, List<ActionType>> actionMap)
        {
            actionMap[Variable.TeamTask].Add(this);
        }
    }

    internal class TeamActionSuppress : Action
    {
        protected Object handle_;

        internal TeamActionSuppress(NonPlayableCharacterAbstract character, Object handle)
            : base(character)
        {
            handle_ = handle;
        }

        internal override bool initialize()
        {
            throw new NotImplementedException();
        }

        internal override ActionStatus update()
        {
            throw new NotImplementedException();
        }
    }
}
