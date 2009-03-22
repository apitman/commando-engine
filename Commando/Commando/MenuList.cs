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
using Commando.controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Commando
{
    /// <summary>
    /// Creates a Menu made of different items, 
    /// holds a cursor position for item selection
    /// highlights a selected item
    /// </summary>
    public class MenuList
    {
        readonly Color DEFAULT_BASE_COLOR = Color.Green;
        readonly Color DEFAULT_SELECTED_COLOR = Color.White;
        const float DEFAULT_SPACING = 40.0f;
        const FontEnum DEFAULT_FONT = FontEnum.Kootenay;
        const float DEFAULT_ROTATION = 0.0f;
        const float DEFAULT_SCALE = 1.0f;
        const float DEFAULT_DEPTH = Constants.DEPTH_MENU_TEXT;
        const SpriteEffects DEFAULT_SPRITE_EFFECTS = SpriteEffects.None;

        //list of strings used to make each menu item
        //each string in the list makes up a different line
        public List<string> StringList_ { get; private set; }

        //position of the first menu item
        public Vector2 Position_ { get; set; }

        //color of an unselected menu item
        public Color BaseColor_ { get; set; }

        //color of a selected menu item
        public Color SelectedColor_ { get; set; }

        //position of the selected menu item
        public int CursorPos_ { get; set; }

        //rotation value of individual items
        public float Rotation_ { get; set; }

        //scale of printed font with respect to original font
        public float Scale_ { get; set; }

        //any sprite effects for the menu
        public SpriteEffects SpriteEffects_ { get; set; }

        //layer depth of menu
        public float LayerDepth_ { get; set; }

        //space between each line in the menu
        public float Spacing_ { get; set; }

        //font to display menu
        public FontEnum Font_
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                font_ = FontMap.getInstance().getFont(value);
            }
        }
        protected GameFont font_;

        protected void setDefaults()
        {
            this.CursorPos_ = 0;
            this.BaseColor_ = DEFAULT_BASE_COLOR;
            this.SelectedColor_ = DEFAULT_SELECTED_COLOR;
            this.Scale_ = DEFAULT_SCALE;
            this.Spacing_ = DEFAULT_SPACING;
            this.SpriteEffects_ = DEFAULT_SPRITE_EFFECTS;
            this.LayerDepth_ = DEFAULT_DEPTH;
            this.Font_ = DEFAULT_FONT;
        }

        public MenuList(List<string> stringList, Vector2 position)
        {
            setDefaults();
            this.StringList_ = stringList;
            this.Position_ = position;
        }

        #region FontList Members

        /// <summary>
        /// draws each item in the menu spacing_ points apart from each other
        /// currently center justifies each item in the menu
        /// if the current menu item is selected, it will be drawn with selectedColor_
        /// </summary>
        public void draw()
        {
            int listLength = StringList_.Count;
            Vector2 curPos = Position_;
            Color myColor;
            for (int i = 0; i < listLength; i++)
            {
                if (i == CursorPos_)
                {
                    myColor = SelectedColor_;
                }
                else
                {
                    myColor = BaseColor_;
                }
                string curString = StringList_[i];

                font_.drawStringCentered(curString,
                                          curPos,
                                          myColor,
                                          Rotation_,
                                          Scale_,
                                          SpriteEffects_,
                                          LayerDepth_);
                curPos.Y = curPos.Y + Spacing_;
            }
        }

        public int getCursorPos()
        {
            return CursorPos_;
        }

        public void setCursorPos(int newPos)
        {
            CursorPos_ = newPos;
        }

        /// <summary>
        /// increments cursor position with wraparound
        /// </summary>
        public void incrementCursorPos()
        {
            CursorPos_++;
            CursorPos_ = CursorPos_ % StringList_.Count;
        }

        /// <summary>
        /// decrements cursor position with wraparound
        /// </summary>
        public void decrementCursorPos()
        {
            CursorPos_--;
            if (CursorPos_ < 0) 
            {
                CursorPos_ = StringList_.Count - 1;
            }
        }

        public string getCurrentString()
        {
            return StringList_[CursorPos_];
        }

        public void setString(int index, string replacement)
        {
            StringList_[index] = replacement;
        }

        #endregion

    }
}
    