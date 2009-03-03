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

namespace Commando.ai
{
    class SensorEyes : Sensor
    {

        const float FIELD_OF_VIEW = (float)Math.PI;

        public SensorEyes(AI ai) : base(ai) { }

        // TODO
        // Eye sensors probably need a link back to their AI or to their
        //  owning character, to get that character's current direction,
        //  position, etc.
        // That way the eye sensors can be parameterized with only angle
        //  of sight, distance of sight, decay of sight accuracy, etc.
        // Another option would be to change the interface to collect()
        //  to include the information about the character, but not all
        //  sensors would need it, so what's the best option?

        public override void collect()
        {
            Dictionary<int, Stimulus>.Enumerator cur = WorldState.Visual_.GetEnumerator();
            for (int i = 0; i < WorldState.Visual_.Count; i++)
            {
                cur.MoveNext();
                int id = cur.Current.Key;
                Stimulus stim = cur.Current.Value;
                filter(id, stim);
            }
        }

        private void filter(int id, Stimulus stim)
        {
            // TODO
            // This type of task should be moved into an inference system
            //   Might want to subclass handlers via map lookup for enum types
            // Also, the code here should perform Line of Sight checks, distance
            //   checks, etc. as an actual filter
            // Furthermore, Eyes might be just an interface?  Actually probably not,
            //   but it should be easily extendable for more specific eyes and such
            if (stim.source_ == StimulusSource.CharacterAbstract &&
                Raycaster.inFieldOfView(AI_.Character_.getDirection(), AI_.Character_.getPosition(), stim.position_, FIELD_OF_VIEW) &&
                Raycaster.canSeePoint(AI_.Character_.getPosition(), stim.position_, new Height(true, false), new Height(true, true)))
            {
                AI_.Memory_.Beliefs_.Remove(id);
                AI_.Memory_.Beliefs_.Add(id, new Belief(BeliefType.EnemyLoc, 100, stim.position_.X, stim.position_.Y));
            }
        }

    }
}
