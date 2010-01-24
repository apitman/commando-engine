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
using Commando.graphics.multithreading;
using Microsoft.Xna.Framework.Graphics;

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
        /// Hidden default constructor.
        /// </summary>
        protected LevelObjectAbstract() { }

        /// <summary>
        /// Create a LevelObject with the specified image, position, direction, and depth.
        /// </summary>
        /// <param name="pipeline">List of objects from which the object should be drawn.</param>
        /// <param name="image">GameTexture for this object.</param>
        /// <param name="position">Position of the object as a Vector relative to the top left corner</param>
        /// <param name="direction">Direction of the object as a Vector</param>
        /// <param name="depth">Drawing depth of the object</param>
        public LevelObjectAbstract(List<DrawableObjectAbstract> pipeline, GameTexture image, Vector2 position, Vector2 direction, float depth) :
            base(pipeline, position, direction, depth)
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

        /// <summary>
        /// Default behavior for level objects is no change.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void update(GameTime gameTime)
        {

        }

        /// <summary>
        /// Default draw type for level objects is no rotation,
        /// simple draw of position and depth, frame 0.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void draw(GameTime gameTime)
        {
            //image_.drawImage(0, position_, depth_);
            DrawStack stack = DrawBuffer.getInstance().getUpdateStack();
            TextureDrawer td = stack.getNext();
            td.Texture = image_;
            td.ImageIndex = 0;
            td.Position = position_;
            td.Dest = false;
            td.CoordinateType = CoordinateTypeEnum.RELATIVE;
            td.Depth = depth_;
            td.Centered = false;
            td.Color = Color.White;
            td.Effects = SpriteEffects.None;
            td.Rotation = 0.0f;
            td.Scale = 1.0f;
            stack.push();
        }

        /// <summary>
        /// Dummy function, overridden in ItemAbstract
        /// </summary>
        /// <param name="collisionDetector"></param>
        public virtual void setCollisionDetector(Commando.collisiondetection.CollisionDetectorInterface collisionDetector)
        {
            // Dummy function, overridden in ItemAbstract
        }
    }
}
