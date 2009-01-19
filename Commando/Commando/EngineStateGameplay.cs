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
using Commando.objects;

namespace Commando
{
    class EngineStateGameplay : EngineStateInterface
    {
        //Jared's test stuff
        protected objects.MainPlayer player_;
        //END Jared's test stuff


        protected Engine engine_;
        protected HeadsUpDisplayObject healthBar_;
        protected HeadsUpDisplayObject weapon_;
        protected Vector2 healthBarPos_;
        protected Vector2 weaponPos_;

        public EngineStateGameplay(Engine engine)
        {
            engine_ = engine;

            //Jared's test stuff
            player_ = new objects.MainPlayer();
            //END Jared's test stuff

            healthBarPos_ = new Vector2(100.0f, 550.0f);
            weaponPos_ = new Vector2(200.0f, 550.0f);
            healthBar_ = new HeadsUpDisplayObject(TextureMap.getInstance().getTexture("healthBarFiller"), healthBarPos_, new Vector2(0.0f), 0.8f);
            weapon_ = new HeadsUpDisplayObject(TextureMap.getInstance().getTexture("pistol"), weaponPos_, new Vector2(0.0f), 0.8f);
            player_.getHealth().addObserver(healthBar_);
            player_.getWeapon().addObserver(weapon_);
        }

        #region EngineStateInterface Members

        public EngineStateInterface update(GameTime gameTime)
        {
            InputSet inputs = engine_.getInputs();

            if (inputs.getCancelButton())
            {
                inputs.setToggle(InputsEnum.CANCEL_BUTTON, true);
                return new EngineStatePause(engine_, this);
            }

            //Jared's test stuff
            player_.setInputSet(inputs);
            player_.update(gameTime);
            //END Jared's test stuff

            return this;
        }

        public void draw()
        {
            engine_.GraphicsDevice.Clear(Color.Chocolate);

            //Jared's test stuff
            player_.draw(new GameTime());
            //END Jared's test stuff

            healthBar_.draw(new GameTime());
            TextureMap.getInstance().getTexture("healthBarOutline").drawImage(0, healthBarPos_, 0.0f, 0.8f);
            weapon_.draw(new GameTime());
        }

        #endregion
    }
}
