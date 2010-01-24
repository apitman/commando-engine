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
using Commando.objects;

namespace Commando.ai.planning
{
    /// <summary>
    /// ActionType encapsulates the logic used in planning with a particular
    /// action.
    /// </summary>
    internal abstract class ActionType
    {
        protected NonPlayableCharacterAbstract character_;

        internal ActionType(NonPlayableCharacterAbstract character)
        {
            character_ = character;
        }

        // -----------------------------------------------------------------------
        // Methods in this class should only be implemented by Ken prior
        // to the end of Spring Semester 2009, as part of his DAI project.

        /// <summary>
        /// Determine whether this action could have preceded a particular
        /// planning search node.
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        internal abstract bool testPreConditions(SearchNode node);

        /// <summary>
        /// Determine a preceding state if this action had preceded the
        /// provided search node.
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        internal abstract SearchNode unifyRegressive(ref SearchNode node);

        /// <summary>
        /// Register an action with an action map.
        /// </summary>
        /// <param name="actionMap"></param>
        internal abstract void register(Dictionary<int, List<ActionType>> actionMap);
    }

    /// <summary>
    /// Action encapsulates the logic to carry out a particular action, verify that it
    /// is still valid, determine success, etc.
    /// </summary>
    internal abstract class Action
    {
        protected NonPlayableCharacterAbstract character_;

        internal Action(NonPlayableCharacterAbstract character)
        {
            character_ = character;
        }

        /// <summary>
        /// Reserve resources for this action.
        /// </summary>
        internal /*abstract */virtual void reserve() { }

        /// <summary>
        /// Clean up reservations for this action.
        /// </summary>
        internal /*abstract */virtual void unreserve() { }


        // -----------------------------------------------------------------------
        // Methods in this region can be implemented by anyone.

        internal /*abstract */virtual bool checkIsStillValid() { return true; }

        internal abstract bool initialize();

        internal abstract ActionStatus update();
    }

    enum ActionStatus
    {
        IN_PROGRESS,
        SUCCESS,
        FAILED,
        UNKNOWN
    }
}
