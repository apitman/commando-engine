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
    public class SystemCommunication
    {
        protected int broadcastRadius_;

        protected AI AI_ { get; set; }

        protected int key_;

        /// <summary>
        /// Basic constructor
        /// </summary>
        public SystemCommunication(AI ai, int broadcastRadius)
        {
            AI_ = ai;
            broadcastRadius_ = broadcastRadius;
            key_ = StimulusIDGenerator.getNext();
        }

        /// <summary>
        /// Decides what, if anything, to broadcast, and broadcasts
        /// it
        /// </summary>
        public void update()
        {
            decideWhatToBroadcast();
        }

        /// <summary>
        /// Broadcasts the belief to all agents within the broadcast radius
        /// </summary>
        /// <param name="belief"></param>
        protected void broadcastBelief(Belief belief)
        {
            WorldState.Audial_.Remove(key_);
            WorldState.Audial_.Add(key_,
                new Stimulus(StimulusSource.CharacterAbstract, StimulusType.Position, broadcastRadius_, AI_.Character_.getPosition(), this));
        }

        /// <summary>
        /// Decides which beliefs to broadcast
        /// </summary>
        protected void decideWhatToBroadcast()
        {
            for (int i = 0; i < AI_.Memory_.beliefs_[BeliefType.EnemyLoc].Count; i++)
            {
                broadcastBelief(AI_.Memory_.beliefs_[BeliefType.EnemyLoc][i]);
            }
        }
    }
}
