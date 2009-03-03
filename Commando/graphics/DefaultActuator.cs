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

namespace Commando.graphics
{
    public class DefaultActuator : ActuatorInterface
    {
        protected Dictionary<string, Dictionary<string, CharacterActionInterface>> actions_;

        protected CharacterActionInterface currentAction_;

        protected string currentActionSet_;

        protected CharacterAbstract character_;

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
        }

        public void update()
        {
            if (!currentAction_.isFinished())
            {
                currentAction_.update();
            }
            else
            {
                currentAction_ = actions_[currentActionSet_]["rest"];
                currentAction_.update();
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
            character_.setDirection(new Vector2(location.X - position.X, location.Y - position.Y));
        }

        public void look(Vector2 direction)
        {
            if (direction != Vector2.Zero)
            {
                character_.setDirection(direction);
            }
        }
    }
}
