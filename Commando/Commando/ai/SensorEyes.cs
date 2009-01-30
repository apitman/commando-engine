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
    class SensorEyes : Sensor
    {
        public SensorEyes(Memory memory_) : base(memory_) { }

        public override void collect()
        {
            for (int i = 0; i < WorldState.visual.Count; i++)
            {
                filter(WorldState.visual[i]);
            }
        }

        private void filter(Stimulus stim)
        {
            if (stim.type_ == StimulusSourceType.CharacterAbstract)
            {
                memory_.beliefs_.Add(new Belief(BeliefType.EnemyLoc, 100, stim.position_.X, stim.position_.Y));
            }
        }

    }
}
