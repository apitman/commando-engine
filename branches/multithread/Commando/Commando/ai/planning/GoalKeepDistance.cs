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

namespace Commando.ai.planning
{
    class GoalKeepDistance : Goal
    {
        internal GoalKeepDistance(AI ai)
            : base(ai)
        {
            node_ = new SearchNode();
            node_.setBool(Variable.FarFromTarget, true);
        }

        internal override void refresh()
        {
            Belief b = AI_.Memory_.getFirstBelief(BeliefType.BestTarget);
            if (b == null)
            {
                this.handle_ = null;
                Relevance_ = 0.0f;
            }
            else
            {
                this.handle_ = b.handle_;
                float distRelevance = (200 - CommonFunctions.distance(AI_.Character_.getPosition(), b.position_));
                float healthRelevance = (100 - AI_.Character_.getHealth().getValue());
                if (distRelevance < 0) distRelevance = 0;
                if (healthRelevance < 0) healthRelevance = 0;
                Relevance_ = distRelevance + healthRelevance;
            }
        }
    }
}
