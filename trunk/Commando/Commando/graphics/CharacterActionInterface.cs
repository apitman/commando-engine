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

namespace Commando.graphics
{
    public interface CharacterActionInterface
    {
        void update();

        void draw();

        void draw(Color color);

        bool isFinished();

        CharacterActionInterface interrupt(CharacterActionInterface newAction);

        int getPriority();

        //string getActionLevel();

        //void setParameters(ActionParameters parameters);

        void setCharacter(CharacterAbstract character);

        void start();

        //ConvexPolygonInterface getBounds(HeightEnum tileHeight);
    }
    /*
    public class InvalidActionParamtersException : System.ApplicationException
    {
        public InvalidActionParamtersException()
            : base()
        {
        }

        public InvalidActionParamtersException(string message)
            : base(message)
        {
        }

        public InvalidActionParamtersException(string message, System.Exception inner)
            : base(message, inner)
        {
        }


        protected InvalidActionParamtersException(System.Runtime.Serialization.SerializationInfo info, 
            System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
        }
    }
    */
}
