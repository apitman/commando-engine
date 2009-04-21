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
    class ActionTypeAggressiveFire : ActionType
    {
        protected internal const int COST = 3;

        internal ActionTypeAggressiveFire(NonPlayableCharacterAbstract character)
            : base(character)
        {

        }

        internal override bool testPreConditions(SearchNode node)
        {
            return
                (character_.AI_.Memory_.getFirstBelief(BeliefType.BestTarget) != null) &&
                node.boolPasses(Variable.Weapon, true) &&
                node.boolPasses(Variable.Ammo, true);
        }

        internal override SearchNode unifyRegressive(ref SearchNode node)
        {
            Belief target = character_.AI_.Memory_.getFirstBelief(BeliefType.BestTarget);

            SearchNode parent = node.getPredecessor();
            parent.action = new ActionAggressiveFire(character_, target.handle_);
            parent.cost += COST;
            parent.setInt(Variable.TargetHealth, node.values[Variable.TargetHealth].i + 1);
            parent.setBool(Variable.Ammo, true);

            return parent;
        }

        internal override void register(Dictionary<int, List<ActionType>> actionMap)
        {
            actionMap[Variable.TargetHealth].Add(this);
        }
    }

    class ActionAggressiveFire : Action
    {
        protected CharacterAbstract target;
        protected Object handle_;
        protected SystemAiming aiming;

        protected ActionGoto gotoAction;

        internal ActionAggressiveFire(NonPlayableCharacterAbstract character, Object handle)
            : base(character)
        {
            handle_ = handle;
        }

        internal override bool initialize()
        {
            target = (handle_ as CharacterAbstract);
            aiming = new SystemAiming(character_.AI_, target);
            return true;
        }

        internal override ActionStatus update()
        {
            aiming.update();

            // TODO
            // Move this into checkIsStillValid
            if (character_.Weapon_.CurrentAmmo_ <= 0)
            {
                character_.reload();
                return ActionStatus.IN_PROGRESS;
            }

            if (!aiming.lossFlag)
            {
                return ActionStatus.IN_PROGRESS;
            }
            else if (aiming.lossFlag && gotoAction == null)
            {
                Belief b = character_.AI_.Memory_.getBelief(BeliefType.EnemyLoc, target);
                if (b == null)
                    return ActionStatus.FAILED;

                gotoAction = new ActionGoto(character_, GlobalHelper.getInstance().getCurrentLevelTileGrid().getTileIndex(b.position_));
                gotoAction.initialize();

                return gotoAction.update();
            }
            else
            {
                return gotoAction.update();
            }
        }
    }
}
