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
    public class AI
    {
        public NonPlayableCharacterAbstract Character_ { get; private set; }

        public Memory Memory_ { get; private set; }

        internal List<Action> Actions_ { get; set; }
        internal SearchNode CurrentGoal_ { get; set; }
        internal List<Action> CurrentPlan_ { get; set; }

        protected List<Sensor> sensors_ = new List<Sensor>();
        protected List<System> systems_ = new List<System>();

        protected List<TileIndex> path_; // temporary

        protected int lastPathfindUpdate_;

        protected const int PATHFIND_THRESHOLD = 15;

        protected InferenceEngine inferenceEngine_;

        public SystemCommunication CommunicationSystem_ { get; private set; }

        public AI(NonPlayableCharacterAbstract npc)
        {
            Character_ = npc;
            Memory_ = new Memory();
            sensors_.Add(new SensorEars(this));
            sensors_.Add(new SensorSeeCharacter(this));
            systems_.Add(new SystemAiming(this));
            path_ = new List<TileIndex>();
            lastPathfindUpdate_ = 0;
            inferenceEngine_ = new InferenceEngine(this);
            CommunicationSystem_ = new SystemCommunication(this, 100);
        }

        public void update()
        {
            CommunicationSystem_.isListening_ = false;
            for (int i = 0; i < sensors_.Count; i++)
            {
                sensors_[i].collect();
            }

            inferenceEngine_.update();

            for (int i = 0; i < systems_.Count; i++)
            {
                systems_[i].update();
            }

            CommunicationSystem_.update();

            // TODO Temporary block
            // Tells the AI to proceed to the location of a known enemy
            //  if it does not currently have a place to go
            lastPathfindUpdate_++;
            TileGrid grid = GlobalHelper.getInstance().getCurrentLevelTileGrid();
            if (path_ == null || path_.Count == 0 || lastPathfindUpdate_ > PATHFIND_THRESHOLD)
            {
                List<Belief> list = Memory_.getBeliefs(BeliefType.EnemyLoc);
                for (int i = 0; i < list.Count; i++)
                {
                    Belief b = list[i];
                    //Character_.moveTo(b.position_);
                    //Character_.lookAt(b.position_);
                    Vector2 start = Character_.getPosition();
                    Vector2 dest = b.position_;
                    if (TileIndex.equals(grid.getTileIndex(start), grid.getTileIndex(dest)))
                    {
                        Memory_.removeBelief(b);
                        continue;
                    }
                    float radius = Character_.getRadius();
                    Height h = new Height(true, true);
                    path_ = 
                        AStarPathfinder.run(grid, start, dest, radius, h);
                    lastPathfindUpdate_ = 0;
                    if (path_ != null)
                        break;               
                }
            }

            // TODO Temporary block
            // Tells the AI to proceed to the location of a suspicious
            // noise if there is no known enemy location
            if (path_ == null || path_.Count == 0 || lastPathfindUpdate_ > PATHFIND_THRESHOLD)
            {
                List<Belief> list = Memory_.getBeliefs(BeliefType.SuspiciousNoise);
                for (int i = 0; i < list.Count; i++ )
                {
                    Belief b = list[i];
                    Character_.lookAt(b.position_);
                    Vector2 start = Character_.getPosition();
                    Vector2 dest = b.position_;
                    float radius = Character_.getRadius();
                    Height h = new Height(true, true);
                    path_ =
                        AStarPathfinder.run(grid, start, dest, radius, h);
                    lastPathfindUpdate_ = 0;
                    if (path_ != null)
                        break;
                }
            }

            if (path_ == null || path_.Count == 0)
            {
                return;
            }

            TileIndex cur = path_[0];
            if (grid.isPointWithinTile(Character_.getPosition(), cur))
            {
                path_.RemoveAt(0);
                if (path_.Count != 0)
                {
                    Character_.moveTo(grid.getTileCenter(path_[0]));
                    //Character_.lookAt(grid.getTileCenter(path_[0]));
                }
            }
            else
            {
                Character_.moveTo(grid.getTileCenter(path_[0]));
                //Character_.lookAt(grid.getTileCenter(path_[0]));
            }

            // End test block
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
