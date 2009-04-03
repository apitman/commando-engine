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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Commando.levels;

namespace Commando.ai.sensors
{
    class SensorSeeCharacter : SensorVisual
    {
        public SensorSeeCharacter(AI ai, float fov)
            : base(ai, fov)
        {

        }

        /// <summary>
        /// Examine all characters stored in the world state, determine if we can see them, and if
        /// we can, deduce useful information about them.
        /// </summary>
        public override void collect()
        {
            CharacterAbstract me = AI_.Character_;
            for (int i = 0; i < WorldState.CharacterList_.Count; i++)
            {
                CharacterAbstract character = WorldState.CharacterList_[i];
                BeliefType locationType;
                BeliefType healthType;
                if (me.Allegiance_ == character.Allegiance_)
                {
                    locationType = BeliefType.AllyLoc;
                    healthType = BeliefType.AllyHealth;
                }
                else
                {
                    locationType = BeliefType.EnemyLoc;
                    healthType = BeliefType.EnemyHealth;
                }

                // First see if the character is dead
                if (character.isDead())
                {
                    AI_.Memory_.removeBelief(locationType, character);
                    AI_.Memory_.removeBelief(healthType, character);
                    continue;
                }

                // Then see if I can see the character
                if (me != character &&
                    Raycaster.inFieldOfView(me.getDirection(), me.getPosition(), character.getPosition(), fieldOfView) &&
                    Raycaster.canSeePoint(me.getPosition(), character.getPosition(), me.getHeight(), character.getHeight()))
                {
                    Belief posBelief = new Belief(locationType, character, 100);
                    posBelief.position_ = character.getPosition();
                    Belief healthBelief = new Belief(healthType, character, 100);
                    healthBelief.position_ = character.getPosition();
                    healthBelief.data_.int1 = character.getHealth().getValue();
                    AI_.Memory_.setBelief(posBelief);
                    AI_.Memory_.setBelief(healthBelief);
                }
            }

            // TODO
            // Reimplement this correctly
            // Update the position and confidence of the AllyLoc and EnemyLoc beliefs
            foreach (Belief bel in AI_.Memory_.getBeliefs(BeliefType.EnemyLoc))
            {
                bel.confidence_ -= .4f;
            }
            
        }

    }
}
