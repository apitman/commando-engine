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

namespace Commando.ai.planning
{
    /// <summary>
    /// GoalTeamwork is a placeholder goal which will be filled in by the TeamGoal
    /// that a TeamPlanner has deemed the most relevant.  Essentially, it stores the
    /// logic to tell an agent what its team wants it to accomplish.
    /// </summary>
    internal class GoalTeamwork : Goal
    {

        internal GoalTeamwork(AI ai)
            : base(ai)
        {
            // This will be replaced by the TeamPlanner using setNode()
            node_ = new SearchNode();
        }

        /// <summary>
        /// Sets the goal state to be used by IndividualPlanner.
        /// </summary>
        /// <param name="node">The goal state.</param>
        internal void setNode(SearchNode node)
        {
            node_ = node;
        }

        /// <summary>
        /// Required by Goal, this normally makes the goal contain a recent assessment.
        /// However, that is handled for this class by TeamPlanner in a push manner.
        /// </summary>
        internal override void refresh()
        {
            // Do nothing
        }
    }
}
