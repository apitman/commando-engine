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
    /// All objects drawn on the level inherit from this class.
    /// </summary>
    public abstract class LevelObjectAbstract : DrawableObjectAbstract
    {

        protected GameTexture image_;

        //Additional info to be added for passable/unpassable, rooms, level, etc.

        /// <summary>
        /// Create a base LevelObject.
        /// </summary>
        public LevelObjectAbstract() :
            base()
        {
        }

        /// <summary>
        /// Create a LevelObject with the specified texture.
        /// </summary>
        /// <param name="image">GameTexture for this object</param>
        public LevelObjectAbstract(GameTexture image) :
            base()
        {
            image_ = image;
        }

        /// <summary>
        /// Create a LevelObject with the specified image, position, direction, and depth.
        /// </summary>
        /// <param name="image">GameTexture for this object.</param>
        /// <param name="position">Position of the object as a Vector relative to the top left corner</param>
        /// <param name="direction">Direction of the object as a Vector</param>
        /// <param name="depth">Drawing depth of the object</param>
        public LevelObjectAbstract(GameTexture image, Vector2 position, Vector2 direction, float depth) :
            base(position, direction, depth)
        {
            image_ = image;
        }

        public GameTexture getImage()
        {
            return image_;
        }

        public void setImage(GameTexture image)
        {
            image_ = image;
        }
    }
}
