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
    internal class ActionAttackRangedCoverType : ActionType
    {
        protected internal const int COST = 1;

        internal ActionAttackRangedCoverType(NonPlayableCharacterAbstract character)
            : base(character)
        {
            
        }

        internal override bool testPreConditions(SearchNode node)
        {
            return
                node.boolPasses(Variable.Cover, true) && 
                node.boolPasses(Variable.Weapon, true) &&
                node.boolPasses(Variable.Ammo, true);
        }

        internal override SearchNode unifyRegressive(ref SearchNode node)
        {
            SearchNode parent = node.getPredecessor();
            parent.action = new ActionAttackRangedCover(character_);
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
        protected SystemAiming aiming;

        internal ActionAttackRangedCover(NonPlayableCharacterAbstract character)
            : base(character)
        {
            
        }

        internal override bool update()
        {
            aiming.update();

            // Move this into checkIsStillValid
            if (character_.Weapon_.CurrentAmmo_ <= 0 || aiming.lossFlag)
            {
                Belief bestTarget = character_.AI_.Memory_.getFirstBelief(BeliefType.BestTarget);
                if (bestTarget != null)
                {
                    Belief enemyLoc = character_.AI_.Memory_.getBelief(BeliefType.EnemyLoc, bestTarget.handle_);
                    if (enemyLoc != null)
                    {
                        character_.AI_.Memory_.removeBelief(enemyLoc);
                    }
                    character_.AI_.Memory_.removeBelief(bestTarget);
                }
                character_.AI_.Memory_.removeBeliefs(BeliefType.BestTarget);
                character_.AI_.Memory_.removeBeliefs(BeliefType.EnemyLoc);
                character_.AI_.Memory_.removeBeliefs(BeliefType.SuspiciousNoise);

                // remove from cover, then go after target's last known location
                DefaultActuator da = (character_.getActuator() as DefaultActuator);
                CoverObject cover = da.getCoverObject();
                if (cover != null)
                    da.cover(da.getCoverObject());
                (character_.getActuator() as DefaultActuator).moveTo(bestTarget.position_);

                return true;
            }

            return false;
        }

        internal override bool initialize()
        {
            aiming = new SystemAiming(character_.AI_);

            //throw new NotImplementedException();
            Belief bestTarget =
                character_.AI_.Memory_.getFirstBelief(BeliefType.BestTarget);
            Vector2 target = bestTarget.position_;

            DefaultActuator da = (character_.getActuator() as DefaultActuator);
            da.lookAt(target);
            da.update();
            da.throwGrenade(
                new FragGrenade(character_.pipeline_, character_.getCollisionDetector(),
                    character_.getDirection(), character_.getPosition(),
                    character_.getDirection()));
            da.update();
            return true;
        }
    }
}
