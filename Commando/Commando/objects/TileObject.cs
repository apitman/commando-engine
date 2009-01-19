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

namespace Commando.objects
{
    class TileObject : LevelObjectAbstract
    {


        public TileObject() :
            base()
        {
        }

        public TileObject(GameTexture image) :
            base(image)
        {
        }

        public TileObject(GameTexture image, Vector2 position, Vector2 direction, float depth) :
            base(image, position, direction, depth)
        {
        }

        public override void draw(GameTime gameTime)
        {
            image_.drawImage(0, position_, depth_);
        }

        public override void update(GameTime gameTime)
        {
            
        }
    }
}
