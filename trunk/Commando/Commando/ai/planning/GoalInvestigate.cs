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
    class GoalInvestigate : Goal
    {
        internal GoalInvestigate(AI ai)
            : base(ai)
        {
            node_ = new SearchNode();
            node_.setBool(Variable.HasInvestigated, true);
        }

        internal override void refresh()
        {
            Belief investigateTarget = AI_.Memory_.getFirstBelief(BeliefType.InvestigateTarget);
            if (investigateTarget != null)
            {
                if (investigateTarget.handle_ != this.handle_)
                {
                    // new target, so reset the HasFailed flag
                    HasFailed_ = false;
                }
                this.handle_ = investigateTarget.handle_;
                Relevance_ = 10f + investigateTarget.confidence_ / 2;
            }
            else
            {
                this.handle_ = null;
                Relevance_ = 0.0f;
            }
        }
    }
}
