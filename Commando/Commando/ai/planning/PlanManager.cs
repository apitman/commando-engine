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
using Microsoft.Xna.Framework;

namespace Commando.ai.planning
{
    internal class PlanManager
    {
        protected AI AI_;
        protected List<ActionType> actions_ = new List<ActionType>();
        protected List<Action> currentPlan_ = new List<Action>();
        protected Goal previousGoal_;

        internal bool HasFailed_ { get; set; }

        internal PlanManager(AI ai)
        {
            AI_ = ai;
            HasFailed_ = false;
        }

        internal void update()
        {
            bool differencesFlag =
                !Goal.areSame(previousGoal_, AI_.CurrentGoal_);

            if (AI_.CurrentGoal_ != null &&
                (differencesFlag || currentPlan_.Count == 0 || HasFailed_))
            {
                cleanupPlan(currentPlan_);
                HasFailed_ = false;

                IndividualPlanner planner = new IndividualPlanner(actions_);
                planner.execute(getInitialState(), AI_.CurrentGoal_.getNode());
                currentPlan_ = planner.getResult();

                if (currentPlan_ != null && currentPlan_.Count > 0)
                {
                    reservePlan(currentPlan_);
                    currentPlan_[0].initialize();
                }
                else
                {
                    HasFailed_ = true;
                }
            }

            previousGoal_ = AI_.CurrentGoal_;

            executePlan();

            if (HasFailed_)
            {
                AI_.CurrentGoal_.HasFailed_ = true;
            }
        }

        internal void executePlan()
        {
            if (currentPlan_ != null && currentPlan_.Count > 0)
            {
                bool isValid = currentPlan_[0].checkIsStillValid();
                if (!isValid)
                {
                    HasFailed_ = true;
                }
                else
                {
                    ActionStatus status = currentPlan_[0].update();
                    if (status == ActionStatus.SUCCESS)
                    {
                        currentPlan_.RemoveAt(0);
                        if (currentPlan_.Count > 0)
                        {
                            currentPlan_[0].initialize();
                        }
                    }
                    else if (status == ActionStatus.FAILED)
                    {

                        HasFailed_ = true;
                    }
                    else if (status == ActionStatus.IN_PROGRESS)
                    {
                        // Do nothing
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
                hasAmmo = AI_.Character_.Weapon_.CurrentAmmo_ > 0 ||
                    AI_.Character_.Inventory_.Ammo_[AI_.Character_.Weapon_.AmmoType_] > 0;
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
            initial.setTask(TeamTask.CLEAR);
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
            if (plan == null)
                return;
            for (int i = 0; i < plan.Count; i++)
            {
                plan[i].unreserve();
            }
            plan.Clear();
        }

        internal void addAction(ActionType action)
        {
            actions_.Add(action);
        }

        internal void die()
        {
            cleanupPlan(currentPlan_);
        }

        internal void draw()
        {
#if !XBOX
            if (AI_.CurrentGoal_ == null)
                return;
            StringBuilder planString = new StringBuilder();
            planString.Append("Goal: ");
            string goalName = AI_.CurrentGoal_.ToString();
            planString.Append(goalName.Substring(goalName.LastIndexOf('.')));
            planString.Append("\n");
            for (int i = 0; i < currentPlan_.Count; i++)
            {
                string planName = currentPlan_[i].ToString();
                planString.Append(planName.Substring(planName.LastIndexOf('.')));
                planString.Append("\n");
            }

            Vector2 prettyOffset = new Vector2(0.0f, -15.0f);
            Vector2 drawPosition = new Vector2(AI_.Character_.getPosition().X, AI_.Character_.getPosition().Y);
            drawPosition.X -= GlobalHelper.getInstance().getCurrentCamera().getX();
            drawPosition.Y -= GlobalHelper.getInstance().getCurrentCamera().getY();
            drawPosition += prettyOffset;
            FontMap.getInstance().getFont(FontEnum.Kootenay8).drawStringCentered(planString.ToString(), drawPosition, Microsoft.Xna.Framework.Graphics.Color.White, 0.0f, 0.9f);
#endif
        }
    }
}
