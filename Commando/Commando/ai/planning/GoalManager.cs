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
    class GoalManager
    {
        protected AI AI_;

        protected List<Goal> goals_ = new List<Goal>();

        internal GoalManager(AI ai)
        {
            AI_ = ai;
        }

        internal void addGoal(Goal goal)
        {
            goals_.Add(goal);
        }

        internal void setTeamGoal(Goal goal)
        {
            throw new NotImplementedException();
        }

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
