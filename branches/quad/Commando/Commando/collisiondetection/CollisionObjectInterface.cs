﻿/*
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
    public interface CollisionObjectInterface
    {
        Vector2 getPosition();

        Vector2 getDirection();

        float getRadius();

        ConvexPolygonInterface getBounds(HeightEnum height);

        Vector2 checkCollisionWith(CollisionObjectInterface obj, CollisionDetectorInterface detector, HeightEnum height, float radDistance, Vector2 velocity);

        Vector2 checkCollisionInto(CollisionObjectInterface obj, CollisionDetectorInterface detector, Height height, float radDistance, Vector2 translate);

        void collidedWith(CollisionObjectInterface obj);

        void collidedInto(CollisionObjectInterface obj);

        Height getHeight();
    }
}
