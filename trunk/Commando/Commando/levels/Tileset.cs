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

namespace Commando.levels
{
    /// <summary>
    /// Data structure for a tileset read in from an XML file; maps tile index
    /// numbers to pictures and tile information such as heights.
    /// </summary>
    public class Tileset
    {
        protected Dictionary<int, Height> heights_ { get; private set;}
        protected Dictionary<int, string> textures_{ get; private set;}

        public static Tileset constructTileset(string filepath)
        {
            throw new NotImplementedException("Tileset class not ready");
        }

        internal void test()
        {
            int totalTiles = 15;
            int tilesWide = 5;
            int tilesHigh = 3;

            int width = 15;
            int height = 15;

            Rectangle[] imageDimensions = new Rectangle[totalTiles];

            for (int i = 0; i < totalTiles; i++)
            {
                int row = i / tilesWide;
                int col = (i - row * tilesWide);
                imageDimensions[i].X = row * (width + 1);
                imageDimensions[i].Y = col * (height + 1);
                imageDimensions[i].Width = width;
                imageDimensions[i].Height = height;
            }

            /*
            GameTexture texture = new GameTexture(
                filepath,
                spriteBatch,
                graphics,
                imageDimensions);
             */
        }
    }
}
