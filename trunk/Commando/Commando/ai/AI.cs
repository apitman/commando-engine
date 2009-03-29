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

using System.Collections.Generic;
using System.Linq;
using System.Text;
using Commando.objects;
using Commando.levels;
using Microsoft.Xna.Framework;
using Commando.ai.planning;

namespace Commando.ai
{
    /// <summary>
    /// An encapsulation of an NPC's intelligence.
    /// </summary>
    public abstract class AI
    {
        public NonPlayableCharacterAbstract Character_ { get; private set; }

        public Memory Memory_ { get; private set; }

        internal List<Action> Actions_ { get; set; }
        internal List<Action> CurrentPlan_ { get; set; }

        internal List<Goal> Goals_ { get; set; }
        internal Goal CurrentGoal_ { get; set; }
        

        protected List<Sensor> sensors_ = new List<Sensor>();
        protected List<System> systems_ = new List<System>();

        //protected List<TileIndex> path_;
        //protected int lastPathfindUpdate_;
        //protected const int PATHFIND_THRESHOLD = 15;

        public SystemCommunication CommunicationSystem_ { get; private set; }

        public AI(NonPlayableCharacterAbstract npc)
        {
            Character_ = npc;

            Memory_ = new Memory();

            Goals_ = new List<Goal>();
            CurrentGoal_ = null;

            Actions_ = new List<Action>();
            CurrentPlan_ = new List<Action>();

            //systems_.Add(new SystemAiming(this));
            systems_.Add(new SystemTargetSelection(this));
            systems_.Add(new SystemCoverSelection(this));
            systems_.Add(new SystemGoalSelection(this));
            systems_.Add(new SystemPlanning(this));

            //path_ = new List<TileIndex>();
            //lastPathfindUpdate_ = 0;

            CommunicationSystem_ = new SystemCommunication(this, 100);
        }

        public void update()
        {
            CommunicationSystem_.isListening_ = false;
            for (int i = 0; i < sensors_.Count; i++)
            {
                sensors_[i].collect();
            }

            for (int i = 0; i < systems_.Count; i++)
            {
                systems_[i].update();
            }

            CommunicationSystem_.update();

            if (CurrentPlan_ != null && CurrentPlan_.Count > 0)
            {
                bool isValid = CurrentPlan_[0].checkIsStillValid();
                if (!isValid)
                {
                    for (int i = 0; i < CurrentPlan_.Count; i++)
                    {
                        CurrentPlan_[i].unreserve();
                    }
                    CurrentPlan_.Clear();
                }
                else
                {
                    bool done = CurrentPlan_[0].update();
                    if (done)
                    {
                        CurrentPlan_.RemoveAt(0);
                        if (CurrentPlan_.Count > 0)
                        {
                            CurrentPlan_[0].initialize();
                        }
                    }
                }
            }
        }

        public void draw()
        {
            CommunicationSystem_.draw();
        }

        public void die()
        {
            CommunicationSystem_.die();
        }
    }
}
