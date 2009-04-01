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
    /// Performs goal-oriented action planning for a single agent.
    /// </summary>
    internal class IndividualPlanner
    {
        private Dictionary<int, List<ActionType>> actionMap_
            = new Dictionary<int, List<ActionType>>();

        private List<Action> plan_;

        /// <summary>
        /// Create an individual planner.
        /// </summary>
        /// <param name="actions">Actions available to the character being planned for.</param>
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

        /// <summary>
        /// Create a plan from the initial state to the goal.
        /// </summary>
        /// <param name="initial">Current believed state of the world.</param>
        /// <param name="goal">Desired state of the world.</param>
        public void execute(SearchNode initial, SearchNode goal)
        {
            List<SearchNode> openlist = new List<SearchNode>();
            List<int> failedConditions = new List<int>();
            SearchNode current = goal; // BACKWARDS, START WITH GOAL

            // perform search
            while (true)
            {
                current.unifiesWith(initial, failedConditions);

                // If current state and initial states unify,
                // we have found a complete plan, so finish
                if (failedConditions.Count == 0)
                {
                    break;
                }

                // Otherwise, we go through our action map looking up actions which
                // fulfill all of the ways in which the current state didn't unify
                // with the initial, and try those actions.
                for (int i = 0; i < failedConditions.Count; i++)
                {
                    List<ActionType> solutions = actionMap_[failedConditions[i]];

                    for (int j = 0; j < solutions.Count; j++)
                    {
                        if (solutions[j].testPreConditions(current))
                        {
                            SearchNode predecessor = solutions[j].unifyRegressive(ref current);

                            predecessor.fValue = predecessor.cost + predecessor.dist(initial);
                            openlist.Add(predecessor);
                        }
                    }
                }

                // No search states left in the open list, so no plan
                // could be found
                if (openlist.Count == 0)
                {
                    current = null;
                    break;
                }

                // Get the cheapest search state and try from there
                // TODO
                // Might be worth it to profile this against a data structure
                // with O(log n) insertion but no necessary sort (ie sorted list)
                openlist.Sort(current);
                current = openlist[0];
                openlist.RemoveAt(0);
            }

            // Reconstruct the generated plan; plan will be empty if we failed.
            // Therefore, an empty plan could have two meanings:
            //  1. Plan failed, so we need to switch goals to try another.
            //  2. Initial state satisfies the current goal, so we should try to
            //      satisfy a different goal anyway.
            // This small amount of ambiguity helps reduce (plan == null) errors.
            List<Action> plan = new List<Action>();
            while (current != null && current.action != null)
            {
                plan.Add(current.action);
                current = current.next;
            }
            // Note: the current.action != null check prevents a bug in the situation
            //  where the initial state fulfills the goal state

            plan_ = plan;
        }

        /// <summary>
        /// Fetch the result of planning.
        /// </summary>
        /// <returns>The generated plan from the last call to 'execute'.</returns>
        public List<Action> getResult()
        {
            return plan_;
        }
    }
}
