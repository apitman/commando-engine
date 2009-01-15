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
    class MainPlayer : PlayableCharacterAbstract
    {

        public MainPlayer() :
            base(new CharacterHealth(), new CharacterAmmo(), new CharacterWeapon(), "Woger Ru", null, 5.0f, Vector2.Zero, Vector2.Zero, new Vector2(1.0f,0.0f), 0.5f)
        {
            List<GameTexture> anims = new List<GameTexture>();
            anims.Add(TextureMap.getInstance().getTexture("Woger_Ru"));
            animations_ = new AnimationSet(anims);
        }

        public override void draw(GameTime gameTime)
        {
            animations_.drawNextFrame(position_, getRotationAngle(), depth_);
        }

        public override void update(GameTime gameTime)
        {
            position_.Y++;
            position_.X++;
        }
    }
}