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
    class TeamGoalEliminate : TeamGoal
    {
        protected const float RELEVANCE = 0.5f;

        protected CharacterAbstract target_;

        internal override bool isValid(List<AI> teamMembers)
        {
            if (teamMembers.Count < 3)
            {
                return false;
            }

            // Create a map of how many agents have each target as their best
            // See if any target has at least half of the team members after him
            Dictionary<Object, int> targetMap = new Dictionary<object, int>();
            Object mutualTarget = null;
            for (int i = 0; i < teamMembers.Count; i++)
            {
                Belief bestTarget = teamMembers[i].Memory_.getFirstBelief(BeliefType.BestTarget);
                if (bestTarget == null)
                {
                    continue;
                }

                if (targetMap.ContainsKey(bestTarget.handle_))
                {
                    targetMap[bestTarget.handle_] = targetMap[bestTarget.handle_] + 1;

                    // We're looking for a target with at least half of the team members
                    //  gunning for him
                    if (targetMap[bestTarget.handle_] >= teamMembers.Count / 2f)
                    {
                        mutualTarget = bestTarget.handle_;
                        break;
                    }
                }
                else
                {
                    targetMap.Add(bestTarget.handle_, 1);
                }
            }

            if (mutualTarget == null)
            {
                return false;
            }

            target_ = (CharacterAbstract)mutualTarget;
            return true;
        }

        internal override void refresh(List<AI> teamMembers)
        {
            Relevance_ = RELEVANCE;
        }

        internal override void allocateTasks(List<AI> teamMembers)
        {
            throw new NotImplementedException();
        }
    }
}
