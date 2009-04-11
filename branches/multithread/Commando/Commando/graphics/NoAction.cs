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

namespace Commando.graphics
{
    public class NoAction : CharacterActionInterface
    {
        private const int PRIORITY = -100;

        protected int priority_;

        protected string actionLevel_;

        public NoAction(string actionLevel)
        {
            actionLevel_ = actionLevel;
            priority_ = PRIORITY;
        }

        public void update()
        {
            
        }

        public void draw()
        {
            
        }

        public void draw(Microsoft.Xna.Framework.Graphics.Color color)
        {
            
        }

        public bool isFinished()
        {
            return true;
        }

        public CharacterActionInterface interrupt(CharacterActionInterface newAction)
        {
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
            
        }

        public void setCharacter(CharacterAbstract character)
        {
            
        }

        public void start()
        {
            
        }

        public Commando.collisiondetection.ConvexPolygonInterface getBounds(Commando.levels.HeightEnum height)
        {
            throw new NotImplementedException();
        }
    }
}
