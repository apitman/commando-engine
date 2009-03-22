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
using System;

namespace Commando
{
    /// <summary>
    /// State of being at the main game menu
    /// </summary>
    class EngineStateMenu : EngineStateInterface
    {
        protected readonly FontEnum MENU_FONT = FontEnum.Kootenay;
        protected readonly Color MENU_SELECTED_COLOR = Color.White;
        protected readonly Color MENU_UNSELECTED_COLOR = Color.Green;
        protected const float MENU_DEPTH = Constants.DEPTH_MENU_TEXT;
        protected const float MENU_ROTATION = 0.0f;
        protected const float MENU_SPACING = 40.0f;
        protected const int MENU_DEFAULT_CURSOR_POSITION = 0;
        protected Vector2 MENU_POSITION
        {
            get
            {
                Rectangle r = engine_.GraphicsDevice.Viewport.TitleSafeArea;
                return new Vector2(r.X + r.Width / 2.0f, r.Y + r.Height / 2.0f + 50.0f);
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        protected const FontEnum CONTROL_TIPS_FONT = FontEnum.Kootenay;
        protected readonly Color CONTROL_TIPS_COLOR = Color.White;
        protected const float CONTROL_TIPS_ROTATION = 0.0f;
        protected const float CONTROL_TIPS_DEPTH = Constants.DEPTH_MENU_TEXT;
        protected Vector2 CONTROL_TIPS_POSITION
        {
            get
            {
                Rectangle r = engine_.GraphicsDevice.Viewport.TitleSafeArea;
                return new Vector2(r.X + r.Width / 2.0f, r.Y + r.Height / 2.0f + 10.0f);
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        protected Vector2 LOGO_POSITION
        {
            get
            {
                Rectangle r = engine_.GraphicsDevice.Viewport.TitleSafeArea;
                return new Vector2(r.X + (r.Width - logo_.getImageDimensions()[0].Width) / 2, r.Y);
            }
            set
            {
                throw new NotImplementedException();
            }
        }
        protected const float LOGO_DEPTH = Constants.DEPTH_LOW;

        protected const string STR_MENU_START_GAME = "Start Game";
        protected const string STR_MENU_CONTROLS = "Controls";
        protected const string STR_MENU_LEVEL_EDITOR = "Level Editor";
        protected const string STR_MENU_QUIT = "Quit";

        protected Engine engine_;
        protected GameTexture logo_;
        protected MenuList mainMenuList_;
        protected string controlTips_;

        /// <summary>
        /// Creates a main menu state
        /// </summary>
        /// <param name="engine">A reference to the engine running the state</param>
        public EngineStateMenu(Engine engine)
        {
            engine_ = engine;
            logo_ = TextureMap.fetchTexture("TitleScreen");

            List<string> menuString = new List<string>();
            menuString.Add(STR_MENU_START_GAME);
            menuString.Add(STR_MENU_CONTROLS);
            menuString.Add(STR_MENU_LEVEL_EDITOR);
            menuString.Add(STR_MENU_QUIT);

            GlobalHelper.getInstance().setCurrentCamera(new Camera());

            InputSet inputs = InputSet.getInstance();
            /*controlTips_ = "Select: " +
                            inputs.getControlName(InputsEnum.CONFIRM_BUTTON) +
                            " | " +
                            "Cancel: " +
                            inputs.getControlName(InputsEnum.CANCEL_BUTTON);*/
            controlTips_ = ""; // currrently refreshed every frame in draw()
            mainMenuList_ = new MenuList(menuString, MENU_POSITION);
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

            InputSet inputs = InputSet.getInstance();

            if (inputs.getButton(InputsEnum.CONFIRM_BUTTON) ||
                inputs.getButton(InputsEnum.BUTTON_1))
            {
                inputs.setToggle(InputsEnum.CONFIRM_BUTTON);
                inputs.setToggle(InputsEnum.BUTTON_1);
                
                int cursorPos = mainMenuList_.getCursorPos();
                switch(cursorPos)
                {
                    case 0:
                        return new EngineStateLevelLoad(engine_, EngineStateLevelLoad.EngineStateTarget.GAMEPLAY);
                    case 1:
                        return new EngineStateControls(engine_);
                    case 2:
                        return new EngineStateLevelLoad(engine_, EngineStateLevelLoad.EngineStateTarget.LEVEL_EDITOR);
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

            if (inputs.getButton(InputsEnum.CANCEL_BUTTON) ||
                inputs.getButton(InputsEnum.BUTTON_2))
            {
                #if XBOX
                    inputs.setToggle(InputsEnum.CANCEL_BUTTON);
                    inputs.setToggle(InputsEnum.BUTTON_2);
                    return new EngineStateStart(engine_);
                #endif
            }

            controlTips_ = "Press " + inputs.getControlName(InputsEnum.CONFIRM_BUTTON) + " to select an option";

            return this;
        }

        /// <summary>
        /// Draw the menu to the screen
        /// </summary>
        public void draw()
        {
            engine_.GraphicsDevice.Clear(Color.Black);

            logo_.drawImageAbsolute(0, LOGO_POSITION, LOGO_DEPTH);

            //print control Tips for main menu
            GameFont myFont = FontMap.getInstance().getFont(CONTROL_TIPS_FONT);
            myFont.drawStringCentered(controlTips_,
                                        CONTROL_TIPS_POSITION,
                                        CONTROL_TIPS_COLOR,
                                        CONTROL_TIPS_ROTATION,
                                        CONTROL_TIPS_DEPTH);

            mainMenuList_.Position_ = MENU_POSITION;
            mainMenuList_.draw();
        }

        #endregion

    }
}
