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
using Commando.ai.planning;
using Commando.ai.sensors;

namespace Commando.ai.brains
{
    class BossAI : AI
    {
        const float FIELD_OF_VIEW = (float)Math.PI * 2;

        public BossAI(NonPlayableCharacterAbstract npc)
            : base(npc)
        {
            GoalManager_.addGoal(new GoalKill(this));

            PlanManager_.addAction(new ActionTypeGoto(npc));
            PlanManager_.addAction(new ActionTypeAttackRanged(npc));

            sensors_.Add(new SensorSeeCharacter(this, FIELD_OF_VIEW));

            systems_.Add(new BossSystemMissiles(this));
        }
    }
}
