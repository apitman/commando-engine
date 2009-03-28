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
using Commando.ai.planning;

namespace Commando.ai
{
    class SystemGoalSelection : System
    {
        internal SystemGoalSelection(AI ai) : base(ai) { }

        internal override void update()
        {
            float highestRelevance = float.MinValue;
            Goal mostRelevant = null;
            for (int i = 0; i < AI_.Goals_.Count; i++)
            {
                AI_.Goals_[i].refresh();
                if (AI_.Goals_[i].Relevance_ > highestRelevance)
                {
                    highestRelevance = AI_.Goals_[i].Relevance_;
                    mostRelevant = AI_.Goals_[i];
                }
            }

            if (mostRelevant == null)
            {
                throw new NotImplementedException("AIs without goals not yet supported");
            }

            AI_.CurrentGoal_ = mostRelevant;
        }
    }
}
