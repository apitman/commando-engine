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
using Commando.graphics;
using Commando.objects.weapons;
using Microsoft.Xna.Framework;

namespace Commando.ai.planning
{
    internal class ActionTypeAttackRangedCover : ActionType
    {
        protected internal const int COST = 1;

        internal ActionTypeAttackRangedCover(NonPlayableCharacterAbstract character)
            : base(character)
        {
            
        }

        internal override bool testPreConditions(SearchNode node)
        {
            return
                (character_.AI_.Memory_.getFirstBelief(BeliefType.BestTarget) != null) &&
                node.boolPasses(Variable.Cover, true) && 
                node.boolPasses(Variable.Weapon, true) &&
                node.boolPasses(Variable.Ammo, true);
        }

        internal override SearchNode unifyRegressive(ref SearchNode node)
        {
            Belief target = character_.AI_.Memory_.getFirstBelief(BeliefType.BestTarget);

            SearchNode parent = node.getPredecessor();
            parent.action = new ActionAttackRangedCover(character_, target.handle_);
            parent.cost += COST;
            parent.setInt(Variable.TargetHealth, node.values[Variable.TargetHealth].i + 1);
            parent.setBool(Variable.Weapon, true);
            parent.setBool(Variable.Ammo, true);
            parent.setBool(Variable.Cover, true);
            return parent;
        }

        internal override void register(Dictionary<int, List<ActionType>> actionMap)
        {
            actionMap[Variable.TargetHealth].Add(this);
        }
    }

    internal class ActionAttackRangedCover : Action
    {
        protected CharacterAbstract target;
        protected Object handle_;
        protected SystemAiming aiming;

        internal ActionAttackRangedCover(NonPlayableCharacterAbstract character, Object handle)
            : base(character)
        {
            handle_ = handle;
        }

        internal override ActionStatus update()
        {
            aiming.update();

            // Move this into checkIsStillValid
            if (character_.Weapon_.CurrentAmmo_ <= 0 || aiming.lossFlag)
            {
                character_.reload();

                // remove from cover, then go after target's last known location
                DefaultActuator da = (character_.getActuator() as DefaultActuator);
                CoverObject cover = da.getCoverObject();
                if (cover != null)
                {
                    da.cover(da.getCoverObject());
                    if (ReservationTable.isReservedBy(cover, character_))
                    {
                        ReservationTable.freeResource(cover, character_);
                    }
                }
                //(character_.getActuator() as DefaultActuator).moveTo(bestTarget.position_);

                return ActionStatus.SUCCESS;
            }

            return ActionStatus.IN_PROGRESS;
        }

        internal override bool initialize()
        {
            target = (handle_ as CharacterAbstract);
            aiming = new SystemAiming(character_.AI_, target);

            return true;
        }
    }
}
