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

namespace Commando
{
    class EngineStateOutofFocus : EngineStateInterface
    {
        protected EngineStateInterface outOfFocusEngine_;

        protected Engine engine_;

        public EngineStateOutofFocus(Engine engine, EngineStateInterface outOfFocusEngine)
        {
            engine_ = engine;
            outOfFocusEngine_ = outOfFocusEngine;
        }

        #region EngineStateInterface Members

        public EngineStateInterface update(GameTime gameTime)
        {
            if (engine_.IsActive)
            {
                return outOfFocusEngine_;
            }
            return this;
        }

        public void draw()
        {
            outOfFocusEngine_.draw();
        }

        #endregion
    }
}
