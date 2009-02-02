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

namespace Commando.objects
{
    public class BasicActuator : ActuatorInterface
    {
        protected float maxVelocity_;

        protected Vector2 movingToward_;

        protected Vector2 lookingAt_;

        protected GameObject owner_;

        public BasicActuator(GameObject owner, float maxVelocity)
        {
            owner_ = owner;
            maxVelocity_ = maxVelocity;
        }

        #region ActuatorInterface Members

        public void moveTo(Vector2 position)
        {
            movingToward_ = position;
        }

        public void lookAt(Vector2 position)
        {
            lookingAt_ = position;
        }

        public void push(Vector2 force)
        {
        }

        #endregion

        #region ComponentInterface Members

        public void update()
        {
            Vector2 position = owner_.getPosition();
            Vector2 move = movingToward_ - position;
            if (move.Length() > maxVelocity_)
            {
                move.Normalize();
                move *= maxVelocity_;
            }
            Vector2 newP = position + move;
            owner_.setNewPosition(newP);

            owner_.setDirection(lookingAt_ - newP);
        }

        public GameObject getOwner()
        {
            return owner_;
        }

        #endregion
    }
}
