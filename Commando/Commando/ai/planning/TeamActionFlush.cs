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
using Commando.graphics;
using Commando.objects.weapons;
using Commando.objects;

namespace Commando.ai.planning
{
    internal class TeamActionTypeFlush : ActionType
    {
        protected internal const int COST = 5;

        internal TeamActionTypeFlush(NonPlayableCharacterAbstract character)
            : base(character)
        {

        }

        internal override bool testPreConditions(SearchNode node)
        {
            return
                node.taskPasses(Variable.TeamTask, TeamTask.FLUSH) &&
                node.boolPasses(Variable.Cover, true);
        }

        internal override SearchNode unifyRegressive(ref SearchNode node)
        {
            Belief target = character_.AI_.Memory_.getFirstBelief(BeliefType.TeamInfo);

            SearchNode parent = node.getPredecessor();
            parent.action = new TeamActionFlush(character_, target.handle_);
            parent.cost += COST;
            parent.setBool(Variable.Cover, true);
            parent.setTask(TeamTask.CLEAR);
            return parent;
        }

        internal override void register(Dictionary<int, List<ActionType>> actionMap)
        {
            actionMap[Variable.TeamTask].Add(this);
        }
    }

    internal class TeamActionFlush : Action
    {
        protected Object handle_;
        protected CharacterAbstract enemy_;

        internal TeamActionFlush(NonPlayableCharacterAbstract character, Object handle)
            : base(character)
        {
            handle_ = handle;
        }

        internal override bool initialize()
        {
            enemy_ = (handle_ as CharacterAbstract);

            Belief belief = character_.AI_.Memory_.getBelief(BeliefType.EnemyLoc, handle_);

            if ((enemy_ == null) || (belief == null))
            {
                return false;
            }

            (character_.getActuator() as DefaultActuator).throwGrenade(
                new FragGrenade(character_.pipeline_,
                    character_.getCollisionDetector(),
                    belief.position_ - character_.getPosition(),
                    character_.getPosition(),
                    character_.getDirection())
            );

            return true;
        }

        internal override ActionStatus update()
        {
            bool done =
                (character_.getActuator() as DefaultActuator).isFinished();

            if (done)
            {
                return ActionStatus.SUCCESS;
            }
            
            return ActionStatus.FAILED;
        }
    }
}
