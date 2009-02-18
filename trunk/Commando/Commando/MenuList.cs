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

        const FontEnum DEFAULT_FONT = FontEnum.Kootenay;
        const float DEFAULT_ROTATION = 0.0f;
        const float DEFAULT_SCALE = 1.0f;
        const float DEFAULT_DEPTH = Constants.DEPTH_MENU_TEXT;
        const SpriteEffects DEFAULT_SPRITE_EFFECTS = SpriteEffects.None;

        //list of strings used to make each menu item
        //each string in the list makes up a different line
        protected List<string> stringList_;

        //position of the first menu item
        protected Vector2 listPos_;

        //color of an unselected menu item
        protected Color baseColor_;

        //color of a selected menu item
        protected Color selectedColor_;

        //position of the selected menu item
        protected int cursorPos_;

        //rotation value of individual items
        protected float rotation_;

        //scale of printed font with respect to original font
        protected float scale_;

        //any sprite effects for the menu
        protected SpriteEffects spriteEffects_;

        //layer depth of menu
        protected float layerDepth_;

        //space between each line in the menu
        protected float spacing_;

        //font to display menu
        protected GameFont font_;

        /// <summary>
        /// Creates a menuList object 
        /// </summary>
        /// <param name="stringList">list of menu items</param>
        /// <param name="font">font to display menu</param>
        /// <param name="pos">position of menu</param>
        /// <param name="baseColor">color of unselected item</param>
        /// <param name="selectedColor">color of selected item</param>
        /// <param name="cursorPos">initial cursor position</param>
        /// <param name="scale">scale of printed font with respect to original font</param>
        /// <param name="spacing">space between each line in the menu</param>
        public MenuList(List<string> stringList, FontEnum font, Vector2 pos, Color baseColor, Color selectedColor, int cursorPos, float spacing)
        {
            stringList_ = stringList;
            font_ = FontMap.getInstance().getFont(font);
            listPos_ = pos;
            baseColor_ = baseColor;
            selectedColor_ = selectedColor;
            cursorPos_ = cursorPos;
            rotation_ = DEFAULT_ROTATION;
            scale_ = DEFAULT_SCALE;
            spriteEffects_ = DEFAULT_SPRITE_EFFECTS;
            layerDepth_ = DEFAULT_DEPTH;
            spacing_ = spacing;
        }

        /// <summary>
        /// Creates a menuList object 
        /// </summary>
        /// <param name="stringList">list of menu items</param>
        /// <param name="pos">position of </param>
        /// <param name="baseColor">color of unselected item</param>
        /// <param name="selectedColor">color of selected item</param>
        /// <param name="cursorPos">initial cursor position</param>
        /// <param name="rotation">rotation value of individual items</param>
        /// <param name="scale">scale of printed font with respect to original font</param>
        /// <param name="effects">any sprite effects for the menu</param>
        /// <param name="layerDepth">layer depth of menu</param>
        /// <param name="spacing">space between each line in the menu</param>
        public MenuList(List<string> stringList, Vector2 pos, Color baseColor, Color selectedColor, int cursorPos, float rotation, float scale, SpriteEffects effects, float layerDepth, float spacing)
        {
            stringList_ = stringList;
            font_ = FontMap.getInstance().getFont(DEFAULT_FONT);
            listPos_ = pos;
            baseColor_ = baseColor;
            selectedColor_ = selectedColor;
            cursorPos_ = cursorPos;
            rotation_ = rotation;
            scale_ = scale;
            spriteEffects_ = effects;
            layerDepth_ = layerDepth;
            spacing_ = spacing;
        }

        #region FontList Members

        /// <summary>
        /// draws each item in the menu spacing_ points apart from each other
        /// currently center justifies each item in the menu
        /// if the current menu item is selected, it will be drawn with selectedColor_
        /// </summary>
        public void draw()
        {
            int listLength = stringList_.Count;
            Vector2 curPos = listPos_;
            Color myColor;
            for (int i = 0; i < listLength; i++)
            {
                if (i == cursorPos_)
                {
                    myColor = selectedColor_;
                }
                else
                {
                    myColor = baseColor_;
                }
                string curString = stringList_[i];

                font_.drawStringCentered(curString,
                                          curPos,
                                          myColor,
                                          rotation_,
                                          scale_,
                                          spriteEffects_,
                                          layerDepth_);
                curPos.Y = curPos.Y + spacing_;
            }
        }

        public int getCursorPos()
        {
            return cursorPos_;
        }

        public void setCursorPos(int newPos)
        {
            cursorPos_ = newPos;
        }

        /// <summary>
        /// increments cursor position with wraparound
        /// </summary>
        public void incrementCursorPos()
        {
            cursorPos_++;
            cursorPos_ = cursorPos_ % stringList_.Count;
        }

        /// <summary>
        /// decrements cursor position with wraparound
        /// </summary>
        public void decrementCursorPos()
        {
            cursorPos_--;
            if (cursorPos_ < 0) 
            {
                cursorPos_ = stringList_.Count - 1;
            }
        }

        public void setString(int index, string replacement)
        {
            stringList_[index] = replacement;
        }

        #endregion

    }
}
    