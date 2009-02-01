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
using Commando.ai;

namespace Commando.objects
{
    public class DummyEnemy : NonPlayableCharacterAbstract
    {
        private const float FRAMELENGTHMODIFIER = 4.0f;

        protected Vector2 movingToward_;

        protected bool atLocation_;

        protected Vector2 lookingAt_;

        public DummyEnemy() :
            base(new CharacterHealth(), new CharacterAmmo(), new CharacterWeapon(), "dummy", null, FRAMELENGTHMODIFIER, Vector2.Zero, new Vector2(30.0f, 30.0f), new Vector2(1.0f, 0.0f), 0.5f)
        {
            List<GameTexture> animationTextures = new List<GameTexture>();
            animationTextures.Add(TextureMap.getInstance().getTexture("basic_enemy_walk"));
            animations_ = new AnimationSet(animationTextures);
            movingToward_ = new Vector2(150.0f, 260.0f);
            lookingAt_ = new Vector2(250.0f, 60.0f);
            atLocation_ = false;
        }

        public override void update(GameTime gameTime)
        {
            AI_.update();

            // TODO Change/fix how this is done, modularize it, etc.
            // This enemy should add a stimulus to the world, but right now
            //  there is no way to identify its stimulus from the player's
            // Possibly use an allegiance tag, or a reference to an owner?
            /*WorldState.Visual_.Add(
                visualStimulusId_,
                new Stimulus(StimulusSource.CharacterAbstract, StimulusType.Position, 5, getPosition())
            );*/


            if (!atLocation_)
            {
                Vector2 move = movingToward_ - position_;
                if (move.Length() > 2.0f)
                {
                    move.Normalize();
                    move *= 2.0f;
                }
                position_ += move;
                moved_ += move;
                if (position_.Equals(movingToward_))
                {
                    atLocation_ = true;
                }
                direction_ = lookingAt_ - position_;
                updateFrameNumber();
                currentFrame_ = animations_.setNextFrame(currentFrame_);
            }
            else
            {
                animations_.setNextFrame(0);
            }
        }

        public override void draw(GameTime gameTime)
        {
            animations_.drawNextFrame(position_, getRotationAngle(), depth_);
        }

        public override void moveTo(Vector2 position)
        {
            movingToward_ = position;
            atLocation_ = false;
        }

        public override void lookAt(Vector2 location)
        {
            lookingAt_ = location;
        }
    }
}
