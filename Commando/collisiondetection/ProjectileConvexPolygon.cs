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
using Commando.graphics;

namespace Commando.collisiondetection
{
    public class ProjectileConvexPolygon
    {
        protected Vector2[] points_;

        protected Vector2[] edgesNormals_;

        protected Vector2[] original_;

        protected Vector2 center_;

        protected Vector2 curCenter_;

        protected Vector2 curAxis_;

        protected Vector2 previousPosition_;

        protected int numPoints_;

        protected Vector2 curVelocity_;

        public ProjectileConvexPolygon(List<Vector2> points, Vector2 center)
        {
            points_ = points.ToArray();
            original_ = points.ToArray();
            edgesNormals_ = points.ToArray();
            curAxis_ = new Vector2(1.0f, 0.0f);
            center_ = center;
            numPoints_ = original_.Length;
            generateEdgeNormals();
            curVelocity_ = Vector2.Zero;
        }

        public Vector2 getPoint(int index)
        {
            return points_[index];
        }

        public Vector2[] getPoints()
        {
            return points_;
        }

        public int getNumberOfPoints()
        {
            return numPoints_;
        }

        public Vector2 getCenter()
        {
            return curCenter_;
        }

        public Vector2 getEdge(int edgeNumber)
        {
            return edgesNormals_[edgeNumber];
        }

        public void rotate(Vector2 newAxis, Vector2 position)
        {
            curVelocity_ = position - previousPosition_;
            if (newAxis == curAxis_)
            {
                translate(position - curCenter_);
                curCenter_ = previousPosition_ + curVelocity_ / 2f;
                return;
            }
            curCenter_ = previousPosition_ + curVelocity_ / 2f;
            float rotationAngle = getRotationAngle(newAxis);
            float cosA = (float)Math.Cos(rotationAngle);
            float sinA = (float)Math.Sin(rotationAngle);
            float centerX = center_.X;
            float centerY = center_.Y;
            Vector2 curVec;
            Vector2 newVec;
            for (int i = 0; i < numPoints_; i++)
            {
                curVec = original_[i];
                points_[i].X = (curVec.X - centerX) * cosA - (curVec.Y - centerY) * sinA + previousPosition_.X;
                points_[i].Y = (curVec.X - centerX) * sinA + (curVec.Y - centerY) * cosA + previousPosition_.Y;
                if (i > 0)
                {
                    edgesNormals_[i - 1].X = -(points_[i].Y - points_[i - 1].Y);
                    edgesNormals_[i - 1].Y = points_[i].X - points_[i - 1].X;
                }
            }
            edgesNormals_[numPoints_ - 1].X = -(points_[0].Y - points_[numPoints_ - 1].Y);
            edgesNormals_[numPoints_ - 1].Y = points_[0].X - points_[numPoints_ - 1].X;
            curAxis_ = newAxis;
        }

        public void projectPolygonOnAxis(Vector2 axis, ref float min, ref float max)
        {
            float dotProd = dotProduct(axis, points_[0]);
            min = dotProd;
            max = dotProd;

            for (int i = 1; i < numPoints_; i++)
            {
                dotProd = dotProduct(axis, points_[i]);
                if (dotProd < min)
                {
                    min = dotProd;
                }
                else if (dotProd > max)
                {
                    max = dotProd;
                }
            }
            float velocityProjection = dotProduct(curVelocity_, axis);
            if (velocityProjection < 0)
            {
                min += velocityProjection;
            }
            else
            {
                max += velocityProjection;
            }
        }


        public static float dotProduct(Vector2 first, Vector2 second)
        {
            return (first.X * second.X) + (first.Y * second.Y);
        }

        public void draw()
        {
            for (int i = 0; i < points_.Length - 1; i++)
            {
                Illustrator.drawLine(points_[i], points_[i + 1]);
            }
            Illustrator.drawLine(points_[points_.Length - 1], points_[0]);
        }

        public void translate(Vector2 translation)
        {
            for (int i = 0; i < numPoints_; i++)
            {
                points_[i].X = points_[i].X + translation.X;
                points_[i].Y = points_[i].Y + translation.Y;
            }
            curCenter_ += translation;
        }

        protected float getRotationAngle(Vector2 direction)
        {
            return MathHelper.WrapAngle((float)Math.Atan2((double)direction.Y, (double)direction.X));
        }

        protected void generateEdgeNormals()
        {
            int second_i;
            for (int i = 0; i < numPoints_; i++)
            {
                second_i = (i + 1) % numPoints_;
                edgesNormals_[i].X = -(points_[second_i].Y - points_[i].Y);
                edgesNormals_[i].Y = points_[second_i].X - points_[i].X;
            }
        }
    }
}
