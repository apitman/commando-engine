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
        protected Dictionary<BeliefType, List<Belief>> beliefs_;

        protected Dictionary<BeliefType, Belief> cachedBeliefs_;

        public Memory()
        {
            beliefs_ = new Dictionary<BeliefType, List<Belief>>();
            cachedBeliefs_ = new Dictionary<BeliefType, Belief>();
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
                cachedBeliefs_.Add(belief.type_, belief);
                return;
            }
            Belief temp =
                list.Find(delegate(Belief b) { return b.handle_ == belief.handle_; });
            if (temp != null)
            {
                bool relevanceReduced = (temp.relevance_ < belief.relevance_);
                temp.replace(belief);
                if (relevanceReduced && isCached(temp))
                {
                    updateCachedBelief(belief.type_);
                }
            }
            else
            {
                list.Add(belief);
                updateCachedBelief(belief);
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
        /// Fetches the cached belief of a specific type.
        /// </summary>
        /// <param name="type">Type of belief to be fetched.</param>
        /// <returns>The currently cached belief of that type, or null if there isn't one.</returns>
        public Belief getFirstBelief(BeliefType type)
        {
            Belief cached;
            cachedBeliefs_.TryGetValue(type, out cached);
            return cached;
        }

        /// <summary>
        /// Removes all beliefs of a certain type.
        /// </summary>
        /// <param name="type">Type of beliefs to be deleted.</param>
        /// <returns>True if a list of that type of belief existed at some time (not
        ///     whether there actually were any beliefs of that type).</returns>
        public bool removeBeliefs(BeliefType type)
        {
            cachedBeliefs_.Remove(type);
            return beliefs_.Remove(type);
        }

        /// <summary>
        /// Removes a belief from memory.
        /// </summary>
        /// <param name="belief">The exact belief to be removed (not a copy).</param>
        /// <returns>True if the belief was removed, false if it was not there.</returns>
        public bool removeBelief(Belief belief)
        {
            // Test that we even have beliefs of this type
            List<Belief> list;
            if (!beliefs_.TryGetValue(belief.type_, out list))
            {
                return false;
            }

            // Try and remove this belief, and update the cache if necessary
            bool wasRemoved = list.Remove(belief);
            if (isCached(belief))
            {
                updateCachedBelief(belief.type_);
            }

            return list.Remove(belief);
        }

        /// <summary>
        /// Removes a belief from memory.
        /// </summary>
        /// <param name="type">Type of the belief to be deleted.</param>
        /// <param name="handle">Handle of the belief to be deleted.</param>
        /// <returns>True if the belief was removed, false if it was not there.</returns>
        public bool removeBelief(BeliefType type, Object handle)
        {
            Belief belief = getBelief(type, handle);
            if (belief == null)
                return false;
            return removeBelief(belief);
        }

        /// <summary>
        /// Tests whether a given belief is the currently cached belief.
        /// </summary>
        /// <param name="belief">The belief to test against the cache.</param>
        /// <returns>True if the belief is the cached belief, false otherwise.</returns>
        protected bool isCached(Belief belief)
        {
            return (cachedBeliefs_.ContainsKey(belief.type_) && cachedBeliefs_[belief.type_] == belief);
        }

        /// <summary>
        /// Compares a belief against the cached belief and makes
        /// a replacement of the cached belief if the argument belief
        /// has a higher relevance.
        /// </summary>
        /// <param name="belief">Belief to be tested against the cache.</param>
        protected void updateCachedBelief(Belief belief)
        {
            Belief cached;
            cachedBeliefs_.TryGetValue(belief.type_, out cached);
            if (cached == null)
            {
                cachedBeliefs_.Add(belief.type_, belief);
            }
            else
            {
                if (belief.relevance_ > cached.relevance_)
                {
                    cachedBeliefs_[belief.type_] = belief;
                }
            }
        }

        /// <summary>
        /// Runs through all beliefs of a certain type and updates the cache
        /// to contain the most relevant.
        /// </summary>
        /// <param name="type">Type of belief in cache we want to refresh.</param>
        protected void updateCachedBelief(BeliefType type)
        {
            List<Belief> beliefs = getBeliefs(type);

            Belief bestBelief = null;
            float bestScore = float.MinValue;
            for (int i = 0; i < beliefs.Count; i++)
            {
                if (beliefs[i].relevance_ > bestScore)
                {
                    bestBelief = beliefs[i];
                    bestScore = beliefs[i].relevance_;
                }
            }

            cachedBeliefs_[type] = bestBelief;
        }

        /// <summary>
        /// Safely changes the relevance of a belief in memory.
        /// </summary>
        /// <param name="belief">Belief whose relevance will be changed.</param>
        /// <param name="relevance">New relevance for the belief.</param>
        public void updateRelevance(Belief belief, float relevance)
        {
            bool relevanceReduced = (relevance < belief.relevance_);
            belief.relevance_ = relevance;

            // if relevance was reduced and this belief was cached, refresh
            //  the cache for that type
            if (relevanceReduced && isCached(belief))
            {
                updateCachedBelief(belief.type_);
            }

            // otherwise if relevance improved or stayed safe, see if this
            //  belief now belongs in the cache
            else if (!relevanceReduced)
            {
                updateCachedBelief(belief);
            }
        }
    }
}
