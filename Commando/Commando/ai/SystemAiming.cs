﻿/*
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
        internal CharacterAbstract enemy_;
        internal Vector2[] velocities_ = new Vector2[REACTION_TIME];

        internal int countUp = 0;
        internal int countDown = LOSS_TIME;
        internal bool lossFlag = false;

        const int REACTION_TIME = 10;
        const int HOLD_AIM_TIME = 10;
        const int LOSS_TIME = 5;

        internal SystemAiming(AI ai, CharacterAbstract enemy) : base(ai)
        {
            enemy_ = enemy;
        }

        internal override void update()
        {

            // TODO
            // change this to can see object, remove hard-codedness
            // also change it to not actually access the character directly;
            //      that character should drop EnemyVelocity stimuli or something

            // Don't fire at dead enemies
            if (enemy_.isDead())
            {
                lossFlag = true;
                return;
            }

            // If can still see actual character, we might fire
            if (Raycaster.canSeePoint(AI_.Character_.getPosition(),
                                    enemy_.getPosition(),
                                    AI_.Character_.getHeight(),
                                    enemy_.getHeight()))
            {
                lossFlag = false;
                velocities_[countUp % REACTION_TIME] = enemy_.getVelocity();
                countUp++;
                Vector2 target = predictTargetPosition(enemy_.getPosition());
                //(AI_.Character_.getActuator() as DefaultActuator).lookAt(target);
                AI_.Character_.getActuator().perform("lookAt", new ActionParameters(target));
                if (countUp >= REACTION_TIME)
                {
                    //AI_.Character_.Weapon_.shoot();
                    //(AI_.Character_.getActuator() as DefaultActuator).shoot(AI_.Character_.Weapon_)
                    AI_.Character_.getActuator().perform("shoot", new ActionParameters());
                }
            }
            else
            {
                countDown--;
                if (countDown == 0)
                {
                    countDown = LOSS_TIME;
                    countUp = 0;
                    lossFlag = true;
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
            // Add this to credits
            // The following code was taken from Programming Game AI by Example,
            //  by Mat Buckland

            float distance = (float)(AI_.Character_.getPosition() - currentPosition).Length();

            // TODO
            // Replace this with a lookup
            float weaponSpeed = 15.0f;
            float lookAheadTime = distance / (averageVelocity.Length() + weaponSpeed);
            
            return currentPosition + averageVelocity * lookAheadTime;
        }
    }
}
