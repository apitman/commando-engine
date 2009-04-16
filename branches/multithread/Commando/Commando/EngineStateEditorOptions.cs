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
using System.Text;
using Microsoft.Xna.Framework.GamerServices;
using Commando.controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Commando.graphics.multithreading;

namespace Commando
{
    class EngineStateEditorOptions : EngineStateInterface
    {
        static readonly Color BACKGROUND_COLOR = Color.Black;

        const FontEnum OPTIONS_FONT = FontEnum.Pericles;
        readonly Color OPTIONS_MENU_SELECTED_COLOR = Color.White;
        readonly Color OPTIONS_MENU_UNSELECTED_COLOR = Color.Green;
        protected Vector2 OPTIONS_MENU_POSITION
        {
            get
            {
                Rectangle r = engine_.GraphicsDevice.Viewport.TitleSafeArea;
                return new Vector2(r.X + r.Width / 2, r.Y + r.Height / 2 - OPTIONS_MENU_SPACING * 3);
            }

            set
            {
                throw new NotImplementedException();
            }
        }
        const int OPTIONS_MENU_DEFAULT_SELECTED_ITEM = 0;
        const float OPTIONS_MENU_SPACING = 30.0f;


        const string STR_SOUND_ON = "Sound: On";
        const string STR_SOUND_OFF = "Sound: Off";
        const string STR_RESOLUTION = "Change Screen Resolution";
        const string STR_CHANGE_STORAGE = "Change Storage Device";
        const string STR_DEBUG_MODE_ON = "Debug Mode: On";
        const string STR_DEBUG_MODE_OFF = "Debug Mode: Off";
        const string STR_SAVE_CONTINUE = "Save and Continue";
        const string STR_SAVE_QUIT = "Save and Quit to Main Menu";
        const string STR_QUIT_NOSAVE = "Quit Without Saving";
        const string STR_RETURN = "Return";


        protected Engine engine_;
        protected EngineStateLevelEditor savedState_;
        protected MenuList menuList_;
        protected bool gotoSelectStorage_; // whether we're waiting on an async call

        /// <summary>
        /// Creates an options state which allows the user to adjust game settings
        /// </summary>
        /// <param name="engine">Reference to the engine running the state</param>
        /// <param name="savedState">The state of play to return to once the user finishes</param>
        public EngineStateEditorOptions(Engine engine, EngineStateLevelEditor savedState)
        {
            engine_ = engine;
            savedState_ = savedState;

            List<string> menuString = new List<string>();
         
            if (Settings.getInstance().IsSoundAllowed_)
            {
                menuString.Add(STR_SOUND_ON);
            }
            else
            {
                menuString.Add(STR_SOUND_OFF);
            }
            menuString.Add(STR_RESOLUTION);
            if (Settings.getInstance().IsGamerServicesAllowed_)
            {
                menuString.Add(STR_CHANGE_STORAGE);
            }
            if (Settings.getInstance().IsInDebugMode_)
            {
                menuString.Add(STR_DEBUG_MODE_ON);
            }
            else
            {
                menuString.Add(STR_DEBUG_MODE_OFF);
            }
            menuString.Add(STR_SAVE_CONTINUE);
            menuString.Add(STR_SAVE_QUIT);
            menuString.Add(STR_QUIT_NOSAVE);
            menuString.Add(STR_RETURN);
            menuList_ = new MenuList(menuString, OPTIONS_MENU_POSITION);
            menuList_.Font_ = OPTIONS_FONT;
            menuList_.BaseColor_ = OPTIONS_MENU_UNSELECTED_COLOR;
            menuList_.SelectedColor_ = OPTIONS_MENU_SELECTED_COLOR;
            menuList_.Spacing_ = OPTIONS_MENU_SPACING;
            menuList_.CursorPos_ = OPTIONS_MENU_DEFAULT_SELECTED_ITEM;
        }

        public EngineStateInterface update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (gotoSelectStorage_)
            {
                if (!Guide.IsVisible)
                {
                    try
                    {
                        IAsyncResult result =
                            Guide.BeginShowStorageDeviceSelector(fetchStorageDevice, "selectStorage");
                    }
                    catch (GuideAlreadyVisibleException)
                    {
                        // No code here, but this catch block is needed
                        // because !Guide.IsVisible doesn't always work
                    }
                }
                return this;
            }

            InputSet inputs = InputSet.getInstance();

            if (inputs.getButton(InputsEnum.CONFIRM_BUTTON) ||
                inputs.getButton(InputsEnum.BUTTON_1))
            {
                inputs.setToggle(InputsEnum.CONFIRM_BUTTON);
                inputs.setToggle(InputsEnum.BUTTON_1);
                int currentPos = menuList_.CursorPos_;
                string currentSelection = menuList_.getCurrentString();
                switch (currentSelection)
                {
                   
                    case STR_SOUND_ON:
                        Settings.getInstance().IsSoundAllowed_ = false;
                        menuList_.setString(currentPos, STR_SOUND_OFF);
                        break;
                    case STR_SOUND_OFF:
                        Settings.getInstance().IsSoundAllowed_ = true;
                        menuList_.setString(currentPos, STR_SOUND_ON);
                        break;

                    case STR_RESOLUTION:
                        Resolution res = Settings.getInstance().Resolution_;
                        Resolution next = (Resolution)(((int)res + 1) % (int)Resolution.LENGTH);
                        Settings.getInstance().Resolution_ = next;
                        menuList_.Position_ = OPTIONS_MENU_POSITION;
                        break;

                    case STR_CHANGE_STORAGE:
                        if (Settings.getInstance().IsGamerServicesAllowed_)
                        {
                            gotoSelectStorage_ = true;
                            return this;
                        }
                        break;

                    case STR_DEBUG_MODE_ON:
                        Settings.getInstance().IsInDebugMode_ = false;
                        menuList_.setString(currentPos, STR_DEBUG_MODE_OFF);
                        break;
                    case STR_DEBUG_MODE_OFF:
                        Settings.getInstance().IsInDebugMode_ = true;
                        menuList_.setString(currentPos, STR_DEBUG_MODE_ON);
                        break;

                    case STR_SAVE_CONTINUE:
                        return new EngineStateLevelSave(engine_, savedState_, savedState_, this);
                    case STR_SAVE_QUIT:
                        return new EngineStateLevelSave(engine_, savedState_,new EngineStateMenu(engine_), this);
                    case STR_QUIT_NOSAVE:
                        return new EngineStateMenu(engine_);
                    case STR_RETURN:
                        return savedState_;
                }
            }

            if (inputs.getButton(InputsEnum.CANCEL_BUTTON) ||
                inputs.getButton(InputsEnum.BUTTON_2))
            {
                inputs.setToggle(InputsEnum.BUTTON_2);
                inputs.setToggle(InputsEnum.CANCEL_BUTTON);
                return savedState_;
            }

            if (inputs.getLeftDirectionalY() > 0)
            {
                inputs.setToggle(InputsEnum.LEFT_DIRECTIONAL);
                menuList_.decrementCursorPos();
            }
            else if (inputs.getLeftDirectionalY() < 0)
            {
                inputs.setToggle(InputsEnum.LEFT_DIRECTIONAL);
                menuList_.incrementCursorPos();
            }

            return this;
        }

        public void draw()
        {
            //engine_.GraphicsDevice.Clear(BACKGROUND_COLOR);
            DrawBuffer.getInstance().getUpdateStack().ScreenClearColor_ = BACKGROUND_COLOR;

            menuList_.draw();
        }

        /// <summary>
        /// Asynchronous call to run once the user selects a storage device from the
        /// Guide menu.
        /// </summary>
        /// <param name="result"></param>
        private void fetchStorageDevice(IAsyncResult result)
        {
            StorageDevice storageDevice = Guide.EndShowStorageDeviceSelector(result);

            // User selected a device, so we set it and return to normal functionality
            if (storageDevice != null)
            {
                InputSet.getInstance().setAllToggles();
                Settings.getInstance().StorageDevice_ = storageDevice;
                gotoSelectStorage_ = false;
            }

            // User cancelled, so we make no changes and return to normal functionality
            else
            {
                InputSet.getInstance().setAllToggles();
                gotoSelectStorage_ = false;
            }
        }

    }
}
