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

namespace Commando.ai.sensors
{
    class SensorEars : Sensor
    {
        public SensorEars(AI ai) : base(ai) { }

        public override void collect()
        {
            Dictionary<int, Stimulus>.Enumerator cur = WorldState.Audial_.GetEnumerator();
            for (int i = 0; i < WorldState.Audial_.Count; i++)
            {
                cur.MoveNext();
                int id = cur.Current.Key;
                Stimulus stim = cur.Current.Value;
                filter(id, stim);
            }

            foreach (Belief bel in AI_.Memory_.getBeliefs(BeliefType.SuspiciousNoise))
            {
                if (CommonFunctions.distance(bel.position_, AI_.Character_.getPosition()) < Commando.levels.Tiler.tileSideLength_)
                {
                    bel.confidence_ /= 2;
                }
                Belief temp = bel.convert(BeliefType.InvestigateTarget);
                AI_.Memory_.setBelief(temp);
            }
        }

        private void filter(int id, Stimulus stim)
        {
            if (CommonFunctions.distance(AI_.Character_.getPosition(), stim.position_) < (double) stim.radius_)
            {
                AI_.CommunicationSystem_.isListening_ = true;
                if (stim.type_ == StimulusType.Position && stim.sourceAllegiance_ == AI_.Character_.allegiance_)
                {
                    Belief b = new Belief(BeliefType.SuspiciousNoise, null, 100);
                    b.position_ = stim.position_;
                    AI_.Memory_.setBelief(b);
                }
                else if (stim.type_ == StimulusType.Message && stim.sourceAllegiance_ == AI_.Character_.allegiance_)
                {
                    // For now, we believe messages that we receive with 100% certainty
                    AI_.Memory_.setBelief(stim.message_);
                }
            }
        }
    }
}
