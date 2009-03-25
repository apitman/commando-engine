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

namespace Commando.ai
{
    public class SensorCover : Sensor
    {
        public SensorCover(AI ai) : base(ai) { }

        public override void collect()
        {
            if (AI_.Memory_.getFirstBelief(BeliefType.CoverLoc) == null)
            {
                List<CoverObject> coverObjects = WorldState.CoverList_;
                for (int i = 0; i < coverObjects.Count; i++)
                {
                    Belief cover = new Belief(BeliefType.CoverLoc, coverObjects[i], 100f, coverObjects[i].getPosition(), 0f);
                    AI_.Memory_.setBelief(cover);
                }
            }
        }
    }
}
