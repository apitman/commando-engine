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
using Commando.objects;
using Microsoft.Xna.Framework.Graphics;

namespace Commando.graphics
{
    public class CharacterRunAction : MoveActionInterface
    {
        private const int RUNPRIORITY = 10;

        protected float speed_;

        protected AnimationInterface animation_;

        protected Vector2 moved_;

        protected Vector2 runDirection_;

        protected int priority_;

        protected CharacterAbstract character_;

        protected bool finished_;

        public CharacterRunAction(CharacterAbstract character, AnimationInterface animation, float speed)
        {
            character_ = character;
            animation_ = animation;
            speed_ = speed;
            moved_ = Vector2.Zero;
            runDirection_ = Vector2.Zero;
            priority_ = RUNPRIORITY;
            finished_ = true;
        }

        public void move(Vector2 direction)
        {
            runDirection_ = direction;
            finished_ = false;
        }

        public void update()
        {
            Vector2 position = character_.getPosition();
            Vector2 direction = character_.getDirection();
            
            //Create movement Vector
            Vector2 moving = runDirection_;
            moving.X *= speed_;
            moving.Y *= speed_;

            Vector2 newPosition = position;
            newPosition.X += moving.X;
            newPosition.Y += moving.Y;

            /*
            // TODO: Implement slower movement backwards
            float moveDiff = (float)Math.Atan2(moving.Y, moving.X) - getRotationAngle();
            moveDiff = MathHelper.WrapAngle(moveDiff);
            moveVector *= (MathHelper.TwoPi - Math.Abs(moveDiff)) / MathHelper.Pi;
            */
            Console.Out.WriteLine("OldPosBeforeCollision: " + position);
            Console.Out.WriteLine("NewPosBeforeCollision: " + newPosition);

            newPosition = character_.getCollisionDetector().checkCollisions(character_, newPosition);

            Console.Out.WriteLine("NewPosAfterCollision: " + newPosition);

            animation_.update(newPosition, direction);
            character_.setPosition(newPosition);
            finished_ = true;
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

        public void draw()
        {
            animation_.draw();
        }

        public void draw(Color color)
        {
            animation_.draw(color);
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
            animation_.reset();
            animation_.setPosition(character_.getPosition());
            animation_.setRotation(character_.getDirection());
        }
    }
}
