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
    /// Provide global access to all TeamPlanners; enforces a limit of one
    /// TeamPlanner per allegiance value and one team per agent.
    /// </summary>
    static class TeamPlannerManager
    {
        private static Dictionary<AI, int> registry_; // Map of agents to allegiances
        private static Dictionary<int, TeamPlanner> map_; // Map of allegiances to planners

        static TeamPlannerManager()
        {
            registry_ = new Dictionary<AI, int>();
            map_ = new Dictionary<int, TeamPlanner>();
        }

        /// <summary>
        /// Add an AI to the appropriate TeamPlanner, or move it to the
        /// appropriate TeamPlanner if it is already in one.
        /// </summary>
        /// <param name="ai">Agent's AI to be registered.</param>
        internal static void register(AI ai)
        {
            if (ai == null)
            {
                return;
            }

            int allegiance = ai.Character_.Allegiance_;
            if (registry_.ContainsKey(ai))
            {
                int previousAllegiance =
                    registry_[ai];
                map_[previousAllegiance].removeMember(ai);
                registry_[ai] = ai.Character_.Allegiance_;
            }
            else
            {
                registry_.Add(ai, allegiance);
            }

            if (map_.ContainsKey(allegiance))
            {
                map_[allegiance].addMember(ai);
            }
            else
            {
                TeamPlanner tp = new TeamPlanner(allegiance);
                tp.addMember(ai);
                tp.addGoal(new TeamGoalEliminate());
                map_.Add(allegiance, tp);
            }
        }

        /// <summary>
        /// Run all known TeamPlanners.
        /// </summary>
        internal static void update()
        {
            Dictionary<int, TeamPlanner>.ValueCollection planners = map_.Values;
            foreach (TeamPlanner tp in planners)
            {
                tp.update();
            }
        }
    }
}
