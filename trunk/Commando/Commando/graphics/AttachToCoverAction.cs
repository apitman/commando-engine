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
using Commando.objects;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Commando.collisiondetection;
using Commando.levels;

namespace Commando.graphics
{
    public class AttachToCoverAction : CoverActionInterface
    {
        private const int PRIORITY = 15;

        private const float TURNSPEED = 0.8f;

        protected CharacterAbstract character_;

        protected int priority_;

        protected AnimationInterface animation_;
        
        protected CoverObject coverObject_;

        protected Vector2 positionToMoveTo_;

        protected bool finished_;

        protected float speed_;

        protected String nextActionSet_;

        protected Height nextHeight_;

        public AttachToCoverAction(CharacterAbstract character, AnimationInterface animation, String nextActionSet, Height nextHeight, float speed)
        {
            character_ = character;
            animation_ = animation;
            priority_ = PRIORITY;
            coverObject_ = null;
            positionToMoveTo_ = Vector2.Zero;
            finished_ = false;
            speed_ = speed;
            nextActionSet_ = nextActionSet;
            nextHeight_ = nextHeight;
        }
   
        public void setCover(CoverObject coverObject)
        {
            coverObject_ = coverObject;
        }

        public void update()
        {
            Vector2 position = character_.getPosition();
            Vector2 direction = character_.getDirection();

            //Create movement Vector
            Vector2 moving = positionToMoveTo_;
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

            if (newPosition == positionToMoveTo_)
            {
                finished_ = true;
                (character_.getActuator() as DefaultActuator).setCoverObject(coverObject_);
                (character_.getActuator() as DefaultActuator).setCurrentActionSet(nextActionSet_);
                character_.setHeight(nextHeight_);
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
            if (newAction == this)
            {
                finished_ = true;
                return this;
            }
            if (newAction.getPriority() <= priority_ && !finished_)
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
            Vector2 position = character_.getPosition();
            positionToMoveTo_ = coverObject_.needsToMove(position, character_.getRadius());
            finished_ = false;
            animation_.reset();
            animation_.setPosition(position);
            animation_.setRotation(character_.getDirection());
        }
    }
}
