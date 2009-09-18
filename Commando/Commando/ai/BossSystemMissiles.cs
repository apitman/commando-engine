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
using Commando.objects.weapons;
using Microsoft.Xna.Framework;
using Commando.levels;
using Commando.graphics;

namespace Commando.ai
{
    class BossSystemMissiles : System
    {
        private const double MISSILE_ANGLE = Math.PI / 2;
        private Missile missile_ = null;

        internal BossSystemMissiles(AI ai)
            : base(ai)
        {

        }

        internal override void update()
        {
            if (missile_ == null || missile_.isDead())
            {
                Belief b = AI_.Memory_.getFirstBelief(BeliefType.BestTarget);

                if (b == null)
                {
                    return;
                }

                Vector2 target = b.position_;

                if (Raycaster.inFieldOfView(AI_.Character_.getDirection(), AI_.Character_.getPosition(), target, (float)MISSILE_ANGLE))
                {

                    if (Raycaster.canSeePoint(AI_.Character_.getPosition(), b.position_, AI_.Character_.getHeight(), new Height(true, false)))
                    {
                        Vector2 dir = AI_.Character_.getDirection();
                        dir.Normalize();
                        missile_ =
                            new Missile(AI_.Character_.pipeline_, AI_.Character_.getCollisionDetector(),
                                (b.handle_ as CharacterAbstract));
                        AI_.Character_.getActuator().perform("throw", new ActionParameters(missile_));
                    }

                }
                else
                {
                    AI_.Character_.getActuator().perform("lookAt", new ActionParameters(target));
                }
            }
        }
    }
}
