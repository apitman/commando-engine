﻿/*
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
using Commando.levels;

namespace Commando.collisiondetection
{
    public class CircularConvexPolygon : ConvexPolygonInterface
    {
        protected float radius_;

        protected Vector2 center_;

        public CircularConvexPolygon(float radius, Vector2 position)
        {
            center_ = position;
            radius_ = radius;
        }

        public Vector2 getCenter()
        {
            return center_;
        }

        public Vector2 getEdge(int edgeNumber)
        {
            return Vector2.Zero;
        }

        public int getNumberOfPoints()
        {
            return 0;
        }

        public Vector2 getPoint(int index)
        {
            return Vector2.Zero;
        }

        public Vector2[] getPoints()
        {
            return null;
        }

        public void projectPolygonOnAxis(Vector2 axis, Height height, ref float min, ref float max)
        {
            axis.Normalize();
            min = ConvexPolygon.dotProduct(axis, center_);
            max = min + radius_;
            min -= radius_;
            if (min > max)
            {
                float temp = min;
                min = max;
                max = temp;
            }
        }

        public void rotate(Vector2 newAxis, Vector2 position)
        {
            center_ = position;
        }

        public void translate(Vector2 translation)
        {
            center_ += translation;
        }

        public float getRadius()
        {
            return radius_;
        }
    }
}
