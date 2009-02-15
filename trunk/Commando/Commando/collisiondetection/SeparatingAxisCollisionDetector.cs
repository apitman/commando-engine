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

/*
 * The basis for this class (and the ConvexPolygon) was found at this
 * online tutorial: http://www.codeproject.com/KB/GDI-plus/PolygonCollision.aspx
 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Commando.collisiondetection
{
    public class SeparatingAxisCollisionDetector : CollisionDetectorInterface
    {
        List<CollisionObjectInterface> objects_;

        public SeparatingAxisCollisionDetector()
        {
            objects_ = new List<CollisionObjectInterface>();
        }

        public void register(CollisionObjectInterface obj)
        {
            if (!objects_.Contains(obj))
            {
                objects_.Add(obj);
            }
        }

        public Vector2 checkCollisions(CollisionObjectInterface obj, Vector2 newPosition)
        {
            float radius = obj.getRadius();
            ConvexPolygonInterface movingObjectPolygon = obj.getBounds();
            Vector2 direction = obj.getDirection();
            movingObjectPolygon.rotate(direction, newPosition);
            Vector2 translate;
            float dist;
            foreach (CollisionObjectInterface cObj in objects_)
            {
                dist = distanceBetweenPoints(newPosition, cObj.getPosition());
                //Console.Out.WriteLine(dist);
                if (cObj != obj && dist < radius + cObj.getRadius())
                {
                    translate = checkCollision(movingObjectPolygon, cObj.getBounds(), dist);
                    //Console.Out.WriteLine(translate);
                    if (translate != Vector2.Zero)
                    {
                        newPosition += translate;
                        movingObjectPolygon.rotate(direction, newPosition);
                    }
                }
            }
            return newPosition;
        }

        protected Vector2 checkCollision(ConvexPolygonInterface polygonA, ConvexPolygonInterface polygonB, float radDistance)
        {
            int edgesCountPolygonA = polygonA.getNumberOfPoints();
            int edgesCountPolygonB = polygonB.getNumberOfPoints();
            Vector2 centerA = polygonA.getCenter();
            Vector2 centerB = polygonB.getCenter();
            if(polygonA is CircularConvexPolygon && polygonB is CircularConvexPolygon)
            {
                Vector2 tempRet = centerA - centerB;
                tempRet.Normalize();
                return tempRet * radDistance;
            }
            else if(polygonA is CircularConvexPolygon)
            {
                edgesCountPolygonA = edgesCountPolygonB;
            }
            else if(polygonB is CircularConvexPolygon)
            {
                edgesCountPolygonB = edgesCountPolygonA;
            }
            float minDistance = float.PositiveInfinity;
            Vector2 minAxis = Vector2.Zero;
            Vector2 currentEdgeNormal;
            Vector2 temp;
            //Console.Out.WriteLine(edgesCountPolygonA + " " + edgesCountPolygonB);

            for (int i = 0; i < edgesCountPolygonA + edgesCountPolygonB; i++)
            {
                if (i < edgesCountPolygonA)
                {
                    if (polygonA is CircularConvexPolygon)
                    {
                        currentEdgeNormal = polygonB.getPoint(i) - centerA;
                    }
                    else
                    {
                        currentEdgeNormal = polygonA.getEdge(i);
                    }
                }
                else
                {
                    if (polygonB is CircularConvexPolygon)
                    {
                        currentEdgeNormal = polygonA.getPoint(i) - centerB;
                    }
                    else
                    {
                        currentEdgeNormal = polygonB.getEdge(i - edgesCountPolygonA);
                    }
                }

                currentEdgeNormal.Normalize();

                float minA = 0, minB = 0, maxA = 0, maxB = 0;
                polygonA.projectPolygonOnAxis(currentEdgeNormal, ref minA, ref maxA);
                polygonB.projectPolygonOnAxis(currentEdgeNormal, ref minB, ref maxB);

                float distance = distanceBetweenProjections(minA, maxA, minB, maxB);
                //Console.Out.WriteLine(minA + "," + maxA+ "," + minB + "," + maxB + "," + distance);
                if (distance > 0)
                {
                    return Vector2.Zero;
                }

                distance = Math.Abs(distance);
                if (distance < minDistance)
                {
                    //Console.Out.WriteLine("In Here: " + distance);
                    minDistance = distance;
                    if (ConvexPolygon.dotProduct(centerA - centerB, currentEdgeNormal) < 0)
                    {
                        minAxis = -currentEdgeNormal;
                    }
                    else
                    {
                        minAxis = currentEdgeNormal;
                    }
                }
            }
            return minAxis * minDistance;
        }

        protected float distanceBetweenProjections(float minA, float maxA, float minB, float maxB)
        {
            if (minA < minB)
            {
                return minB - maxA;
            }
            else
            {
                return minA - maxB;
            }
        }

        protected static float distanceBetweenPoints(Vector2 first, Vector2 second)
        {
            return (float)Math.Sqrt(Math.Pow(first.X - second.X, 2) + Math.Pow(first.Y - second.Y, 2));
        }
    }
}
