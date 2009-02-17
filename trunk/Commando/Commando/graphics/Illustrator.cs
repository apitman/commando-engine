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

namespace Commando.graphics
{
    public static class Illustrator
    {
        public static GameTexture blank_ = null;

        public static void drawLine(Vector2 point1, Vector2 point2)
        {
            if (blank_ == null)
            {
                init();
            }
            Vector2 center = point1;
            center.X += point2.X;
            center.Y += point2.Y;
            center.X /= 2.0f;
            center.Y /= 2.0f;
            Vector2 rotation = point2 - point1;
            float rotationAngle = (float)Math.Atan2((double)rotation.Y, (double)rotation.X);
            blank_.drawImageWithDim(0, new Rectangle((int)point1.X, (int)point1.Y, (int)rotation.Length(), 2), rotationAngle, 1f, Vector2.Zero, Color.LimeGreen);
        }

        public static void init()
        {
            blank_ = TextureMap.getInstance().getTexture("blank");
        }
    }
}
