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
using Commando.collisiondetection;
using Commando.levels;
using Microsoft.Xna.Framework;

namespace Commando.graphics
{
    public class MultiLevelActuator : ActuatorInterface
    {
        private const string RESTINGSTATE = "rest";

        protected Dictionary<string, Dictionary<string, CharacterActionInterface>> actions_;

        protected string currentActionSet_;

        protected int currentAnimationSet_;
        
        protected Dictionary<string, CharacterActionInterface> currentActions_;

        protected List<string> actionLevels_;

        protected CharacterAbstract character_;

        protected string restingAction_;

        protected string highActionLevel_;

        protected string lowActionLevel_;

        public MultiLevelActuator(Dictionary<string, Dictionary<string, CharacterActionInterface>> actions, List<string> actionLevels, CharacterAbstract character, string initialActionSet, string initialAction, string lowActionLevel, string highActionLevel)
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
            actionLevels_ = actionLevels;
            CharacterActionInterface initAction = actions_[currentActionSet_][initialAction];
            foreach (string s in actionLevels_)
            {
                currentActions_.Add(s, initAction);
            }
            restingAction_ = RESTINGSTATE;
            lowActionLevel_ = lowActionLevel;
            highActionLevel_ = highActionLevel;
        }

        public MultiLevelActuator(Dictionary<string, Dictionary<string, CharacterActionInterface>> actions, List<string> actionLevels, CharacterAbstract character, string initialActionSet)
            : this(actions, actionLevels, character, initialActionSet, RESTINGSTATE, actionLevels[0], "")
        {
            if (actionLevels.Count > 1)
            {
                highActionLevel_ = actionLevels_[1];
            }
        }

        public void update()
        {
            Vector2 oldPos = character_.getPosition();
            CharacterActionInterface curAction;
            foreach (string curLevel in actionLevels_)
            {
                curAction = currentActions_[curLevel];
                if (curAction.isFinished())
                {
                    currentActions_[curLevel] = actions_[currentActionSet_][restingAction_];
                    (currentActions_[curLevel] as CharacterStayStillAction).update(curLevel);
                }
                else
                {
                    curAction.update();
                }
            }
            character_.setVelocity(character_.getPosition() - oldPos);
        }

        public void addAction(CharacterActionInterface action)
        {
            throw new NotImplementedException();
        }

        public void draw()
        {
            foreach (string curLevel in actionLevels_)
            {
                currentActions_[curLevel].draw();
            }
        }

        public void draw(Color color)
        {
            foreach (string curLevel in actionLevels_)
            {
                currentActions_[curLevel].draw(color);
            }
        }

        public bool perform(String actionName, ActionParameters parameters)
        {
            if (!actions_[currentActionSet_].ContainsKey(actionName))
            {
                return false;
            }
            CharacterActionInterface action = actions_[currentActionSet_][actionName];
            action.setParameters(parameters);
            string actionLevel = action.getActionLevel();
            currentActions_[actionLevel] = currentActions_[actionLevel].interrupt(action);
            if (currentActions_[actionLevel] == action)
            {
                return true;
            }
            return false;
        }

        public ConvexPolygonInterface getBounds(HeightEnum height)
        {
            if (height == HeightEnum.HIGH)
            {
                return currentActions_[highActionLevel_].getBounds(height);
            }
            return currentActions_[lowActionLevel_].getBounds(height);
        }
    }

    public struct ActionParameters
    {
        public Vector2 vector1;
        public Vector2 vector2;
        public Object object1;
        public Object object2;

    }
}
