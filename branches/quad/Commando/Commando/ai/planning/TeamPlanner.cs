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
    /// Performs team maintenance and cleanup, as well as selecting the TeamGoal
    /// whose subtasks will be distributed to team members.
    /// </summary>
    internal class TeamPlanner
    {
        protected int allegiance_;

        protected int counter = 0;
        protected const int UPDATE_RATE = 90; // every three seconds

        protected List<AI> teamMembers_ = new List<AI>();
        protected List<TeamGoal> teamGoals_ = new List<TeamGoal>();

        internal TeamPlanner(int allegiance)
        {
            allegiance_ = allegiance;
            counter = RandomManager.get().Next(UPDATE_RATE);
        }

        /// <summary>
        /// Refreshes the team, detects the current goal, and allocates
        /// tasks to individual team members.
        /// </summary>
        internal void update()
        {
            counter++;
            if (counter < UPDATE_RATE)
            {
                return;
            }
            counter = 0;

            refreshTeamMembers();

            selectBestGoal();
        }

        /// <summary>
        /// This method makes sure that dead team members aren't a
        /// part of the team.
        /// </summary>
        protected void refreshTeamMembers()
        {
            for (int i = 0; i < teamMembers_.Count; i++)
            {
                if (teamMembers_[i].Character_.isDead())
                {
                    teamMembers_.RemoveAt(i);
                    i--;
                }
            }
        }

        /// <summary>
        /// This method should be responsible for looking at distances amongst
        /// characters of a certain allegiance, and forming groups accordingly.
        /// </summary>
        protected void reconstructTeam()
        {
            // TODO
            throw new NotImplementedException();
        }

        /// <summary>
        /// Runs through all TeamGoals and selects the most relevant, if any.
        /// This goal is then pushed via GoalTeamwork to individual agents.
        /// </summary>
        protected void selectBestGoal()
        {
            float highestRelevance = float.MinValue;
            TeamGoal mostRelevant = null;
            for (int i = 0; i < teamGoals_.Count; i++)
            {
                teamGoals_[i].refresh(teamMembers_);
                if (!teamGoals_[i].HasFailed_ && teamGoals_[i].isValid(teamMembers_) && teamGoals_[i].Relevance_ > highestRelevance)
                {
                    highestRelevance = teamGoals_[i].Relevance_;
                    mostRelevant = teamGoals_[i];
                }
            }

            if (mostRelevant == null)
            {
                clearTeamGoals();
                return;
            }

            mostRelevant.allocateTasks(teamMembers_);
        }

        /// <summary>
        /// Tell all team members that there is no team goal.
        /// </summary>
        protected void clearTeamGoals()
        {
            for (int i = 0; i < teamMembers_.Count; i++)
            {
                teamMembers_[i].GoalManager_.getTeamGoal().HasFailed_ = true;
            }
        }

        /// <summary>
        /// Add a team member.
        /// </summary>
        /// <param name="ai">AI of the team member to be added.</param>
        internal void addMember(AI ai)
        {
            teamMembers_.Add(ai);
        }

        /// <summary>
        /// Remove a team member.
        /// </summary>
        /// <param name="ai">Ai of the team member to be removed.</param>
        internal void removeMember(AI ai)
        {
            teamMembers_.Remove(ai);
        }

        /// <summary>
        /// Add a TeamGoal for this planner to push to team members when it
        /// is the most relevant.
        /// </summary>
        /// <param name="goal">The possible team goal.</param>
        internal void addGoal(TeamGoal goal)
        {
            teamGoals_.Add(goal);
        }
    }
}
