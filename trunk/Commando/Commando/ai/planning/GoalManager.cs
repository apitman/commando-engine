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
    /// <summary>
    /// Manages an agent's Goals and selects the most relevant.
    /// </summary>
    class GoalManager
    {
        protected AI AI_;

        protected List<Goal> goals_ = new List<Goal>();
        protected GoalTeamwork teamworkGoal_;

        internal GoalManager(AI ai)
        {
            AI_ = ai;
        }

        /// <summary>
        /// Add a goal for the agent to consider.
        /// </summary>
        /// <param name="goal">Goal to be considered.</param>
        internal void addGoal(Goal goal)
        {
            goals_.Add(goal);
        }

        /// <summary>
        /// Used by TeamPlanner to push information about team goals
        /// to the agent.
        /// </summary>
        /// <returns>The agent's GoalTeamwork object.</returns>
        internal GoalTeamwork getTeamGoal()
        {
            if (teamworkGoal_ == null)
            {
                teamworkGoal_ = new GoalTeamwork(AI_);
            }
            return teamworkGoal_;
        }

        /// <summary>
        /// Sets a GoalTeamwork object which will be used by a TeamPlanner.
        /// </summary>
        /// <param name="goal">The GoalTeamwork object being set.</param>
        internal void setTeamGoal(GoalTeamwork goal)
        {
            goals_.Remove(teamworkGoal_);
            teamworkGoal_ = goal;
            goals_.Add(teamworkGoal_);
        }

        /// <summary>
        /// Refresh all Goals to get updated relevance ratings, then select
        /// the most relevant as the agent's current goal.
        /// </summary>
        internal void update()
        {
            float highestRelevance = float.MinValue;
            Goal mostRelevant = null;
            for (int i = 0; i < goals_.Count; i++)
            {
                goals_[i].refresh();
                if (!goals_[i].HasFailed_ && goals_[i].Relevance_ > highestRelevance)
                {
                    highestRelevance = goals_[i].Relevance_;
                    mostRelevant = goals_[i];
                }
            }

            if (mostRelevant == null)
            {
                throw new NotImplementedException("AIs without goals not yet supported");
            }

            AI_.CurrentGoal_ = mostRelevant;
        }
    }
}
