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
using Microsoft.Xna.Framework;
using Commando.ai.planning;
using Commando.levels;

namespace Commando.ai
{
    public class SystemCoverSelection : System
    {

        public SystemCoverSelection(AI ai)
            : base(ai)
        {
        }



        internal override void update()
        {
            // No need to select cover if there is no enemy nearby
            Belief bestTarget = AI_.Memory_.getFirstBelief(BeliefType.BestTarget);
            if (bestTarget == null)
            {
                return;
            }

            List<Belief> beliefs = AI_.Memory_.getBeliefs(BeliefType.CoverLoc);
            if (beliefs.Count == 0)
            {
                return;
            }

            float lowValue = float.PositiveInfinity;
            int lowBelief = -1;
            float tempVal;
            Vector2 coverPos = Vector2.Zero, coverDir = Vector2.Zero;
            Vector2 myPos = AI_.Character_.getPosition();
            Vector2 targetPos = bestTarget.position_;

            AI_.Memory_.removeBeliefs(BeliefType.AvailableCover);

            for (int i = 0; i < beliefs.Count; i++)
            {
                if (ReservationTable.isReserved(beliefs[i].handle_))
                {
                    continue;
                }
                Tile coverTile = GlobalHelper.getInstance().getCurrentLevelTileGrid().getTile(beliefs[i].data_.tile1);
                if ((coverTile.highDistance_) * TileGrid.TILEWIDTH < AI_.Character_.getRadius() || (coverTile.lowDistance_) * TileGrid.TILEWIDTH < AI_.Character_.getRadius())
                {
                    continue;
                }
                CoverObject cover = (CoverObject)beliefs[i].handle_;

                coverPos = beliefs[i].position_;
                tempVal = (float)CommonFunctions.distance(myPos, coverPos);
                //tempVal += (float)CommonFunctions.distance(myPos, targetPos);
                tempVal = (float)Math.Sqrt(tempVal);

                float angleToPlayer = MathHelper.WrapAngle(CommonFunctions.getAngle(targetPos - coverPos));
                float angleOfCover = MathHelper.WrapAngle(CommonFunctions.getAngle(cover.getCoverDirection()));
                float angleBetweenCoverPlayer = MathHelper.WrapAngle(angleOfCover - angleToPlayer);
                angleBetweenCoverPlayer *= 2f;
                tempVal *= angleBetweenCoverPlayer * angleBetweenCoverPlayer;

                Belief availableCover = beliefs[i].convert(BeliefType.AvailableCover);
                availableCover.relevance_ = 1/tempVal;
                AI_.Memory_.setBelief(availableCover);
            }
        }
    }
}
