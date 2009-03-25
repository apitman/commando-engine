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
using Commando.ai.planning;

namespace Commando.ai
{
    /// <summary>
    /// A particular piece of information that an NPC believes about the
    /// current state of the world.
    /// </summary>
    public class Belief
    {
        public BeliefType type_;
        public Object handle_;
        public float confidence_;
        public Vector2 position_;
        public float value_;
        internal VariableValue data_;

        public Belief(BeliefType type, Object handle, float conf, Vector2 position, float value)
        {
            type_ = type;
            handle_ = handle;
            confidence_ = conf;
            position_ = position;
            value_ = value;
        }

        public void replace(Belief b)
        {
            confidence_ = b.confidence_;
            position_ = b.position_;
            value_ = b.value_;
        }

        public override string ToString()
        {
            string retVal = type_.ToString();
            retVal += " at ";
            retVal += position_.ToString();
            return retVal;
        }
    }

    public enum BeliefType
    {
        EnemyLoc,
        EnemyHealth,
        AllyLoc,
        AllyHealth,
        SuspiciousNoise,
        CoverLoc,

        BestTarget,
        BestCover,
        InvestigateTarget
    }
}
