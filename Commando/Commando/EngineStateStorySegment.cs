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
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Commando.controls;
using Commando.levels;
using Commando.objects;
using Microsoft.Xna.Framework.Storage;
using System.IO;
using Commando.objects.enemies;
using Microsoft.Xna.Framework.Input;
using Commando.graphics.multithreading;

namespace Commando
{

    /// <summary>
    /// The main class for story slides
    /// </summary>
    public class EngineStateStorySegment : EngineStateInterface
    {
        protected const int FRAMERATE = 30;

        /// <summary>
        /// In seconds
        /// </summary>
        public int DurationOfStory_ { get; set; }

        protected Engine engine_;

        protected EngineStateInterface nextState_;

        protected int framesSpentInStory_;

        protected string storyText_;

        public EngineStateStorySegment(Engine engine, EngineStateInterface nextState)
        {
            engine_ = engine;
            nextState_ = nextState;
            framesSpentInStory_ = 0;
            DurationOfStory_ = 1;
            storyText_ = "You hear the cold, metallic clunks of robot feet walking\n toward you, but you must keep moving forward.";
        }

        public EngineStateStorySegment(Engine engine, EngineStateInterface nextState, int durationOfStorySegment)
        {
            engine_ = engine;
            nextState_ = nextState;
            framesSpentInStory_ = 0;
            DurationOfStory_ = durationOfStorySegment;
            storyText_ = "You have many questions. The answers lie just around the corner.";
        }

        public EngineStateStorySegment(Engine engine, EngineStateInterface nextState, int durationOfStorySegment, string storyText)
        {
            engine_ = engine;
            nextState_ = nextState;
            framesSpentInStory_ = 0;
            DurationOfStory_ = durationOfStorySegment;
            storyText_ = storyText;
        }

        public EngineStateInterface update(GameTime gameTime)
        {
            framesSpentInStory_++;
            if (framesSpentInStory_ > DurationOfStory_ * FRAMERATE)
            {
                return nextState_;
            }
            else if (InputSet.getInstance().getButton(InputsEnum.BUTTON_2))
            {
                InputSet.getInstance().setToggle(InputsEnum.BUTTON_2);
                return nextState_;
            }
            else if (InputSet.getInstance().getButton(InputsEnum.CANCEL_BUTTON))
            {
                InputSet.getInstance().setToggle(InputsEnum.CANCEL_BUTTON);
                return nextState_;
            }
            else if (InputSet.getInstance().getButton(InputsEnum.CONFIRM_BUTTON))
            {
                InputSet.getInstance().setToggle(InputsEnum.CONFIRM_BUTTON);
                return nextState_;
            }
            
            return this;
        }

        public void draw()
        {
            //engine_.GraphicsDevice.Clear(Color.GhostWhite);
            DrawBuffer.getInstance().getUpdateStack().ScreenClearColor_ = Color.GhostWhite;

            Rectangle r = engine_.GraphicsDevice.Viewport.TitleSafeArea;
            Vector2 pos = new Vector2(r.Left + r.Width/2, r.Top + r.Height/2);
            FontMap.getInstance().getFont(FontEnum.Kootenay14).drawStringCentered(storyText_, pos, Color.Black, 0.0f, Constants.DEPTH_HUD_TEXT);
            pos += new Vector2(0.0f, (r.Bottom - pos.Y) / 2);
            FontMap.getInstance().getFont(FontEnum.Pericles).drawStringCentered((framesSpentInStory_ / FRAMERATE).ToString() + "/" + DurationOfStory_.ToString(), pos, Color.Black, 0.0f, Constants.DEPTH_HUD_TEXT);
        }
    }
}
