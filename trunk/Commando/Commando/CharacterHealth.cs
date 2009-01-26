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

namespace Commando
{
    /// <summary>
    /// Keeps track of the characters health
    /// </summary>
    public class CharacterHealth : CharacterStatusElementInterface
    {

        protected List<CharacterStatusObserverInterface> observers_;

        protected int health_value_;

        public CharacterHealth()
        {
            observers_ = new List<CharacterStatusObserverInterface>();
            health_value_ = 100;
        }
        /// <summary>
        /// Add an observer to this CharacterHealth.
        /// </summary>
        /// <param name="obs">Observer to be added</param>
        public void addObserver(CharacterStatusObserverInterface obs)
        {
            observers_.Add(obs);
        }

        /// <summary>
        /// Update the value of this CharacterHealth.  Notifies all observers.
        /// </summary>
        /// <param name="value">New value of this CharacterHealth</param>
        public void update(int value)
        {
            health_value_ = value;
            foreach (CharacterStatusObserverInterface observer in observers_)
            {
                observer.notifyOfChange(value);
            }
        }

        public int getValue()
        {
            return health_value_;
        }
    }
}
