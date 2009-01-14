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
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace Commando
{
    class GameTexture
    {
        //Texture image for this GameTexture
        protected Texture2D texture_;

        //Each Vector4 is the bounds (x of top right corner, y of top right corner,
        //width, height) of each individual frame, or image in this texture
        protected Vector4[] imageDimensions_;

        public GameTexture()
        {
        }

        public GameTexture(string filename)
        {
        }

        public Texture2D getTexture()
        {

            return null;
        }

        public void setTexture(Texture2D tex)
        {
        }

        public Vector4[] getImageDimensions()
        {

            return new Vector4[1];
        }

        public void setImageDimensions(Vector4[] dims)
        {
        }

        //preconditions: texture_ and imageDimensions_ are not NULL
        //postconditions: the specified image is drawn to the screen at origin with the
        //  specified rotation and depth.
        public void drawImage(uint imageNumber, Vector2 origin, float rotation, float depth)
        {
        }
    }
}
