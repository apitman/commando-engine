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
using Commando.controls;

namespace Commando.objects
{
    class MainPlayer : PlayableCharacterAbstract
    {
        const bool CONTROLSTYLE = true;

        const float TURNSPEED = .30f;

        public MainPlayer() :
            base(new CharacterHealth(), new CharacterAmmo(), new CharacterWeapon(), "Woger Ru", null, 5.0f, Vector2.Zero, new Vector2(45.0f, 45.0f), new Vector2(1.0f,0.0f), 0.5f)
        {
            List<GameTexture> anims = new List<GameTexture>();
            anims.Add(TextureMap.getInstance().getTexture("SamplePlayer_Small"));
            animations_ = new AnimationSet(anims);
        }

        public override void draw(GameTime gameTime)
        {
            animations_.drawNextFrame(position_, getRotationAngle(), depth_);
        }
        
        public override void update(GameTime gameTime)
        {
            int MaxX = 345;
            int MinX = 30;
            int MaxY = 300;
            int MinY = 30;
            if (CONTROLSTYLE)
            {
                Vector2 moveVector = Vector2.Zero;
                if (inputSet_.getLeftDirectionalX() < 0 && position_.X > MinX)
                {
                    moveVector.X -= 1.0F;// *GLOBALSPEEDMULTIPLIER;
                }
                if (inputSet_.getLeftDirectionalX() > 0 && position_.X < MaxX)
                {
                    moveVector.X += 1.0F;// *GLOBALSPEEDMULTIPLIER;
                }
                if (inputSet_.getLeftDirectionalY() > 0 && position_.Y > MinY)
                {
                    moveVector.Y -= 1.0F;// *GLOBALSPEEDMULTIPLIER;
                }
                if (inputSet_.getLeftDirectionalY() < 0 && position_.Y < MaxY)
                {
                    moveVector.Y += 1.0F;// *GLOBALSPEEDMULTIPLIER;
                }
                float magnitude = (float)Math.Sqrt(moveVector.X * moveVector.X + moveVector.Y * moveVector.Y);
                if (magnitude > 0)
                {
                    //System.Console.Out.WriteLine(moveVector);
                    moveVector.X /= magnitude;
                    moveVector.Y /= magnitude;
                    moveVector *= 2.0f;
                    //moveVector.X = (float)Math.Round(moveVector.X * GLOBALSPEEDMULTIPLIER);
                    //moveVector.Y = (float)Math.Round(moveVector.Y * GLOBALSPEEDMULTIPLIER);
                    //moveVector.X = moveVector.X;// *GLOBALSPEEDMULTIPLIER;
                    //moveVector.Y = moveVector.Y;// *GLOBALSPEEDMULTIPLIER;
                    //direction_.X = moveVector.X;
                    //direction_.Y = moveVector.Y;
                }
                Vector2 rightD = new Vector2(inputSet_.getRightDirectionalX(), inputSet_.getRightDirectionalY());
                float rotAngle = getRotationAngle();
                if (rightD.LengthSquared() > 0)
                {

                    float rotationDirectional = (float)Math.Atan2(rightD.Y, rightD.X);

                    float rotDiff = MathHelper.WrapAngle(rotAngle - rotationDirectional);
                    if (Math.Abs(rotDiff) <= TURNSPEED || Math.Abs(rotDiff) >= MathHelper.TwoPi - TURNSPEED)
                    {
                        direction_ = rightD;
                    }
                    else if (rotDiff < 0f && rotDiff > -MathHelper.Pi)
                    {
                        rotAngle += TURNSPEED;
                        direction_.X = (float)Math.Cos((double)rotAngle);
                        direction_.Y = (float)Math.Sin((double)rotAngle);
                    }
                    else
                    {
                        rotAngle -= TURNSPEED;
                        direction_.X = (float)Math.Cos((double)rotAngle);
                        direction_.Y = (float)Math.Sin((double)rotAngle);
                    }
                }
                float moveDiff = (float)Math.Atan2(moveVector.Y, moveVector.X) - getRotationAngle();
                moveDiff = MathHelper.WrapAngle(moveDiff);
                moveVector *= (MathHelper.TwoPi - Math.Abs(moveDiff)) / MathHelper.Pi;
                position_ += moveVector;
            }
            else
            {
                Vector2 rightD = new Vector2(inputSet_.getRightDirectionalX(), inputSet_.getRightDirectionalY());
                float rotAngle = getRotationAngle();
                if (rightD.LengthSquared() > 0)
                {

                    float rotationDirectional = (float)Math.Atan2(rightD.Y, rightD.X);

                    float rotDiff = MathHelper.WrapAngle(rotAngle - rotationDirectional);
                    if (Math.Abs(rotDiff) <= TURNSPEED || Math.Abs(rotDiff) >= MathHelper.TwoPi - TURNSPEED)
                    {
                        direction_ = rightD;
                    }
                    else if (rotDiff < 0f)
                    {
                        rotAngle += TURNSPEED;
                        direction_.X = (float)Math.Cos((double)rotAngle);
                        direction_.Y = (float)Math.Sin((double)rotAngle);
                    }
                    else
                    {
                        rotAngle -= TURNSPEED;
                        direction_.X = (float)Math.Cos((double)rotAngle);
                        direction_.Y = (float)Math.Sin((double)rotAngle);
                    }
                }
                rotAngle = getRotationAngle();
                Vector2 moveVector = new Vector2(inputSet_.getLeftDirectionalY(), inputSet_.getLeftDirectionalX());
                float moveDiff = (float)Math.Atan2(moveVector.Y, moveVector.X);
                moveDiff = MathHelper.WrapAngle(moveDiff);
                moveVector *= (MathHelper.TwoPi - Math.Abs(moveDiff)) / MathHelper.Pi;
                float X = moveVector.X;
                float Y = moveVector.Y;
                moveVector.X = (float)Math.Cos((double)rotAngle) * X - (float)Math.Sin((double)rotAngle) * Y;
                moveVector.Y = (float)Math.Sin((double)rotAngle) * X + (float)Math.Cos((double)rotAngle) * Y;
                moveVector *= 2.0f;
                position_ += moveVector;
                if (position_.X < MinX)
                {
                    position_.X = MinX;
                }
                else if (position_.X > MaxX)
                {
                    position_.X = MaxX;
                }
                if (position_.Y < MinY)
                {
                    position_.Y = MinY;
                }
                else if (position_.Y > MaxY)
                {
                    position_.Y = MaxY;
                }
            }
        }
    }
}
