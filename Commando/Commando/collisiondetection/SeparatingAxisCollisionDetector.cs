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
using Commando.levels;

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

        public void remove(CollisionObjectInterface obj)
        {
            objects_.Remove(obj);
        }

        public Vector2 checkCollisions(CollisionObjectInterface obj, Vector2 newPosition)
        {
            Vector2 oldPosition = obj.getPosition();
            float radius = obj.getRadius();
            Vector2 velocity = newPosition - oldPosition;
            ConvexPolygonInterface movingObjectPolygon= obj.getBounds(HeightEnum.LOW);
            ConvexPolygonInterface movingObjectPolygonHigh = obj.getBounds(HeightEnum.HIGH);
            Vector2 direction = obj.getDirection();
            Vector2 position = movingObjectPolygon.getCenter();
            movingObjectPolygon.rotate(direction, position);
            movingObjectPolygonHigh.rotate(direction, position);
            //TODO: Check collisions on rotations
            Vector2 translate;
            float dist;
            List<CollisionObjectInterface> collisions = new List<CollisionObjectInterface>();
            foreach (CollisionObjectInterface cObj in objects_)
            {
                dist = distanceBetweenPoints(position, cObj.getPosition());
                //Console.Out.WriteLine(dist);
                if (cObj != obj && dist < radius + cObj.getRadius())
                {
                    dist = radius + cObj.getRadius() - dist;
                    translate = checkCollision(movingObjectPolygon, cObj.getBounds(HeightEnum.LOW), dist);
                    //Console.Out.WriteLine(translate);
                    if (translate != Vector2.Zero)
                    {
                        obj.collidedInto(cObj);
                        cObj.collidedWith(obj);
                        newPosition += translate;
                        //movingObjectPolygon.translate(translate);
                        movingObjectPolygon.rotate(direction, newPosition);
                        position = movingObjectPolygon.getCenter();
                        collisions.Add(cObj);
                    }
                }
            }
            bool noMoreCollisions = true;
/*            foreach (CollisionObjectInterface cObj in collisions)
            {
                dist = distanceBetweenPoints(position, cObj.getPosition());
                //Console.Out.WriteLine(dist);
                if (cObj != obj && dist < radius + cObj.getRadius())
                {
                    dist = radius + cObj.getRadius() - dist;
                    translate = checkCollision(movingObjectPolygon, cObj.getBounds(), dist);
                    //Console.Out.WriteLine(translate);
                    if (translate != Vector2.Zero)
                    {
                        noMoreCollisions = false;
                        break;
                    }
                }
            }*/
            if (noMoreCollisions)
            {
                return newPosition;
            }
            movingObjectPolygon.rotate(direction, oldPosition);
            return oldPosition;
        }

        public void draw()
        {
            foreach (CollisionObjectInterface cObj in objects_)
            {
                ConvexPolygonInterface poly = cObj.getBounds(HeightEnum.LOW);
                if (poly is ConvexPolygon)
                {
                    (poly as ConvexPolygon).draw();
                }
            }
        }

        public bool checkCollisions(CollisionObjectInterface obj, ref Vector2 velocity, ref Vector2 newDirection)
        {
            Vector2 oldDirection = newDirection;
            Vector2 oldVelocity = velocity;
            Vector2 translate = checkRotationalCollisions(obj, ref newDirection);
            velocity.X += translate.X;
            velocity.Y += translate.Y;
            velocity = checkTranslationalCollisions(obj, velocity);
            return oldDirection != newDirection || oldVelocity != velocity;
        }

        protected Vector2 checkRotationalCollisions(CollisionObjectInterface obj, ref Vector2 newDirection)
        {
            Height objectHeight = obj.getHeight();
            if (objectHeight.blocksHigh_ && objectHeight.blocksLow_)
            {
                return checkRotationalCollisionsMultiHeight(obj, ref newDirection);
            }
            else if (objectHeight.blocksLow_)
            {
                return checkRotationalCollisionsSingleHeight(obj, ref newDirection, HeightEnum.LOW);
            }
            else if (objectHeight.blocksHigh_)
            {
                return checkRotationalCollisionsSingleHeight(obj, ref newDirection, HeightEnum.HIGH);
            }
            else
            {
                return Vector2.Zero;
            }
        }

        protected Vector2 checkRotationalCollisionsMultiHeight(CollisionObjectInterface obj, ref Vector2 newDirection)
        {
            float radius = obj.getRadius();
            Vector2 position = obj.getPosition();
            ConvexPolygonInterface movingObjectPolygonHigh = obj.getBounds(HeightEnum.HIGH);
            ConvexPolygonInterface movingObjectPolygonLow = obj.getBounds(HeightEnum.LOW);
            Vector2 oldDirection = obj.getDirection();
            movingObjectPolygonHigh.rotate(newDirection, position);
            movingObjectPolygonLow.rotate(newDirection, position);
            Height objectHeight = obj.getHeight(), otherObjectHeight;
            Height translationHeight;

            Vector2 translate = Vector2.Zero, translateHigh = Vector2.Zero, translateLow = Vector2.Zero;
            float dist;
            List<Vector2> collisions = new List<Vector2>();
            foreach (CollisionObjectInterface cObj in objects_)
            {
                dist = distanceBetweenPoints(position, cObj.getPosition());
                otherObjectHeight = cObj.getHeight();
                if (cObj != obj && objectHeight.collides(otherObjectHeight) && dist < radius + cObj.getRadius())
                {
                    dist = radius + cObj.getRadius() - dist;
                    //translate = checkCollision(movingObjectPolygon, cObj.getBounds(height), dist, Vector2.Zero);
                    translateHigh = Vector2.Zero;
                    translateLow = Vector2.Zero;
                    translationHeight = new Height(false, false);
                    if (otherObjectHeight.blocksLow_)
                    {
                        translateLow = cObj.checkCollisionWith(obj, this, HeightEnum.LOW, dist, Vector2.Zero);
                    }
                    if (otherObjectHeight.blocksHigh_)
                    {
                        translateHigh = cObj.checkCollisionWith(obj, this, HeightEnum.HIGH, dist, Vector2.Zero);
                    }
                    if (translateLow.LengthSquared() > translateHigh.LengthSquared())
                    {
                        translate = translateLow;
                        translationHeight.blocksLow_ = true;
                    }
                    else
                    {
                        translate = translateHigh;
                        translationHeight.blocksHigh_ = true;
                    }
                    if (translate != Vector2.Zero)
                    {
                        obj.collidedInto(cObj);
                        cObj.collidedWith(obj);
                        //velocity += translate;
                        //movingObjectPolygon.rotate(direction, newPosition);
                        translate = obj.checkCollisionInto(cObj, this, translationHeight, dist, translate);
                        if (translate != Vector2.Zero)
                        {
                            collisions.Add(translate);
                        }
                    }
                }
            }
            if (collisions.Count == 1)
            {
                return collisions[0];
            }
            if (collisions.Count > 1)
            {
                translate = Vector2.Zero;
                for (int i = 0; i < collisions.Count; i++)
                {
                    Vector2 colVector = collisions[i];
                    for (int j = i; j < collisions.Count; j++)
                    {
                        if (ConvexPolygon.dotProduct(colVector, collisions[j]) / (colVector.Length() * collisions[j].Length()) < 0)
                        {
                            newDirection = oldDirection;
                            movingObjectPolygonLow.rotate(oldDirection, position);
                            movingObjectPolygonHigh.rotate(oldDirection, position);
                            return Vector2.Zero;
                        }
                    }
                    translate += colVector;
                }
                return translate;
            }
            return Vector2.Zero;
        }

        protected Vector2 checkRotationalCollisionsSingleHeight(CollisionObjectInterface obj, ref Vector2 newDirection, HeightEnum height)
        {
            float radius = obj.getRadius();
            Vector2 position = obj.getPosition();
            ConvexPolygonInterface movingObjectPolygon = obj.getBounds(height);
            Vector2 oldDirection = obj.getDirection();
            movingObjectPolygon.rotate(newDirection, position);
            Height objectHeight = obj.getHeight();

            Vector2 translate = Vector2.Zero;
            float dist;
            List<Vector2> collisions = new List<Vector2>();
            foreach (CollisionObjectInterface cObj in objects_)
            {
                dist = distanceBetweenPoints(position, cObj.getPosition());
                if (cObj != obj && objectHeight.collides(cObj.getHeight()) && dist < radius + cObj.getRadius())
                {
                    dist = radius + cObj.getRadius() - dist;
                    //translate = checkCollision(movingObjectPolygon, cObj.getBounds(height), dist, Vector2.Zero);
                    translate = cObj.checkCollisionWith(obj, this, height, dist, Vector2.Zero);
                    if (translate != Vector2.Zero)
                    {
                        obj.collidedInto(cObj);
                        cObj.collidedWith(obj);
                        //velocity += translate;
                        //movingObjectPolygon.rotate(direction, newPosition);
                        translate = obj.checkCollisionInto(cObj, this, Height.getHeight(height), dist, translate);
                        if (translate != Vector2.Zero)
                        {
                            collisions.Add(translate);
                        }
                    }
                }
            }
            if (collisions.Count == 1)
            {
                return collisions[0];
            }
            if (collisions.Count > 1)
            {
                translate = Vector2.Zero;
                for (int i = 0; i < collisions.Count; i++)
                {
                    Vector2 colVector = collisions[i];
                    for (int j = i; j < collisions.Count; j++)
                    {
                        if (ConvexPolygon.dotProduct(colVector, collisions[j]) / (colVector.Length() * collisions[j].Length()) < 0)
                        {
                            newDirection = oldDirection;
                            movingObjectPolygon.rotate(oldDirection, position);
                            return Vector2.Zero;
                        }
                    }
                    translate += colVector;
                }
                return translate;
            }
            return Vector2.Zero;
        }

        protected Vector2 checkTranslationalCollisions(CollisionObjectInterface obj, Vector2 velocity)
        {
            Height objectHeight = obj.getHeight();
            if (objectHeight.blocksHigh_ && objectHeight.blocksLow_)
            {
                return checkTranslationalCollisionsMultiHeight(obj, velocity);
            }
            else if (objectHeight.blocksLow_)
            {
                return checkTranslationalCollisionsSingleHeight(obj, velocity, HeightEnum.LOW);
            }
            else if (objectHeight.blocksHigh_)
            {
                return checkTranslationalCollisionsSingleHeight(obj, velocity, HeightEnum.HIGH);
            }
            else
            {
                return velocity;
            }
        }

        protected Vector2 checkTranslationalCollisionsMultiHeight(CollisionObjectInterface obj, Vector2 velocity)
        {
            float radius = obj.getRadius() + velocity.Length();
            Vector2 position = obj.getPosition();
            ConvexPolygonInterface movingObjectPolygonLow = obj.getBounds(HeightEnum.LOW);
            ConvexPolygonInterface movingObjectPolygonHigh = obj.getBounds(HeightEnum.HIGH);
            Height objectHeight = obj.getHeight(), otherObjectHeight;
            Height translationHeight;

            Vector2 translate = Vector2.Zero, translateHigh = Vector2.Zero, translateLow = Vector2.Zero;
            float dist;
            List<Vector2> collisions = new List<Vector2>();
            foreach (CollisionObjectInterface cObj in objects_)
            {
                dist = distanceBetweenPoints(position, cObj.getPosition());
                otherObjectHeight = cObj.getHeight();
                if (cObj != obj && objectHeight.collides(otherObjectHeight) && dist < radius + cObj.getRadius())
                {
                    dist = radius + cObj.getRadius() - dist;
                    //translate = checkCollision(movingObjectPolygon, cObj.getBounds(height), dist, velocity);
                    //translate = cObj.checkCollisionWith(obj, this, height, dist, velocity);
                    translateHigh = Vector2.Zero;
                    translateLow = Vector2.Zero;
                    translationHeight = new Height(false, false);
                    if (otherObjectHeight.blocksLow_)
                    {
                        translateLow = cObj.checkCollisionWith(obj, this, HeightEnum.LOW, dist, velocity);
                    }
                    if (otherObjectHeight.blocksLow_)
                    {
                        translateHigh = cObj.checkCollisionWith(obj, this, HeightEnum.LOW, dist, velocity);
                    }
                    if (translateLow.LengthSquared() > translateHigh.LengthSquared())
                    {
                        translate = translateLow;
                        translationHeight.blocksLow_ = true;
                    }
                    else
                    {
                        translate = translateHigh;
                        translationHeight.blocksHigh_ = true;
                    }
                    if (translate != Vector2.Zero)
                    {
                        obj.collidedInto(cObj);
                        cObj.collidedWith(obj);
                        //velocity += translate;
                        //movingObjectPolygon.rotate(direction, newPosition);
                        position = movingObjectPolygonHigh.getCenter();
                        translate = obj.checkCollisionInto(cObj, this, translationHeight, dist, translate);
                        if (translate != Vector2.Zero)
                        {
                            collisions.Add(translate);
                        }
                    }
                }
            }
            if (collisions.Count == 0)
            {
                movingObjectPolygonHigh.translate(velocity);
                movingObjectPolygonLow.translate(velocity);
                return velocity;
            }
            if (collisions.Count == 1)
            {
                movingObjectPolygonHigh.translate(velocity + collisions[0]);
                movingObjectPolygonLow.translate(velocity + collisions[0]);
                return (velocity + collisions[0]);
            }
            velocity += correctMultipleCollisions(velocity, collisions);
            movingObjectPolygonHigh.translate(velocity);
            movingObjectPolygonLow.translate(velocity);
            return velocity;
        }

        protected Vector2 checkTranslationalCollisionsSingleHeight(CollisionObjectInterface obj, Vector2 velocity, HeightEnum height)
        {
            float radius = obj.getRadius() + velocity.Length();
            Vector2 position = obj.getPosition();
            ConvexPolygonInterface movingObjectPolygon = obj.getBounds(height);
            Height objectHeight = obj.getHeight();

            Vector2 translate;
            float dist;
            List<Vector2> collisions = new List<Vector2>();
            foreach (CollisionObjectInterface cObj in objects_)
            {
                dist = distanceBetweenPoints(position, cObj.getPosition());
                if (cObj != obj && objectHeight.collides(cObj.getHeight()) && dist < radius + cObj.getRadius())
                {
                    dist = radius + cObj.getRadius() - dist;
                    //translate = checkCollision(movingObjectPolygon, cObj.getBounds(height), dist, velocity);
                    translate = cObj.checkCollisionWith(obj, this, height, dist, velocity);
                    if (translate != Vector2.Zero)
                    {
                        obj.collidedInto(cObj);
                        cObj.collidedWith(obj);
                        //velocity += translate;
                        //movingObjectPolygon.rotate(direction, newPosition);
                        position = movingObjectPolygon.getCenter();
                        translate = obj.checkCollisionInto(cObj, this, Height.getHeight(height), dist, translate);
                        if (translate != Vector2.Zero)
                        {
                            collisions.Add(translate);
                        }
                    }
                }
            }
            if (collisions.Count == 0)
            {
                movingObjectPolygon.translate(velocity);
                return velocity;
            }
            if (collisions.Count == 1)
            {
                movingObjectPolygon.translate(velocity + collisions[0]);
                return (velocity + collisions[0]);
            }
            velocity += correctMultipleCollisions(velocity, collisions);
            movingObjectPolygon.translate(velocity);
            return velocity;
        }

        public Vector2 checkCollision(ConvexPolygonInterface polygonA, ConvexPolygonInterface polygonB, float radDistance, Vector2 velocity)
        {
            int edgesCountPolygonA = polygonA.getNumberOfPoints();
            int edgesCountPolygonB = polygonB.getNumberOfPoints();
            Vector2 centerA = polygonA.getCenter();
            Vector2 centerB = polygonB.getCenter();
            if (polygonA is CircularConvexPolygon && polygonB is CircularConvexPolygon)
            {
                Vector2 tempRet = centerA - centerB;
                tempRet.Normalize();
                return tempRet * radDistance;
            }
            else if (polygonA is CircularConvexPolygon)
            {
                edgesCountPolygonA = edgesCountPolygonB;
            }
            else if (polygonB is CircularConvexPolygon)
            {
                edgesCountPolygonB = edgesCountPolygonA;
            }
            float minDistance = float.PositiveInfinity;
            Vector2 minAxis = Vector2.Zero;
            Vector2 currentEdgeNormal;
            Vector2 temp;

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
                        currentEdgeNormal = polygonA.getPoint(i - edgesCountPolygonA) - centerB;
                    }
                    else
                    {
                        currentEdgeNormal = polygonB.getEdge(i - edgesCountPolygonA);
                    }
                }

                currentEdgeNormal.Normalize();

                float minA = 0, minB = 0, maxA = 0, maxB = 0;
                polygonA.projectPolygonOnAxis(currentEdgeNormal, new Height(), ref minA, ref maxA);
                polygonB.projectPolygonOnAxis(currentEdgeNormal, new Height(), ref minB, ref maxB);

                float velocityProjection = ConvexPolygon.dotProduct(velocity, currentEdgeNormal);

                if (velocityProjection < 0)
                {
                    minA += velocityProjection;
                }
                else
                {
                    maxA += velocityProjection;
                }

                float distance = distanceBetweenProjections(minA, maxA, minB, maxB);
                if (distance > 0)
                {
                    return Vector2.Zero;
                }

                distance = Math.Abs(distance);
                if (distance < minDistance)
                {
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

        public Vector2 checkCollision(ConvexPolygonInterface polygonA, ConvexPolygonInterface polygonB, float radDistance)
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
                        currentEdgeNormal = polygonA.getPoint(i - edgesCountPolygonA) - centerB;
                    }
                    else
                    {
                        currentEdgeNormal = polygonB.getEdge(i - edgesCountPolygonA);
                    }
                }

                currentEdgeNormal.Normalize();

                float minA = 0, minB = 0, maxA = 0, maxB = 0;
                polygonA.projectPolygonOnAxis(currentEdgeNormal, new Height(), ref minA, ref maxA);
                polygonB.projectPolygonOnAxis(currentEdgeNormal, new Height(), ref minB, ref maxB);

                float distance = distanceBetweenProjections(minA, maxA, minB, maxB);
                if (distance > 0)
                {
                    return Vector2.Zero;
                }

                distance = Math.Abs(distance);
                if (distance < minDistance)
                {
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

        protected Vector2 correctMultipleCollisions(Vector2 velocity, List<Vector2> collisionVectors)
        {
            Vector2 maxTranslate = Vector2.Zero;
            Vector2 tempTranslateVector;
            foreach (Vector2 t in collisionVectors)
            {
                tempTranslateVector = calculateVelocityTranslationVector(velocity, t);
                if (tempTranslateVector.LengthSquared() > maxTranslate.LengthSquared())
                {
                    maxTranslate = tempTranslateVector;
                }
            }
            return maxTranslate;
        }

        protected Vector2 calculateVelocityTranslationVector(Vector2 velocity, Vector2 translate)
        {
            velocity *= -1f;
            float dot = ConvexPolygon.dotProduct(velocity, translate);
            float magnitudeOfTranslate = translate.LengthSquared();
            //If they are at right angles to each other
            if (dot == 0)
            {
                //TODO: Improve this step to find the exact distance to move back
                return velocity;
            }
            Vector2 finalTranslate = Math.Abs(magnitudeOfTranslate / dot) * velocity;
            if (finalTranslate.LengthSquared() > velocity.LengthSquared())
            {
                return velocity;
            }
            return finalTranslate;
        }

        protected static float distanceBetweenPoints(Vector2 first, Vector2 second)
        {
            return (float)Math.Sqrt(Math.Pow(first.X - second.X, 2) + Math.Pow(first.Y - second.Y, 2));
        }
    }
}
