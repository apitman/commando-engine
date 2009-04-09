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
        /// <param name="pipeline">Drawing pipeline in which to register the object</param>
        /// <param name="tiles">Array of ints representing types of tiles</param>
        /// <returns>List of TileObjects</returns>
/*        public static List<TileObject> getTiles(List<DrawableObjectAbstract> pipeline, int[,] tiles)
        {
            List<TileObject> retList = new List<TileObject>();
            for (int y = 0; y < tiles.GetLength(0); y++)
            {
                for (int x = 0; x < tiles.GetLength(1); x++)
                {
                    retList.Add(new TileObject(tiles[y, x], pipeline, TextureMap.getInstance().getTexture("Tile_" + tiles[y, x]), new Vector2((float)x * tileSideLength_, (float)y * tileSideLength_), Vector2.Zero, 0.0f));
                }
            }
            return retList;
        }
 */

        public static List<BoxObject> mergeBoxes(Tile[,] boxes)
        {
            //List<List<BoxObject>> tempMergedBoxes = new List<List<BoxObject>>();
            List<List<int[]>> tempMergedDimsHigh = new List<List<int[]>>();
            List<List<int[]>> tempMergedDimsLow = new List<List<int[]>>();
            List<Vector2> tempPoints = new List<Vector2>();
            tempPoints.Add(new Vector2(-7.5f, -7.5f));
            tempPoints.Add(new Vector2(7.5f, -7.5f));
            tempPoints.Add(new Vector2(7.5f, 7.5f));
            tempPoints.Add(new Vector2(-7.5f, 7.5f));
            int[] tempSetHigh = new int[4];
            int[] tempSetLow = new int[4];
            bool startedHigh = false;
            bool startedLow = false;
            List<BoxObject> returnBoxes = new List<BoxObject>();
            for (int y = 0; y < boxes.GetLength(0); y++)
            {
                tempMergedDimsHigh.Add(new List<int[]>());
                tempMergedDimsLow.Add(new List<int[]>());
                for (int x = 0; x < boxes.GetLength(1); x++)
                {
                    if (!startedHigh && boxes[y,x].highDistance_ == 0)
                    {
                        startedHigh = true;
                        tempSetHigh[0] = x;
                        tempSetHigh[1] = y;
                        tempSetHigh[2] = x;
                        tempSetHigh[3] = y;
                    }
                    else if (startedHigh && boxes[y,x].highDistance_ == 0)
                    {
                        tempSetHigh[2] = x;
                    }
                    else if (startedHigh && !(boxes[y,x].highDistance_ == 0))
                    {
                        tempMergedDimsHigh[y].Add(tempSetHigh);
                        tempSetHigh = new int[4];
                        startedHigh = false;
                    }
                    if (!startedLow && boxes[y, x].lowDistance_ == 0)
                    {
                        startedLow = true;
                        tempSetLow[0] = x;
                        tempSetLow[1] = y;
                        tempSetLow[2] = x;
                        tempSetLow[3] = y;
                    }
                    else if (startedLow && boxes[y, x].lowDistance_ == 0)
                    {
                        tempSetLow[2] = x;
                    }
                    else if (startedLow && !(boxes[y, x].lowDistance_ == 0))
                    {
                        tempMergedDimsLow[y].Add(tempSetLow);
                        tempSetLow = new int[4];
                        startedLow = false;
                    }
                }
                if (startedHigh)
                {
                    tempMergedDimsHigh[y].Add(tempSetHigh);
                    tempSetHigh = new int[4];
                    startedHigh = false;
                }
                if (startedLow)
                {
                    tempMergedDimsLow[y].Add(tempSetLow);
                    tempSetLow = new int[4];
                    startedLow = false;
                }
            }
            for (int i = 0; i < boxes.GetLength(0); i++)
            {
                for (int j = tempMergedDimsHigh[i].Count - 1; j >= 0; j--)
                {
                    bool finished = false;
                    int[] tempCheckingSet = tempMergedDimsHigh[i][j];
                    tempMergedDimsHigh[i].RemoveAt(j);
                    for (int k = i + 1; k < boxes.GetLength(0); k++)
                    {
                        finished = true;
                        for (int l = tempMergedDimsHigh[k].Count - 1; l >= 0; l--)
                        {
                            int[] temp = tempMergedDimsHigh[k][l];
                            if (temp[0] == tempCheckingSet[0] && temp[2] == tempCheckingSet[2])
                            {
                                tempCheckingSet[3] = temp[3];
                                finished = false;
                                tempMergedDimsHigh[k].RemoveAt(l);
                                break;
                            }
                        }
                        if (finished)
                        {
                            break;
                        }
                    }
                    returnBoxes.Add(createBox(tempCheckingSet[0], tempCheckingSet[1], tempCheckingSet[2], tempCheckingSet[3], HeightEnum.HIGH));
                }
                for (int j = tempMergedDimsLow[i].Count - 1; j >= 0; j--)
                {
                    bool finished = false;
                    int[] tempCheckingSet = tempMergedDimsLow[i][j];
                    tempMergedDimsLow[i].RemoveAt(j);
                    for (int k = i + 1; k < boxes.GetLength(0); k++)
                    {
                        finished = true;
                        for (int l = tempMergedDimsLow[k].Count - 1; l >= 0; l--)
                        {
                            int[] temp = tempMergedDimsLow[k][l];
                            if (temp[0] == tempCheckingSet[0] && temp[2] == tempCheckingSet[2])
                            {
                                tempCheckingSet[3] = temp[3];
                                finished = false;
                                tempMergedDimsLow[k].RemoveAt(l);
                                break;
                            }
                        }
                        if (finished)
                        {
                            break;
                        }
                    }
                    returnBoxes.Add(createBox(tempCheckingSet[0], tempCheckingSet[1], tempCheckingSet[2], tempCheckingSet[3], HeightEnum.LOW));
                }
            }
            return returnBoxes;

        }

        public static BoxObject createBox(int x1, int y1, int x2, int y2, HeightEnum height)
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
            BoxObject temp = new BoxObject(points, center, Height.getHeight(height));
            temp.getBounds(height).rotate(new Vector2(1.0f, 0.0f), center);
            return temp;
        }
    }
}
