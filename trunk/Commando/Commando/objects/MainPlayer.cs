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
using Commando.controls;

namespace Commando.objects
{
    class MainPlayer : PlayableCharacterAbstract
    {

        const float TURNSPEED = .40f;

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
            int MaxX =640;
            int MinX = 0;
            int MaxY = 480;
            int MinY = 0;

            Vector2 moveVector = Vector2.Zero;
            if (inputSet_.getLeftDirectionalX() < 0 && position_.X > MinX)
            {
                moveVector.X -= 2.0F;// *GLOBALSPEEDMULTIPLIER;
            }
            if (inputSet_.getLeftDirectionalX() > 0 && position_.X < MaxX)
            {
                moveVector.X += 2.0F;// *GLOBALSPEEDMULTIPLIER;
            }
            if (inputSet_.getLeftDirectionalY() > 0 && position_.Y > MinY)
            {
                moveVector.Y -= 2.0F;// *GLOBALSPEEDMULTIPLIER;
            }
            if (inputSet_.getLeftDirectionalY() < 0 && position_.Y < MaxY)
            {
                moveVector.Y += 2.0F;// *GLOBALSPEEDMULTIPLIER;
            }
            float magnitude = (float)Math.Sqrt(moveVector.X * moveVector.X + moveVector.Y * moveVector.Y);
            if (magnitude > 0)
            {
                //System.Console.Out.WriteLine(moveVector);
                moveVector.X /= magnitude;
                moveVector.Y /= magnitude;
                //moveVector.X = (float)Math.Round(moveVector.X * GLOBALSPEEDMULTIPLIER);
                //moveVector.Y = (float)Math.Round(moveVector.Y * GLOBALSPEEDMULTIPLIER);
                //moveVector.X = moveVector.X;// *GLOBALSPEEDMULTIPLIER;
                //moveVector.Y = moveVector.Y;// *GLOBALSPEEDMULTIPLIER;
                //direction_.X = moveVector.X;
                //direction_.Y = moveVector.Y;
            }
            Vector2 rightD = new Vector2(inputSet_.getRightDirectionalX(), inputSet_.getRightDirectionalY());
            Console.Out.WriteLine(rightD);
            float rotationDirectional = (float)Math.Atan2(rightD.Y, rightD.X);
            float rotAngle = getRotationAngle();
            float rotDiff = rotAngle - rotationDirectional;
            if (Math.Abs(rotDiff) <= TURNSPEED)
            {
                direction_ = rightD;
            }
            else if (rotDiff < 0f)
            {
                rotAngle += TURNSPEED;
            }
            else
            {
                rotAngle -= TURNSPEED;
            }
            direction_.X = (float)Math.Cos((double)rotAngle);
            direction_.Y = (float)Math.Sin((double)rotAngle);
            position_ += moveVector;
        }
    }
}
