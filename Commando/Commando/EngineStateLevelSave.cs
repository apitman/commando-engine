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
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Storage;
using Commando.controls;
using System.IO;

namespace Commando
{
    public class EngineStateLevelSave : EngineStateInterface
    {
#if !XBOX
        public const string CONTAINER_NAME = "Commando";
#else
        public const string CONTAINER_NAME = "CommandoXbox";
#endif
        public const string DIRECTORY_NAME = "levels";
        public const string LEVEL_EXTENSION = ".commandolevel";

        protected const string MESSAGE = "Please enter a filename to save as :";
        protected const FontEnum MESSAGE_FONT = FontEnum.Kootenay;
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
        protected EngineStateInterface returnState_;
        protected bool saved_ = false;
        protected bool cancelFlag_ = false;

        protected GameFont mainMessage_;
        protected string currentFilename_ = "";

        public EngineStateLevelSave(Engine engine, EngineStateLevelEditor state)
        {
            engine_ = engine;
            state_ = state;
            returnState_ = this;
            mainMessage_ = FontMap.getInstance().getFont(MESSAGE_FONT);

        }

        public EngineStateInterface update(GameTime gameTime)
        {
            if (cancelFlag_)
            {
                return returnState_;
            }

            if (currentFilename_ == "")
            {
                IAsyncResult result =
                    Guide.BeginShowKeyboardInput(
                        PlayerIndex.One,
                        "Enter a name for this level",
                        "Or else you die",
                        "defaultlevel",
                        enterFilename,
                        "enterFilename");
            }

            if (currentFilename_ != "" && !Guide.IsVisible && !saved_)
            {
                try
                {
                    IAsyncResult result2 =
                        Guide.BeginShowStorageDeviceSelector(findStorageDevice, "saveRequest");
                    saved_ = true;
                }
                catch (GuideAlreadyVisibleException e)
                {
                    saved_ = false;
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
                returnState_ = state_;
                cancelFlag_ = true;
                return;
            }

            currentFilename_ = filename + LEVEL_EXTENSION;
        }

        private void findStorageDevice(IAsyncResult result)
        {
            StorageDevice storageDevice = Guide.EndShowStorageDeviceSelector(result);
            if (storageDevice != null)
            {
                if ((string)result.AsyncState == "saveRequest")
                    saveGame(storageDevice);
            }
            else
            {
                returnState_ = state_;
                cancelFlag_ = true;
            }
        }

        private void saveGame(StorageDevice storageDevice)
        {
            StorageContainer container = storageDevice.OpenContainer(CONTAINER_NAME);
            string directory = Path.Combine(container.Path, DIRECTORY_NAME);
            System.IO.Directory.CreateDirectory(directory);
            string filename = Path.Combine(directory, currentFilename_);
            state_.myLevel_.writeLevelToFile(filename);
            container.Dispose();

            returnState_ = new EngineStateMenu(engine_);
        }

        public void draw()
        {
            engine_.GraphicsDevice.Clear(Color.Black);

            mainMessage_.drawStringCentered(MESSAGE, MESSAGE_POSITION, MESSAGE_COLOR, 0, MESSAGE_DEPTH);

            Vector2 fileNamePos = MESSAGE_POSITION + FILENAME_OFFSET;
            mainMessage_.drawStringCentered(currentFilename_, fileNamePos, FILENAME_COLOR, 0, MESSAGE_DEPTH);
        }

    }
}
