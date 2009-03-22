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

        protected Vector2 position_;

        protected readonly Vector2 DIRECTION = new Vector2(1f, 0f);

        protected Vector2 direction_;

        protected readonly Height HEIGHT = new Height(true, true);

        protected Vector2 leftOrTop_;

        protected Vector2 rightOrBottom_;

        public CoverObject(CollisionDetectorInterface detector, List<Vector2> points, Vector2 position, Vector2 leftOrTop, Vector2 rightOrBottom)
        {
            if (detector != null)
            {
                detector.register(this);
            }
            collisionDetector_ = detector;
            boundsPolygon_ = new ConvexPolygon(points, new Vector2(0f, 0f));
            position_ = position;
            direction_ = DIRECTION;
            leftOrTop_ = leftOrTop;
            rightOrBottom_ = rightOrBottom;
        }

        public override void update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        public override void draw(GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        public float getRadius()
        {
            throw new NotImplementedException();
        }

        public ConvexPolygonInterface getBounds(HeightEnum height)
        {
            throw new NotImplementedException();
        }

        public Vector2 checkCollisionWith(CollisionObjectInterface obj, CollisionDetectorInterface detector, HeightEnum height, float radDistance, Vector2 velocity)
        {
            throw new NotImplementedException();
        }

        public Vector2 checkCollisionInto(CollisionObjectInterface obj, CollisionDetectorInterface detector, Height height, float radDistance, Vector2 translate)
        {
            throw new NotImplementedException();
        }

        public void collidedWith(CollisionObjectInterface obj)
        {
            throw new NotImplementedException();
        }

        public void collidedInto(CollisionObjectInterface obj)
        {
            throw new NotImplementedException();
        }

        public Height getHeight()
        {
            return HEIGHT;
        }
    }
}
