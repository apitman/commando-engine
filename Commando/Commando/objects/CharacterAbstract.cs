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
using Commando.collisiondetection;
using Commando.graphics;
using Commando.levels;

namespace Commando
{
    /// <summary>
    /// CharacterAbstract inherits from AnimatedObjectAbstract and is an ancestor of all 
    /// playable and non-playable characters.
    /// </summary>
    public abstract class CharacterAbstract : AnimatedObjectAbstract, CollisionObjectInterface
    {

        protected CharacterHealth health_;

        protected CharacterAmmo ammo_;

        protected CharacterWeapon weapon_;

        public WeaponAbstract Weapon_ { get; protected set; }

        protected string name_;

        protected CollisionDetectorInterface collisionDetector_;

        protected List<CollisionObjectInterface> collidedWith_ = new List<CollisionObjectInterface>();

        protected List<CollisionObjectInterface> collidedInto_ = new List<CollisionObjectInterface>();

        protected Height height_;

        /// <summary>
        /// Create a default Character
        /// </summary>
        public CharacterAbstract() :
            this(new CharacterHealth(), new CharacterAmmo(), new CharacterWeapon(), "", null)
        {
        }

        /// <summary>
        /// Create a Character with the specified health, ammo, and weapon objects, plus the given name
        /// </summary>
        /// <param name="health">CharacterStatusElement for health</param>
        /// <param name="ammo">CharacterStatusElement for ammo</param>
        /// <param name="weapon">CharacterStatusElement for the current weapon</param>
        /// <param name="name">The character's name</param>
        public CharacterAbstract(CharacterHealth health, CharacterAmmo ammo, CharacterWeapon weapon, string name, CollisionDetectorInterface collisionDetector) :
            base()
        {
            health_ = health;
            ammo_ = ammo;
            weapon_ = weapon;
            name_ = name;
            collisionDetector_ = collisionDetector;
            if (collisionDetector_ != null)
            {
                collisionDetector_.register(this);
            }
            height_ = new Height(true, true);
        }

        /// <summary>
        /// Create a Character with the specified health, ammo, and weapon objects, plus the given name.
        /// Also, specify the AnimationSet, frameLengthModifier, velocity, position, direction,
        /// and depth of the character.
        /// </summary>
        /// <param name="health">CharacterStatusElement for health</param>
        /// <param name="ammo">CharacterStatusElement for ammo</param>
        /// <param name="weapon">CharacterStatusElement for the current weapon</param>
        /// <param name="name">The character's name</param>
        /// <param name="animations">AnimationSet containing all animations for this object</param>
        /// <param name="frameLengthModifier">Float representing the ratio of frames in an animation to movement along the screen</param>
        /// <param name="velocity">Vector of velocity, representing both direction of movement and magnitude</param>
        /// <param name="position">Position of object relative to the top left corner</param>
        /// <param name="direction">Vector representing the direction of the object</param>
        /// <param name="depth">Depth the object is to be drawn to</param>
        public CharacterAbstract(List<DrawableObjectAbstract> pipeline, CharacterHealth health, CharacterAmmo ammo, CharacterWeapon weapon, string name, CollisionDetectorInterface collisionDetector, AnimationSet animations, float frameLengthModifier, Vector2 velocity, Vector2 position, Vector2 direction, float depth) :
            base(pipeline, animations, frameLengthModifier, velocity, position, direction, depth)
        {
            health_ = health;
            ammo_ = ammo;
            weapon_ = weapon;
            name_ = name;
            collisionDetector_ = collisionDetector;
            if (collisionDetector_ != null)
            {
                collisionDetector_.register(this);
            }
            height_ = new Height(true, true);
        }

        public CharacterHealth getHealth()
        {
            return health_;
        }

        public CharacterAmmo getAmmo()
        {
            return ammo_;
        }

        public CharacterWeapon getWeapon()
        {
            return weapon_;
        }

        public string getName()
        {
            return name_;
        }

        public CollisionDetectorInterface getCollisionDetector()
        {
            return collisionDetector_;
        }

        public Height getHeight()
        {
            return height_;
        }

        public abstract ActuatorInterface getActuator();

        public void setHealth(CharacterHealth health)
        {
            health_ = health;
        }

        public void setAmmo(CharacterAmmo ammo)
        {
            ammo_ = ammo;
        }

        public void setWeapon(CharacterWeapon weapon)
        {
            weapon_ = weapon;
        }

        public void setName(string name)
        {
            name_ = name;
        }

        public void setCollisionDetector(CollisionDetectorInterface collisionDetector)
        {
            collisionDetector_ = collisionDetector;
            if (collisionDetector_ != null)
            {
                collisionDetector_.register(this);
            }
        }

        public virtual void collidedWith(CollisionObjectInterface obj)
        {
            collidedWith_.Add(obj);
        }

        public virtual void collidedInto(CollisionObjectInterface obj)
        {
            collidedInto_.Add(obj);
        }

        public virtual Vector2 checkCollisionWith(CollisionObjectInterface obj, CollisionDetectorInterface detector, HeightEnum height, float radDistance, Vector2 velocity)
        {
            return detector.checkCollision(obj.getBounds(height), getBounds(height), radDistance, velocity);
        }

        public virtual Vector2 checkCollisionInto(CollisionObjectInterface obj, CollisionDetectorInterface detector, Height height, float radDistance, Vector2 translate)
        {
            return translate;
        }

        public abstract void damage(int amount, CollisionObjectInterface obj);

        public abstract float getRadius();

        public abstract ConvexPolygonInterface getBounds(HeightEnum height);

        public override void die()
        {
            if (collisionDetector_ != null)
            {
                collisionDetector_.remove(this);
            }
            base.die();
        }
    }
}
