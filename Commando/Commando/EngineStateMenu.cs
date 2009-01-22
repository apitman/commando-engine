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

using System.Collections.Generic;
using Commando.controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Commando
{
    /// <summary>
    /// State of being at the main game menu
    /// </summary>
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

        /// <summary>
        /// Creates a main menu state
        /// </summary>
        /// <param name="engine">A reference to the engine running the state</param>
        public EngineStateMenu(Engine engine)
        {
            cursorPos_ = 0;
            engine_ = engine;
            menuList_ = new List<string>();

            menuList_.Add("Start Game");
            menuList_.Add("Controls");
            menuList_.Add("Exit");


        }

        protected void drawStringList(List<string> stringList, Vector2 pos, Color color1, Color color2, int selected, float rotation,float scale, SpriteEffects effects, float layerDepth, float spacing)
        {
            GameFont myFont = FontMap.getInstance().getFont(FontEnum.Kootenay);
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

        #region EngineStateInterface Members

        /// <summary>
        /// Update the menu by one frame, handle user input
        /// </summary>
        /// <param name="gameTime">GameTime parameter</param>
        /// <returns>A handle to the state of play to be run next frame</returns>
        public EngineStateInterface update(GameTime gameTime)
        {

            InputSet inputs = engine_.getInputs();

            if (inputs.getCancelButton()) // tenatively Escape / Back
            {
                engine_.Exit();
            }

            if (inputs.getConfirmButton()) // tenatively Enter / Start
            {
                switch(cursorPos_)
                {
                    case 0:
                        return new EngineStateGameplay(engine_);
                        break;
                    case 1:
                        break;
                    case 2:
                        engine_.Exit();
                        break;
                }
            }
            if (inputs.getLeftDirectionalY() > 0)
            {
                inputs.setToggle(InputsEnum.LEFT_DIRECTIONAL);
                cursorPos_--;
                if (cursorPos_ < 0)
                {
                    cursorPos_ = 2;
                }
            }
            if (inputs.getLeftDirectionalY() < 0)
            {
                inputs.setToggle(InputsEnum.LEFT_DIRECTIONAL);
                cursorPos_++;
                cursorPos_ = cursorPos_ % 3;
            }

            return this;
        }

        /// <summary>
        /// Draw the menu to the screen
        /// </summary>
        public void draw()
        {
            if (menu_ == null)
            {
                menu_ = TextureMap.getInstance().getTexture("TitleScreen");
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

            menu_.drawImage(0, new Vector2((engine_.GraphicsDevice.Viewport.Width - menu_.getImageDimensions()[0].Width) / 2, 0), 0.0f);

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
                        engine_.GraphicsDevice.Viewport.Height / 2 + 50),
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
