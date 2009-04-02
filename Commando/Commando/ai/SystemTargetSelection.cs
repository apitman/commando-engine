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
    class SystemTargetSelection : System
    {
        internal SystemTargetSelection(AI ai) : base(ai) { }

        internal override void update()
        {
            // Translate Enemy Locations into targets
            List<Belief> targets = AI_.Memory_.getBeliefs(BeliefType.EnemyLoc);
            for (int i = 0; i < targets.Count; i++)
            {
                Belief bestTarget = targets[i].convert(BeliefType.BestTarget);
                bestTarget.relevance_ = 1 / (targets[i].position_ - AI_.Character_.getPosition()).LengthSquared();
                AI_.Memory_.setBelief(bestTarget);
            }

            // Clean up the most relevant BestTarget if necessary
            Belief best = AI_.Memory_.getFirstBelief(BeliefType.BestTarget);
            if (best == null)
                return;
            CharacterAbstract enemy = (best.handle_ as CharacterAbstract);
            if (enemy.isDead())
            {
                AI_.Memory_.removeBelief(best);
            }
        }
    }
}
