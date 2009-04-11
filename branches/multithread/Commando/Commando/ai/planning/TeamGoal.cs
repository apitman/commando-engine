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
    /// Encapsulation of a goal to be achieved by a team of agents.
    /// </summary>
    internal abstract class TeamGoal
    {
        internal bool HasFailed_ { get; set; }
        internal float Relevance_ { get; set; } // TODO figure out why protected internal doesn't work

        /// <summary>
        /// Whether or not the goal is valid, and whether or not the
        /// team should even attempt to achieve it.
        /// </summary>
        /// <param name="teamMembers">Team to check for validity.</param>
        /// <returns>True if the goal is valid and achievable, false otherwise.</returns>
        internal abstract bool isValid(List<AI> teamMembers);

        /// <summary>
        /// Update the current relevance and failure flags.
        /// </summary>
        /// <param name="teamMembers">The team considering the goal.</param>
        internal abstract void refresh(List<AI> teamMembers);

        /// <summary>
        /// Adjust a team's GoalTeamwork goals to match this goal.
        /// </summary>
        /// <param name="teamMembers">The team carrying out the goal.</param>
        internal abstract void allocateTasks(List<AI> teamMembers);
    }
}
