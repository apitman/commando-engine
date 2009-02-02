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

    public class GameObject
    {
        protected Vector2 position_;

        protected Vector2 newPostion_;

        protected Vector2 direction_;

        protected ComponentInterface[] components_;

        public GameObject()
        {
            components_ = new ComponentInterface[(int)ComponentEnum.LENGTH];
            for (int i = 0; i < components_.Length; i++)
            {
                components_[i] = null;
            }
            position_ = Vector2.Zero;
            newPostion_ = Vector2.Zero;
            direction_ = new Vector2(1.0f, 0.0f);
        }

        public ComponentInterface getComponent(ComponentEnum componentType)
        {
             return components_[(int)componentType];
        }

        public void setComponent(ComponentEnum componentType, ComponentInterface component)
        {
            components_[(int)componentType] = component;
        }

        public void update(GameTime gameTime)
        {
            for (int i = 0; i < components_.Length; i++)
            {
                if (components_[i] != null)
                {
                    components_[i].update();
                }
            }
            position_ = newPostion_;
        }

        public void draw()
        {

        }

        public Vector2 getPosition()
        {
            return position_;
        }

        public void setPosition(Vector2 position)
        {
            position_ = position;
        }

        public Vector2 getDirection()
        {
            return direction_;
        }

        public void setDirection(Vector2 direction)
        {
            direction_ = direction;
        }

        public void setNewPosition(Vector2 position)
        {
            newPostion_ = position;
        }
    }
}
