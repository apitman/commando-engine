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
using Commando.collisiondetection;
using Commando.levels;

namespace Commando.graphics
{
    public class CharacterRunAction : CharacterActionInterface
    {
        private const int RUNPRIORITY = 10;

        protected float speed_;

        protected AnimationInterface animation_;

        protected Vector2 moved_;

        protected Vector2 runDirection_;

        protected int priority_;

        protected CharacterAbstract character_;

        protected bool finished_;

        protected string actionLevel_;

        public CharacterRunAction(CharacterAbstract character,
                                    AnimationInterface animation,
                                    float speed,
                                    string actionLevel)
        {
            character_ = character;
            animation_ = animation;
            speed_ = speed;
            moved_ = Vector2.Zero;
            runDirection_ = Vector2.Zero;
            priority_ = RUNPRIORITY;
            finished_ = true;
            actionLevel_ = actionLevel;
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
            
            /*
            // TODO: Implement slower movement backwards
            float moveDiff = (float)Math.Atan2(moving.Y, moving.X) - getRotationAngle();
            moveDiff = MathHelper.WrapAngle(moveDiff);
            moveVector *= (MathHelper.TwoPi - Math.Abs(moveDiff)) / MathHelper.Pi;
            */

            character_.getCollisionDetector().checkCollisions(character_, ref moving, ref direction);
            
            newPosition.X += moving.X;
            newPosition.Y += moving.Y;

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
            if (newAction == this || (newAction.getPriority() <= priority_ && !newAction.isFinished()))
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

        public string getActionLevel()
        {
            return actionLevel_;
        }

        public void setParameters(ActionParameters parameters)
        {
            runDirection_ = parameters.vector1;
            finished_ = false;
        }

        public ConvexPolygonInterface getBounds(HeightEnum height)
        {
            return animation_.getBounds();
        }
    }
}
