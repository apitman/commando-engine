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
    public class CollisionDetector : CollisionDetectorInterface
    {
        protected List<BoundingPolygon> bounds_;
        protected List<CollisionObjectInterface> objects_;

        public CollisionDetector(List<BoundingPolygon> bounds)
        {
            bounds_ = bounds;
            objects_ = new List<CollisionObjectInterface>();
        }

        #region CollisionDetectorInterface Members

        public void register(CollisionObjectInterface obj)
        {
            if (!objects_.Contains(obj))
            {
                objects_.Add(obj);
            }
        }

        public Vector2 checkCollisions(CollisionObjectInterface obj, Vector2 newPosition)
        {
            if(checkObjectCollisions(obj, newPosition))
            {
                return obj.getPosition();
            }
            return checkBoundsCollisions(obj, newPosition);
        }

        #endregion

        protected Vector2 checkBoundsCollisions(CollisionObjectInterface obj, Vector2 newPosition)
        {
            //Point position = new Point((int)Math.Round(newPosition.X), (int)Math.Round(newPosition.Y));
            float radius = obj.getRadius();
            for (int i = 0; i < bounds_.Count; i++)
            {
                newPosition = bounds_[i].checkCollision(newPosition, radius);
            }
            return new Vector2(newPosition.X, newPosition.Y);
        }
        protected bool checkObjectCollisions(CollisionObjectInterface obj, Vector2 newPosition)
        {
            for (int i = 0; i < objects_.Count; i++)
            {
                if (obj != objects_[i] && distance(newPosition, objects_[i].getPosition()) < (obj.getRadius() + objects_[i].getRadius()))
                {
                    return true;
                }
            }
            return false;
        }

        protected float distance(Vector2 p1, Vector2 p2)
        {
            return (float)(Math.Sqrt(Math.Pow((double)(p1.X - p2.X), 2.0) + Math.Pow((double)(p1.Y - p2.Y), 2.0)));
        }
    }
}
