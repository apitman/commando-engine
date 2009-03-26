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
using Microsoft.Xna.Framework;
using Commando.levels;
using Commando.graphics;

namespace Commando.ai
{
    internal class SystemAiming : System
    {
        internal Vector2[] velocities_ = new Vector2[REACTION_TIME];

        internal int countUp = 0;
        internal int countDown = LOSS_TIME;

        const int REACTION_TIME = 10;
        const int HOLD_AIM_TIME = 10;
        const int LOSS_TIME = 5;

        internal SystemAiming(AI ai) : base(ai) { }

        internal override void update()
        {
            // TODO
            // change this to most relevant enemy
            Belief belief = AI_.Memory_.getFirstBelief(BeliefType.EnemyLoc);

            // TODO
            // change this to can see object, remove hard-codedness
            // also change it to not actually access the character directly;
            //      that character should drop EnemyVelocity stimuli or something
            if (belief != null &&
                Raycaster.canSeePoint(AI_.Character_.getPosition(),
                                        belief.position_,
                                        new Height(true, true),
                                        new Height(true, true)))
            {
                CharacterAbstract ca = (belief.handle_ as CharacterAbstract);

                // if can still see actual character, we might fire
                if (Raycaster.canSeePoint(AI_.Character_.getPosition(),
                                        ca.getPosition(),
                                        new Height(true, true),
                                        new Height(true, true)))
                {
                    velocities_[countUp % REACTION_TIME] = ca.getVelocity();
                    countUp++;
                    Vector2 target = predictTargetPosition(belief.position_);
                    (AI_.Character_.getActuator() as DefaultActuator).lookAt(target);
                    if (countUp >= REACTION_TIME)
                    {
                        AI_.Character_.Weapon_.shoot();
                    }
                }
                else
                {
                    countDown--;
                    if (countDown == 0)
                    {
                        countDown = LOSS_TIME;
                        countUp = 0;
                    }
                }
            }     
        }

        internal Vector2 calculateAverageVelocity()
        {
            int samples = Math.Min(countUp, REACTION_TIME);
            float x = 0;
            float y = 0;
            for (int i = 0; i < samples; i++)
            {
                x += velocities_[i].X;
                y += velocities_[i].Y;
            }
            x /= samples;
            y /= samples;
            return new Vector2(x, y);
        }

        internal Vector2 predictTargetPosition(Vector2 currentPosition)
        {
            Vector2 averageVelocity = calculateAverageVelocity();
            // TODO
            // actually perform the intersection calculation!
            return currentPosition;
        }
    }
}
