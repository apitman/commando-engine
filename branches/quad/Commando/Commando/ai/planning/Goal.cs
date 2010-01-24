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
    /// Goal encapsulates a desire which an NPC wants to fulfill, and it
    /// contains the logic for calculating how relevant this goal is at any
    /// point in time and the state the world would be in once fulfilled.
    /// </summary>
    internal abstract class Goal
    {
        protected SearchNode node_;
        protected Object handle_;

        internal AI AI_ { get; private set; }
        internal virtual bool HasFailed_ { get; set; }
        internal virtual float Relevance_ { get; set; } // TODO figure out why protected internal doesn't work

        internal Goal(AI ai)
        {
            AI_ = ai;
        }

        /// <summary>
        /// Force the goal to recalculate its relevance value.
        /// </summary>
        internal abstract void refresh();

        /// <summary>
        /// Get a SearchNode from the goal which can be used by the IndividualPlanner.
        /// </summary>
        /// <returns>The goal SearchNode.</returns>
        internal SearchNode getNode()
        {
            return node_;
        }

        /// <summary>
        /// Determine whether two goals are identical.
        /// </summary>
        /// <param name="lhs">First goal.</param>
        /// <param name="rhs">Second goal.</param>
        /// <returns>True if the two goals might accomplish the same end state, false otherwise.</returns>
        internal static bool areSame(Goal lhs, Goal rhs)
        {
            if (lhs == null && rhs == null) return true;
            if (lhs == null || rhs == null) return false;
            return (lhs.handle_ == rhs.handle_) && testNodeEquality(lhs.node_, rhs.node_);
        }

        /// <summary>
        /// Determine whether two goal SearchNodes could unify to the same state.
        /// </summary>
        /// <param name="lhs">First SearchNode.</param>
        /// <param name="rhs">Second SearchNode.</param>
        /// <returns>True if the goals could both be resolved by a state, false otherwise.</returns>
        protected internal static bool testNodeEquality(SearchNode lhs, SearchNode rhs)
        {
            if (lhs == null && rhs == null)
                return true;

            if (lhs == null || rhs == null)
                return false;

            if (lhs.values.Length != rhs.resolved.Length)
                return false;

            for (int i = 0; i < lhs.values.Length; i++)
            {
                if (lhs.values[i].i != rhs.values[i].i)
                    return false;
                if (rhs.resolved[i] != rhs.resolved[i])
                    return false;
            }
            return true;
        }
    }
}
