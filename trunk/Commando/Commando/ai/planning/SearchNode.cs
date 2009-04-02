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
    /// A node in the search space used by IndividualPlanner.
    /// </summary>
    internal class SearchNode : IComparer<SearchNode>
    {
        internal Action action;

        internal float fValue;
        internal float cost;
        internal SearchNode next;

        internal VariableValue[] values = new VariableValue[Variable.LENGTH];
        internal bool[] resolved = new bool[Variable.LENGTH];

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

        internal bool boolPasses(int var, bool val)
        {
            return
                this.resolved[var] == false ||
                this.values[var].b == val;
        }

        internal bool taskPasses(int var, TeamTask val)
        {
            return
                this.resolved[var] == false ||
                this.values[var].task == val;
        }

        internal void setBool(int var, bool val)
        {
            this.values[var].b = val;
            this.resolved[var] = true;
        }

        internal void setInt(int var, int val)
        {
            this.values[var].i = val;
            this.resolved[var] = true;
        }

        internal void setPosition(int var, ref TileIndex tile)
        {
            this.values[var].t = tile;
            this.resolved[var] = true;
        }

        internal void setTask(TeamTask val)
        {
            this.values[Variable.TeamTask].task = val;
            this.resolved[Variable.TeamTask] = true;
        }

        internal void unresolve(int var)
        {
            this.resolved[var] = false;
        }

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

        // Remove this when a wrapper Goal class is used
        /*
        public static bool testGoalEquality(SearchNode lhs, SearchNode rhs)
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
         */

        #region IComparer<SearchNode> Members

        public int Compare(SearchNode x, SearchNode y)
        {
            if (x.fValue < y.fValue)
                return -1;
            if (x.fValue > y.fValue)
                return 1;
            return 0;
        }

        #endregion
    }

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
    /// Class for maintaining an enumeration for the variables stored
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
        internal const int TeamTask = 8;

        internal const int LENGTH = 9;
    }

    internal enum TeamTask
    {
        CLEAR,
        SUPPRESS
    }
}
