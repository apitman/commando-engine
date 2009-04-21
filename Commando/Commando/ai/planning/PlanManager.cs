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
    /// <summary>
    /// Manages an agent's action plan and replans when necessary.
    /// </summary>
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

        /// <summary>
        /// Determine whether goals have changed or plans have failed, and
        /// replan if necessary.  Then execute the plan.
        /// </summary>
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

        /// <summary>
        /// Execute current action in the plan and handle the result
        /// of that action.
        /// </summary>
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
                        // Go to next step in plan
                        currentPlan_.RemoveAt(0);
                        if (currentPlan_.Count > 0)
                        {
                            currentPlan_[0].initialize();
                        }

                        // Unless there isn't one
                        else
                        {
                            // If the goal was GoalTeamwork, mark it as
                            //  irrelevant so we don't try it again until the
                            //  team goals refresh
                            if (AI_.CurrentGoal_ is GoalTeamwork)
                            {
                                AI_.CurrentGoal_.Relevance_ = 0f;
                            }
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
                    else if (status == ActionStatus.UNKNOWN)
                    {
                        throw new NotImplementedException();
                    }
                }
            }
        }

        /// <summary>
        /// Get the current perceived world state to be used by IndividualPlanner.
        /// </summary>
        /// <returns>An initial search node for the planner.</returns>
        internal SearchNode getInitialState()
        {
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
            initial.setBool(Variable.FarFromTarget, false);
            initial.setTask(TeamTask.CLEAR);
            return initial;
        }

        /// <summary>
        /// Reserve the resources necessary for a plan.
        /// </summary>
        /// <param name="plan">Plan being reserved.</param>
        protected void reservePlan(List<Action> plan)
        {
            for (int i = 0; i < plan.Count; i++)
            {
                plan[i].reserve();
            }
        }

        /// <summary>
        /// Unreserve resources of a plan being discarded.
        /// </summary>
        /// <param name="plan">The plan being discarded.</param>
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

        /// <summary>
        /// Add an action type which the agent can use in planning.
        /// </summary>
        /// <param name="action">Type of action which the agent can perform.</param>
        internal void addAction(ActionType action)
        {
            actions_.Add(action);
        }

        /// <summary>
        /// Cleanup resources used by the planner.
        /// </summary>
        internal void die()
        {
            cleanupPlan(currentPlan_);
        }

        /// <summary>
        /// Draw current goal and plan above an agent's head.
        /// For debugging and demoing purposes.
        /// </summary>
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
            FontMap.getInstance().getFont(FontEnum.Kootenay14).drawStringCentered(planString.ToString(), drawPosition, Microsoft.Xna.Framework.Graphics.Color.White, 0.0f, 0.9f);
#endif
        }
    }
}
