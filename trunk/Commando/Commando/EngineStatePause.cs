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

using Commando.controls;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Commando
{
    /// <summary>
    /// A state of play which waits for the user to return playing;
    /// might implement a menu in later functionality
    /// </summary>
    public class EngineStatePause : EngineStateInterface
    {

        const FontEnum PAUSE_FONT = FontEnum.Pericles;
        readonly Color PAUSE_MENU_SELECTED_COLOR = Color.Green;
        readonly Color PAUSE_MENU_UNSELECTED_COLOR = Color.White;
        /*const Vector2 PAUSE_MENU_POSITION =
            new Vector2(engine_.GraphicsDevice.Viewport.Width / 2.0f,
                        engine_.GraphicsDevice.Viewport.Height / 2.0f + 120.0f);*/
        const int PAUSE_MENU_DEFAULT_SELECTED_ITEM = MENU_OPTION_RETURN;
        const float PAUSE_MENU_SPACING = 50.0f;

        const string STR_RETURN_TO_GAME = "Return to Game";
        const string STR_MOVEMENT_TYPE_ABSOLUTE = "Movement Type: Absolute";
        const string STR_MOVEMENT_TYPE_RELATIVE = "Movement Type: Relative";
        const string STR_QUIT_GAME = "Quit Game";

        const int MENU_OPTION_RETURN = 0;
        const int MENU_OPTION_MOVEMENT_TYPE = 1;
        const int MENU_OPTION_QUIT_GAME = 2;

        /// <summary>
        /// The main engine class
        /// </summary>
        protected Engine engine_;

        /// <summary>
        /// The EngineState to be saved - typically an EngineStateGameplay
        /// </summary>
        protected EngineStateInterface savedState_;

        /// <summary>
        /// Settings which can be changed from the pause menu
        /// </summary>
        protected MenuList pauseMenu_;

        /// <summary>
        /// Creates a pause state which waits for the user to resume play
        /// </summary>
        /// <param name="engine">Reference to the engine running the state</param>
        /// <param name="savedState">The state of play to return to once the user unpauses</param>
        public EngineStatePause(Engine engine, EngineStateInterface savedState)
        {
            engine_ = engine;
            savedState_ = savedState;

            List<string> menuString = new List<string>();
            menuString.Add(STR_RETURN_TO_GAME);
            if (Settings.getInstance().getMovementType() == MovementType.ABSOLUTE)
            {
                menuString.Add(STR_MOVEMENT_TYPE_ABSOLUTE);
            }
            else
            {
                menuString.Add(STR_MOVEMENT_TYPE_RELATIVE);
            }
            menuString.Add(STR_QUIT_GAME);
            int cursor = (int)Settings.getInstance().getMovementType();
            pauseMenu_ = new MenuList(menuString,
                                        PAUSE_FONT,
                                        new Vector2(engine_.GraphicsDevice.Viewport.Width / 2.0f,
                                            100.0f),
                                        PAUSE_MENU_SELECTED_COLOR,
                                        PAUSE_MENU_UNSELECTED_COLOR,
                                        PAUSE_MENU_DEFAULT_SELECTED_ITEM,
                                        PAUSE_MENU_SPACING);

        }

        #region EngineStateInterface Members

        /// <summary>
        /// Handles user input to determine whether to switch out of pause
        /// </summary>
        /// <param name="gameTime">GameTime parameter</param>
        /// <returns>The engine state to be in next frame</returns>
        public EngineStateInterface update(GameTime gameTime)
        {
            InputSet inputs = engine_.getInputs();

            if (inputs.getConfirmButton())
            {
                inputs.setToggle(InputsEnum.CONFIRM_BUTTON);
                switch (pauseMenu_.getCursorPos())
                {
                    case MENU_OPTION_RETURN:
                        return savedState_;

                    case MENU_OPTION_MOVEMENT_TYPE:
                        Settings.getInstance().swapMovementType();
                        if (Settings.getInstance().getMovementType() == MovementType.ABSOLUTE)
                        {
                            pauseMenu_.setString(MENU_OPTION_MOVEMENT_TYPE, STR_MOVEMENT_TYPE_ABSOLUTE);
                        }
                        else
                        {
                            pauseMenu_.setString(MENU_OPTION_MOVEMENT_TYPE, STR_MOVEMENT_TYPE_RELATIVE);
                        }
                        break;

                    case MENU_OPTION_QUIT_GAME:
                        engine_.Exit();
                        break;
                }
            }

            if (inputs.getCancelButton())
            {
                inputs.setToggle(InputsEnum.CANCEL_BUTTON);
                return savedState_;
            }

            if (inputs.getLeftDirectionalY() > 0)
            {
                inputs.setToggle(InputsEnum.LEFT_DIRECTIONAL);
                pauseMenu_.decremnentCursorPos();
            }
            else if (inputs.getLeftDirectionalY() < 0)
            {
                inputs.setToggle(InputsEnum.LEFT_DIRECTIONAL);
                pauseMenu_.incrementCursorPos();
            }

            return this;
        }

        /// <summary>
        /// Draws the pause screen
        /// </summary>
        public void draw()
        {
            engine_.GraphicsDevice.Clear(Color.Black);
            pauseMenu_.draw();
        }

        #endregion
    }
}
