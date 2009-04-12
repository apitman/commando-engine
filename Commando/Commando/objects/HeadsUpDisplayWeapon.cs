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
using Commando.objects;
using Microsoft.Xna.Framework;
using Commando.graphics.multithreading;
using Microsoft.Xna.Framework.Graphics;

namespace Commando.objects
{
    /// <summary>
    /// Concrete version of the HeadsUpDisplayObjectAbstract.
    /// </summary>
    public class HeadsUpDisplayWeapon : HeadsUpDisplayObjectAbstract
    {
        /// <summary>
        /// Hidden default constructor.
        /// </summary>
        protected HeadsUpDisplayWeapon() { }

        /// <summary>
        /// Create a HeadsUpDisplayObject with the specified texture, position, direction, and depth.
        /// </summary>
        /// <param name="pipeline">Drawing pipeline in which to register the object</param>
        /// <param name="tex">GameTexture for this HeadsUpDisplayObject</param>
        /// <param name="pos">Position as a Vector relative to the top left corner</param>
        /// <param name="dir">Direction of the HeadsUpDisplayObject as a Vector</param>
        /// <param name="depth">Drawing depth of the object</param>
        public HeadsUpDisplayWeapon(List<DrawableObjectAbstract> pipeline, GameTexture tex, Vector2 pos, Vector2 dir, float depth)
            : base(pipeline, tex, pos, dir, depth)
        {
        }

        public override void updateImage()
        {
            // TODO: This is where the code will go to alter the health bar
            // image or the weapon image when the player character loses/gains
            // health or switches weapons. For now, it will just display what
            // it displays at the very start
        }

        /// <summary>
        /// Actually draw the object to the screen
        /// </summary>
        /// <param name="gameTime"></param>
        public override void draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            //texture_.drawImageAbsolute(0, position_, 0.0f, depth_);
            DrawStack stack = DrawBuffer.getInstance().getUpdateStack();
            TextureDrawer td = stack.getNext();
            td.Texture = texture_;
            td.ImageIndex = 0;
            td.Position = position_;
            td.Dest = false;
            td.CoordinateType = CoordinateTypeEnum.ABSOLUTE;
            td.Depth = depth_;
            td.Centered = true;
            td.Color = Color.White;
            td.Effects = SpriteEffects.None;
            td.Rotation = 0.0f;
            td.Scale = 1.0f;
            stack.push();
        }
    }
}
