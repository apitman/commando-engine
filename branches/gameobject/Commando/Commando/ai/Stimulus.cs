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
    /// <summary>
    /// A piece of information about the world to be picked up by Sensors.
    /// </summary>
    struct Stimulus
    {
        public StimulusSource source_;
        public StimulusType type_;
        public float radius_;
        public Vector2 position_;

        public Stimulus(StimulusSource source, StimulusType type, float radius, Vector2 pos)
        {
            source_ = source;
            type_ = type;
            radius_ = radius;
            position_ = pos;
        }
    }

    enum StimulusSource
    {
        CharacterAbstract
    }

    enum StimulusType
    {
        Position
    }

    // key so that objects can update the stimuli they produce
    //  without searching for them - remember, this is a struct,
    //  so they can't simply hold a reference to it (it copies)

    // using this same key for beliefs generated from a stimulus
    // should also more easily allow creatures to modify existing
    // beliefs instead of creating new ones and trying to remove
    // the old ones
    public class StimulusIDGenerator
    {
        static int id = 0;

        public static int getNext()
        {
            return id++;
        }

        private StimulusIDGenerator()
        {
            // do nothing
        }
    }
}
