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
    class EngineStateMenu : EngineStateInterface
    {
        protected Engine engine_;
        protected GameTexture menu_;
        protected GameTexture startSelected_;
        protected GameTexture startReg_;
        protected List<string> menuList_;
        //tracks position of cursor, should be either
        //0 - start game
        //1 - view controls
        //2 - view credits
        protected int cursorPos_;
        public EngineStateMenu(Engine engine)
        {
            cursorPos_ = 0;
            engine_ = engine;
            menuList_ = new List<string>();

            menuList_.Add("Start Game");
            menuList_.Add("Controls");
            menuList_.Add("Exit");


        }



        #region EngineStateInterface Members

        protected void drawStringList(List<string> stringList, Vector2 pos, Color color1, Color color2, int selected, float rotation,float scale, SpriteEffects effects, float layerDepth, float spacing)
        {
            GameFont myFont = FontMap.getInstance().getFont("Kootenay");
            int listLength = stringList.Count;
            Vector2 curPos = pos;
            Color myColor;
            for (int i = 0; i < listLength; i++)
            {
                if (i == selected)
                {
                    myColor = color2;
                }
                else
                {
                    myColor = color1;
                }
                string curString = stringList[i];
                
                myFont.drawStringCentered(curString, 
                                          curPos, 
                                          myColor, 
                                          rotation, 
                                          scale,
                                       
                                          effects, 
                                          layerDepth);
                curPos.Y = curPos.Y + spacing;
            }
        }
            //string text, Vector2 pos, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth
        

        public EngineStateInterface update(GameTime gameTime)
        {

            InputSet inputs = engine_.getInputs();

            if (inputs.getCancelButton()) // tenatively Escape / Back
            {
                engine_.Exit();
            }

            if (inputs.getConfirmButton() && (cursorPos_ == 0)) // tenatively Enter / Start
            {
                return new EngineStateGameplay(engine_);
            }
            if (inputs.getLeftDirectionalY() > 0)
            {
                inputs.setToggle(InputsEnum.LEFT_DIRECTIONAL, true);
                cursorPos_--;
                if (cursorPos_ < 0)
                {
                    cursorPos_ = 2;
                }
            }
            if (inputs.getLeftDirectionalY() < 0)
            {
                inputs.setToggle(InputsEnum.LEFT_DIRECTIONAL, true);
                cursorPos_++;
                cursorPos_ = cursorPos_ % 3;
            }

            return this;
        }

        public void draw()
        {
            if (menu_ == null)
            {
                menu_ = TextureMap.getInstance().getTexture("testMenu");
            }
            if (startSelected_ == null)
            {
                startSelected_ = TextureMap.getInstance().getTexture("MenuStartSelected");
            }
            if (startReg_ == null)
            {
                startReg_ = TextureMap.getInstance().getTexture("MenuStartReg");
            }
            engine_.GraphicsDevice.Clear(Color.Black);

            menu_.drawImageWithDim(0, new Rectangle(0, 0, engine_.GraphicsDevice.Viewport.Width, engine_.GraphicsDevice.Viewport.Height), 0.0f);
            /*if (cursorPos_ == 0)
            {
                Vector2 mypos;
                mypos.X = 400;
                mypos.Y = 300;
                startSelected_.drawImage(0, mypos, 0.0f, 0.5f);
            }
            else
            {
                Vector2 mypos;
                mypos.X = 400;
                mypos.Y = 300;
                startReg_.drawImage(0, mypos, 0.0f, 0.5f);
            }*/

            drawStringList(menuList_,
                new Vector2(engine_.GraphicsDevice.Viewport.Width / 2,
                        engine_.GraphicsDevice.Viewport.Height / 2),
                        Color.Green,
                        Color.White,
                        cursorPos_,
                        0.0f,
                         1.0f,
                        SpriteEffects.None,
                        1.0f,
                        40.0f);
        }
        #endregion

    }
}
