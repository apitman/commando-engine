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

        public EngineStateControls(Engine engine)
        {
            controlTitle_ = "PC CONTROLS";
            engine_ = engine;
            List<string> gameControlsString = new List<string>();
            gameControlsString.Add("GAMEPLAY CONTROLS");
            gameControlsString.Add("W - move foreward");
            gameControlsString.Add("S - move backward");
            gameControlsString.Add("A - sidestep left");
            gameControlsString.Add("D - sidestep right");
            gameControlsString.Add("Mouse - turn");
            gameControlsString.Add("Esc - pause");
            gameControlsString.Add("Press Enter to return to main menu");

            List<string> editorControlsString = new List<string>();
            editorControlsString.Add("EDITOR CONTROLS");
            editorControlsString.Add("W - move curor up");
            editorControlsString.Add("S - move cursor down");
            editorControlsString.Add("A - move cursor left");
            editorControlsString.Add("D - move cursor right");
            editorControlsString.Add("Left Shift - Change Tile +");
            editorControlsString.Add("Spacebar - Change Tile -");
            editorControlsString.Add("Enter - place tile");
            editorControlsString.Add("Esc - return to main menu");
            gameControlsMenuList_ = new MenuList(gameControlsString,
                                               new Vector2(200.0f,
                                               engine_.GraphicsDevice.Viewport.Height / 2.0f - 50.0f),
                                               Color.Green,
                                               Color.White,
                                               8,
                                               0.0f,
                                               1.0f,
                                               SpriteEffects.None,
                                               1.0f,
                                               40.0f);
            editorControlsMenuList_ = new MenuList(editorControlsString,
                                   new Vector2(engine_.GraphicsDevice.Viewport.Width / 2.0f + 200.0f,
                                   engine_.GraphicsDevice.Viewport.Height / 2.0f - 50.0f),
                                   Color.Green,
                                   Color.Green,
                                   7,
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

            if (inputs.getButton(InputsEnum.CONFIRM_BUTTON)) // tenatively Enter / Start
            {
                inputs.setToggle(InputsEnum.CONFIRM_BUTTON);
                return new EngineStateMenu(engine_);
            }

            return this;
        }


        public void draw()
        {
            engine_.GraphicsDevice.Clear(Color.Black);
            GameFont myFont = FontMap.getInstance().getFont(FontEnum.Kootenay);
            //print title of controls screen
            myFont.drawStringCentered(controlTitle_,
                                           new Vector2(engine_.GraphicsDevice.Viewport.Width / 2.0f,
                                           50.0f),
                                           Color.White,
                                           0.0f,
                                           4.0f,
                                           SpriteEffects.None,
                                           1.0f);
            gameControlsMenuList_.draw();
            editorControlsMenuList_.draw();
        }
        #endregion

    }

}

