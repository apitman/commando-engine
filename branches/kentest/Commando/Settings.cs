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
    /// Singleton for control and game settings.
    /// </summary>
    public class Settings
    {
        protected static Settings instance_;

        protected MovementType movementType_;

        internal bool UsingMouse_ { get; set; }

        internal bool DebugMode_ { get; set; }

        static Settings()
        {
            // TODO Read from settings file here?

            instance_ = new Settings();
            instance_.movementType_ = MovementType.ABSOLUTE;
            instance_.DebugMode_ = false;
        }

        private Settings()
        {
            // Singleton
        }

        public static Settings getInstance()
        {
            return instance_;
        }

        /// <summary>
        /// Change movement type between relative and absolute.
        /// </summary>
        public void swapMovementType()
        {
            if (movementType_ == MovementType.ABSOLUTE)
            {
                movementType_ = MovementType.RELATIVE;
            }
            else
            {
                movementType_ = MovementType.ABSOLUTE;
            }
        }

        /// <summary>
        /// Get whether movement should be relative to character
        /// direction or absolute.
        /// </summary>
        /// <returns>Type of movement</returns>
        public MovementType getMovementType()
        {
            return movementType_;
        }
    }

    public enum MovementType
    {
        RELATIVE,
        ABSOLUTE
    }
}
