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
using Commando.levels;

namespace Commando.ai.planning
{
    internal class PlanManager
    {
        protected AI AI_;
        protected List<Action> currentPlan_;
        protected Goal previousGoal_;

        internal bool HasFailed_ { get; set; }

        internal PlanManager(AI ai)
        {
            AI_ = ai;
            currentPlan_ = new List<Action>();

            HasFailed_ = false;
        }

        internal void update()
        {
            bool differencesFlag =
                !Goal.areSame(previousGoal_, AI_.CurrentGoal_);

            if (AI_.CurrentGoal_ != null &&
                (differencesFlag || currentPlan_.Count == 0))
            {
                IndividualPlanner planner = new IndividualPlanner(AI_.Actions_);
                planner.execute(getInitialState(), AI_.CurrentGoal_.getNode());
                currentPlan_ = planner.getResult();
                if (currentPlan_ != null && currentPlan_.Count > 0)
                {
                    reservePlan(currentPlan_);
                    currentPlan_[0].initialize();
                }

            }

            previousGoal_ = AI_.CurrentGoal_;

            executePlan();
        }

        internal void executePlan()
        {
            if (currentPlan_ != null && currentPlan_.Count > 0)
            {
                bool isValid = currentPlan_[0].checkIsStillValid();
                if (!isValid)
                {
                    HasFailed_ = true;
                    cleanupPlan(currentPlan_);
                }
                else
                {
                    bool done = currentPlan_[0].update();
                    if (done)
                    {
                        currentPlan_.RemoveAt(0);
                        if (currentPlan_.Count > 0)
                        {
                            currentPlan_[0].initialize();
                        }
                    }
                }
            }
        }

        internal SearchNode getInitialState()
        {
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
            initial.setInt(Variable.TargetHealth, 1); // TODO change this to TargetHealth/WeaponDamage
            initial.setBool(Variable.Weapon, hasWeapon);
            initial.setBool(Variable.Ammo, hasAmmo);
            initial.setBool(Variable.HasInvestigated, false);
            initial.setBool(Variable.HasPatrolled, false);
            initial.setPosition(Variable.Location, ref myLoc);
            return initial;
        }

        protected void reservePlan(List<Action> plan)
        {
            for (int i = 0; i < plan.Count; i++)
            {
                plan[i].reserve();
            }
        }

        protected void cleanupPlan(List<Action> plan)
        {
            for (int i = 0; i < plan.Count; i++)
            {
                plan[i].unreserve();
            }
            plan.Clear();
        }
    }
}
