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
    public class TileGrid
    {
        public const int TILEWIDTH = 15;

        public const int TILEHEIGHT = 15;

        protected Tile[,] tiles_;

        public Tile[,] getTiles()
        {
            return tiles_;
        }

        public static TileIndex getTileIndex(Vector2 position)
        {
            return new TileIndex((int)position.X % TILEWIDTH, (int)position.Y % TILEHEIGHT);
        }
    }

    public struct Tile
    {
        public TileHeight tileHeight_;
    }

    public enum TileHeight
    {
        GROUND,
        LOW,
        HIGH
    }

    public struct TileIndex
    {
        public int x_;
        public int y_;

        public TileIndex(int x, int y)
        {
            x_ = x;
            y_ = y;
        }

        public static bool equals(TileIndex lhs, TileIndex rhs)
        {
            return lhs.x_ == rhs.x_ && lhs.y_ == rhs.y_;
        }
    }
}
