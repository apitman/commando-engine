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
    public class MenuList
    {
        protected List<string> stringList_;

        protected Vector2 listPos_;
        protected Color baseColor_;
        protected Color selectedColor_;
        protected int cursorPos_;
        protected float rotation_;
        protected float scale_;
        protected SpriteEffects spriteEffects_;
        protected float layerDepth_;
        protected float spacing_;
      
        public MenuList(List<string> stringList, Vector2 pos, Color baseColor, Color selectedColor, int cursorPos, float rotation, float scale, SpriteEffects effects, float layerDepth, float spacing)
        {

            stringList_ = new List<string>();
            stringList_ = stringList;
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

        public void draw()
        {
            GameFont myFont = FontMap.getInstance().getFont(FontEnum.Kootenay);
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

                myFont.drawStringCentered(curString,
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
        //increments cursor position with wraparound
        public void incrementCursorPos()
        {
            cursorPos_++;
            cursorPos_ = cursorPos_ % stringList_.Count;
        }
        //decrements cursor position with wraparount
        public void decremnentCursorPos()
        {
            cursorPos_--;
            if (cursorPos_ < 0) 
            {
                cursorPos_ = stringList_.Count - 1;
            }
        }
        #endregion
    }
}
    