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

        public List<Belief> getAllBeliefs()
        {
            List<Belief> list = new List<Belief>();
            beliefs_.Values.ToList<List<Belief>>().ForEach(delegate(List<Belief> b) { list.AddRange(b); });
            return list;
        }

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

        public List<Belief> getBeliefs(Object handle)
        {
            List<Belief> list;
            throw new NotImplementedException();
        }

        public Belief getFirstBelief(BeliefType type)
        {
            List<Belief> list = getBeliefs(type);
            if (list.Count > 0)
                return list[0];
            return null;
        }

        public bool removeBeliefs(BeliefType type)
        {
            return beliefs_.Remove(type);
        }

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
