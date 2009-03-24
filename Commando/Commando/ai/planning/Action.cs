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
    internal abstract class Action
    {
        protected NonPlayableCharacterAbstract character_;

        internal Action()
        {
            throw new NotImplementedException();
            // make compiler happy, remove this once real actions exist
        }

        internal Action(NonPlayableCharacterAbstract character)
        {
            character_ = character;
        }

        #region Planning Methods
        // -----------------------------------------------------------------------

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
        internal abstract SearchNode unifyRegressive(SearchNode node);

        /// <summary>
        /// Register an action with an action map.
        /// </summary>
        /// <param name="actionMap"></param>
        internal abstract void register(Dictionary<int, Action> actionMap);

        /// <summary>
        /// Reserve resources for this action.
        /// </summary>
        internal /*abstract */virtual void reserve() { }

        /// <summary>
        /// Clean up reservations for this action.
        /// </summary>
        internal /*abstract */virtual void unreserve() { }

        #endregion

        #region Runtime Methods
        // -----------------------------------------------------------------------

        internal /*abstract */bool checkIsStillValid() { return false; }

        internal /*abstract */bool initialize() { return false; }

        internal /*abstract */bool update() { return false; }

        #endregion
    }
}
