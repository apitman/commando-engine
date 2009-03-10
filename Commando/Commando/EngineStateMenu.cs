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

        protected readonly FontEnum MENU_FONT = FontEnum.Kootenay;
        protected readonly Color MENU_SELECTED_COLOR = Color.White;
        protected readonly Color MENU_UNSELECTED_COLOR = Color.Green;
        protected const float MENU_DEPTH = Constants.DEPTH_MENU_TEXT;
        protected const float MENU_ROTATION = 0.0f;
        protected const float MENU_SPACING = 40.0f;
        protected const int MENU_DEFAULT_CURSOR_POSITION = 0;

        protected const FontEnum CONTROL_TIPS_FONT = FontEnum.Kootenay;
        protected readonly Color CONTROL_TIPS_COLOR = Color.White;
        protected const float CONTROL_TIPS_ROTATION = 0.0f;
        protected const float CONTROL_TIPS_DEPTH = Constants.DEPTH_MENU_TEXT;

        protected const string STR_MENU_START_GAME = "Start Game";
        protected const string STR_MENU_CONTROLS = "Controls";
        protected const string STR_MENU_LEVEL_EDITOR = "Level Editor";
        protected const string STR_MENU_QUIT = "Quit";

        protected Engine engine_;
        protected GameTexture menu_;
        protected MenuList mainMenuList_;
        protected string controlTips_;

        /// <summary>
        /// Creates a main menu state
        /// </summary>
        /// <param name="engine">A reference to the engine running the state</param>
        public EngineStateMenu(Engine engine)
        {
            engine_ = engine;
            engine_.setScreenSize(SCREEN_SIZE_X, SCREEN_SIZE_Y);

            List<string> menuString = new List<string>();
            menuString.Add(STR_MENU_START_GAME);
            menuString.Add(STR_MENU_CONTROLS);
#if !XBOX
            menuString.Add(STR_MENU_LEVEL_EDITOR);
#endif
            menuString.Add(STR_MENU_QUIT);

            GlobalHelper.getInstance().setCurrentCamera(new Camera());

            InputSet inputs = InputSet.getInstance();
            /*controlTips_ = "Select: " +
                            inputs.getControlName(InputsEnum.CONFIRM_BUTTON) +
                            " | " +
                            "Cancel: " +
                            inputs.getControlName(InputsEnum.CANCEL_BUTTON);*/
            controlTips_ = ""; // currrently refreshed every frame in draw()
            Vector2 menuPos = new Vector2(engine_.GraphicsDevice.Viewport.Width / 2.0f,
                                                engine_.GraphicsDevice.Viewport.Height / 2.0f + 50.0f);
            mainMenuList_ = new MenuList(menuString, menuPos);
            mainMenuList_.BaseColor_ = MENU_UNSELECTED_COLOR;
            mainMenuList_.SelectedColor_ = MENU_SELECTED_COLOR;
            mainMenuList_.CursorPos_ = MENU_DEFAULT_CURSOR_POSITION;
            mainMenuList_.Font_ = MENU_FONT;
            mainMenuList_.Spacing_ = MENU_SPACING;
            mainMenuList_.LayerDepth_ = MENU_DEPTH;
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

            if (inputs.getButton(InputsEnum.CONFIRM_BUTTON) ||
                inputs.getButton(InputsEnum.BUTTON_1))
            {
                inputs.setToggle(InputsEnum.CONFIRM_BUTTON);
                inputs.setToggle(InputsEnum.BUTTON_1);
                //get position of cursor from mainMenuList_
                int cursorPos = mainMenuList_.getCursorPos();
#if XBOX
                if (cursorPos == 2) cursorPos = 3;
#endif
                switch(cursorPos)
                {
                    case 0:
                        return new EngineStateGameplay(engine_);
                    case 1:
                        return new EngineStateControls(engine_);
                    case 2:
                        return new EngineStateLevelEditor(engine_, this , SCREEN_SIZE_X, SCREEN_SIZE_Y);
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

            menu_.drawImageAbsolute(0, new Vector2((engine_.GraphicsDevice.Viewport.Width - menu_.getImageDimensions()[0].Width) / 2, 0), 0.0f);

            //print control Tips for main menu
            GameFont myFont = FontMap.getInstance().getFont(CONTROL_TIPS_FONT);
            Vector2 controlTipsPos =
                new Vector2(engine_.GraphicsDevice.Viewport.Width / 2.0f,
                            engine_.GraphicsDevice.Viewport.Height / 2.0f - 10.0f);
            myFont.drawStringCentered(controlTips_,
                                        controlTipsPos,
                                        CONTROL_TIPS_COLOR,
                                        CONTROL_TIPS_ROTATION,
                                        CONTROL_TIPS_DEPTH);
            mainMenuList_.draw();
        }

        #endregion

    }
}
