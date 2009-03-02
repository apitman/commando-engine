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
    /*
    using ActionMap = Dictionary<int, ActionType>;

    /// <summary>
    /// Performs goal-oriented action planning for a single agent.
    /// </summary>
    internal class IndividualPlanner
    {

        private ActionMap actionMap_;

        private List<Action> plan_;

        public IndividualPlanner(List<ActionType> actions)
        {
            // initialize empty lists within ActionMap
            for (int i = 0; i < Variable.LENGTH; i++)
            {
                actionMap_.Add(i, new List<ActionType>());
            }

            // and then register each ActionType appropriately
            for (int i = 0; i < actions.Count; i++)
            {
                actions[i].register(actionMap_);
            }
        }

        public WorkPerformed execute(SearchNode initial, SearchNode goal)
        {
            List<SearchNode> openlist;
            List<int> failedConditions;
            SearchNode current = goal; // BACKWARDS, START WITH GOAL

            // perform search
            while (true)
            {
                current.resolvesWith(initial, failedConditions);

                if (failedConditions.Count == 0)
                {
                    break;
                }

                for (int i = 0; i < failedConditions.Count; i++)
                {
                    List<ActionType> solutions = actionMap_[failedConditions[i]];

                    for (int j = 0; j < solutions.Count; j++)
                    {
                        if (solutions[j].testPreConditions(current))
                        {
                            SearchNode predecessor = solutions[j].resolve(current);

                            predecessor.fValue = predecessor.cost + predecessor.dist(initial);
                            openlist.Add(predecessor);
                        }
                    }
                }

                if (openlist.Count == 0)
                {
                    current = null;
                    break;
                }

                // TODO
                // might be worth it to profile this against a data structure
                // with O(log n) insertion but no necessary sort (ie sorted list)
                openlist.Sort(current);

                current = openlist[0];
                openlist.RemoveAt(0);
            }

            // retrieve results
            List<Action> plan = new List<Action>();
            while (current != null)
            {
                plan.Add(current.action);
                current = current.next;
            }

            plan_ = plan;
        }

        public List<Action> getResult()
        {
            return plan_;
        }
    }
    */
}
