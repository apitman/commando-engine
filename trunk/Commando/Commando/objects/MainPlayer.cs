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
using Commando.collisiondetection;

namespace Commando.objects
{
    /// <summary>
    /// The main player in the game.
    /// </summary>
    public class MainPlayer : PlayableCharacterAbstract, CollisionObjectInterface
    {
        const bool CONTROLSTYLE = false;

        const float TURNSPEED = .30f;

        const float RADIUS = 15.0f;

        protected float radius_;

        protected CollisionDetectorInterface collisionDetector_;

        /// <summary>
        /// Create the main player of the game.
        /// </summary>
        public MainPlayer() :
            base(new CharacterHealth(), new CharacterAmmo(), new CharacterWeapon(), "Woger Ru", null, 8.0f, Vector2.Zero, new Vector2(100.0f, 200.0f), new Vector2(1.0f,0.0f), 0.5f)
        {
            PlayerHelper.Player_ = this;

            List<GameTexture> anims = new List<GameTexture>();
            anims.Add(TextureMap.getInstance().getTexture("PlayerWalk"));
            animations_ = new AnimationSet(anims);
            radius_ = RADIUS;
            collisionDetector_ = new CollisionDetector(null);
        }

        public void setCollisionDetector(CollisionDetectorInterface detector)
        {
            collisionDetector_ = detector;
            collisionDetector_.register(this);
        }

        /// <summary>
        /// Draw the main player at his current position.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void draw(GameTime gameTime)
        {
            animations_.drawNextFrame(position_, getRotationAngle(), depth_);
        }
        
        /// <summary>
        /// Update the player's current position, animation, and action based on the 
        /// user input.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void update(GameTime gameTime)
        {

            int MaxX = 345;
            int MinX = 30;
            int MaxY = 300;
            int MinY = 30;
            Vector2 newPosition;
            if (Settings.getInstance().getMovementType() == MovementType.ABSOLUTE)
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
                newPosition = position_ + moveVector;
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
                newPosition = position_ + moveVector;
                if (newPosition.X < MinX)
                {
                    newPosition.X = MinX;
                }
                else if (newPosition.X > MaxX)
                {
                    newPosition.X = MaxX;
                }
                if (newPosition.Y < MinY)
                {
                    newPosition.Y = MinY;
                }
                else if (newPosition.Y > MaxY)
                {
                    newPosition.Y = MaxY;
                }
            }

            Vector2 newPos = collisionDetector_.checkCollisions(this, newPosition);
            moved_ += newPos - position_;
            position_ = newPos;
            updateFrameNumber();
            currentFrame_ = animations_.setNextFrame(currentFrame_);

            // TODO Change/fix how this is done, modularize it, etc.
            // Essentially, the player updates his visual location in the WorldState
            // Must remove before adding because Dictionaries don't like duplicate keys
            // Removing a nonexistent key (for first frame) does no harm
            // Also, need to make it so the radius isn't hardcoded - probably all
            //  objects which will have a visual stimulus should have a radius
            WorldState.Visual_.Remove(visualStimulusId_);
            WorldState.Visual_.Add(
                visualStimulusId_,
                new Stimulus(StimulusSource.CharacterAbstract, StimulusType.Position, 5, getPosition())
            );
        }

        public float getRadius()
        {
            return radius_;
        }
    }
}
