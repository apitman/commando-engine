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
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Commando.ai
{
    class SystemTargetSelection : System
    {
        internal SystemTargetSelection(AI ai) : base(ai) { }

        internal override void update()
        {
            Belief firstEnemy = AI_.Memory_.getFirstBelief(BeliefType.EnemyLoc);
            if (firstEnemy != null)
            {
                AI_.Memory_.removeBeliefs(BeliefType.BestTarget);
                AI_.Memory_.setBelief(new Belief(BeliefType.BestTarget, firstEnemy.handle_, 100, firstEnemy.position_, firstEnemy.value_));
            }
        }
    }
}