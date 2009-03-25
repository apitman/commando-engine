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
using Commando.levels;
using Commando.objects;

namespace Commando.ai.planning
{
    class ActionGoto : Action
    {
        internal TileIndex target_;

        public ActionGoto(NonPlayableCharacterAbstract character, TileIndex target)
            : base(character)
        {
            target_ = target;
        }

        internal override bool testPreConditions(SearchNode node)
        {
            // assume we can always get from point A to point B
            return true;
        }

        internal override SearchNode unifyRegressive(ref SearchNode node)
        {
            SearchNode parent = node.getPredecessor();
            TileIndex target = node.values[Variable.Location].t;
            parent.action = new ActionGoto(character_, target);

            // TODO calculate distance
            float distance = 0;
            parent.cost += 1.0f + distance;

            // Don't know where we came from to get here, so it could
            //  be anywhere - unresolve the value
            parent.resolved[Variable.Location] = false;

            // TODO
            // Figure out if this setBool is supposed to be false or true
            // Was originally true, but that doesn't seem like it does anything...
            if (node.values[Variable.Cover].b == true && node.resolved[Variable.Cover] == true)
                parent.setBool(Variable.Cover, false);

            return parent;
        }

        internal override void register(Dictionary<int, List<Action>> actionMap)
        {
            actionMap[Variable.Location].Add(this);
        }
    }
}
