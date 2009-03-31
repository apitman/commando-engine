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
using System.Runtime.InteropServices;
using Commando.ai.planning;
using Commando.levels;
using Microsoft.Xna.Framework;

namespace Commando.ai
{
    /// <summary>
    /// A particular piece of information that an NPC believes about the
    /// current state of the world.
    /// </summary>
    public class Belief
    {
        // Total size 32 Bytes
        internal BeliefType type_;
        internal Object handle_;
        internal float confidence_;
        internal float relevance_;
        internal Vector2 position_;
        internal BeliefValue data_;

        private Belief() { }

        public Belief(BeliefType type, Object handle, float confidence)
        {
            type_ = type;
            handle_ = handle;
            confidence_ = confidence;
        }

        public Belief(BeliefType type, Object handle, float confidence, float relevance)
        {
            type_ = type;
            handle_ = handle;
            confidence_ = confidence;
            relevance_ = relevance;
        }

        public Belief convert(BeliefType type)
        {
            Belief temp = new Belief();
            temp.type_ = type;
            temp.handle_ = this.handle_;
            temp.confidence_ = this.confidence_;
            temp.relevance_ = this.relevance_;
            temp.position_ = this.position_;
            temp.data_ = this.data_;
            return temp;
        }

        /// <summary>
        /// Replaces this belief with another.
        /// This function should ONLY be used by Memory.
        /// It essentially updates a specific belief with new values.
        /// As such, if type or handle do not match (the values used
        /// to bucket beliefs in memory), this shouldn't be done as
        /// it would change the bucket the belief should be in.
        /// </summary>
        /// <param name="b">The belief with which to replace this belief.</param>
        public void replace(Belief b)
        {
            if (type_ != b.type_ || handle_ != b.handle_)
            {
                throw new InvalidOperationException("Cannot replace a belief with a different belief.");
            }
            confidence_ = b.confidence_;
            relevance_ = b.relevance_;
            position_ = b.position_;
            data_ = b.data_;
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
        Error, // used as a flag to throw exceptions

        EnemyLoc, // 
        EnemyHealth, // data.int1 = health
        AllyLoc, // 
        AllyHealth, // data.int1 = health
        SuspiciousNoise, // 
        CoverLoc, // data.tile1 = tile index of position

        BestTarget, // 
        AvailableCover, // data.tile1 = tile index of position
        InvestigateTarget // 
    }

    [StructLayout(LayoutKind.Explicit)]
    internal struct BeliefValue
    {
        [FieldOffset(0)]
        internal int int1;
        [FieldOffset(4)]
        internal int int2;

        [FieldOffset(0)]
        internal TileIndex tile1;
        [FieldOffset(4)]
        internal TileIndex tile2;

        [FieldOffset(0)]
        internal float float1;
        [FieldOffset(4)]
        internal float float2;

        [FieldOffset(0)]
        internal Vector2 vector;
    }
}
