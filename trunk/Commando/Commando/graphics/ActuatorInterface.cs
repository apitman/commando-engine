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
using Commando.levels;
using Commando.collisiondetection;

namespace Commando.graphics
{
    public interface ActuatorInterface
    {
        void update();

        void addAction(CharacterActionInterface action);

        void draw();

        void draw(Color color);

        bool perform(String actionName, ActionParameters parameters);

        ConvexPolygonInterface getBounds(HeightEnum height);

        string getCurrentActionSet();

        void setCurrentActionSet(string actionSet);

        int getCurrentAnimationSet();

        void setCurrentAnimationSet(int animationSet);

        void setResource(int resourceid, Object resource);

        Object getResource(int resourceid);

        bool isFinished(string actionName);
    }

    public class InvalidActionSetException : System.ApplicationException
    {
        public InvalidActionSetException()
            : base()
        {
        }

        public InvalidActionSetException(string message)
            : base(message)
        {
        }

        public InvalidActionSetException(string message, System.Exception inner)
            : base(message, inner)
        {
        }

/*
        protected InvalidActionSetException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
        }
*/
    }

    public struct ActionParameters
    {
        public ActionParameters(Vector2 v1, Vector2 v2, Object o1, Object o2)
        {
            vector1 = v1;
            vector2 = v2;
            object1 = o1;
            object2 = o2;
        }

        public ActionParameters(Vector2 v1)
            : this(v1, Vector2.Zero, null, null)
        { }

        public ActionParameters(Object o1)
            : this(Vector2.Zero, Vector2.Zero, o1, null)
        { }

        public Vector2 vector1;
        public Vector2 vector2;
        public Object object1;
        public Object object2;
    }
}
