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
using Commando.objects;
using Microsoft.Xna.Framework;

namespace Commando.levels
{
    /// <summary>
    /// Class for generating a list of Tiles from an array of ints.
    /// </summary>
    static class Tiler
    {
        public const int tileSideLength_ = 15;

        /// <summary>
        /// Generate a list of TileObjects from a two dimensional array of ints, where each 
        /// number in the array represents the type of tile to use at that cooridinate.
        /// </summary>
        /// <param name="tiles">Array of ints representing types of tiles</param>
        /// <returns>List of TileObjects</returns>
        public static List<TileObject> getTiles(int[,] tiles)
        {
            List<TileObject> retList = new List<TileObject>();
            for (int y = 0; y < tiles.GetLength(0); y++)
            {
                for (int x = 0; x < tiles.GetLength(1); x++)
                {
                    retList.Add(new TileObject(TextureMap.getInstance().getTexture("Tile_" + tiles[y, x]), new Vector2((float)x * tileSideLength_, (float)y * tileSideLength_), Vector2.Zero, 0.0f));
                }
            }
            return retList;
        }
    }
}
