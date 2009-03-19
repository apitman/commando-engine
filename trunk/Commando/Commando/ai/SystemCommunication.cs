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

namespace Commando.ai
{
    public class SystemCommunication : System
    {
        public bool isBroadcasting_;

        public string broadcastMessage_;

        protected int framesLeftToBroadcast_;

        protected int broadcastRadius_;

        protected int key_;

        public bool talkative_;

        /// <summary>
        /// Basic constructor
        /// </summary>
        public SystemCommunication(AI ai, int broadcastRadius) : base(ai)
        {
            broadcastRadius_ = broadcastRadius;
            isBroadcasting_ = false;
            broadcastMessage_ = "Default Message";
            framesLeftToBroadcast_ = 0;
            key_ = StimulusIDGenerator.getNext();
            talkative_ = true;
        }

        /// <summary>
        /// Decides what, if anything, to broadcast, and broadcasts
        /// it
        /// </summary>
        public override void update()
        {
            if (isBroadcasting_)
            {
                if (framesLeftToBroadcast_ <= 0)
                {
                    isBroadcasting_ = false;
                }
                framesLeftToBroadcast_--;
            }
            else
            {
                decideWhatToBroadcast();
            }
        }

        public override void draw()
        {
            if (isBroadcasting_)
            {
                Vector2 prettyOffset = new Vector2(0.0f, -15.0f);
                Vector2 drawPosition = new Vector2(AI_.Character_.getPosition().X, AI_.Character_.getPosition().Y);
                drawPosition.X -= GlobalHelper.getInstance().getCurrentCamera().getX();
                drawPosition.Y -= GlobalHelper.getInstance().getCurrentCamera().getY();
                drawPosition += prettyOffset;
                FontMap.getInstance().getFont(FontEnum.MiramonteBold).drawStringCentered(broadcastMessage_, drawPosition, Microsoft.Xna.Framework.Graphics.Color.White, 0.0f, 0.9f);
            }
        }

        /// <summary>
        /// Broadcasts the belief to all agents within the broadcast radius
        /// </summary>
        /// <param name="belief"></param>
        protected void broadcastBelief(Belief belief)
        {
            isBroadcasting_ = true;
            framesLeftToBroadcast_ = 60;
            broadcastMessage_ = belief.ToString();
            // Resume here
            WorldState.Audial_.Remove(key_);
            WorldState.Audial_.Add(key_,
                new Stimulus(StimulusSource.CharacterAbstract, StimulusType.Message, broadcastRadius_, AI_.Character_.getPosition(), this, belief));
        }

        /// <summary>
        /// Decides which beliefs to broadcast
        /// </summary>
        protected void decideWhatToBroadcast()
        {
            List<Belief> bList = AI_.Memory_.getBeliefs(BeliefType.EnemyLoc);
            for (int i = 0; i < bList.Count; i++)
            {
                broadcastBelief(bList[i]);
                return; // Only broadcast one thing at a time
            }
            if (talkative_)
            {
                List<Belief> bList2 = AI_.Memory_.getBeliefs(BeliefType.SuspiciousNoise);
                if (bList2.Count > 0)
                {
                    broadcastBelief(bList2[0]);
                }
            }
        }
    }
}
