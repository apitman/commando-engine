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
using Microsoft.Xna.Framework;
using Commando.levels;

namespace Commando.objects
{
    public class BoxObject : CollisionObjectInterface
    {
        protected ConvexPolygonInterface boundsPolygon_;

        protected Vector2 position_;

        protected Height height_;

        public BoxObject(List<Vector2> points, Vector2 center, Height height)
        {
            position_ = center;
            boundsPolygon_ = new ConvexPolygon(points, Vector2.Zero);
            height_ = height;
        }

        #region CollisionObjectInterface Members

        public Vector2 getPosition()
        {
            return position_;
        }

        public Vector2 getDirection()
        {
            return new Vector2(1.0f, 0.0f);
        }

        public float getRadius()
        {
            return boundsPolygon_.getPoint(0).Length();
        }

        public ConvexPolygonInterface getBounds(HeightEnum height)
        {
            return boundsPolygon_;
        }

        public Height getHeight()
        {
            return height_;
        }

        public void collidedWith(CollisionObjectInterface obj)
        {
            
        }

        public void collidedInto(CollisionObjectInterface obj)
        {
            
        }

        public Vector2 checkCollisionWith(CollisionObjectInterface obj, CollisionDetectorInterface detector, HeightEnum height, float radDistance, Vector2 velocity)
        {
            return detector.checkCollision(obj.getBounds(height), getBounds(height), radDistance, velocity);
        }

        public Vector2 checkCollisionInto(CollisionObjectInterface obj, CollisionDetectorInterface detector, Height height, float radDistance, Vector2 translate)
        {
            return translate;
        }

        #endregion
    }
}
