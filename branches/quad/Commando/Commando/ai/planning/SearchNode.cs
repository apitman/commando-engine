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
using System.Runtime.InteropServices;
using System.Text;
using Commando.levels;

namespace Commando.ai.planning
{
    /// <summary>
    /// A node in the search space used by IndividualPlanner for
    /// goal-oriented action planning.
    /// </summary>
    internal class SearchNode : IComparer<SearchNode>
    {
        internal Action action; // action performed from this node to reach the next

        internal float fValue; // A* cost + heuristic value for search space ordering
        internal float cost; // cost to reach this node in the space

        // descendent of this node in time, predecessor in search space
        // i.e., the node after 'action' is performed on this node
        internal SearchNode next;

        // values associated with variables used to describe the state of the world
        internal VariableValue[] values = new VariableValue[Variable.LENGTH];

        // whether each of those variables has actually unified to a value
        internal bool[] resolved = new bool[Variable.LENGTH];

        /// <summary>
        /// Essentially, copy this node so that an action can 'undo' itself from the
        /// node.
        /// </summary>
        /// <returns>A node which might have preceded this node in time.</returns>
        internal SearchNode getPredecessor()
        {
            SearchNode node = new SearchNode();
            node.next = this;
            node.cost = this.cost;
            for (int i = 0; i < values.Length; i++)
            {
                node.values[i] = this.values[i];
                node.resolved[i] = this.resolved[i];
            }
            return node;
        }

        /// <summary>
        /// Whether a particular variable in this node could have a
        /// particular Boolean value.
        /// </summary>
        /// <param name="var">Variable identifier.</param>
        /// <param name="val">Possible value.</param>
        /// <returns>True if it is possible, false otherwise</returns>
        internal bool boolPasses(int var, bool val)
        {
            return
                this.resolved[var] == false ||
                this.values[var].b == val;
        }

        /// <summary>
        /// Whether a particular variable in this node could have a
        /// particular TeamTask enumeration value.
        /// </summary>
        /// <param name="var">Variable identifier.</param>
        /// <param name="val">Possible value.</param>
        /// <returns>True if it is possible, false otherwise</returns>
        internal bool taskPasses(int var, TeamTask val)
        {
            return
                this.resolved[var] == false ||
                this.values[var].task == val;
        }

        /// <summary>
        /// Unify a Boolean value to a particular Variable.
        /// </summary>
        /// <param name="var">Variable identifier.</param>
        /// <param name="val">Value to set.</param>
        internal void setBool(int var, bool val)
        {
            this.values[var].b = val;
            this.resolved[var] = true;
        }

        /// <summary>
        /// Unify an integer value to a particular Variable.
        /// </summary>
        /// <param name="var">Variable identifier.</param>
        /// <param name="val">Value to set.</param>
        internal void setInt(int var, int val)
        {
            this.values[var].i = val;
            this.resolved[var] = true;
        }

        /// <summary>
        /// Unify a TileIndex value to a particular Variable.
        /// </summary>
        /// <param name="var">Variable identifier.</param>
        /// <param name="val">Value to set.</param>
        internal void setPosition(int var, ref TileIndex tile)
        {
            this.values[var].t = tile;
            this.resolved[var] = true;
        }

        /// <summary>
        /// Unify a TeamTask enum value to a particular Variable.
        /// </summary>
        /// <param name="var">Variable identifier.</param>
        /// <param name="val">Value to set.</param>
        internal void setTask(TeamTask val)
        {
            this.values[Variable.TeamTask].task = val;
            this.resolved[Variable.TeamTask] = true;
        }

        /// <summary>
        /// Clear a unified Variable so that it could have any value.
        /// </summary>
        /// <param name="var">Variable identifier to clear.</param>
        internal void unresolve(int var)
        {
            this.resolved[var] = false;
        }

        /// <summary>
        /// Determine whether another search node could be the same as
        /// this one, i.e., if variable values could unify.
        /// </summary>
        /// <param name="other">The node being compared.</param>
        /// <param name="failures">
        /// A list which will be filled with Variable
        /// identifiers which conflicted.
        /// </param>
        internal void unifiesWith(SearchNode other, List<int> failures)
        {
            failures.Clear();
            for (int i = 0; i < Variable.LENGTH; i++)
            {
                if (this.resolved[i])
                {
                    if (this.values[i].i != other.values[i].i)
                    {
                        failures.Add(i);
                    }
                }
            }
        }
        
        /// <summary>
        /// Guess how many actions would take to get from this SearchNode to
        /// another based on how many Variables conflict.
        /// </summary>
        /// <param name="other">The other SearchNode.</param>
        /// <returns>Number of variables which conflict.</returns>
        internal int dist(SearchNode other)
        {
            int dist = 0;
            for (int i = 0; i < Variable.LENGTH; i++)
            {
                if (this.resolved[i])
                {
                    if (this.values[i].i != other.values[i].i)
                    {
                        dist++;
                    }
                }
            }
            return dist;
        }

        /// <summary>
        /// Used by C# library functions to sort SearchNodes.
        /// </summary>
        /// <param name="x">First node in current ordering.</param>
        /// <param name="y">Second node in current ordering.</param>
        /// <returns>lt = -1, eq = 0, gt = 1</returns>
        public int Compare(SearchNode x, SearchNode y)
        {
            if (x.fValue < y.fValue)
                return -1;
            if (x.fValue > y.fValue)
                return 1;
            return 0;
        }

    }

    /// <summary>
    /// A C-style Union used to store different value types for
    /// different Variables.
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    internal struct VariableValue
    {
        [FieldOffset(0)]
        internal int i;
        [FieldOffset(0)]
        internal bool b;
        [FieldOffset(0)]
        internal TileIndex t;
        [FieldOffset(0)]
        internal TeamTask task;
    }

    /// <summary>
    /// Class for maintaining an enumeration for the Variables stored
    /// in the SearchNode class.
    /// </summary>
    internal static class Variable
    {
        internal const int Ammo = 0;
        internal const int Cover = 1;
        internal const int TargetHealth = 2;
        internal const int Health = 3;
        internal const int Location = 4;
        internal const int Weapon = 5;
        internal const int HasInvestigated = 6;
        internal const int HasPatrolled = 7;
        internal const int FarFromTarget = 8;
        internal const int TeamTask = 9;

        internal const int LENGTH = 10;
    }

    /// <summary>
    /// Enumeration representing different tasks requested by the team.
    /// </summary>
    internal enum TeamTask
    {
        CLEAR,
        SUPPRESS,
        FLUSH
    }
}
