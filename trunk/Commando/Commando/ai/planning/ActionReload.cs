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
    class ActionReloadType : ActionType
    {
        internal ActionReloadType(NonPlayableCharacterAbstract character)
            : base(character)
        {
            throw new NotImplementedException();
        }

        internal override bool testPreConditions(SearchNode node)
        {
            return
                node.boolPasses(Variable.Weapon, true) &&
                node.boolPasses(Variable.Ammo, false);
        }

        internal override SearchNode unifyRegressive(ref SearchNode node)
        {
            SearchNode parent = node.getPredecessor();
            throw new NotImplementedException();
            //parent.action = new ActionReload(character_);
            parent.cost += 1;
            parent.setBool(Variable.Ammo, false);
            parent.setBool(Variable.Weapon, true);
            return parent;
        }

        internal override void register(Dictionary<int, List<ActionType>> actionMap)
        {
            actionMap[Variable.Ammo].Add(this);
        }

    }
}
