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
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using Commando.controls;

namespace Commando
{
    class EngineStateGameplay : EngineStateInterface
    {
        //Jared's test stuff
        //protected objects.MainPlayer player_;
        //END Jared's test stuff


        protected Engine engine_;

        public EngineStateGameplay(Engine engine)
        {
            engine_ = engine;

            //Jared's test stuff
            //player_ = new objects.MainPlayer();
            //END Jared's test stuff
        }

        #region EngineStateInterface Members

        public EngineStateInterface update(GameTime gameTime)
        {
            InputSet inputs = engine_.getInputs();

            if (inputs.getCancelButton())
            {
                return new EngineStatePause(engine_, this);
            }

            //Jared's test stuff
            //player_.update(gameTime);
            //END Jared's test stuff

            return this;
        }

        public void draw()
        {
            engine_.GraphicsDevice.Clear(Color.Chocolate);

            //Jared's test stuff
            //player_.draw(new GameTime());
            //END Jared's test stuff
        }

        #endregion
    }
}
