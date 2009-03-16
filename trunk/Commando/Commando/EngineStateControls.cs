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
    class EngineStateControls : EngineStateInterface
    {
        protected Engine engine_;
        protected MenuList gameControlsMenuList_;
        protected MenuList editorControlsMenuList_;
        protected string controlTitle_;

        protected readonly FontEnum CONTROLS_FONT = FontEnum.Kootenay;
        protected readonly Color CONTROLS_COLOR = Color.Green;
        protected readonly Color CONTROLS_TITLE_COLOR = Color.WhiteSmoke;
        protected const float CONTROLS_SPACING = 40.0f;

        protected readonly FontEnum HEADER_FONT = FontEnum.Kootenay48;
        protected readonly Color HEADER_COLOR = Color.White;
        protected const float HEADER_ROTATION = 0.0f;
        protected const float HEADER_DEPTH = Constants.DEPTH_MENU_TEXT;

        public EngineStateControls(Engine engine)
        {
            InputSet inputs = InputSet.getInstance();

            controlTitle_ = "CONTROLS";
            engine_ = engine;
            List<string> gameControlsString = new List<string>();
            gameControlsString.Add("GAMEPLAY CONTROLS");
            gameControlsString.Add("Move: " + inputs.getControlName(InputsEnum.LEFT_DIRECTIONAL));
            gameControlsString.Add("Rotate: " + inputs.getControlName(InputsEnum.RIGHT_DIRECTIONAL));
            gameControlsString.Add("Fire: " + inputs.getControlName(InputsEnum.RIGHT_TRIGGER));
            gameControlsString.Add("Pause: " + inputs.getControlName(InputsEnum.CANCEL_BUTTON));

            List<string> editorControlsString = new List<string>();
            editorControlsString.Add("EDITOR CONTROLS");
#if XBOX
            editorControlsString.Add("(Coming Soon)");
#endif
            editorControlsString.Add("Move Cursor: " + inputs.getControlName(InputsEnum.LEFT_DIRECTIONAL));
            editorControlsString.Add("Next Tile: " + inputs.getControlName(InputsEnum.BUTTON_1));
            editorControlsString.Add("Prev Tile: " + inputs.getControlName(InputsEnum.BUTTON_2));
            editorControlsString.Add("Next Palette: " + inputs.getControlName(InputsEnum.LEFT_BUMPER));
            editorControlsString.Add("Prev Palette: " + inputs.getControlName(InputsEnum.RIGHT_BUMPER));
            editorControlsString.Add("Place Tile: " + inputs.getControlName(InputsEnum.RIGHT_TRIGGER));
            editorControlsString.Add("Quit: " + inputs.getControlName(InputsEnum.CANCEL_BUTTON));

            Vector2 gameControlsMenuPos =
                new Vector2(engine_.GraphicsDevice.Viewport.Width / 2.0f - 200.0f,
                            engine_.GraphicsDevice.Viewport.Height / 2.0f - 125.0f);
            gameControlsMenuList_ =
                new MenuList(gameControlsString, gameControlsMenuPos);
            gameControlsMenuList_.Font_ = CONTROLS_FONT;
            gameControlsMenuList_.BaseColor_ = CONTROLS_COLOR;
            gameControlsMenuList_.SelectedColor_ = CONTROLS_TITLE_COLOR;
            gameControlsMenuList_.Spacing_ = CONTROLS_SPACING;

            Vector2 editorControlsMenuPos =
                new Vector2(engine_.GraphicsDevice.Viewport.Width / 2.0f + 200.0f,
                            engine_.GraphicsDevice.Viewport.Height / 2.0f - 125.0f);
            editorControlsMenuList_ = new MenuList(editorControlsString, editorControlsMenuPos);
            editorControlsMenuList_.BaseColor_ = CONTROLS_COLOR;
            editorControlsMenuList_.SelectedColor_ = CONTROLS_TITLE_COLOR;
            editorControlsMenuList_.Font_ = CONTROLS_FONT;
            editorControlsMenuList_.Spacing_ = CONTROLS_SPACING;
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
                return new EngineStateMenu(engine_);
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
            gameControlsMenuList_.draw();
            editorControlsMenuList_.draw();
        }
        #endregion

    }

}

