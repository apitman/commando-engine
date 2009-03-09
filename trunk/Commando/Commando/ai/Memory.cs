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

namespace Commando.ai
{
    /// <summary>
    /// Storage location for an NPC's beliefs about the world.
    /// </summary>
    public class Memory
    {
        public Dictionary<BeliefType, List<Belief>> beliefs_;

        public Memory()
        {
            beliefs_ = new Dictionary<BeliefType, List<Belief>>();
        }

        /// <summary>
        /// Replaces a belief with matching type_ and handle_, or adds a new
        /// belief if a match does not exist.
        /// </summary>
        /// <param name="belief">Belief to store in memory.</param>
        public void setBelief(Belief belief)
        {
            List<Belief> list;
            if (!beliefs_.TryGetValue(belief.type_, out list))
            {
                beliefs_[belief.type_] = new List<Belief>();
                beliefs_[belief.type_].Add(belief);
                return;
            }
            Belief temp =
                list.Find(delegate(Belief b) { return b.handle_ == belief.handle_; });
            if (temp != null)
            {
                temp.replace(belief);
            }
            else
            {
                list.Add(belief);
            }
        }

        /// <summary>
        /// Combines all beliefs in memory into a single list.
        /// </summary>
        /// <returns>List of all current beliefs.</returns>
        public List<Belief> getAllBeliefs()
        {
            List<Belief> list = new List<Belief>();
            beliefs_.Values.ToList<List<Belief>>().ForEach(delegate(List<Belief> b) { list.AddRange(b); });
            return list;
        }

        /// <summary>
        /// Safely fetches all beliefs of a specific type.
        /// </summary>
        /// <param name="type">Desired belief type to fetch.</param>
        /// <returns>A list of all beliefs of that type, or an empty list if there are none.</returns>
        public List<Belief> getBeliefs(BeliefType type)
        {
            List<Belief> list;
            if (!beliefs_.TryGetValue(type, out list))
            {
                beliefs_[type] = new List<Belief>();
                return beliefs_[type];
            }
            return list;
        }

        /// <summary>
        /// Safely fetches all beliefs of a specific handle.
        /// Not yet implemented.
        /// </summary>
        /// <param name="handle">Desired handle of beliefs.</param>
        /// <returns>A list of all beliefs with that handle, or an empty list if there are none.</returns>
        public List<Belief> getBeliefs(Object handle)
        {
            List<Belief> list;
            throw new NotImplementedException();
        }

        /// <summary>
        /// Fetches the first-found belief with a specific type and handle.
        /// </summary>
        /// <param name="type">Type of belief to find.</param>
        /// <param name="handle">Handle of desired belief.</param>
        /// <returns>The first belief matching these criteria, or null if one is not found.</returns>
        public Belief getBelief(BeliefType type, Object handle)
        {
            List<Belief> list = getBeliefs(type);
            return
                list.Find(delegate(Belief b) { return b.handle_ == handle; });
        }

        /// <summary>
        /// Fetches the first belief of a specific type.
        /// </summary>
        /// <param name="type">Type of belief to be fetched.</param>
        /// <returns>The first belief of that type in memory, or null.</returns>
        public Belief getFirstBelief(BeliefType type)
        {
            List<Belief> list = getBeliefs(type);
            if (list.Count > 0)
                return list[0];
            return null;
        }

        /// <summary>
        /// Removes all beliefs of a certain type.
        /// </summary>
        /// <param name="type">Type of beliefs to be deleted.</param>
        /// <returns>True if a list of that type of belief existed at some time (not
        ///     whether there actually were any beliefs of that type).</returns>
        public bool removeBeliefs(BeliefType type)
        {
            return beliefs_.Remove(type);
        }

        /// <summary>
        /// Removes a belief from memory.
        /// </summary>
        /// <param name="belief">The exact belief to be removed (not a copy).</param>
        /// <returns>True if the belief was removed, false if it was not there.</returns>
        public bool removeBelief(Belief belief)
        {
            List<Belief> list;
            if (!beliefs_.TryGetValue(belief.type_, out list))
            {
                return false;
            }
            return list.Remove(belief);
        }
    }
}
