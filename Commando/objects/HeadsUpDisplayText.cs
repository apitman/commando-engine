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

namespace Commando.objects
{
    public class HeadsUpDisplayText : CharacterStatusObserverInterface
    {
        protected int newValue_;
        protected FontEnum font_;
        protected Vector2 pos_;
        protected float depth_;

        public HeadsUpDisplayText()
        {
            font_ = FontEnum.Kootenay;
            newValue_ = 20;
        }

        public HeadsUpDisplayText(Vector2 pos, float depth, FontEnum font)
        {
            pos_ = pos;
            depth_ = depth;
            font_ = font;
            newValue_ = 20;
        }

        /// <summary>
        /// Notifies the observer that the value has changed
        /// </summary>
        /// <param name="value">The value to change to</param>
        public void notifyOfChange(int value)
        {
            newValue_ = value;
        }

        public void drawString(string text, string textToReplace, Color color, float rotation)
        {
            FontMap.getInstance().getFont(font_).drawStringCentered(text.Replace(textToReplace, newValue_.ToString()), pos_, color, rotation, depth_);
        }
    }
}
