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
    class GoalKill : Goal
    {
        internal GoalKill(AI ai)
            : base(ai)
        {
            node_ = new SearchNode();
            node_.setInt(Variable.TargetHealth, 0);
        }

        internal override void refresh()
        {
            Belief target = AI_.Memory_.getFirstBelief(BeliefType.BestTarget);
            if (target != null)
            {
                if (target.handle_ != this.handle_)
                {
                    // new target, so reset the HasFailed flag
                    HasFailed_ = false;
                }
                this.handle_ = target.handle_;
                Relevance_ = 0.5f + target.confidence_ / 2;
            }
            else
            {
                this.handle_ = null;
                Relevance_ = 0.0f;
            }
        }
    }
}
