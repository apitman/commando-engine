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
using Commando.ai;

namespace Commando.objects
{
    public class PlayerActuator : ActuatorInterface
    {
        protected GameObject owner_;

        protected InputSet inputSet_;

        protected const float TURNSPEED = .30f;

        protected const bool CONTROLSTYLE = true;

        public PlayerActuator(GameObject owner)
        {
            owner_ = owner;
            inputSet_ = InputSet.getInstance();
        }

        #region ActuatorInterface Members

        public void moveTo(Vector2 position)
        {
            
        }

        public void lookAt(Vector2 position)
        {
            
        }

        public void push(Vector2 force)
        {
            
        }

        #endregion

        #region ComponentInterface Members

        public void update()
        {
            int MaxX = 345;
            int MinX = 30;
            int MaxY = 300;
            int MinY = 30;
            Vector2 position = owner_.getPosition();
            Vector2 direction = owner_.getDirection();
            if (CONTROLSTYLE)
            {
                Vector2 moveVector = Vector2.Zero;
                if (inputSet_.getLeftDirectionalX() < 0 && position.X > MinX)
                {
                    moveVector.X -= 1.0F;// *GLOBALSPEEDMULTIPLIER;
                }
                if (inputSet_.getLeftDirectionalX() > 0 && position.X < MaxX)
                {
                    moveVector.X += 1.0F;// *GLOBALSPEEDMULTIPLIER;
                }
                if (inputSet_.getLeftDirectionalY() > 0 && position.Y > MinY)
                {
                    moveVector.Y -= 1.0F;// *GLOBALSPEEDMULTIPLIER;
                }
                if (inputSet_.getLeftDirectionalY() < 0 && position.Y < MaxY)
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
                        direction = rightD;
                    }
                    else if (rotDiff < 0f && rotDiff > -MathHelper.Pi)
                    {
                        rotAngle += TURNSPEED;
                        direction.X = (float)Math.Cos((double)rotAngle);
                        direction.Y = (float)Math.Sin((double)rotAngle);
                    }
                    else
                    {
                        rotAngle -= TURNSPEED;
                        direction.X = (float)Math.Cos((double)rotAngle);
                        direction.Y = (float)Math.Sin((double)rotAngle);
                    }
                }
                float moveDiff = (float)Math.Atan2(moveVector.Y, moveVector.X) - getRotationAngle();
                moveDiff = MathHelper.WrapAngle(moveDiff);
                moveVector *= (MathHelper.TwoPi - Math.Abs(moveDiff)) / MathHelper.Pi;
                position += moveVector;

                owner_.setNewPosition(position);
                owner_.setDirection(direction);
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
                        direction = rightD;
                    }
                    else if (rotDiff < 0f)
                    {
                        rotAngle += TURNSPEED;
                        direction.X = (float)Math.Cos((double)rotAngle);
                        direction.Y = (float)Math.Sin((double)rotAngle);
                    }
                    else
                    {
                        rotAngle -= TURNSPEED;
                        direction.X = (float)Math.Cos((double)rotAngle);
                        direction.Y = (float)Math.Sin((double)rotAngle);
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
                position += moveVector;
                if (position.X < MinX)
                {
                    position.X = MinX;
                }
                else if (position.X > MaxX)
                {
                    position.X = MaxX;
                }
                if (position.Y < MinY)
                {
                    position.Y = MinY;
                }
                else if (position.Y > MaxY)
                {
                    position.Y = MaxY;
                }
            }

            // TODO Change/fix how this is done, modularize it, etc.
            // Essentially, the player updates his visual location in the WorldState
            // Must remove before adding because Dictionaries don't like duplicate keys
            // Removing a nonexistent key (for first frame) does no harm
            // Also, need to make it so the radius isn't hardcoded - probably all
            //  objects which will have a visual stimulus should have a radius
            
            /*WorldState.Visual_.Remove(visualStimulusId_);
            WorldState.Visual_.Add(
                visualStimulusId_,
                new Stimulus(StimulusSource.CharacterAbstract, StimulusType.Position, 5, getPosition())
            );*/
        }

        public GameObject getOwner()
        {
            return owner_;
        }

        #endregion

        /// <summary>
        /// Convert the direction vector into a float representing the angle of the object
        /// </summary>
        /// <returns>Angle of the rotation in radians</returns>
        protected float getRotationAngle()
        {
            return (float)Math.Atan2((double)owner_.getDirection().Y, (double)owner_.getDirection().X);
        }
    }
}
