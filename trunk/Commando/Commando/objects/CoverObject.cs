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
using Commando.collisiondetection;
using Commando.levels;
using Commando.graphics;
using Microsoft.Xna.Framework;

namespace Commando.objects
{
    public class CoverObject : DrawableObjectAbstract, CollisionObjectInterface
    {
        protected ConvexPolygonInterface boundsPolygon_;

        protected CollisionDetectorInterface collisionDetector_;

        protected readonly Vector2 DIRECTION = new Vector2(1f, 0f);

        protected readonly Height HEIGHT = new Height(true, true);

        protected Vector2 leftOrTop_;

        protected Vector2 rightOrBottom_;

        protected enum CoverOrientationEnum
        {
            HORIZONTAL,
            VERTICAL
        }

        protected CoverOrientationEnum orientation_;

        protected float radius_;

        protected Vector2 coverDirection_;

        public CoverObject(CollisionDetectorInterface detector, List<Vector2> points, Vector2 position, Vector2 leftOrTop, Vector2 rightOrBottom)
        {
            if (detector != null)
            {
                detector.register(this);
            }
            collisionDetector_ = detector;
            boundsPolygon_ = new ConvexPolygon(points, Vector2.Zero);
            radius_ = 0;
            foreach (Vector2 vec in points)
            {
                if (vec.Length() > radius_)
                {
                    radius_ = vec.Length();
                }
            }
            position_ = position;
            direction_ = DIRECTION;
            boundsPolygon_.rotate(direction_, position_);
            leftOrTop_ = leftOrTop;
            rightOrBottom_ = rightOrBottom;
            if (leftOrTop_.X == rightOrBottom_.X)
            {
                orientation_ = CoverOrientationEnum.VERTICAL;
            }
            else
            {
                orientation_ = CoverOrientationEnum.HORIZONTAL;
            }
            coverDirection_ = needsToFace(position_);
        }

        public Vector2 needsToMove(Vector2 position, float radius)
        {
            Vector2 retVector = position;
            if (orientation_ == CoverOrientationEnum.HORIZONTAL)
            {
                if (retVector.Y < leftOrTop_.Y)
                {
                    retVector.Y = leftOrTop_.Y - radius;
                }
                else
                {
                    retVector.Y = leftOrTop_.Y + radius;
                }
                if (retVector.X < leftOrTop_.X + radius)
                {
                    retVector.X = leftOrTop_.X + radius;
                }
                else if (retVector.X > rightOrBottom_.X - radius)
                {
                    retVector.X = rightOrBottom_.X - radius;
                }
            }
            else
            {
                if (retVector.X < leftOrTop_.X)
                {
                    retVector.X = leftOrTop_.X - radius;
                }
                else
                {
                    retVector.X = leftOrTop_.X + radius;
                }
                if (retVector.Y < leftOrTop_.Y + radius)
                {
                    retVector.Y = leftOrTop_.Y + radius;
                }
                else if (retVector.Y > rightOrBottom_.Y - radius)
                {
                    retVector.Y = rightOrBottom_.Y - radius;
                }
            }
            return retVector;
        }

        public Vector2 needsToFace(Vector2 position)
        {
            if (orientation_ == CoverOrientationEnum.HORIZONTAL)
            {
                if (leftOrTop_.Y < position.Y)
                {
                    return new Vector2(0f, -1f);
                }
                else
                {
                    return new Vector2(0f, 1f);
                }
            }
            else
            {
                if (leftOrTop_.X < position.X)
                {
                    return new Vector2(-1f, 0f);
                }
                else
                {
                    return new Vector2(1f, 0f);
                }
            }
        }

        public override void update(GameTime gameTime)
        {
        }

        public override void draw(GameTime gameTime)
        {
        }

        public float getRadius()
        {
            return radius_;
        }

        public ConvexPolygonInterface getBounds(HeightEnum height)
        {
            return boundsPolygon_;
        }

        public Vector2 checkCollisionWith(CollisionObjectInterface obj, CollisionDetectorInterface detector, HeightEnum height, float radDistance, Vector2 velocity)
        {
            if (detector.checkCollision(obj.getBounds(height), getBounds(height), radDistance, velocity) != Vector2.Zero)
            {
                collidedWith(obj);
                obj.collidedInto(this);
            }
            return Vector2.Zero;
        }

        public Vector2 checkCollisionInto(CollisionObjectInterface obj, CollisionDetectorInterface detector, Height height, float radDistance, Vector2 translate)
        {
            return Vector2.Zero;
        }

        public void collidedWith(CollisionObjectInterface obj)
        {
            if (obj is CharacterAbstract)
            {
                (obj as CharacterAbstract).setCoverObject(this);
            }
        }

        public void collidedInto(CollisionObjectInterface obj)
        {
            if (obj is CharacterAbstract)
            {
                (obj as CharacterAbstract).setCoverObject(this);
            }
        }

        public Height getHeight()
        {
            return HEIGHT;
        }

        public Vector2 getCoverDirection()
        {
            return coverDirection_;
        }
    }
}
