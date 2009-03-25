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
using Commando.ai.planning;

namespace Commando.ai
{
    class SystemGoalSelection : System
    {
        internal SystemGoalSelection(AI ai) : base(ai) { }

        internal override void update()
        {
            SearchNode goal = new SearchNode();

            Belief target = AI_.Memory_.getFirstBelief(BeliefType.EnemyLoc);
            Belief noise = AI_.Memory_.getFirstBelief(BeliefType.InvestigateTarget);

            // We have an enemy, so go after it
            if (target != null)
            {
                goal.setInt(Variable.TargetHealth, 0);
            }

            // Otherwise we investigate a noise if we're aware of one
            else if (noise != null)
            {
                goal.setBool(Variable.HasInvestigated, false);
            }

            // Otherwise we just patrol
            else
            {
                goal.setBool(Variable.HasPatrolled, false);
            }

            AI_.CurrentGoal_ = goal;
        }
    }
}
