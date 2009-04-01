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
using Commando.ai.planning;
using Commando.levels;

namespace Commando.ai.sensors
{
    class SensorAmmo : SensorVisual
    {
        public SensorAmmo(AI ai, float fov)
            : base(ai, fov)
        {

        }

        public override void collect()
        {
            CharacterAbstract me = AI_.Character_;
            for (int i = 0; i < WorldState.AmmoList_.Count; i++)
            {
                AmmoBox box = WorldState.AmmoList_[i];
                if (ReservationTable.isReserved(box))
                {
                    continue;
                }
                if (Raycaster.inFieldOfView(me.getDirection(), me.getPosition(), box.getPosition(), fieldOfView) &&
                    Raycaster.canSeePoint(me.getPosition(), box.getPosition(), me.getHeight(), box.getHeight()))
                {
                    Belief posBelief = new Belief(BeliefType.AmmoLoc, box, 100);
                    posBelief.position_ = box.getPosition();
                    posBelief.relevance_ = (float)(1 / (box.getPosition() - AI_.Character_.getPosition()).LengthSquared());
                    posBelief.data_.tile1 = GlobalHelper.getInstance().getCurrentLevelTileGrid().getTileIndex(box.getPosition());
                    AI_.Memory_.setBelief(posBelief);
                }
            }
        }
    }
}
