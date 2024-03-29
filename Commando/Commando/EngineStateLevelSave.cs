﻿/*
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
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Storage;
using Commando.controls;
using System.IO;
using Commando.graphics.multithreading;
#if !XBOX
using System.Windows.Forms;
#endif

namespace Commando
{
    public class EngineStateLevelSave : EngineStateInterface
    {
        protected const string KEYBOARD_MESSAGE = "Name Your Level";
        protected const string KEYBOARD_DESCRIPTION = "Enter a name for this level.";

        public const string DIRECTORY_NAME = "levels";
        public const string LEVEL_EXTENSION = ".commandolevel";

        protected const string MESSAGE = "Please do not interrupt this process.";
        protected const FontEnum MESSAGE_FONT = FontEnum.Kootenay14;
        protected Vector2 MESSAGE_POSITION
        {
            get
            {
                Rectangle r = engine_.GraphicsDevice.Viewport.TitleSafeArea;
                return new Vector2(r.Width / 2 + r.X, r.Height / 2 + r.Y);
            }

            set
            {
                throw new NotImplementedException();
            }
        }
        protected static readonly Color MESSAGE_COLOR = Color.White;
        protected const float MESSAGE_DEPTH = Constants.DEPTH_MENU_TEXT;
        protected static readonly Color FILENAME_COLOR = Color.Green;
        protected static readonly Vector2 FILENAME_OFFSET = new Vector2(0, 50);

        protected Engine engine_;
        protected EngineStateLevelEditor state_;
        protected EngineStateInterface successState_;
        protected EngineStateInterface cancelState_;
        protected EngineStateInterface returnState_;
        
        protected bool saved_ = false;
        protected bool cancelFlag_ = false;
        protected bool tryingToSave_ = false;

        protected GameFont mainMessage_;
        protected string currentFilename_ = "";

        public EngineStateLevelSave(Engine engine, EngineStateLevelEditor state, EngineStateInterface successState, EngineStateInterface cancelState)
        {
            engine_ = engine;
            state_ = state;
            successState_ = successState;
            cancelState_ = cancelState;
            returnState_ = this;
            mainMessage_ = FontMap.getInstance().getFont(MESSAGE_FONT);
            StorageDevice sd = Settings.getInstance().StorageDevice_;
            if (sd == null)
            {
#if !XBOX
                saveDialog();
#endif
            }
        }

        public EngineStateInterface update(GameTime gameTime)
        {
            if (cancelFlag_)
            {
                return cancelState_;
            }

            if (!tryingToSave_ && !cancelFlag_)
            {
                tryingToSave_ = true;
                if (Settings.getInstance().IsGamerServicesAllowed_)
                {
                    IAsyncResult result =
                        Guide.BeginShowKeyboardInput(
                            Settings.getInstance().CurrentPlayer_,
                            KEYBOARD_MESSAGE,
                            KEYBOARD_DESCRIPTION,
                            Path.GetFileNameWithoutExtension(state_.currentFilepath_),
                            enterFilename,
                            "enterFilename");
                }
                else
                {
                    // TODO
                    // Create Windows filepath dialog box here if on PC
                }
            }

            return returnState_;

        }

        private void enterFilename(IAsyncResult result)
        {
            string filename = Guide.EndShowKeyboardInput(result);

            // If they cancelled, go back to LevelEditor
            if (filename == null)
            {
                returnState_ = cancelState_;
                cancelFlag_ = true;
                return;
            }

            currentFilename_ = filename + LEVEL_EXTENSION;

            // TODO
            // Must ensure that if KeyboardInput works, this will always
            //  have a valid storage device
            saveLevel(Settings.getInstance().StorageDevice_);
        }

        private void saveLevel(StorageDevice storageDevice)
        {
            StorageContainer container =
                ContainerManager.getOpenContainer();
            string directory = Path.Combine(container.Path, DIRECTORY_NAME);
            System.IO.Directory.CreateDirectory(directory);
            string filename = Path.Combine(directory, currentFilename_);
            state_.myLevel_.writeLevelToFile(filename);
            container.Dispose();

            //returnState_ = new EngineStateMenu(engine_);
            returnState_ = successState_;
        }

        public void draw()
        {
            //engine_.GraphicsDevice.Clear(Color.Black);
            DrawBuffer.getInstance().getUpdateStack().ScreenClearColor_ = Color.Black;

            mainMessage_.drawStringCentered(MESSAGE, MESSAGE_POSITION, MESSAGE_COLOR, 0, MESSAGE_DEPTH);

            Vector2 fileNamePos = MESSAGE_POSITION + FILENAME_OFFSET;
            mainMessage_.drawStringCentered(currentFilename_, fileNamePos, FILENAME_COLOR, 0, MESSAGE_DEPTH);
        }

#if !XBOX
        [STAThread]
        private void saveDialog()
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "Commando Level Files (*" + LEVEL_EXTENSION + ")|*" + LEVEL_EXTENSION;
            dialog.RestoreDirectory = true;
            dialog.Title = "Commando Engine Save Prompt";
            InputSet.getInstance().setToggle(InputsEnum.CONFIRM_BUTTON);
            InputSet.getInstance().setToggle(InputsEnum.RIGHT_TRIGGER);
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                state_.myLevel_.writeLevelToFile(dialog.FileName);
                returnState_ = new EngineStateMenu(engine_);
                cancelFlag_ = true;
            }
            else
            {
                returnState_ = state_;
                cancelFlag_ = true;
            }
        }
#endif
    }
}
