﻿/*
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

namespace Commando.ai
{
    class SensorSeeCharacter : Sensor
    {

        const float FIELD_OF_VIEW = (float)Math.PI;

        public SensorSeeCharacter(AI ai) : base(ai) { }

        /// <summary>
        /// Examine all characters stored in the world state, determine if we can see them, and if
        /// we can, deduce useful information about them.
        /// </summary>
        public override void collect()
        {
            CharacterAbstract me = AI_.Character_;
            for (int i = 0; i < WorldState.EnemyList_.Count; i++)
            {
                
                CharacterAbstract ally = WorldState.EnemyList_[i];
                if (me != ally &&
                    Raycaster.inFieldOfView(me.getDirection(), me.getPosition(), ally.getPosition(), FIELD_OF_VIEW) &&
                    Raycaster.canSeePoint(me.getPosition(), ally.getPosition(), me.getHeight(), ally.getHeight()))
                {
                    Belief posBelief = new Belief(BeliefType.AllyLoc, ally, 100);
                    posBelief.position_ = ally.getPosition();
                    Belief healthBelief = new Belief(BeliefType.AllyHealth, ally, 100);
                    healthBelief.position_ = ally.getPosition();
                    healthBelief.data_.int1 = ally.getHealth().getValue();
                    AI_.Memory_.setBelief(posBelief);
                    AI_.Memory_.setBelief(healthBelief);
                }
            }
            CharacterAbstract enemy = WorldState.MainPlayer_;
            if (Raycaster.inFieldOfView(me.getDirection(), me.getPosition(), enemy.getPosition(), FIELD_OF_VIEW) &&
                Raycaster.canSeePoint(me.getPosition(), enemy.getPosition(), me.getHeight(), enemy.getHeight()))
            {
                Belief posBelief = new Belief(BeliefType.EnemyLoc, enemy, 100);
                posBelief.position_ = enemy.getPosition();
                Belief healthBelief = new Belief(BeliefType.EnemyHealth, enemy, 100);
                healthBelief.position_ = enemy.getPosition();
                healthBelief.data_.int1 = enemy.getHealth().getValue();
                AI_.Memory_.setBelief(posBelief);
                AI_.Memory_.setBelief(healthBelief);
            }
            else
            {
                // Update the position and confidence of the EnemyLoc beliefs
                foreach (Belief bel in AI_.Memory_.getBeliefs(BeliefType.EnemyLoc))
                {
                    if (CommonFunctions.distance(bel.position_, me.getPosition()) < Tiler.tileSideLength_)
                    {
                        bel.confidence_ /= 2;
                    }
                }
            }
        }

    }
}