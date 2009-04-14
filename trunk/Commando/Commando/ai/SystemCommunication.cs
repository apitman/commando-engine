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
    public class SystemCommunication
    {
        /// <summary>
        /// The number of frames per second
        /// </summary>
        private const int FRAMERATE = 30;

        private const int SECONDS_BEFORE_REPEATING_SELF = 5;

        public AI AI_;

        public bool isBroadcasting_;

        public string broadcastMessage_;

        public CommunicationLevel communicationLevel_;

        public bool IsListening_
        {
            get
            {
                return isListening_;
            }
            set
            {
                isListening_ = value;
                if (value)
                {
                    framesSinceLastCommunication_ = 0;
                }
            }
        }

        public bool communicationRequested_;

        protected bool isListening_;

        protected int framesLeftToBroadcast_;

        protected int broadcastRadius_;

        protected int key_;

        protected int framesSinceLastCommunication_;

        protected Dictionary<BeliefType, int> framesSinceCommunicatingThisType_;

        /// <summary>
        /// Basic constructor
        /// </summary>
        public SystemCommunication(AI ai, int broadcastRadius)
        {
            AI_ = ai;
            broadcastRadius_ = broadcastRadius;
            isBroadcasting_ = false;
            broadcastMessage_ = "Default Message";
            framesLeftToBroadcast_ = 0;
            key_ = StimulusIDGenerator.getNext();
            communicationLevel_ = CommunicationLevel.Medium;
            isListening_ = false;
            communicationRequested_ = false;
            framesSinceLastCommunication_ = 100;
            framesSinceCommunicatingThisType_ = new Dictionary<BeliefType, int>();
            framesSinceCommunicatingThisType_[BeliefType.AllyHealth] = 0;
            framesSinceCommunicatingThisType_[BeliefType.AllyLoc] = 0;
            framesSinceCommunicatingThisType_[BeliefType.AmmoLoc] = 0;
            framesSinceCommunicatingThisType_[BeliefType.AvailableCover] = 0;
            framesSinceCommunicatingThisType_[BeliefType.BestTarget] = 0;
            framesSinceCommunicatingThisType_[BeliefType.CoverLoc] = 0;
            framesSinceCommunicatingThisType_[BeliefType.EnemyHealth] = 0;
            framesSinceCommunicatingThisType_[BeliefType.EnemyLoc] = 0;
            framesSinceCommunicatingThisType_[BeliefType.Error] = 0;
            framesSinceCommunicatingThisType_[BeliefType.InvestigateTarget] = 0;
            framesSinceCommunicatingThisType_[BeliefType.SuspiciousNoise] = 0;
            framesSinceCommunicatingThisType_[BeliefType.TeamInfo] = 0;
        }

        /// <summary>
        /// Decides what, if anything, to broadcast, and broadcasts
        /// it
        /// </summary>
        public void update()
        {
            if (isBroadcasting_)
            {
                if (framesLeftToBroadcast_ <= 0)
                {
                    isBroadcasting_ = false;
                    WorldState.Audial_.Remove(key_);
                }
                framesLeftToBroadcast_--;
            }
            else if (!isListening_)
            {
                decideWhatToBroadcast();
                framesSinceLastCommunication_++;
            }
        }

        public void draw()
        {
            if (isBroadcasting_)
            {
                Vector2 prettyOffset = new Vector2(0.0f, -30.0f);
                Vector2 drawPosition = new Vector2(AI_.Character_.getPosition().X, AI_.Character_.getPosition().Y);
                drawPosition.X -= GlobalHelper.getInstance().getCurrentCamera().getX();
                drawPosition.Y -= GlobalHelper.getInstance().getCurrentCamera().getY();
                drawPosition += prettyOffset;
                FontMap.getInstance().getFont(FontEnum.Miramonte).drawStringCentered(broadcastMessage_, drawPosition, Microsoft.Xna.Framework.Graphics.Color.DeepPink, 0.0f, 0.9f);
            }

            // DEBUG STUFF
            //string debugOut = "I believe:\n";
            //List<Belief> bList = AI_.Memory_.getAllBeliefs();
            //foreach (Belief b in bList)
            //{
            //    debugOut += b.ToString() + "\n";
            //}
            //debugOut += "World State's Audial Stimulus messages:\n";
            //foreach (KeyValuePair<int, Stimulus> kvp in WorldState.Audial_)
            //{
            //    if (kvp.Value.type_ == StimulusType.Message && kvp.Value.message_ != null)
            //    {
            //        debugOut += kvp.Value.message_.ToString() + "\n";                    
            //    }
            //}
            //FontMap.getInstance().getFont(FontEnum.Kootenay14).drawString(debugOut, Vector2.One, Microsoft.Xna.Framework.Graphics.Color.Black);
            // END DEBUG STUFF
        }

        /// <summary>
        /// Broadcasts the belief to all agents within the broadcast radius
        /// </summary>
        /// <param name="message">The message to broadcast</param>
        protected void broadcastMessage(Message message)
        {
            isBroadcasting_ = true;
            //communicationRequested_ = false;
            framesSinceLastCommunication_ = 0;

            framesLeftToBroadcast_ = message.TimeToBroadcast();
            broadcastMessage_ = message.ToString();

            WorldState.Audial_.Remove(key_);
            WorldState.Audial_.Add(key_,
                new Stimulus(StimulusSource.CharacterAbstract, AI_.Character_.Allegiance_, StimulusType.Message, broadcastRadius_, AI_.Character_.getPosition(), AI_.Character_, message));
            if (communicationLevel_ == CommunicationLevel.High && message.MessageType_ == Message.MessageType.Data)
            {
                CommLogger.sentMsg();
            }
        }

        /// <summary>
        /// Decides which belief to broadcast
        /// </summary>
        protected void decideWhatToBroadcast()
        {
            List<BeliefType> bTList = new List<BeliefType>();
            switch (communicationLevel_)
            {
                case CommunicationLevel.None:
                    // Don't communicate at all
                    break;
                case CommunicationLevel.Low:
                    // Only broadcast EnemyLoc beliefs.
                    // Periodically request communication.
                    bTList.Add(BeliefType.EnemyLoc);
                    broadcastHelper(bTList, 8);
                    break;
                case CommunicationLevel.Medium:
                default:
                    // Broadcast EnemyLoc, SuspiciousNoise, and EnemyHealth.
                    // Request commmunication more frequently.
                    bTList.Add(BeliefType.EnemyLoc);
                    bTList.Add(BeliefType.SuspiciousNoise);
                    bTList.Add(BeliefType.EnemyHealth);
                    broadcastHelper(bTList, 4);
                    break;
                case CommunicationLevel.High:
                    // Broadcast nearly any BeliefType.
                    // Request communication very frequently.
                    bTList.Add(BeliefType.EnemyLoc);
                    bTList.Add(BeliefType.AmmoLoc);
                    bTList.Add(BeliefType.SuspiciousNoise);
                    bTList.Add(BeliefType.EnemyHealth);
                    //bTList.Add(BeliefType.CoverLoc);
                    broadcastHelper(bTList, 2);
                    break;
            }
        }

        /// <summary>
        /// Try to broadcast something based on a preference list of BeliefTypes
        /// </summary>
        /// <param name="bTList">The preference list of BeliefTypes</param>
        /// <param name="hiTime">How long to wait before issuing a Hi message</param>
        protected void broadcastHelper(List<BeliefType> bTList, int hiTime)
        {
            for (int i = 0; i < bTList.Count; i++)
            {
                framesSinceCommunicatingThisType_[bTList[i]]++;
            }

            bool communicatedSomething = false;

            if (communicationRequested_)
            {
                // Attempt to broadcast new info
                for (int i = 0; i < bTList.Count; i++)
                {
                    if (AI_.Memory_.getBeliefs(bTList[i]).Count > 0 && framesSinceCommunicatingThisType_[bTList[i]] > SECONDS_BEFORE_REPEATING_SELF * FRAMERATE)
                    {
                        Message msg = new Message(Message.MessageType.Data);
                        msg.Belief_ = AI_.Memory_.getFirstBelief(bTList[i]);
                        broadcastMessage(msg);
                        framesSinceCommunicatingThisType_[bTList[i]] = 0;
                        communicatedSomething = true;
                        break;
                    }
                }

                // If we can't broadcast new info, then re-broadcast old info.
                if (!communicatedSomething)
                {
                    for (int i = 0; i < bTList.Count; i++)
                    {
                        if (AI_.Memory_.getBeliefs(bTList[i]).Count > 0)
                        {
                            Message msg = new Message(Message.MessageType.Data);
                            msg.Belief_ = AI_.Memory_.getFirstBelief(bTList[i]);
                            broadcastMessage(msg);
                            framesSinceCommunicatingThisType_[bTList[i]] = 0;
                            communicatedSomething = true;
                            break;
                        }
                    }
                }
            }

            // If there is still nothing to be communicated, then we announce
            // that we are ready to communicate.
            if (!communicatedSomething)
            {
                if (framesSinceLastCommunication_ > hiTime * FRAMERATE)
                {
                    Message msg = new Message(Message.MessageType.Hi);
                    broadcastMessage(msg);
                }
            }
        }

        public void die()
        {
            WorldState.Audial_.Remove(key_);
        }

        public enum CommunicationLevel
        {
            None,
            Low,
            Medium,
            High
        }
    }
}
