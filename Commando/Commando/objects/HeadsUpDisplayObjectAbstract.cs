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
    /// HeadsUpDisplayObjectAbstract is an abstract class which all objects of the 
    /// Heads Up Display inherit.
    /// </summary>
    public abstract class HeadsUpDisplayObjectAbstract : DrawableObjectAbstract, CharacterStatusObserverInterface
    {
        //public abstract void notifyOfChange(CharacterStatusElementInterface statusElement);

        protected GameTexture texture_;

        protected bool modified_;

        protected int newValue_;

        /// <summary>
        /// Hidden default constructor.
        /// </summary>
        protected HeadsUpDisplayObjectAbstract() { }

        /// <summary>
        /// Create a HeadsUpDisplayObject with the specified texture, position, direction, and depth.
        /// </summary>
        /// <param name="pipeline">Drawing pipeline in which to register the object</param>
        /// <param name="texture">GameTexture for this HeadsUpDisplayObject</param>
        /// <param name="position">Position as a Vector relative to the top left corner</param>
        /// <param name="direction">Direction of the HeadsUpDisplayObject as a Vector</param>
        /// <param name="depth">Drawing depth of the object</param>
        public HeadsUpDisplayObjectAbstract(List<DrawableObjectAbstract> pipeline, GameTexture texture, Vector2 position, Vector2 direction, float depth) :
            base(pipeline, position, direction, depth)
        {
            modified_ = false;
            newValue_ = 0;
            texture_ = texture;
        }
        
        /// <summary>
        /// HeadsUpDisplayObject's notify function.
        /// </summary>
        /// <param name="value">New value of the CharacterStatusElement</param>
        public virtual void notifyOfChange(int value)
        {
            newValue_ = value;
            modified_ = true;
        }

        /// <summary>
        /// Update the object's image if the Element has been modified
        /// </summary>
        /// <param name="gameTime">Game time from the parent update function</param>
        public override void update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (modified_)
            {
                updateImage();
            }
        }

        /// <summary>
        /// Function to update the image based off the concrete HeadsUpDisplayObject.
        /// </summary>
        public abstract void updateImage();
    }
}
