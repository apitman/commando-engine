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
    abstract class SolidMovableObjectAbstract : CollisionObjectInterface
    {
        public abstract Vector2 getPosition();

        public abstract Vector2 getDirection();

        public abstract float getRadius();

        public abstract ConvexPolygonInterface getBounds(HeightEnum height);

        public virtual Vector2 checkCollisionWith(CollisionObjectInterface obj, CollisionDetectorInterface detector, HeightEnum height, float radDistance, Vector2 velocity)
        {
            return detector.checkCollision(obj.getBounds(height), getBounds(height), radDistance, velocity);
        }

        public virtual Vector2 checkCollisionInto(CollisionObjectInterface obj, CollisionDetectorInterface detector, Height height, float radDistance, Vector2 translate)
        {
            return translate;
        }

        public abstract void collidedWith(CollisionObjectInterface obj);

        public abstract void collidedInto(CollisionObjectInterface obj);

        public abstract Height getHeight();
    }
}
