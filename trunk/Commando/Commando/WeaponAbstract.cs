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
using Commando.graphics;
using Commando.objects;
using Microsoft.Xna.Framework;
using Commando.collisiondetection;
using Commando.ai;
using Commando.levels;

namespace Commando
{
    public class WeaponAbstract
    {
        protected List<DrawableObjectAbstract> drawPipeline_;
        
        protected CharacterAbstract character_;

        protected GameTexture animation_;

        protected Vector2 position_;

        protected Vector2 rotation_;

        protected Vector2 gunHandle_;

        protected Vector2 guessTarget_;

        protected float gunLength_;

        protected int recoil_;

        protected int audialStimulusId_;

        protected bool weaponFired_;

        public WeaponAbstract(List<DrawableObjectAbstract> pipeline, CharacterAbstract character, GameTexture animation, Vector2 gunHandle)
        {
            drawPipeline_ = pipeline;
            character_ = character;
            animation_ = animation;
            gunHandle_ = gunHandle;
            position_ = Vector2.Zero;
            rotation_ = Vector2.Zero;
            gunLength_ = animation.getImageDimensions()[0].Width;
            recoil_ = 0;
            audialStimulusId_ = StimulusIDGenerator.getNext();
            weaponFired_ = false;
        }

        public void shoot(CollisionDetectorInterface detector)
        {
            if (recoil_ == 0 && character_.getAmmo().getValue() > 0)
            {
                List<Vector2> points = new List<Vector2>();
                points.Add(new Vector2(2f, 2f));
                points.Add(new Vector2(-2f, 2f));
                points.Add(new Vector2(-2f, -2f));
                points.Add(new Vector2(2f, -2f));
                rotation_.Normalize();
                Vector2 pos = position_ + rotation_ * 15f;
                Projectile bullet = new Projectile(TextureMap.getInstance().getTexture("Bullet"), detector, new ConvexPolygon(points, Vector2.Zero), 2.5f, rotation_ * 20.0f, pos, rotation_, 0.5f);
                drawPipeline_.Add(bullet);
                recoil_ = 10;
                character_.getAmmo().update(character_.getAmmo().getValue() - 1);


                weaponFired_ = true;
            }
        }

        public void update()
        {
            // TODO Change/fix how this is done, modularize it, etc.
            // Essentially, the player updates his visual location in the WorldState
            // Must remove before adding because Dictionaries don't like duplicate keys
            // Removing a nonexistent key (for first frame) does no harm
            // Also, need to make it so the radius isn't hardcoded - probably all
            //  objects which will have a visual stimulus should have a radius
            WorldState.Audial_.Remove(audialStimulusId_);
            if (weaponFired_)
            {
                weaponFired_ = false;
                WorldState.Audial_.Add(
                    audialStimulusId_,
                    new Stimulus(StimulusSource.CharacterAbstract, StimulusType.Position, 150.0f, character_.getPosition())
                );
            }

            Vector2 charPos = character_.getPosition();
            Vector2 newPos = Vector2.Zero;
            rotation_ = character_.getDirection();
            float angle = getRotationAngle();
            float cosA = (float)Math.Cos(angle);
            float sinA = (float)Math.Sin(angle);
            newPos.X = (gunHandle_.X) * cosA - (gunHandle_.Y) * sinA + charPos.X;
            newPos.Y = (gunHandle_.X) * sinA + (gunHandle_.Y) * cosA + charPos.Y;
            position_ = newPos;
            if (recoil_ > 0)
                recoil_--;

            guessTarget_ = Raycaster.roughCollision(position_, rotation_, new Height(false, true));
        }

        public void draw()
        {
            rotation_.Normalize();
            rotation_ *= gunLength_ / 2f;
            animation_.drawImage(0, position_ + rotation_, getRotationAngle(), 0.6f);

            Illustrator.drawLine(position_, guessTarget_);
        }

        protected float getRotationAngle()
        {
            return (float)Math.Atan2((double)rotation_.Y, (double)rotation_.X);
        }
    }
}
