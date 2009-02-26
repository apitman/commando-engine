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
    public class InferenceEngine
    {
        protected const int TOLERANCE = 15;

        protected AI AI_ {get; set;}

        /// <summary>
        /// Basic constructor
        /// </summary>
        /// <param name="ai">A handle back to the AI owner</param>
        public InferenceEngine(AI ai)
        {
            AI_ = ai;
        }

        /// <summary>
        /// The inference engine is the "intelligent" thing that updates
        /// beliefs
        /// </summary>
        public void update()
        {
            for (int i = 0; i < AI_.Memory_.Beliefs_.Count; i++)
            {
                KeyValuePair<int, Belief> pair = AI_.Memory_.Beliefs_.ElementAt(i);
                if (pair.Value.type_ == BeliefType.EnemyLoc)
                {
                    // Don't care about suspicious noises if we know for sure
                    // where the enemy is, so we remove them from our beliefs
                    if (pair.Value.confidence_ == 100)
                    {
                        removeSuspiciousNoiseBeliefs();
                    }

                    // Remove the belief if we have checked out the location
                    if (CommonFunctions.distance(AI_.Character_.getPosition(), pair.Value.position_) < TOLERANCE)
                    {
                        AI_.Memory_.Beliefs_.Remove(pair.Key);
                    }
                }
                if (pair.Value.type_ == BeliefType.SuspiciousNoise)
                {
                    // Remove the belief if we have checked out the location
                    if (CommonFunctions.distance(AI_.Character_.getPosition(), pair.Value.position_) < TOLERANCE)
                    {
                        AI_.Memory_.Beliefs_.Remove(pair.Key);
                    }
                }
            }
        }

        protected void removeSuspiciousNoiseBeliefs()
        {
            // Remove the belief if we know for sure where the enemy is
            for (int i = 0; i < AI_.Memory_.Beliefs_.Count; i++)
            {
                KeyValuePair<int, Belief> pair = AI_.Memory_.Beliefs_.ElementAt(i);
                if (pair.Value.type_ == BeliefType.SuspiciousNoise)
                {
                    AI_.Memory_.Beliefs_.Remove(pair.Key);
                }
            }
        }
    }
}
