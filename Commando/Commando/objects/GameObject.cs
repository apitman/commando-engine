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

namespace Commando.objects
{ 

    public class GameObject
    {

        public Vector2 Position_ { get; set; }

        public Vector2 Heading_ { get; set; }

        public int Radius_ { get; set; }

        protected ComponentInterface[] components_;

        public GameObject()
        {
            components_ = new ComponentInterface[(int)ComponentEnum.LENGTH];
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

        }

        public void draw()
        {

        }
    }
}