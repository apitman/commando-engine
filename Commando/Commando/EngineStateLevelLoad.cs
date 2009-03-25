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
using Microsoft.Xna.Framework.Storage;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Commando.controls;
using System.Xml;
#if !XBOX
using System.Windows.Forms;
#endif
namespace Commando
{
    public class EngineStateLevelLoad : EngineStateInterface
    {
        protected const string PATH_TO_DEFAULT_LEVEL = @".\Content\XML\defaultlevel.xml";

        protected const string INSTRUCTIONS_MESSAGE = "Please select a level to load: ";
        protected const float INSTRUCTIONS_DEPTH = Constants.DEPTH_MENU_TEXT;
        protected const FontEnum INSTRUCTIONS_FONT = FontEnum.Kootenay;
        protected static readonly Color INSTRUCTIONS_COLOR = Color.Yellow;
        protected Vector2 INSTRUCTIONS_POSITION
        {
            get
            {
                Rectangle r = engine_.GraphicsDevice.Viewport.TitleSafeArea;
                return new Vector2(r.X + r.Width / 2, r.Y + 50.0f);
            }

            set
            {
                throw new NotImplementedException();
            }
        }
        protected Vector2 MENU_POSITION
        {
            get
            {
                Rectangle r = engine_.GraphicsDevice.Viewport.TitleSafeArea;
                return new Vector2(r.X + r.Width / 2, r.Y + 150.0f);
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        protected Engine engine_;
        protected MenuList menuList_;
        protected StorageContainer container_;
        protected EngineStateTarget target_;
        protected List<string> filepaths_;
        protected GameFont instructions_;
        protected bool cancelFlag_;
        protected string windowsFileName_;
        protected bool windows_;
        protected EngineStateInterface returnState_;
        protected List<string> fileList_;

        public enum EngineStateTarget
        {
            GAMEPLAY,
            LEVEL_EDITOR,
            LEVEL_TRANSITION
        }

        public EngineStateLevelLoad(Engine engine, EngineStateTarget target, EngineStateInterface returnState)
        {
            engine_ = engine;
            target_ = target;
            returnState_ = returnState;
            instructions_ = FontMap.getInstance().getFont(INSTRUCTIONS_FONT);
            cancelFlag_ = false;

            StorageDevice sd = Settings.getInstance().StorageDevice_;
            if (sd != null)
            {
                loadList(sd);
            }
            else
            {
#if !XBOX
                OpenFileDialog dialog = new OpenFileDialog();
                //dialog.InitialDirectory = EngineStateLevelSave.DIRECTORY_NAME;
                dialog.Filter = "Commando Level Files (*" + EngineStateLevelSave.LEVEL_EXTENSION + ")|*" + EngineStateLevelSave.LEVEL_EXTENSION;
                dialog.Multiselect = false;
                InputSet.getInstance().setToggle(InputsEnum.CONFIRM_BUTTON);
                InputSet.getInstance().setToggle(InputsEnum.RIGHT_TRIGGER);
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    windowsFileName_ = dialog.FileName;
                }
                else
                {
                    cancelFlag_ = true;
                }
                windows_ = true;
#endif
            }
        }

        private void loadList(StorageDevice storageDevice)
        {
            container_ = storageDevice.OpenContainer(EngineStateLevelSave.CONTAINER_NAME);
            string directory = Path.Combine(container_.Path, EngineStateLevelSave.DIRECTORY_NAME);
            Directory.CreateDirectory(directory);
            string[] files = Directory.GetFiles(directory);

            filepaths_ = new List<string>();
            fileList_ = new List<string>();
            for (int i = 0; i < files.Length; i++)
            {
                if (Path.GetExtension(files[i]) == EngineStateLevelSave.LEVEL_EXTENSION)
                {
                    filepaths_.Add(files[i]);
                    fileList_.Add(Path.GetFileNameWithoutExtension(files[i]));
                }
            }

            // if there are no levels found, store the default level
            if (filepaths_.Count == 0)
            {
                XmlTextReader reader = new XmlTextReader(PATH_TO_DEFAULT_LEVEL);
                XmlDocument document = new XmlDocument();
                document.Load(reader);
                document.Save(Path.Combine(directory, "defaultlevel" + EngineStateLevelSave.LEVEL_EXTENSION));
                files = Directory.GetFiles(directory);

                // TODO Extract this C&P'd code into function
                for (int i = 0; i < files.Length; i++)
                {
                    if (Path.GetExtension(files[i]) == EngineStateLevelSave.LEVEL_EXTENSION)
                    {
                        filepaths_.Add(files[i]);
                        fileList_.Add(Path.GetFileNameWithoutExtension(files[i]));
                    }
                }
            }
            menuList_ = new MenuList(fileList_, MENU_POSITION);
        }

        public EngineStateInterface update(GameTime gameTime)
        {
            if (cancelFlag_)
            {
                return new EngineStateMenu(engine_);
            }

            if (windows_)
            {
                switch (target_)
                {
                    case EngineStateTarget.GAMEPLAY:
                        return new EngineStateGameplay(engine_, windowsFileName_, null);
                        break;
                    case EngineStateTarget.LEVEL_EDITOR:
                        return new EngineStateLevelEditor(engine_, this, windowsFileName_, null);
                        break;
                }
            }

            InputSet inputs = InputSet.getInstance();
            if (inputs.getButton(InputsEnum.CONFIRM_BUTTON) ||
                inputs.getButton(InputsEnum.BUTTON_1))
            {
                inputs.setToggle(InputsEnum.CONFIRM_BUTTON);
                inputs.setToggle(InputsEnum.BUTTON_1);

                int cursorPos = menuList_.getCursorPos();
                string filepath = filepaths_[cursorPos];
                switch (target_)
                {
                    case EngineStateTarget.GAMEPLAY:
                        return new EngineStateGameplay(engine_, filepath, container_);
                        break;
                    case EngineStateTarget.LEVEL_EDITOR:
                        return new EngineStateLevelEditor(engine_, this, filepath, container_);
                        break;
                    case EngineStateTarget.LEVEL_TRANSITION:
                        {
                            EngineStateLevelEditor myState = returnState_ as EngineStateLevelEditor;
                            myState.setTransLevel(fileList_[cursorPos]);
                            return returnState_;
                            break;
                        }
                }
                return new EngineStateGameplay(engine_, filepath, container_);
            }
            if (inputs.getLeftDirectionalY() > 0)
            {
                inputs.setToggle(InputsEnum.LEFT_DIRECTIONAL);
                menuList_.decrementCursorPos();
            }
            if (inputs.getLeftDirectionalY() < 0)
            {
                inputs.setToggle(InputsEnum.LEFT_DIRECTIONAL);
                menuList_.incrementCursorPos();
            }
            if (inputs.getButton(InputsEnum.CANCEL_BUTTON) ||
                inputs.getButton(InputsEnum.BUTTON_2))
            {
                inputs.setToggle(InputsEnum.CANCEL_BUTTON);
                inputs.setToggle(InputsEnum.BUTTON_2);
                return new EngineStateMenu(engine_);
            }

            return this;
        }

        public void draw()
        {
            engine_.GraphicsDevice.Clear(Color.Black);

            if (menuList_ != null)
                menuList_.draw();

            instructions_.drawStringCentered(
                INSTRUCTIONS_MESSAGE,
                INSTRUCTIONS_POSITION,
                INSTRUCTIONS_COLOR,
                0,
                INSTRUCTIONS_DEPTH);
        }
    }
}
