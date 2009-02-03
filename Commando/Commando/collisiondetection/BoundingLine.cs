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
    public class BoundingLine
    {
        protected Point begin_;
        protected Point end_;
        protected bool vertical_;

        public BoundingLine(Point p1, Point p2)
        {
            if (p1.X == p2.X)
            {
                if (p1.Y > p2.Y)
                {
                    begin_ = p1;
                    end_ = p2;
                }
                else
                {
                    begin_ = p2;
                    end_ = p1;
                }
                vertical_ = true;
            }
            else
            {
                if (p1.X > p2.X)
                {
                    begin_ = p1;
                    end_ = p2;
                }
                else
                {
                    begin_ = p2;
                    end_ = p1;
                }
                vertical_ = false;
            }
        }

        public bool intersect(Point center, float radius)
        {
            if (center.X <= begin_.X && center.X >= end_.X)
            {
                if (Math.Min(Math.Abs(center.Y - begin_.Y), Math.Abs(center.Y - end_.Y)) < radius)
                {
                    return true;
                }
            }
            else if (center.Y <= begin_.Y && center.Y >= end_.Y)
            {
                if (Math.Min(Math.Abs(center.X - begin_.X), Math.Abs(center.X - end_.X)) < radius)
                {
                    return true;
                }
            }
            else if (distance(begin_, center) < radius || distance(end_, center) < radius)
            {
                return true;
            }
            return false;
        }
        
        public Point checkCollision(Point center, float radius)
        {
            if (center.X <= begin_.X && center.X >= end_.X)
            {
                if (Math.Abs(center.Y - begin_.Y) < radius)
                {
                    center.Y = begin_.Y + (int)Math.Round((float)Math.Sign(center.Y - begin_.Y) * radius);
                    return center;
                }
                if (Math.Abs(center.Y - end_.Y) < radius)
                {
                    center.Y = end_.Y + (int)Math.Round((float)Math.Sign(center.Y - end_.Y) * radius);
                    return center;
                }
            }
            else if (center.Y <= begin_.Y && center.Y >= end_.Y)
            {
                if (Math.Abs(center.X - begin_.X) < radius)
                {
                    center.X = begin_.X + (int)Math.Round((float)Math.Sign(center.X - begin_.X) * radius);
                    return center;
                }
                if (Math.Abs(center.X - end_.X) < radius)
                {
                    center.X = end_.X + (int)Math.Round((float)Math.Sign(center.X - end_.X) * radius);
                    return center;
                }
            }
            else if (distance(begin_, center) < radius)
            {
                if (vertical_)
                {
                    center.Y = begin_.Y + (int)Math.Round(Math.Sqrt(Math.Pow(radius, 2.0) - Math.Pow(begin_.X - center.X, 2.0)));
                    return center;
                }
                else
                {
                    center.X = begin_.X + (int)Math.Round(Math.Sqrt(Math.Pow(radius, 2.0) - Math.Pow(begin_.Y - center.Y, 2.0)));
                    return center;
                }
            }
            else if (distance(end_, center) < radius)
            {
                if (vertical_)
                {
                    center.Y = end_.Y - (int)Math.Round(Math.Sqrt(Math.Pow(radius, 2.0) - Math.Pow(end_.X - center.X, 2.0)));
                    return center;
                }
                else
                {
                    center.X = end_.X - (int)Math.Round(Math.Sqrt(Math.Pow(radius, 2.0) - Math.Pow(end_.Y - center.Y, 2.0)));
                    return center;
                }
            }
            return center;
        }

        protected float distance(Point p1, Point p2)
        {
            return (float)(Math.Sqrt(Math.Pow((double)(p1.X - p2.X), 2.0) + Math.Pow((double)(p1.Y - p2.Y), 2.0)));
        }
    }
}
