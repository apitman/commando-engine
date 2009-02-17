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
using Commando.graphics;

namespace Commando
{
    /// <summary>
    /// State of being at the main game menu
    /// </summary>
    class EngineStateMenu : EngineStateInterface
    {
        const int SCREEN_SIZE_X = 800;
        const int SCREEN_SIZE_Y = 600;

        protected const string MENU_START_GAME = "Start Game";
        protected const string MENU_CONTROLS = "Controls";
        protected const string MENU_LEVEL_EDITOR = "Level Editor";
        protected const string MENU_QUIT = "Quit";

        protected Engine engine_;
        protected GameTexture menu_;
        protected MenuList mainMenuList_;
        protected int cursorPos_;
        protected string controlTips_;

        /// <summary>
        /// Creates a main menu state
        /// </summary>
        /// <param name="engine">A reference to the engine running the state</param>
        public EngineStateMenu(Engine engine)
        {
            cursorPos_ = 0;
            engine_ = engine;
            List<string> menuString = new List<string>();
            menuString.Add(MENU_START_GAME);
            menuString.Add(MENU_CONTROLS);
            menuString.Add(MENU_LEVEL_EDITOR);
            menuString.Add(MENU_QUIT);

            InputSet inputs = InputSet.getInstance();
            /*controlTips_ = "Select: " +
                            inputs.getControlName(InputsEnum.CONFIRM_BUTTON) +
                            " | " +
                            "Cancel: " +
                            inputs.getControlName(InputsEnum.CANCEL_BUTTON);*/
            controlTips_ = ""; // currrently refreshed every fram in draw()
            mainMenuList_ = new MenuList(menuString,
                                                new Vector2(engine_.GraphicsDevice.Viewport.Width / 2.0f,
                                                engine_.GraphicsDevice.Viewport.Height / 2.0f + 50.0f),
                                                Color.Green,
                                                Color.White,
                                                cursorPos_,
                                                0.0f,
                                                1.0f,
                                                SpriteEffects.None,
                                                1.0f,
                                                40.0f);
        }

        #region EngineStateInterface Members

        /// <summary>
        /// Update the menu by one frame, handle user input
        /// </summary>
        /// <param name="gameTime">GameTime parameter</param>
        /// <returns>A handle to the state of play to be run next frame</returns>
        public EngineStateInterface update(GameTime gameTime)
        {
            InputSet inputs = engine_.getInputs();

            // Temporary for Andrew's test purposes
            /*if (inputs.getButton(InputsEnum.BUTTON_1))
            {
                return new EngineStateLevelEditor(engine_, this, SCREEN_SIZE_X, SCREEN_SIZE_Y);
            }
            */

            /* Commenting this out, seems better to just let them go to "Quit"
            if (inputs.getButton(InputsEnum.CANCEL_BUTTON)) // tenatively Escape / Back
            {
                engine_.Exit();
            }
             */

            if (inputs.getButton(InputsEnum.CONFIRM_BUTTON) ||
                inputs.getButton(InputsEnum.BUTTON_1))
            {
                inputs.setToggle(InputsEnum.CONFIRM_BUTTON);
                inputs.setToggle(InputsEnum.BUTTON_1);
                //get position of cursor from mainMenuList_
                int myCursorPos_ = mainMenuList_.getCursorPos();
                switch(myCursorPos_)
                {
                    case 0:
                        return new EngineStateGameplay(engine_);
                    case 1:
                        return new EngineStateControls(engine_);
                    case 2:
                        return new EngineStateLevelEditor(engine_, this , 800, 600);
                    case 3:
                        engine_.Exit();
                        break;
                }
            }
            if (inputs.getLeftDirectionalY() > 0)
            {
                inputs.setToggle(InputsEnum.LEFT_DIRECTIONAL);
                mainMenuList_.decrementCursorPos();
            }
            if (inputs.getLeftDirectionalY() < 0)
            {
                inputs.setToggle(InputsEnum.LEFT_DIRECTIONAL);
                mainMenuList_.incrementCursorPos();
            }

            controlTips_ = "Press " + inputs.getControlName(InputsEnum.CONFIRM_BUTTON) + " to select an option";

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
         
            engine_.GraphicsDevice.Clear(Color.Black);

            menu_.drawImage(0, new Vector2((engine_.GraphicsDevice.Viewport.Width - menu_.getImageDimensions()[0].Width) / 2, 0), 0.0f);
            
            GameFont myFont = FontMap.getInstance().getFont(FontEnum.Kootenay);
            //print control Tips for main menu

            Vector2 controlTipsPos =
                new Vector2(engine_.GraphicsDevice.Viewport.Width / 2.0f,
                            engine_.GraphicsDevice.Viewport.Height / 2.0f - 10.0f);
            myFont.drawStringCentered(controlTips_,
                                          controlTipsPos,
                                          Color.White,
                                          0.0f,
                                          1.0f,
                                          SpriteEffects.None,
                                          1.0f);
            mainMenuList_.draw();
        }

        #endregion

    }
}
