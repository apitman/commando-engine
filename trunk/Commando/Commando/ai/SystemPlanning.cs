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

using System.Collections.Generic;
using System.Text;
using Commando.ai.planning;
using Commando.levels;

namespace Commando.ai
{
    /// <summary>
    /// System responsible for action planning based on beliefs about the
    /// world and the character's available actions.
    /// </summary>
    internal class SystemPlanning : System
    {
        internal SearchNode previousGoal_;

        internal SystemPlanning(AI ai) : base(ai) { }

        internal override void update()
        {
            List<int> differences = new List<int>();
            bool differencesFlag = false;
            if (AI_.CurrentGoal_ != null && previousGoal_ != null)
            {
                AI_.CurrentGoal_.resolvesWith(previousGoal_, differences);
                if (differences.Count > 0)
                {
                    differencesFlag = true;
                }
            }

            if (AI_.CurrentGoal_ != null &&
                (differencesFlag || AI_.CurrentPlan_.Count == 0))
            {
                // Clean up previous plan
                if (AI_.CurrentPlan_ != null)
                {
                    List<Action> oldPlan = AI_.CurrentPlan_;
                    for (int i = 0; i < oldPlan.Count; i++)
                    {
                        oldPlan[i].unreserve();
                    }
                }

                // And now get a new one
                SearchNode initial = new SearchNode();

                bool hasWeapon = AI_.Character_.Weapon_ != null;
                bool hasAmmo = false;
                if (hasWeapon)
                {
                    hasAmmo = AI_.Character_.Weapon_.CurrentAmmo_ > 0;
                }
                TileIndex myLoc =
                    GlobalHelper.getInstance().getCurrentLevelTileGrid().getTileIndex(
                        AI_.Character_.getPosition());

                initial.setInt(Variable.Health, AI_.Character_.getHealth().getValue());
                initial.setInt(Variable.TargetHealth, 0);
                initial.setBool(Variable.Weapon, hasWeapon);
                initial.setBool(Variable.Ammo, hasAmmo);
                initial.setBool(Variable.HasInvestigated, false);
                initial.setBool(Variable.HasPatrolled, false);
                initial.setPosition(Variable.Location, ref myLoc);

                IndividualPlanner planner = new IndividualPlanner(AI_.Actions_);
                planner.execute(initial, AI_.CurrentGoal_);
                AI_.CurrentPlan_ = planner.getResult();
                if (AI_.CurrentPlan_ != null && AI_.CurrentPlan_.Count > 0)
                {
                    AI_.CurrentPlan_[0].initialize();
                }
            }

            previousGoal_ = AI_.CurrentGoal_;
        }
    }

}
