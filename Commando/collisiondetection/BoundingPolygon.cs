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

namespace Commando.collisiondetection
{
    public class BoundingPolygon
    {
        protected List<BoundingLine> lines_;

        public BoundingPolygon(List<Vector2> points)
        {
            lines_ = new List<BoundingLine>();
            if (points.Count > 2)
            {
                lines_.Add(new BoundingLine(points[0], points[points.Count - 1]));
            }
            for (int i = 1; i < points.Count; i++)
            {
                lines_.Add(new BoundingLine(points[i - 1], points[i]));
            }
        }

        /*
        public bool checkCollision(Point center, float radius)
        {
            for (int i = 0; i < lines_.Count; i++)
            {
                if (lines_[i].intersect(center, radius))
                {
                    return true;
                }
            }
            return false;
        }
        */
        public Vector2 checkCollision(Vector2 center, float radius)
        {
            for (int i = 0; i < lines_.Count; i++)
            {
                center = lines_[i].checkCollision(center, radius);
            }
            return center;
        }
    }
}
