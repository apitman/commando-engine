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

namespace Commando.ai
{
    class DummyAI : AIInterface
    {
        public GameObject Character_ { get; private set; }

        public Memory Memory_ { get; private set; }

        protected List<Sensor> sensors_;

        public DummyAI(GameObject npc)
        {
            Character_ = npc;
            Memory_ = new Memory();
            sensors_ = new List<Sensor>();
            sensors_.Add(new SensorEars(Memory_));
            sensors_.Add(new SensorEyes(Memory_));
        }

        public GameObject getOwner()
        {
            return Character_;
        }

        public void update()
        {
            for (int i = 0; i < sensors_.Count; i++)
            {
                sensors_[i].collect();
            }

            // TODO Temporary block
            // Tells the AI to proceed to the location of a known enemy
            IEnumerator<Belief> cur = Memory_.Beliefs_.Values.GetEnumerator();
            while (cur.MoveNext())
            {
                Belief b = cur.Current;
                if (b.type_ == BeliefType.EnemyLoc)
                {
                    ActuatorInterface act =
                        (ActuatorInterface)Character_.getComponent(ComponentEnum.Actuators);
                    act.moveTo(b.position_);
                }
            }
            // End test block
        }
    }
}
