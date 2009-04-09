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
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Commando.graphics
{
    public class CharacterStayStillAction : CharacterActionInterface
    {
        private const int PRIORITY = 0;

        protected CharacterAbstract character_;

        protected int priority_;

        protected AnimationInterface[] animation_;

        protected List<string> actionLevels_;

        protected int numberAnimations_;

        protected int numberActionLevels_;

        protected int numberAnimationSets_;

        protected int[] animationsToDraw_;

        protected int numberAnimationsToDraw_;

        protected int highActionLevel_;

        protected int lowActionLevel_;

        public CharacterStayStillAction(CharacterAbstract character,
                                        AnimationInterface[] animation,
                                        List<string> actionLevels,
                                        string highActionLevel,
                                        string lowActionLevel)
        {
            priority_ = PRIORITY;
            character_ = character;
            animation_ = animation;
            actionLevels_ = actionLevels;
            numberAnimations_ = animation.GetLength(0);
            numberActionLevels_ = actionLevels_.Count;
            if (numberAnimations_ % numberActionLevels_!= 0)
            {
                throw new Exception("CharacterStayStillAction must have a number of animations\n" +
                                    "that is divisible by the number of action levels!");
            }
            animationsToDraw_ = new int[numberActionLevels_];
            numberAnimationsToDraw_ = 0;
            numberAnimationSets_ = numberAnimations_ / numberActionLevels_;
            highActionLevel_ = actionLevels_.IndexOf(highActionLevel) * numberAnimationSets_;
            lowActionLevel_ = actionLevels_.IndexOf(lowActionLevel) * numberAnimationSets_;
        }

        public void update()
        {
            Vector2 position = character_.getPosition();
            Vector2 direction = character_.getDirection();
            Vector2 velocity = Vector2.Zero;
            collisiondetection.CollisionDetectorInterface detector = character_.getCollisionDetector();
            if (detector != null)
            {
                detector.checkCollisions(character_, ref velocity, ref direction);
            }
            position.X += velocity.X;
            position.Y += velocity.Y;
            character_.setPosition(position);
            character_.setDirection(direction);
            foreach (AnimationInterface animation in animation_)
            {
                animation.setPosition(position);
                animation.setRotation(direction);
            }
        }

        public void update(string level)
        {
            Vector2 position = character_.getPosition();
            Vector2 direction = character_.getDirection();
            Vector2 velocity = Vector2.Zero;
            collisiondetection.CollisionDetectorInterface detector = character_.getCollisionDetector();
            if (detector != null)
            {
                detector.checkCollisions(character_, ref velocity, ref direction);
            }
            position.X += velocity.X;
            position.Y += velocity.Y;
            character_.setPosition(position);
            character_.setDirection(direction);
            int animationSet = 0;
            if (numberAnimations_ > numberActionLevels_)
            {
                animationSet = character_.getActuator().getCurrentAnimationSet();
            }
            int index = actionLevels_.IndexOf(level) * numberAnimationSets_ + animationSet;
            animation_[index].setPosition(position);
            animation_[index].setRotation(direction);
            animationsToDraw_[numberAnimationsToDraw_] = index;
            numberAnimationsToDraw_++;
            if (numberAnimationsToDraw_ > numberActionLevels_)
            {
                numberAnimationsToDraw_--;
            }
        }

        public void draw()
        {
            for (int i = 0; i < numberAnimationsToDraw_; i++)
            {
                animation_[animationsToDraw_[i]].draw();
            }
            numberAnimationsToDraw_ = 0;
        }

        public void draw(Color color)
        {
            for (int i = 0; i < numberAnimationsToDraw_; i++)
            {
                animation_[animationsToDraw_[i]].draw(color);
            }
            numberAnimationsToDraw_ = 0;
        }

        public bool isFinished()
        {
            return true;
        }

        public CharacterActionInterface interrupt(CharacterActionInterface newAction)
        {
            if (newAction == this || (newAction.getPriority() <= priority_))
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
            foreach (AnimationInterface anim in animation_)
            {
                anim.reset();
                anim.setPosition(character_.getPosition());
                anim.setRotation(character_.getDirection());
            }
        }

        public void reset()
        {
            numberAnimationsToDraw_ = 0;
        }

        public string getActionLevel()
        {
            return "all";
        }

        public void setParameters(ActionParameters parameters)
        {
            
        }

        public Commando.collisiondetection.ConvexPolygonInterface getBounds(Commando.levels.HeightEnum height)
        {
            int animationSet = 0;
            if (numberAnimations_ > numberActionLevels_)
            {
                animationSet = character_.getActuator().getCurrentAnimationSet();
            }
            if (height == Commando.levels.HeightEnum.HIGH)
            {
                return animation_[highActionLevel_ + animationSet].getBounds();
            }
            return animation_[lowActionLevel_ + animationSet].getBounds();
        }
    }
}
