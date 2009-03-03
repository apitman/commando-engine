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
    /// <summary>
    /// Object representing a tile on the ground.
    /// </summary>
    public class TileObject : LevelObjectAbstract
    {

        /// <summary>
        /// Hidden default constructor.
        /// </summary>
        protected TileObject() { }

        /// <summary>
        /// Create a TileObject with the specified image, position, direction, and depth.
        /// </summary>
        /// <param name="image">GameTexture for this object.</param>
        /// <param name="position">Position of the object as a Vector relative to the top left corner</param>
        /// <param name="direction">Direction of the object as a Vector</param>
        /// <param name="depth">Drawing depth of the object</param>
        public TileObject(List<DrawableObjectAbstract> pipeline, GameTexture image, Vector2 position, Vector2 direction, float depth) :
            base(pipeline, image, position, direction, depth)
        {
        }

        /// <summary>
        /// Draw the tile on the ground.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void draw(GameTime gameTime)
        {
            image_.drawImage(0, position_, depth_);
        }

        /// <summary>
        /// Update does nothing, because tiles don't change.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void update(GameTime gameTime)
        {
            
        }
    }
}
