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
        protected MenuList controlsMenuList_;
        protected string controlTitle_;

        public EngineStateControls(Engine engine)
        {
            controlTitle_ = "PC CONTROLS";
            engine_ = engine;
            List<string> menuString = new List<string>();
            menuString.Add("W - move foreward");
            menuString.Add("S - move backward");
            menuString.Add("A - sidestep left");
            menuString.Add("D - sidestep right");
            menuString.Add("Esc - pause");
            menuString.Add("Mouse - turn");
            menuString.Add("Esc - pause");
            menuString.Add("Press Enter to return to main menu");
            controlsMenuList_ = new MenuList(menuString,
                                               new Vector2(engine_.GraphicsDevice.Viewport.Width / 2.0f,
                                               engine_.GraphicsDevice.Viewport.Height / 2.0f - 50.0f),
                                               Color.Green,
                                               Color.White,
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

            if (inputs.getConfirmButton()) // tenatively Enter / Start
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
            controlsMenuList_.draw();
        }
        #endregion

    }

}

