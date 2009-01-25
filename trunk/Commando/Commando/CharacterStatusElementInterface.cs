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

namespace Commando
{
    /// <summary>
    /// Interface for CharacterStatusElements which requires them to notify a set of 
    /// CharacterStatusObservers when the Element is updated.
    /// </summary>
    interface CharacterStatusElementInterface
    {
        /// <summary>
        /// Get the current value of this element.
        /// </summary>
        /// <returns>The value of this element as an integer</returns>
        int getValue();

        /// <summary>
        /// Update this element to a new value and notify all the observers.
        /// </summary>
        /// <param name="newVal">New value of the element</param>
        void update(int newVal);

        /// <summary>
        /// Add a new observer to this element.
        /// </summary>
        /// <param name="obs">Observer to be added</param>
        void addObserver(objects.CharacterStatusObserverInterface obs);
    }
}
