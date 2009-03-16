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
    /// state of being in the controls menu
    /// </summary>
    class EngineStateEditorControls : EngineStateInterface
    {
        protected Engine engine_;
        
        protected MenuList unselectedControlsMenuList_;
        protected MenuList selectedControlsMenuList_;
        protected string controlTitle_;

        protected readonly FontEnum CONTROLS_FONT = FontEnum.Kootenay;
        protected readonly Color CONTROLS_COLOR = Color.Green;
        protected readonly Color CONTROLS_TITLE_COLOR = Color.WhiteSmoke;
        protected const float CONTROLS_SPACING = 40.0f;

        protected readonly FontEnum HEADER_FONT = FontEnum.Kootenay48;
        protected readonly Color HEADER_COLOR = Color.White;
        protected const float HEADER_ROTATION = 0.0f;
        protected const float HEADER_DEPTH = Constants.DEPTH_MENU_TEXT;

        protected EngineStateInterface returnState_;
        protected int returnScreenSizeX_;
        protected int returnScreenSizeY_;
        public EngineStateEditorControls(Engine engine, EngineStateInterface returnState, int returnScreenSizeX, int returnScreenSizeY)
        {
            InputSet inputs = InputSet.getInstance();

            controlTitle_ = "CONTROLS";
            engine_ = engine;
            returnState_ = returnState;
            returnScreenSizeX_ = returnScreenSizeX;
            returnScreenSizeY_ = returnScreenSizeY;
            List<string> selectedControlsString = new List<string>();
            selectedControlsString.Add("WITH OBJECT SELECTED");
            selectedControlsString.Add("Deslect: " + inputs.getControlName(InputsEnum.RIGHT_TRIGGER));
            selectedControlsString.Add("Place: " + inputs.getControlName(InputsEnum.LEFT_TRIGGER));
            selectedControlsString.Add("Rotate L: " + inputs.getControlName(InputsEnum.LEFT_BUMPER));
            selectedControlsString.Add("Rotate R: " + inputs.getControlName(InputsEnum.RIGHT_BUMPER));
            selectedControlsString.Add("Delete: " + inputs.getControlName(InputsEnum.BUTTON_3));

            List<string> unselectedControlsString = new List<string>();
            unselectedControlsString.Add("NORMAL EDITOR CONTROLS");
#if XBOX
            selectedControlsString.Add("(Coming Soon)");
#endif
            unselectedControlsString.Add("Move Camera/Cursor: " + inputs.getControlName(InputsEnum.LEFT_DIRECTIONAL));
            unselectedControlsString.Add("Select Object/Tile: " + inputs.getControlName(InputsEnum.LEFT_TRIGGER));
            unselectedControlsString.Add("Next Tile: " + inputs.getControlName(InputsEnum.BUTTON_1));
            unselectedControlsString.Add("Prev Tile: " + inputs.getControlName(InputsEnum.BUTTON_2));
            unselectedControlsString.Add("Next Palette: " + inputs.getControlName(InputsEnum.LEFT_BUMPER));
            unselectedControlsString.Add("Prev Palette: " + inputs.getControlName(InputsEnum.RIGHT_BUMPER));
            unselectedControlsString.Add("Place Tile: " + inputs.getControlName(InputsEnum.RIGHT_TRIGGER));
            unselectedControlsString.Add("Quit: " + inputs.getControlName(InputsEnum.CANCEL_BUTTON));

            Vector2 selectedControlsMenuPos =
                new Vector2(engine_.GraphicsDevice.Viewport.Width / 2.0f - 200.0f,
                            engine_.GraphicsDevice.Viewport.Height / 2.0f - 125.0f);
            selectedControlsMenuList_ =
                new MenuList(selectedControlsString, selectedControlsMenuPos);
            selectedControlsMenuList_.Font_ = CONTROLS_FONT;
            selectedControlsMenuList_.BaseColor_ = CONTROLS_COLOR;
            selectedControlsMenuList_.SelectedColor_ = CONTROLS_TITLE_COLOR;
            selectedControlsMenuList_.Spacing_ = CONTROLS_SPACING;

            Vector2 unselectedControlsMenuPos =
                new Vector2(engine_.GraphicsDevice.Viewport.Width / 2.0f + 200.0f,
                            engine_.GraphicsDevice.Viewport.Height / 2.0f - 125.0f);
            unselectedControlsMenuList_ = new MenuList(unselectedControlsString, unselectedControlsMenuPos);
            unselectedControlsMenuList_.BaseColor_ = CONTROLS_COLOR;
            unselectedControlsMenuList_.SelectedColor_ = CONTROLS_TITLE_COLOR;
            unselectedControlsMenuList_.Font_ = CONTROLS_FONT;
            unselectedControlsMenuList_.Spacing_ = CONTROLS_SPACING;
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
                inputs.getButton(InputsEnum.CANCEL_BUTTON) ||
                inputs.getButton(InputsEnum.BUTTON_1))
            {
                inputs.setAllToggles();
                engine_.setScreenSize(returnScreenSizeX_, returnScreenSizeY_);
                return returnState_;
            }

            return this;
        }


        public void draw()
        {
            engine_.GraphicsDevice.Clear(Color.Black);
            GameFont myFont = FontMap.getInstance().getFont(HEADER_FONT);
            //print title of controls screen
            Vector2 headerPos =
                new Vector2(engine_.GraphicsDevice.Viewport.Width / 2.0f,
                                           50.0f);
            myFont.drawStringCentered(controlTitle_,
                                           headerPos,
                                           HEADER_COLOR,
                                           HEADER_ROTATION,
                                           HEADER_DEPTH);
            unselectedControlsMenuList_.draw();
            selectedControlsMenuList_.draw();
        }
        #endregion

    }

}

