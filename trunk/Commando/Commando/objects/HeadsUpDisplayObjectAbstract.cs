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
    abstract class HeadsUpDisplayObjectAbstract : DrawableObjectAbstract, CharacterStatusObserverInterface
    {
        //public abstract void notifyOfChange(CharacterStatusElementInterface statusElement);

        protected GameTexture texture_;

        protected bool modified_;

        protected int newValue_;

        public HeadsUpDisplayObjectAbstract() :
            base()
        {
            modified_ = false;
            newValue_ = 0;
            texture_ = TextureMap.getInstance().getTexture("No_Image");
        }

        public HeadsUpDisplayObjectAbstract(GameTexture texture) :
            base()
        {
            modified_ = false;
            newValue_ = 0;
            texture_ = texture;
        }

        public HeadsUpDisplayObjectAbstract(GameTexture texture, Vector2 position, Vector2 direction, float depth) :
            base(position, direction, depth)
        {
            modified_ = false;
            newValue_ = 0;
            texture_ = texture;
        }
        
        public void notifyOfChange(int value)
        {
            newValue_ = value;
            modified_ = true;
        }

        public override void update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (modified_)
            {
                updateImage();
            }
        }

        public abstract void updateImage();
    }
}
