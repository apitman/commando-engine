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
using Commando.collisiondetection;
using Commando.objects;
using Microsoft.Xna.Framework.Graphics;

namespace Commando.graphics
{
    class CharacterCoverMoveToAction : CharacterActionInterface
    {
        private const int PRIORITY = 10;

        private const float TURNSPEED = 0.8f;

        private static readonly int COVERKEY;

        protected CharacterAbstract character_;

        protected int priority_;

        protected AnimationInterface animation_;

        protected Vector2 moveTo_;

        protected float speed_;

        protected bool finished_;

        protected string actionLevel_;

        static CharacterCoverMoveToAction()
        {
            COVERKEY = "cover".GetHashCode();
        }

        public CharacterCoverMoveToAction(CharacterAbstract character, 
                                        AnimationInterface animation, 
                                        float speed,
                                        string actionLevel)
        {
            priority_ = PRIORITY;
            character_ = character;
            animation_ = animation;
            speed_ = speed;
            moveTo_ = Vector2.Zero;
            finished_ = true;
            actionLevel_ = actionLevel;
        }

        public void update()
        {
            Vector2 position = character_.getPosition();
            Vector2 direction = character_.getDirection();

            //Create movement Vector
            Vector2 moving = moveTo_;
            moving.X -= position.X;
            moving.Y -= position.Y;
            if (moving.Length() > speed_)
            {
                moving.Normalize();
                moving.X *= speed_;
                moving.Y *= speed_;
            }

            Vector2 newPosition = position;
            //newPosition.X += moving.X;
            //newPosition.Y += moving.Y;

            /*
            // TODO: Implement slower movement backwards
            float moveDiff = (float)Math.Atan2(moving.Y, moving.X) - getRotationAngle();
            moveDiff = MathHelper.WrapAngle(moveDiff);
            moveVector *= (MathHelper.TwoPi - Math.Abs(moveDiff)) / MathHelper.Pi;
            */

            //newPosition = character_.getCollisionDetector().checkCollisions(character_, newPosition);
            character_.getCollisionDetector().checkCollisions(character_, ref moving, ref direction);

            newPosition.X += moving.X;
            newPosition.Y += moving.Y;

            if (newPosition == moveTo_)
            {
                finished_ = true;
            }

            animation_.update(newPosition, direction);
            character_.setPosition(newPosition);
        }

        public void draw()
        {
            animation_.draw();
        }

        public void draw(Color color)
        {
            animation_.draw(color);
        }

        public bool isFinished()
        {
            return finished_;
        }

        public CharacterActionInterface interrupt(CharacterActionInterface newAction)
        {
            if (newAction == this || newAction.getPriority() <= priority_)
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

        public void setCharacter(CharacterAbstract character)
        {
            character_ = character;
        }

        public void start()
        {
            finished_ = false;
            animation_.reset();
            animation_.setPosition(character_.getPosition());
            animation_.setRotation(character_.getDirection());
        }

        public string getActionLevel()
        {
            return actionLevel_;
        }

        public void setParameters(ActionParameters parameters)
        {
            Vector2 location = parameters.vector1;
            CoverObject cover = (CoverObject)character_.getActuator().getResource(COVERKEY);
            Vector2 position = character_.getPosition();
            Vector2 dir = cover.needsToFace(position);
            if (cover.needsToFace(location) != dir)
            {
                if (dir.Y != 0)
                {
                    location.Y = position.Y;
                }
                else
                {
                    location.X = position.X;
                }
            }
            moveTo_ = cover.needsToMove(location, character_.getRadius());
        }

        public ConvexPolygonInterface getBounds(Commando.levels.HeightEnum height)
        {
            return animation_.getBounds();
        }
    }
}
