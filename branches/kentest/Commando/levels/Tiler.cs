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
    public static class Tiler
    {
        public const int tileSideLength_ = 15;

        /// <summary>
        /// Generate a list of TileObjects from a two dimensional array of ints, where each 
        /// number in the array represents the type of tile to use at that cooridinate.
        /// </summary>
        /// <param name="tiles">Array of ints representing types of tiles</param>
        /// <returns>List of TileObjects</returns>
        public static List<TileObject> getTiles(List<DrawableObjectAbstract> pipeline, int[,] tiles)
        {
            List<TileObject> retList = new List<TileObject>();
            for (int y = 0; y < tiles.GetLength(0); y++)
            {
                for (int x = 0; x < tiles.GetLength(1); x++)
                {
                    retList.Add(new TileObject(pipeline, TextureMap.getInstance().getTexture("Tile_" + tiles[y, x]), new Vector2((float)x * tileSideLength_, (float)y * tileSideLength_), Vector2.Zero, 0.0f));
                }
            }
            return retList;
        }

        public static List<BoxObject> mergeBoxes(Tile[,] boxes)
        {
            //List<List<BoxObject>> tempMergedBoxes = new List<List<BoxObject>>();
            List<List<int[]>> tempMergedDims = new List<List<int[]>>();
            List<Vector2> tempPoints = new List<Vector2>();
            tempPoints.Add(new Vector2(-7.5f, -7.5f));
            tempPoints.Add(new Vector2(7.5f, -7.5f));
            tempPoints.Add(new Vector2(7.5f, 7.5f));
            tempPoints.Add(new Vector2(-7.5f, 7.5f));
            int[] tempSet = new int[4];
            bool started = false;
            List<BoxObject> returnBoxes = new List<BoxObject>();
            for (int y = 0; y < boxes.GetLength(0); y++)
            {
                tempMergedDims.Add(new List<int[]>());
                for (int x = 0; x < boxes.GetLength(1); x++)
                {
                    if (!started && boxes[y,x].blocksHigh_)
                    {
                        started = true;
                        tempSet[0] = x;
                        tempSet[1] = y;
                        tempSet[2] = x;
                        tempSet[3] = y;
                    }
                    else if (started && boxes[y,x].blocksHigh_)
                    {
                        tempSet[2] = x;
                    }
                    else if (started && !boxes[y,x].blocksHigh_)
                    {
                        tempMergedDims[y].Add(tempSet);
                        tempSet = new int[4];
                        started = false;
                    }
                }
                if (started)
                {
                    tempMergedDims[y].Add(tempSet);
                    tempSet = new int[4];
                    started = false;
                }
            }
            for (int i = 0; i < boxes.GetLength(0); i++)
            {
                for (int j = tempMergedDims[i].Count - 1; j >= 0; j--)
                {
                    bool finished = false;
                    int[] tempCheckingSet = tempMergedDims[i][j];
                    tempMergedDims[i].RemoveAt(j);
                    for (int k = i + 1; k < boxes.GetLength(0); k++)
                    {
                        finished = true;
                        for (int l = tempMergedDims[k].Count - 1; l >= 0; l--)
                        {
                            int[] temp = tempMergedDims[k][l];
                            if (temp[0] == tempCheckingSet[0] && temp[2] == tempCheckingSet[2])
                            {
                                tempCheckingSet[3] = temp[3];
                                finished = false;
                                tempMergedDims[k].RemoveAt(l);
                                break;
                            }
                        }
                        if (finished)
                        {
                            break;
                        }
                    }
                    returnBoxes.Add(createBox(tempCheckingSet[0], tempCheckingSet[1], tempCheckingSet[2], tempCheckingSet[3]));
                }
            }
            return returnBoxes;

        }

        public static BoxObject createBox(int x1, int y1, int x2, int y2)
        {
            float x1f = (float)x1 * 15f;
            float x2f = (float)(x2 + 1) * 15f;
            float y1f = (float)y1 * 15f;
            float y2f = (float)(y2 + 1) * 15f;
            Vector2 center = new Vector2((x1f + x2f) / 2f, (y1f + y2f) / 2f);
            List<Vector2> points = new List<Vector2>();
            points.Add(new Vector2(center.X - x1f, center.Y - y1f));
            points.Add(new Vector2(center.X - x2f, center.Y - y1f));
            points.Add(new Vector2(center.X - x2f, center.Y - y2f));
            points.Add(new Vector2(center.X - x1f, center.Y - y2f));
            BoxObject temp = new BoxObject(points, center);
            temp.getBounds().rotate(new Vector2(1.0f, 0.0f), center);
            return temp;
        }
    }
}
