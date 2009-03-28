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

namespace Commando.ai.brains
{
    public class HumanAI : AI
    {
        public HumanAI(NonPlayableCharacterAbstract npc)
            : base(npc)
        {
            Goals_.Add(new GoalPatrol(this));
            Goals_.Add(new GoalInvestigate(this));
            Goals_.Add(new GoalKill(this));

            Actions_.Add(new ActionTakeCover(npc));
            Actions_.Add(new ActionInvestigate(npc));
            Actions_.Add(new ActionGoto(npc));
            Actions_.Add(new ActionPatrol(npc));
            //Actions_.Add(new ActionAttackRanged(npc));
            Actions_.Add(new ActionAttackRangedCover(npc));
            Actions_.Add(new ActionFlee(npc));

            sensors_.Add(new SensorEars(this));
            sensors_.Add(new SensorSeeCharacter(this));
            sensors_.Add(new SensorCover(this));
        }
    }
}
