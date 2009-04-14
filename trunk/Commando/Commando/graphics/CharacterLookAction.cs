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

namespace Commando.graphics
{
    class CharacterLookAction : CharacterActionInterface
    {
        protected const int PRIORITY = 2;

        protected const float TURNSPEED = 0.3f;

        protected CharacterAbstract character_;

        protected Vector2 newDirection_;

        protected Vector2 direction_;

        protected bool finished_;

        protected int priority_;

        protected string actionLevel_;

        public CharacterLookAction(CharacterAbstract character,
                                    string actionLevel)
        {
            character_ = character;
            actionLevel_ = actionLevel;
            priority_ = PRIORITY;
            newDirection_ = Vector2.Zero;
            direction_ = Vector2.Zero;
            finished_ = true;
        }

        public void update()
        {
            if (direction_ != Vector2.Zero)
            {

                setSlowRotationAngle(direction_);
                //character_.setDirection(direction);
                //newDirection_ = direction;
                Vector2 temp = Vector2.Zero;
                character_.getCollisionDetector().checkCollisions(character_, ref temp, ref newDirection_);
                character_.setDirection(newDirection_);
                character_.setPosition(character_.getPosition() + temp);
            }
            if (newDirection_ == direction_)
            {
                finished_ = true;
            }
        }

        public void draw()
        {
            
        }

        public void draw(Microsoft.Xna.Framework.Graphics.Color color)
        {
            
        }

        public bool isFinished()
        {
            return finished_;
        }

        public CharacterActionInterface interrupt(CharacterActionInterface newAction)
        {
            if (newAction == this || (newAction.getPriority() <= priority_ && !newAction.isFinished()))
            {
                return this;
            }
            newAction.start();
            return newAction;
        }

        public int getPriority()
        {
            return priority_;
        }

        public string getActionLevel()
        {
            return actionLevel_;
        }

        public void setParameters(ActionParameters parameters)
        {
            direction_ = parameters.vector1;
        }

        public void setCharacter(CharacterAbstract character)
        {
            character_ = character;
        }

        public void start()
        {
            finished_ = false;
        }

        public Commando.collisiondetection.ConvexPolygonInterface getBounds(Commando.levels.HeightEnum height)
        {
            throw new NotImplementedException();
        }

        protected void setSlowRotationAngle(Vector2 newDirection)
        {
            float rotationDirectional = (float)Math.Atan2(newDirection.Y, newDirection.X);
            float rotAngle = character_.getRotationAngle();

            float rotDiff = MathHelper.WrapAngle(rotAngle - rotationDirectional);
            if (Math.Abs(rotDiff) <= TURNSPEED || Math.Abs(rotDiff) >= MathHelper.TwoPi - TURNSPEED)
            {
                newDirection_ = newDirection;
            }
            else if (rotDiff < 0f && rotDiff > -MathHelper.Pi)
            {
                rotAngle += TURNSPEED;
                newDirection_.X = (float)Math.Cos((double)rotAngle);
                newDirection_.Y = (float)Math.Sin((double)rotAngle);
            }
            else
            {
                rotAngle -= TURNSPEED;
                newDirection_.X = (float)Math.Cos((double)rotAngle);
                newDirection_.Y = (float)Math.Sin((double)rotAngle);
            }
        }
    }
}
