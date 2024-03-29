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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Commando.objects;

namespace Commando.ai
{
    /// <summary>
    /// Caches pointers to important entities which will be used by Sensors to
    /// gather data.
    /// </summary>
    internal class WorldState
    {
        static internal Dictionary<int, Stimulus> Audial_ {get; private set;}
        static internal List<CharacterAbstract> CharacterList_ { get; set; }
        static internal List<CoverObject> CoverList_ { get; set; }
        static internal List<AmmoBox> AmmoList_ { get; set; }

        static WorldState()
        {
            Audial_ = new Dictionary<int, Stimulus>();
            CharacterList_ = new List<CharacterAbstract>();
            CoverList_ = new List<CoverObject>();
            AmmoList_ = new List<AmmoBox>();
        }

        private WorldState() {}

        static internal void refresh()
        {
            /*
            for (int i = 0; i < CharacterList_.Count; i++)
            {
                if (CharacterList_[i].isDead())
                {
                    CharacterList_.RemoveAt(i);
                    i--;
                }
            }
            */
            for (int i = 0; i < AmmoList_.Count; i++)
            {
                if (AmmoList_[i].isDead())
                {
                    AmmoList_.RemoveAt(i);
                    i--;
                }
            }
        }

        static internal void reset()
        {
            Audial_.Clear();
            CharacterList_.Clear();
            CoverList_.Clear();
            AmmoList_.Clear();
        }
    }

}
