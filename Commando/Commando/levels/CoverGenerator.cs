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
    class CoverGenerator
    {
        protected const float DIAGONAL_ADDITION = 0.412f;

        protected static Tile[,] newTiles_;

        protected static Tile[,] oldTiles_;

        public static List<CoverObject> generateCoverObjects(Tile[,] tiles)
        {
            List<CoverObject> returnList = new List<CoverObject>();
            newTiles_ = tiles;

            for (int i = 0; i < newTiles_.GetLength(0); i++)
            {
                for (int j = 0; j < newTiles_.GetLength(1); j++)
                {
                    //Are we on an edge
                    if (get(j, i, HeightEnum.LOW) == 1f && get(j, i, HeightEnum.HIGH) > 0f)
                    {
                        // We're checking to make sure that this is the case:
                        //     >0  |       |   >0
                        //     ------------------
                        //     >0  |   j,i | 0, low only
                        //     ------------------
                        //         |   >0  | 0, low only
                        //
                        if (((get(j - 1, i - 1, HeightEnum.LOW) > 0f //Upper Left is not a wall
                                && get(j + 1, i - 1, HeightEnum.LOW) > 0f) //Upper Right not a wall
                            || ((get(j - 1, i - 1, HeightEnum.LOW) == 0f //Upper Left is a wall
                                && get(j + 1, i - 1, HeightEnum.LOW) == 0f) //Upper Right is a wall   
                                && get(j, i - 1, HeightEnum.LOW) == 0f) //Top is a wall
                            )
                            && get(j - 1, i, HeightEnum.LOW) > 0f //Left is not a wall
                            && get(j, i + 1, HeightEnum.LOW) > 0f //Below is not a wall
                            && get(j + 1, i, HeightEnum.LOW) == 0f //Right is a wall
                            && get(j + 1, i + 1, HeightEnum.LOW) == 0f //Lower Right is a wall
                            && get(j + 1, i, HeightEnum.HIGH) > 0f //Right is not blocked high
                            && get(j + 1, i + 1, HeightEnum.HIGH) > 0f //Lower Right is not blocked high
                            )
                        {
                            //We are now sure that we need to start cover going down
                            CoverObject temp = genVerticalCoverObjectLeft(j, i);
                            if (temp != null)
                            {
                                returnList.Add(temp);
                            }
                        }
                        // We're checking to make sure that this is the case:
                        //     >0  |       |   >0
                        //     ------------------
                        //  0, low |   j,i |   >0
                        //     ------------------
                        //  0, low |   >0  | 
                        //
                        else if (get(j - 1, i - 1, HeightEnum.LOW) > 0f //Upper Left is not a wall
                            && get(j + 1, i - 1, HeightEnum.LOW) > 0f //Upper Right not a wall
                            && get(j + 1, i, HeightEnum.LOW) > 0f //Right is not a wall
                            && get(j, i + 1, HeightEnum.LOW) > 0f //Below is not a wall
                            && get(j - 1, i, HeightEnum.LOW) == 0f //Left is a wall
                            && get(j - 1, i + 1, HeightEnum.LOW) == 0f //Lower Left is a wall
                            && get(j - 1, i, HeightEnum.HIGH) > 0f //Left is not blocked high
                            && get(j - 1, i + 1, HeightEnum.HIGH) > 0f //Lower Left is not blocked high
                            )
                        {
                            //We are now sure that we need to start cover going down
                            CoverObject temp = genVerticalCoverObjectRight(j, i);
                            if (temp != null)
                            {
                                returnList.Add(temp);
                            }
                        }


                        // We're checking to make sure that this is the case:
                        //     >0  | 0, l  | 0, low only
                        //     ------------------
                        //         |   j,i | >0
                        //     ------------------
                        //     >0  |   >0  | 
                        //
                        if (((get(j - 1, i - 1, HeightEnum.LOW) > 0f //Upper Left is not a wall
                                && get(j - 1, i + 1, HeightEnum.LOW) > 0f) //Lower Left not a wall
                            ||  ((get(j - 1, i + 1, HeightEnum.LOW) == 0f //Lower Left is a wall
                                && get(j - 1, i, HeightEnum.LOW) == 0f) //Left is a wall   
                                && get(j - 1, i - 1, HeightEnum.LOW) == 0f) //Upper Left is a wall
                            )
                            && get(j, i + 1, HeightEnum.LOW) > 0f //Bottom is not a wall
                            && get(j + 1, i, HeightEnum.LOW) > 0f //Right is not a wall
                            && get(j, i - 1, HeightEnum.LOW) == 0f //Top is a wall
                            && get(j + 1, i - 1, HeightEnum.LOW) == 0f //Upper Right is a wall
                            && get(j, i - 1, HeightEnum.HIGH) > 0f //Top is not blocked high
                            && get(j + 1, i - 1, HeightEnum.HIGH) > 0f //Upper Right is not blocked high
                            )
                        {
                            //We are now sure that we need to start cover going right
                            CoverObject temp = genHorizontalCoverObjectBottom(j, i);
                            if (temp != null)
                            {
                                returnList.Add(temp);
                            }
                        }
                        // We're checking to make sure that this is the case:
                        //     >0  |  >0   |
                        //     ------------------
                        //         |   j,i | >0
                        //     ------------------
                        //     >0  | 0, l  | 0, low only
                        //
                        else if (((get(j - 1, i - 1, HeightEnum.LOW) > 0f //Upper Left is not a wall
                                    && get(j - 1, i + 1, HeightEnum.LOW) > 0f) //Lower Left not a wall
                                || ((get(j - 1, i + 1, HeightEnum.LOW) == 0f //Lower Left is a wall
                                    && get(j - 1, i, HeightEnum.LOW) == 0f) //Left is a wall   
                                    && get(j - 1, i - 1, HeightEnum.LOW) == 0f) //Upper Left is a wall
                                )
                                && get(j, i - 1, HeightEnum.LOW) > 0f //Top is not a wall
                                && get(j + 1, i, HeightEnum.LOW) > 0f //Right is not a wall
                                && get(j, i + 1, HeightEnum.LOW) == 0f //Bottom is a wall
                                && get(j + 1, i + 1, HeightEnum.LOW) == 0f //Lower Right is a wall
                                && get(j, i + 1, HeightEnum.HIGH) > 0f //Bottom is not blocked high
                                && get(j + 1, i + 1, HeightEnum.HIGH) > 0f //Lower Right is not blocked high
                                )
                        {
                            //We are now sure that we need to start cover going right
                            CoverObject temp = genHorizontalCoverObjectTop(j, i);
                            if (temp != null)
                            {
                                returnList.Add(temp);
                            }
                        }
                    }
                }
            }
            return returnList;
        }

        protected static CoverObject genVerticalCoverObjectLeft(int j, int i)
        {
            float top = (float)i * 15f;
            float bottom = (float)i * 15f;
            float rightEdge = (float)(j + 1) * 15f;
            float leftEdge = (float)(j - 1) * 15f;
            // We're checking to make sure that this is the case:
            //         |       |
            //     ------------------
            //     >0  |   j,i | 0
            //     ------------------
            //         |       | 
            //
            while (get(j, i, HeightEnum.LOW) == 1f
                    && get(j, i, HeightEnum.HIGH) > 0f
                    && get(j + 1, i, HeightEnum.LOW) == 0f
                    && get(j + 1, i, HeightEnum.HIGH) > 0f
                    && get(j - 1, i, HeightEnum.LOW) > 0f
                    )
            {
                bottom += 15f;
                i++;
            }
            if (bottom - top < 45f)
            {
                return null;
            }
            Vector2 position = new Vector2((rightEdge + leftEdge) / 2f, (bottom + top) / 2f);
            List<Vector2> bounds = new List<Vector2>();
            bounds.Add(new Vector2(leftEdge - position.X, top - position.Y));
            bounds.Add(new Vector2(rightEdge - position.X, top - position.Y));
            bounds.Add(new Vector2(rightEdge - position.X, bottom - position.Y));
            bounds.Add(new Vector2(leftEdge - position.X, bottom - position.Y));
            return new CoverObject(null, bounds, position, new Vector2(rightEdge, top), new Vector2(rightEdge, bottom));
        }

        protected static CoverObject genVerticalCoverObjectRight(int j, int i)
        {
            float top = (float)i * 15f;
            float bottom = (float)i * 15f;
            float rightEdge = (float)(j + 2) * 15f;
            float leftEdge = (float)j * 15f;
            // We're checking to make sure that this is the case:
            //         |       |
            //     ------------------
            //     0   |   j,i | >0
            //     ------------------
            //         |       | 
            //
            while (get(j, i, HeightEnum.LOW) == 1f
                    && get(j, i, HeightEnum.HIGH) > 0f
                    && get(j - 1, i, HeightEnum.LOW) == 0f
                    && get(j - 1, i, HeightEnum.HIGH) > 0f
                    && get(j + 1, i, HeightEnum.LOW) > 0f
                    )
            {
                bottom += 15f;
                i++;
            }
            if (bottom - top < 45f)
            {
                return null;
            }
            Vector2 position = new Vector2((rightEdge + leftEdge) / 2f, (bottom + top) / 2f);
            List<Vector2> bounds = new List<Vector2>();
            bounds.Add(new Vector2(leftEdge - position.X, top - position.Y));
            bounds.Add(new Vector2(rightEdge - position.X, top - position.Y));
            bounds.Add(new Vector2(rightEdge - position.X, bottom - position.Y));
            bounds.Add(new Vector2(leftEdge - position.X, bottom - position.Y));
            return new CoverObject(null, bounds, position, new Vector2(leftEdge, top), new Vector2(leftEdge, bottom));
        }

        protected static CoverObject genHorizontalCoverObjectTop(int j, int i)
        {
            float topEdge = (float)(i - 1) * 15f;
            float bottomEdge = (float)(i + 1) * 15f;
            float right = (float)j * 15f;
            float left = (float)j * 15f;
            // We're checking to make sure that this is the case:
            //         |   >0  |
            //     ------------------
            //         |   j,i | 
            //     ------------------
            //         | 0, l  | 
            //
            while (get(j, i, HeightEnum.LOW) == 1f
                    && get(j, i, HeightEnum.HIGH) > 0f
                    && get(j, i + 1, HeightEnum.LOW) == 0f
                    && get(j, i + 1, HeightEnum.HIGH) > 0f
                    && get(j, i - 1, HeightEnum.LOW) > 0f
                    )
            {
                right += 15f;
                j++;
            }
            if (right - left < 45f)
            {
                return null;
            }
            Vector2 position = new Vector2((right + left) / 2f, (bottomEdge + topEdge) / 2f);
            List<Vector2> bounds = new List<Vector2>();
            bounds.Add(new Vector2(left - position.X, topEdge - position.Y));
            bounds.Add(new Vector2(right - position.X, topEdge - position.Y));
            bounds.Add(new Vector2(right - position.X, bottomEdge - position.Y));
            bounds.Add(new Vector2(left - position.X, bottomEdge - position.Y));
            return new CoverObject(null, bounds, position, new Vector2(left, bottomEdge), new Vector2(right, bottomEdge));
        }

        protected static CoverObject genHorizontalCoverObjectBottom(int j, int i)
        {
            float topEdge = (float)i * 15f;
            float bottomEdge = (float)(i + 2) * 15f;
            float right = (float)j * 15f;
            float left = (float)j * 15f;
            // We're checking to make sure that this is the case:
            //         |   0   |
            //     ------------------
            //         |   j,i | 
            //     ------------------
            //         |   >0  | 
            //
            while (get(j, i, HeightEnum.LOW) == 1f
                    && get(j, i, HeightEnum.HIGH) > 0f
                    && get(j, i - 1, HeightEnum.LOW) == 0f
                    && get(j, i - 1, HeightEnum.HIGH) > 0f
                    && get(j, i + 1, HeightEnum.LOW) > 0f
                    )
            {
                right += 15f;
                j++;
            }
            if (right - left < 45f)
            {
                return null;
            }
            Vector2 position = new Vector2((right + left) / 2f, (bottomEdge + topEdge) / 2f);
            List<Vector2> bounds = new List<Vector2>();
            bounds.Add(new Vector2(left - position.X, topEdge - position.Y));
            bounds.Add(new Vector2(right - position.X, topEdge - position.Y));
            bounds.Add(new Vector2(right - position.X, bottomEdge - position.Y));
            bounds.Add(new Vector2(left - position.X, bottomEdge - position.Y));
            return new CoverObject(null, bounds, position, new Vector2(left, topEdge), new Vector2(right, topEdge));
        }
        /*
         * public static List<CoverObject> generateCoverObjects(Tile[,] tiles)
        {
            List<CoverObject> returnList = new List<CoverObject>();
            newTiles_ = tiles;

            for (int i = 0; i < newTiles_.GetLength(0); i++)
            {
                for (int j = 0; j < newTiles_.GetLength(1); j++)
                {
                    //Are we on an edge
                    if (get(j, i, HeightEnum.LOW) == 0.5f && get(j, i, HeightEnum.HIGH) > 0f)
                    {
                        /// We're checking to make sure that this is the case:
                        ///     >0  |       |   >0
                        ///     ------------------
                        ///     >0  |   j,i | 0, low only
                        ///     ------------------
                        ///         |   >0  | 0, low only
                        ///
                        if (((get(j - 1, i - 1, HeightEnum.LOW) > 0f //Upper Left is not a wall
                                && get(j + 1, i - 1, HeightEnum.LOW) > 0f) //Upper Right not a wall
                            || ((get(j - 1, i - 1, HeightEnum.LOW) == 0f //Upper Left is a wall
                                && get(j + 1, i - 1, HeightEnum.LOW) == 0f) //Upper Right is a wall   
                                && get(j, i - 1, HeightEnum.LOW) == 0f) //Top is a wall
                            )
                            && get(j - 1, i, HeightEnum.LOW) > 0f //Left is not a wall
                            && get(j, i + 1, HeightEnum.LOW) > 0f //Below is not a wall
                            && get(j + 1, i, HeightEnum.LOW) == 0f //Right is a wall
                            && get(j + 1, i + 1, HeightEnum.LOW) == 0f //Lower Right is a wall
                            && get(j + 1, i, HeightEnum.HIGH) > 0f //Right is not blocked high
                            && get(j + 1, i + 1, HeightEnum.HIGH) > 0f //Lower Right is not blocked high
                            )
                        {
                            ///We are now sure that we need to start cover going down
                            CoverObject temp = genVerticalCoverObjectLeft(j, i);
                            if (temp != null)
                            {
                                returnList.Add(temp);
                            }
                        }
                        /// We're checking to make sure that this is the case:
                        ///     >0  |       |   >0
                        ///     ------------------
                        ///  0, low |   j,i |   >0
                        ///     ------------------
                        ///  0, low |   >0  | 
                        ///
                        else if (get(j - 1, i - 1, HeightEnum.LOW) > 0f //Upper Left is not a wall
                            && get(j + 1, i - 1, HeightEnum.LOW) > 0f //Upper Right not a wall
                            && get(j + 1, i, HeightEnum.LOW) > 0f //Right is not a wall
                            && get(j, i + 1, HeightEnum.LOW) > 0f //Below is not a wall
                            && get(j - 1, i, HeightEnum.LOW) == 0f //Left is a wall
                            && get(j - 1, i + 1, HeightEnum.LOW) == 0f //Lower Left is a wall
                            && get(j - 1, i, HeightEnum.HIGH) > 0f //Left is not blocked high
                            && get(j - 1, i + 1, HeightEnum.HIGH) > 0f //Lower Left is not blocked high
                            )
                        {
                            ///We are now sure that we need to start cover going down
                            CoverObject temp = genVerticalCoverObjectRight(j, i);
                            if (temp != null)
                            {
                                returnList.Add(temp);
                            }
                        }


                        /// We're checking to make sure that this is the case:
                        ///     >0  | 0, l  | 0, low only
                        ///     ------------------
                        ///         |   j,i | >0
                        ///     ------------------
                        ///     >0  |   >0  | 
                        ///
                        if (((get(j - 1, i - 1, HeightEnum.LOW) > 0f //Upper Left is not a wall
                                && get(j - 1, i + 1, HeightEnum.LOW) > 0f) //Lower Left not a wall
                            ||  ((get(j - 1, i + 1, HeightEnum.LOW) == 0f //Lower Left is a wall
                                && get(j - 1, i, HeightEnum.LOW) == 0f) //Left is a wall   
                                && get(j - 1, i - 1, HeightEnum.LOW) == 0f) //Upper Left is a wall
                            )
                            && get(j, i + 1, HeightEnum.LOW) > 0f //Bottom is not a wall
                            && get(j + 1, i, HeightEnum.LOW) > 0f //Right is not a wall
                            && get(j, i - 1, HeightEnum.LOW) == 0f //Top is a wall
                            && get(j + 1, i - 1, HeightEnum.LOW) == 0f //Upper Right is a wall
                            && get(j, i - 1, HeightEnum.HIGH) > 0f //Top is not blocked high
                            && get(j + 1, i - 1, HeightEnum.HIGH) > 0f //Upper Right is not blocked high
                            )
                        {
                            ///We are now sure that we need to start cover going right
                            CoverObject temp = genHorizontalCoverObjectBottom(j, i);
                            if (temp != null)
                            {
                                returnList.Add(temp);
                            }
                        }
                        /// We're checking to make sure that this is the case:
                        ///     >0  |  >0   |
                        ///     ------------------
                        ///         |   j,i | >0
                        ///     ------------------
                        ///     >0  | 0, l  | 0, low only
                        ///
                        else if (((get(j - 1, i - 1, HeightEnum.LOW) > 0f //Upper Left is not a wall
                                    && get(j - 1, i + 1, HeightEnum.LOW) > 0f) //Lower Left not a wall
                                || ((get(j - 1, i + 1, HeightEnum.LOW) == 0f //Lower Left is a wall
                                    && get(j - 1, i, HeightEnum.LOW) == 0f) //Left is a wall   
                                    && get(j - 1, i - 1, HeightEnum.LOW) == 0f) //Upper Left is a wall
                                )
                                && get(j, i - 1, HeightEnum.LOW) > 0f //Top is not a wall
                                && get(j + 1, i, HeightEnum.LOW) > 0f //Right is not a wall
                                && get(j, i + 1, HeightEnum.LOW) == 0f //Bottom is a wall
                                && get(j + 1, i + 1, HeightEnum.LOW) == 0f //Lower Right is a wall
                                && get(j, i + 1, HeightEnum.HIGH) > 0f //Bottom is not blocked high
                                && get(j + 1, i + 1, HeightEnum.HIGH) > 0f //Lower Right is not blocked high
                                )
                        {
                            ///We are now sure that we need to start cover going right
                            CoverObject temp = genHorizontalCoverObjectTop(j, i);
                            if (temp != null)
                            {
                                returnList.Add(temp);
                            }
                        }
                    }
                }
            }
            return returnList;
        }

        protected static CoverObject genVerticalCoverObjectLeft(int j, int i)
        {
            float top = (float)i * 15f;
            float bottom = (float)i * 15f;
            float rightEdge = (float)(j + 1) * 15f;
            float leftEdge = (float)(j - 1) * 15f;
            /// We're checking to make sure that this is the case:
            ///         |       |
            ///     ------------------
            ///     >0  |   j,i | 0
            ///     ------------------
            ///         |       | 
            ///
            while (get(j, i, HeightEnum.LOW) == 0.5f
                    && get(j, i, HeightEnum.HIGH) > 0f
                    && get(j + 1, i, HeightEnum.LOW) == 0f
                    && get(j + 1, i, HeightEnum.HIGH) > 0f
                    && get(j - 1, i, HeightEnum.LOW) > 0f
                    )
            {
                bottom += 15f;
                i++;
            }
            if (bottom - top < 45f)
            {
                return null;
            }
            Vector2 position = new Vector2((rightEdge + leftEdge) / 2f, (bottom + top) / 2f);
            List<Vector2> bounds = new List<Vector2>();
            bounds.Add(new Vector2(leftEdge - position.X, top - position.Y));
            bounds.Add(new Vector2(rightEdge - position.X, top - position.Y));
            bounds.Add(new Vector2(rightEdge - position.X, bottom - position.Y));
            bounds.Add(new Vector2(leftEdge - position.X, bottom - position.Y));
            return new CoverObject(null, bounds, position, new Vector2(rightEdge, top), new Vector2(rightEdge, bottom));
        }

        protected static CoverObject genVerticalCoverObjectRight(int j, int i)
        {
            float top = (float)i * 15f;
            float bottom = (float)i * 15f;
            float rightEdge = (float)(j + 2) * 15f;
            float leftEdge = (float)j * 15f;
            /// We're checking to make sure that this is the case:
            ///         |       |
            ///     ------------------
            ///     0   |   j,i | >0
            ///     ------------------
            ///         |       | 
            ///
            while (get(j, i, HeightEnum.LOW) == 0.5f
                    && get(j, i, HeightEnum.HIGH) > 0f
                    && get(j - 1, i, HeightEnum.LOW) == 0f
                    && get(j - 1, i, HeightEnum.HIGH) > 0f
                    && get(j + 1, i, HeightEnum.LOW) > 0f
                    )
            {
                bottom += 15f;
                i++;
            }
            if (bottom - top < 45f)
            {
                return null;
            }
            Vector2 position = new Vector2((rightEdge + leftEdge) / 2f, (bottom + top) / 2f);
            List<Vector2> bounds = new List<Vector2>();
            bounds.Add(new Vector2(leftEdge - position.X, top - position.Y));
            bounds.Add(new Vector2(rightEdge - position.X, top - position.Y));
            bounds.Add(new Vector2(rightEdge - position.X, bottom - position.Y));
            bounds.Add(new Vector2(leftEdge - position.X, bottom - position.Y));
            return new CoverObject(null, bounds, position, new Vector2(leftEdge, top), new Vector2(leftEdge, bottom));
        }

        protected static CoverObject genHorizontalCoverObjectTop(int j, int i)
        {
            float topEdge = (float)(i - 1) * 15f;
            float bottomEdge = (float)(i + 1) * 15f;
            float right = (float)j * 15f;
            float left = (float)j * 15f;
            /// We're checking to make sure that this is the case:
            ///         |   >0  |
            ///     ------------------
            ///         |   j,i | 
            ///     ------------------
            ///         | 0, l  | 
            ///
            while (get(j, i, HeightEnum.LOW) == 0.5f
                    && get(j, i, HeightEnum.HIGH) > 0f
                    && get(j, i + 1, HeightEnum.LOW) == 0f
                    && get(j, i + 1, HeightEnum.HIGH) > 0f
                    && get(j, i - 1, HeightEnum.LOW) > 0f
                    )
            {
                right += 15f;
                j++;
            }
            if (right - left < 45f)
            {
                return null;
            }
            Vector2 position = new Vector2((right + left) / 2f, (bottomEdge + topEdge) / 2f);
            List<Vector2> bounds = new List<Vector2>();
            bounds.Add(new Vector2(left - position.X, topEdge - position.Y));
            bounds.Add(new Vector2(right - position.X, topEdge - position.Y));
            bounds.Add(new Vector2(right - position.X, bottomEdge - position.Y));
            bounds.Add(new Vector2(left - position.X, bottomEdge - position.Y));
            return new CoverObject(null, bounds, position, new Vector2(left, topEdge), new Vector2(right, topEdge));
        }

        protected static CoverObject genHorizontalCoverObjectBottom(int j, int i)
        {
            float topEdge = (float)i * 15f;
            float bottomEdge = (float)(i + 2) * 15f;
            float right = (float)j * 15f;
            float left = (float)j * 15f;
            /// We're checking to make sure that this is the case:
            ///         |   0   |
            ///     ------------------
            ///         |   j,i | 
            ///     ------------------
            ///         |   >0  | 
            ///
            while (get(j, i, HeightEnum.LOW) == 0.5f
                    && get(j, i, HeightEnum.HIGH) > 0f
                    && get(j, i - 1, HeightEnum.LOW) == 0f
                    && get(j, i - 1, HeightEnum.HIGH) > 0f
                    && get(j, i + 1, HeightEnum.LOW) > 0f
                    )
            {
                right += 15f;
                j++;
            }
            if (right - left < 45f)
            {
                return null;
            }
            Vector2 position = new Vector2((right + left) / 2f, (bottomEdge + topEdge) / 2f);
            List<Vector2> bounds = new List<Vector2>();
            bounds.Add(new Vector2(left - position.X, topEdge - position.Y));
            bounds.Add(new Vector2(right - position.X, topEdge - position.Y));
            bounds.Add(new Vector2(right - position.X, bottomEdge - position.Y));
            bounds.Add(new Vector2(left - position.X, bottomEdge - position.Y));
            return new CoverObject(null, bounds, position, new Vector2(left, bottomEdge), new Vector2(right, bottomEdge));
        }

        */
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
            //if (newTiles_[y, x].highDistance_ == 1f)
            //{
            //    newTiles_[y, x].highDistance_ = 0.5f;
            //}
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
            //if (newTiles_[y, x].lowDistance_ == 1f)
            //{
            //    newTiles_[y, x].lowDistance_ = 0.5f;
            //}
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
