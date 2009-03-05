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
        static readonly Tile IMPASSABLE = new Tile(0, 0);

        public const int TILEWIDTH = 15;

        public const int TILEHEIGHT = 15;

        public Tile[,] tiles_;

        protected int width_;

        protected int height_;

        public TileGrid(Tile[,] tiles)
        {
            tiles_ = tiles;
            width_ = tiles.GetLength(1);
            height_ = tiles.GetLength(0);
        }

        public Tile[,] getTiles()
        {
            return tiles_;
        }

        public Tile getTile(TileIndex index)
        {
            if (index.x_ >= width_ || index.y_ >= height_ || index.x_ < 0 || index.y_ < 0)
            {
                return IMPASSABLE;
            }
            return tiles_[index.y_, index.x_]; // TILES IS Y, X
        }

        public Tile getTile(int x, int y)
        {
            if (x >= width_ || y >= height_ || x < 0 || y < 0)
            {
                return IMPASSABLE;
            }
            return tiles_[y, x]; // TILES IS Y, X
        }

        public Tile getTile(Vector2 position)
        {
            TileIndex ti = getTileIndex(position);
            return getTile(ti);
        }

        public bool isPointWithinTile(Vector2 position, TileIndex tile)
        {
            TileIndex actual = getTileIndex(position);
            return TileIndex.equals(actual, tile);
        }

        public TileIndex getTileIndex(Vector2 position)
        {
            return new TileIndex((short)(position.X / TILEWIDTH), (short)(position.Y / TILEHEIGHT));
        }

        public Vector2 getTileCenter(TileIndex tile)
        {
            float x = tile.x_ * TILEWIDTH + TILEWIDTH / 2;
            float y = tile.y_ * TILEHEIGHT + TILEHEIGHT / 2;
            return new Vector2(x, y);
        }
    }

    public struct Tile
    {
        public Tile(int lowDistance, int highDistance)
        {
            lowDistance_ = lowDistance;
            highDistance_ = highDistance;
        }

        public bool collides(Height rhs, float radius)
        {
            bool lowCollision = rhs.blocksLow_ && this.lowDistance_ <= radius;
            bool highCollision = rhs.blocksHigh_ && this.highDistance_ <= radius;
            return (lowCollision || highCollision);
        }

        public int lowDistance_;

        public int highDistance_;
    }

    public struct Height
    {
        public Height(bool blocksLow, bool blocksHigh)
        {
            blocksLow_ = blocksLow;
            blocksHigh_ = blocksHigh;
        }

        public bool collides(Height rhs)
        {
            return (this.blocksLow_ && rhs.blocksLow_ || this.blocksHigh_ && rhs.blocksHigh_);
        }

        public bool blocksLow_;

        public bool blocksHigh_;
    }

    public struct TileIndex
    {
        public short x_;
        public short y_;

        public TileIndex(short x, short y)
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
