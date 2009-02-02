using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Commando.objects
{
    class HumanActuator : ActuatorInterface
    {

        public Vector2 Velocity_ { get; private set; }
        public Vector2 Force_ { get; private set; }

        protected Vector2 moveTarget_;
        protected Vector2 lookTarget_;

        protected GameObject owner_;

        protected bool atLocation_;

        const float MAX_VEL = 2.0f;

        public HumanActuator(GameObject owner)
        {
            owner_ = owner;
        }

        public void moveTo(Vector2 position)
        {
            moveTarget_ = position;
        }

        public void lookAt(Vector2 position)
        {
            lookTarget_ = position;
            atLocation_ = false;
        }

        public void push(Vector2 force)
        {
            Force_ += force;
        }

        public void update()
        {
            if (!atLocation_)
            {
                Vector2 move = moveTarget_ - getOwner().Position_;
                if (move.Length() > MAX_VEL)
                {
                    move.Normalize();
                    move *= MAX_VEL;
                }
                getOwner().Position_ += move;
                if (getOwner().Position_.Equals(moveTarget_))
                {
                    atLocation_ = true;
                }
            }

            
        }

        public GameObject getOwner()
        {
            return owner_;
        }
    }
}
