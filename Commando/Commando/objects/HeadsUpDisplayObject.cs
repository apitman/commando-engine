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

namespace Commando.objects
{
    class HeadsUpDisplayObject : HeadsUpDisplayObjectAbstract
    {
        public HeadsUpDisplayObject()
            : base()
        {
        }

        public HeadsUpDisplayObject(GameTexture tex)
            : base(tex)
        {
        }

        public HeadsUpDisplayObject(GameTexture tex, Vector2 pos, Vector2 dir, float depth)
            : base(tex, pos, dir, depth)
        {
        }

        public override void updateImage()
        {
            // TODO: This is where the code will go to alter the health bar
            // image or the weapon image when the player character loses/gains
            // health or switches weapons. For now, it will just display what
            // it displays at the very start
        }

        public override void draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            texture_.drawImage(0, position_, 0.0f, 0.9f);
        }
    }
}
