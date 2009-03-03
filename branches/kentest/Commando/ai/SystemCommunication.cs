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

        /// <summary>
        /// Basic constructor
        /// </summary>
        public SystemCommunication(AI ai, int broadcastRadius)
        {
            AI_ = ai;
            broadcastRadius_ = broadcastRadius;
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
            // TODO Fill out this function
        }

        /// <summary>
        /// Decides which beliefs to broadcast
        /// </summary>
        protected void decideWhatToBroadcast()
        {
            // TODO Fill out this function
        }
    }
}
