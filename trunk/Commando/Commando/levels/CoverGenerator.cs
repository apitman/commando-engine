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

namespace Commando.levels
{
    class CoverGenerator
    {
        protected const float DIAGONAL_ADDITION = 0.412f;

        protected static Tile[,] newTiles_;

        protected static Tile[,] oldTiles_;

        public static Tile[,] generateRealTileDistances(Tile[,] tiles)
        {
            oldTiles_ = tiles;
            newTiles_ = new Tile[tiles.GetLength(0), tiles.GetLength(1)];
            initialize();
            for (int i = 0; i < newTiles_.GetLength(0); i++)
            {
                for (int j = 0; j < newTiles_.GetLength(1); j++)
                {
                    set(j, i, HeightEnum.HIGH);
                    set(j, i, HeightEnum.LOW);
                }
            }
            return newTiles_;
        }

        protected static void initialize()
        {
            for (int i = 0; i < newTiles_.GetLength(0); i++)
            {
                for (int j = 0; j < newTiles_.GetLength(1); j++)
                {
                    newTiles_[i, j].highDistance_ = float.PositiveInfinity - 1f;
                    newTiles_[i, j].lowDistance_ = float.PositiveInfinity - 1f;
                }
            }
        }

        protected static void set(int x, int y, HeightEnum height)
        {
            if (height == HeightEnum.HIGH)
            {
                setHigh(x, y);
            }
            else
            {
                setLow(x, y);
            }
        }

        protected static void setHigh(int x, int y)
        {
            if (x < 0 || x >= newTiles_.GetLength(1) ||
                y < 0 || y >= newTiles_.GetLength(0))
            {
                return;
            }
            float oldVal = get(x, y, HeightEnum.HIGH);
            if (oldTiles_[y, x].highDistance_ > 0f)
            {
                float a, b, c, d, e, f, g, h;
                a = get(x - 1, y - 1, HeightEnum.HIGH) + DIAGONAL_ADDITION;
                c = get(x + 1, y - 1, HeightEnum.HIGH) + DIAGONAL_ADDITION;
                f = get(x - 1, y + 1, HeightEnum.HIGH) + DIAGONAL_ADDITION;
                h = get(x + 1, y + 1, HeightEnum.HIGH) + DIAGONAL_ADDITION;
                b = get(x, y - 1, HeightEnum.HIGH);
                d = get(x - 1, y, HeightEnum.HIGH);
                e = get(x + 1, y, HeightEnum.HIGH);
                g = get(x, y + 1, HeightEnum.HIGH);
                newTiles_[y, x].highDistance_ = 1f + Math.Min(
                                                        Math.Min(
                                                            Math.Min(a, b),
                                                            Math.Min(c, d)),
                                                        Math.Min(
                                                            Math.Min(e, f),
                                                            Math.Min(g, h)
                                                        )
                                                    );
            }
            else
            {
                newTiles_[y, x].highDistance_ = 0f;
            }
            if (get(x, y, HeightEnum.HIGH) + 0.1 < oldVal)
            {
                setHigh(x - 1, y - 1);
                setHigh(x, y - 1);
                setHigh(x + 1, y - 1);
                setHigh(x - 1, y);
            }
        }

        protected static void setLow(int x, int y)
        {
            if (x < 0 || x >= newTiles_.GetLength(1) ||
                y < 0 || y >= newTiles_.GetLength(0))
            {
                return;
            }
            float oldVal = get(x, y, HeightEnum.LOW);
            if (oldTiles_[y, x].lowDistance_ > 0f)
            {
                float a, b, c, d, e, f, g, h;
                a = get(x - 1, y - 1, HeightEnum.LOW) + DIAGONAL_ADDITION;
                c = get(x + 1, y - 1, HeightEnum.LOW) + DIAGONAL_ADDITION;
                f = get(x - 1, y + 1, HeightEnum.LOW) + DIAGONAL_ADDITION;
                h = get(x + 1, y + 1, HeightEnum.LOW) + DIAGONAL_ADDITION;
                b = get(x, y - 1, HeightEnum.LOW);
                d = get(x - 1, y, HeightEnum.LOW);
                e = get(x + 1, y, HeightEnum.LOW);
                g = get(x, y + 1, HeightEnum.LOW);
                newTiles_[y, x].lowDistance_ = 1f + Math.Min(
                                                        Math.Min(
                                                            Math.Min(a, b), 
                                                            Math.Min(c, d)),
                                                        Math.Min(
                                                            Math.Min(e, f), 
                                                            Math.Min(g, h)
                                                        )
                                                    );
            }
            else
            {
                newTiles_[y, x].lowDistance_ = 0f;
            }
            if (get(x, y, HeightEnum.LOW) + 0.1 < oldVal)
            {
                setLow(x - 1, y - 1);
                setLow(x, y - 1);
                setLow(x + 1, y - 1);
                setLow(x - 1, y);
            }
        }

        protected static float get(int x, int y, HeightEnum height)
        {
            if (x >= 0 && x < newTiles_.GetLength(1) 
                && y >= 0 && y < newTiles_.GetLength(0))
            {
                if (height == HeightEnum.HIGH)
                {
                    return newTiles_[y, x].highDistance_;
                }
                return newTiles_[y, x].lowDistance_;
            }
            return 0f;
        }
    }
}
