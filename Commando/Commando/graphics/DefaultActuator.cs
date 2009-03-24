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
using Microsoft.Xna.Framework.Graphics;
using Commando.objects;

namespace Commando.graphics
{
    public class DefaultActuator : ActuatorInterface
    {
        protected Dictionary<string, Dictionary<string, CharacterActionInterface>> actions_;

        protected CharacterActionInterface currentAction_;

        protected string currentActionSet_;

        protected CharacterAbstract character_;

        protected Vector2 newDirection_;

        protected const float TURNSPEED = 0.3f;

        protected CoverObject currentCoverObject_ = null;

        public DefaultActuator(Dictionary<string, Dictionary<string, CharacterActionInterface>> actions, CharacterAbstract character, string initialActionSet)
        {
            if (ActionSetValidator.validate(actions))
            {
                actions_ = actions;
            }
            else
            {
                throw new InvalidActionSetException("This action set is invalid for the DefaultActuator");
            }
            character_ = character;
            currentActionSet_ = initialActionSet;
            currentAction_ = actions_[currentActionSet_]["rest"];
            currentAction_.update();
        }

        public void update()
        {
            Vector2 oldPos = character_.getPosition();
            if (!currentAction_.isFinished())
            {
                currentAction_.update();
            }
            else
            {
                currentAction_ = actions_[currentActionSet_]["rest"];
                currentAction_.update();
            }
            character_.setVelocity(character_.getPosition() - oldPos);
        }

        public void setCurrentActionSet(String actionSet)
        {
            if (actions_.ContainsKey(actionSet))
            {
                currentActionSet_ = actionSet;
            }
        }

        public void addAction(CharacterActionInterface action)
        {
            throw new NotImplementedException();
        }

        public void draw()
        {
            currentAction_.draw();
        }

        public void draw(Color color)
        {
            currentAction_.draw(color);
        }

        public void crouch()
        {
            CrouchAction crouch = (CrouchAction)actions_[currentActionSet_]["crouch"];
            currentAction_ = currentAction_.interrupt(crouch);
        }

        public void shoot(RangedWeaponAbstract weapon)
        {
            ShootActionInterface shoot = (ShootActionInterface)actions_[currentActionSet_]["shoot"];
            shoot.shoot(weapon);
            currentAction_ = currentAction_.interrupt(shoot);
        }

        public void move(Vector2 direction)
        {
            MoveActionInterface move = (MoveActionInterface)actions_[currentActionSet_]["move"];
            move.move(direction);
            /*
            //Or for efficiency's sake
            CharacterActionInterface move = actions_[currentActionSet_]["move"];
            (move as MoveActionInterface).move(direction);
            */
            currentAction_ = currentAction_.interrupt(move);
        }

        public void moveTo(Vector2 location)
        {
            MoveToActionInterface moveTo = (MoveToActionInterface)actions_[currentActionSet_]["moveTo"];
            moveTo.moveTo(location);
            /*
            //Or for efficiency's sake
            CharacterActionInterface move = actions_[currentActionSet_]["move"];
            (move as MoveActionInterface).move(direction);
            */
            currentAction_ = currentAction_.interrupt(moveTo);
        }

        public void lookAt(Vector2 location)
        {
            Vector2 position = character_.getPosition();
            //character_.setDirection(new Vector2(location.X - position.X, location.Y - position.Y));
            newDirection_ = new Vector2(location.X - position.X, location.Y - position.Y);
            //setSlowRotationAngle(new Vector2(location.X - position.X, location.Y - position.Y));
            Vector2 temp = Vector2.Zero;
            character_.getCollisionDetector().checkCollisions(character_, ref temp, ref newDirection_);
            character_.setDirection(newDirection_);
            character_.setPosition(character_.getPosition() + temp);
        }

        public void look(Vector2 direction)
        {
            if (direction != Vector2.Zero)
            {
                /*
                float rotationDirectional = (float)Math.Atan2(direction.Y, direction.X);
                float rotAngle = character_.getRotationAngle();

                float rotDiff = MathHelper.WrapAngle(rotAngle - rotationDirectional);
                if (Math.Abs(rotDiff) <= TURNSPEED || Math.Abs(rotDiff) >= MathHelper.TwoPi - TURNSPEED)
                {
                    newDirection_ = direction;
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
                */
                setSlowRotationAngle(direction);

                //character_.setDirection(direction);
                //newDirection_ = direction;
                Vector2 temp = Vector2.Zero;
                character_.getCollisionDetector().checkCollisions(character_, ref temp, ref newDirection_);
                character_.setDirection(newDirection_);
                character_.setPosition(character_.getPosition() + temp);
            }
        }

        public void setCoverObject(CoverObject coverObject)
        {
            currentCoverObject_ = coverObject;
        }

        public CoverObject getCoverObject()
        {
            return currentCoverObject_;
        }

        public void cover(CoverObject coverObject)
        {
            CoverActionInterface cover = (CoverActionInterface)actions_[currentActionSet_]["cover"];
            cover.setCover(coverObject);
            currentAction_ = currentAction_.interrupt(cover);
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
