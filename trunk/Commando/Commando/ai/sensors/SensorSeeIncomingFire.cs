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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Commando.levels;

namespace Commando.ai.sensors
{
    class SensorSeeIncomingFire : SensorVisual
    {
        public SensorSeeIncomingFire(AI ai, float fov)
            : base(ai, fov)
        {

        }

        /// <summary>
        /// Examine all projectiles in the world state, determine if we can see any of them.
        /// </summary>
        public override void collect()
        {
            CharacterAbstract me = AI_.Character_;

            // If under fire, create that belief.
            // Else, delete under fire beliefs.
            // AMP: Is this really necessary at the moment?
        }

    }
}
