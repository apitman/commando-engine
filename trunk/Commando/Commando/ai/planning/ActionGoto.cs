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
using Commando.objects;
using Commando.graphics;
using Microsoft.Xna.Framework;

namespace Commando.ai.planning
{
    class ActionTypeGoto : ActionType
    {
        protected const float BASE_COST = 1.0f;
        protected const float COST_PER_TILE = 0.15f;

        public ActionTypeGoto(NonPlayableCharacterAbstract character)
            : base(character)
        {
            
        }

        internal override bool testPreConditions(SearchNode node)
        {
            // assume we can always get from point A to point B
            // TODO
            // Should GOTO only be allowed if Cover = false at current node?
            // If TakeCover sets Cover = false, this would make sense, as we
            //  could never do a GOTO from somewhere and still have cover afterwards
            //  unless we TakeCover
            return true;
        }

        internal override SearchNode unifyRegressive(ref SearchNode node)
        {
            SearchNode parent = node.getPredecessor();
            TileIndex target = node.values[Variable.Location].t;
            parent.action = new ActionGoto(character_, target);

            TileIndex currentTile =
                GlobalHelper.getInstance().getCurrentLevelTileGrid().getTileIndex(character_.getPosition());
            float distance_in_tiles =
                CommonFunctions.distance(target, currentTile);
            parent.cost += BASE_COST + distance_in_tiles * COST_PER_TILE;

            // Don't know where we came from to get here, so it could
            //  be anywhere - unresolve the value
            parent.unresolve(Variable.Location);

            // TODO
            // Figure out if this setBool is supposed to be false or true
            // Was originally true, but that doesn't seem like it does anything...
            if (node.values[Variable.Cover].b == true && node.resolved[Variable.Cover] == true)
                parent.setBool(Variable.Cover, false);

            return parent;
        }

        internal override void register(Dictionary<int, List<ActionType>> actionMap)
        {
            actionMap[Variable.Location].Add(this);
        }

    }

    internal class ActionGoto : Action
    {
        internal TileIndex target_;
        internal List<TileIndex> path_;

        public ActionGoto(NonPlayableCharacterAbstract character, TileIndex target)
            : base(character)
        {
            target_ = target;
        }

        internal override ActionStatus update()
        {
            TileGrid grid = GlobalHelper.getInstance().getCurrentLevelTileGrid();
            TileIndex curIndex = grid.getTileIndex(character_.getPosition());

            if (path_ != null && path_.Count > 0)
            {
                TileIndex curTarget = path_[0];

                // If we've reached our current target
                if (TileIndex.equals(curIndex, curTarget))
                {
                    // Go on to the next if there is another
                    if (path_.Count > 1)
                    {
                        path_.RemoveAt(0);
                        curTarget = path_[0];
                    }
                    else // otherwise we're done
                    {
                        return ActionStatus.SUCCESS;
                    }
                }

                //(character_.getActuator() as DefaultActuator).moveTo(grid.getTileCenter(path_[0]));
                //(character_.getActuator() as DefaultActuator).lookAt(grid.getTileCenter(path_[0]));
                character_.getActuator().perform("moveTo", new ActionParameters(grid.getTileCenter(path_[0])));
                character_.getActuator().perform("lookAt", new ActionParameters(grid.getTileCenter(path_[0])));
            }
            else
            {
                path_ = AStarPathfinder.calculateExactPath(grid, curIndex, target_, character_.getRadius(), character_.getHeight());
                if (path_ == null) // no path could be found
                {
                    //(character_.getActuator() as DefaultActuator).moveTo(grid.getTileCenter(target_));
                    //(character_.getActuator() as DefaultActuator).lookAt(grid.getTileCenter(target_));
                    character_.getActuator().perform("moveTo", new ActionParameters(grid.getTileCenter(target_)));
                    character_.getActuator().perform("lookAt", new ActionParameters(grid.getTileCenter(target_)));
                    return ActionStatus.FAILED;
                }
            }

            return ActionStatus.IN_PROGRESS;
        }

        internal override bool initialize()
        {
            return true;
        }
    }
}
