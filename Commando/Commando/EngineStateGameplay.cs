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
using Commando.objects;
using Commando.levels;

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
        protected List<TileObject> tiles_;

        public EngineStateGameplay(Engine engine)
        {
            engine_ = engine;
            engine_.setScreenSize(375, 375);
            //Jared's test stuff
            player_ = new objects.MainPlayer();
            int[,] tiles = new int[,]   {{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                                        {0,7,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,8,0,0,0,0,0,0,0},
                                        {0,2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,4,0,0,0,0,0,0,0},
                                        {0,2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,4,0,0,0,0,0,0,0},
                                        {0,2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,4,0,0,0,0,0,0,0},
                                        {0,2,1,1,1,10,11,12,10,11,12,1,1,1,1,1,1,19,3,3,3,3,3,8,0},
                                        {0,2,1,1,1,13,14,15,13,14,15,1,1,1,1,1,1,1,1,1,1,1,1,4,0},
                                        {0,2,1,1,1,16,17,18,16,17,18,1,1,1,1,1,1,1,1,1,1,1,1,4,0},
                                        {0,2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,4,0},
                                        {0,2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,4,0},
                                        {0,2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,4,0},
                                        {0,2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,4,0},
                                        {0,2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,4,0},
                                        {0,2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,4,0},
                                        {0,2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,4,0},
                                        {0,2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,4,0},
                                        {0,2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,20,5,5,5,5,5,9,0},
                                        {0,2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,4,0,0,0,0,0,0,0},
                                        {0,2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,4,0,0,0,0,0,0,0},
                                        {0,2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,4,0,0,0,0,0,0,0},
                                        {0,6,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,9,0,0,0,0,0,0,0},
                                        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0}};
            tiles_ = Tiler.getTiles(tiles);
            //END Jared's test stuff

            healthBarPos_ = new Vector2(100.0f, 350.0f);
            weaponPos_ = new Vector2(200.0f, 350.0f);
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
                inputs.setToggle(InputsEnum.CANCEL_BUTTON);
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
            foreach (TileObject tOb in tiles_)
            {
                tOb.draw(new GameTime());
            }
            //END Jared's test stuff

            healthBar_.draw(new GameTime());
            TextureMap.getInstance().getTexture("healthBarOutline").drawImage(0, healthBarPos_, 0.0f, 0.8f);
            weapon_.draw(new GameTime());

            FontMap.getInstance().getFont(FontEnum.Kootenay).drawString("Health", new Vector2(healthBarPos_.X - 27.0f, healthBarPos_.Y - 12.0f), Color.Black, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.9f);
            FontMap.getInstance().getFont(FontEnum.Kootenay).drawString("20/20 bullets", new Vector2(weaponPos_.X + 50.0f, weaponPos_.Y - 14.0f), Color.Black, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.9f);
        }

        #endregion
    }
}
