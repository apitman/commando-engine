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
using Commando.levels;

namespace Commando.collisiondetection
{
    public class ComplexConvexPolygon : ConvexPolygonInterface
    {
        protected ConvexPolygonInterface lowPolygon_;

        protected ConvexPolygonInterface highPolygon_;

        public ComplexConvexPolygon(ConvexPolygonInterface low, ConvexPolygonInterface high)
        {
            lowPolygon_ = low;
            highPolygon_ = high;
        }


        public Vector2 getCenter()
        {
            return lowPolygon_.getCenter();
        }

        public Vector2 getEdge(int edgeNumber)
        {
            throw new NotImplementedException();
        }

        public int getNumberOfPoints()
        {
            throw new NotImplementedException();
        }

        public Vector2 getPoint(int index)
        {
            throw new NotImplementedException();
        }

        public Vector2[] getPoints()
        {
            throw new NotImplementedException();
        }

        public void projectPolygonOnAxis(Vector2 axis, Height height, ref float min, ref float max)
        {
            throw new NotImplementedException();
        }

        public void rotate(Vector2 newAxis, Vector2 position)
        {
            throw new NotImplementedException();
        }

        public void translate(Vector2 translation)
        {
            throw new NotImplementedException();
        }
    }
}
