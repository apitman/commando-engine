﻿/*
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
using Commando.collisiondetection;
using Commando.graphics;
using Microsoft.Xna.Framework.Graphics;

namespace Commando.objects
{
    public class DummyEnemy : NonPlayableCharacterAbstract, CollisionObjectInterface
    {
        private const float FRAMELENGTHMODIFIER = 4.0f;

        protected Vector2 movingToward_;

        protected bool atLocation_;

        protected Vector2 lookingAt_;

        protected CollisionDetector collisionDetector_;

        protected ConvexPolygonInterface boundsPolygon_;

        protected DefaultActuator actuator_;

        protected  const float RADIUS = 12.0f;

        protected float radius_;

        protected Color currentDrawColor_;

        protected int drawColorCount_ = 0;

        public DummyEnemy() :
            base(new CharacterHealth(), new CharacterAmmo(), new CharacterWeapon(), "dummy", null, null, FRAMELENGTHMODIFIER, Vector2.Zero, new Vector2(30.0f, 30.0f), new Vector2(1.0f, 0.0f), 0.49f)
        {
            List<GameTexture> animationTextures = new List<GameTexture>();
            animationTextures.Add(TextureMap.getInstance().getTexture("basic_enemy_walk"));
            animations_ = new AnimationSet(animationTextures);
            movingToward_ = new Vector2(150.0f, 260.0f);
            lookingAt_ = new Vector2(250.0f, 60.0f);
            atLocation_ = false;
            collisionDetector_ = null;
            radius_ = RADIUS;
            boundsPolygon_ = new CircularConvexPolygon(radius_, position_);

            AnimationInterface run = new LoopAnimation(TextureMap.getInstance().getTexture("basic_enemy_walk"), frameLengthModifier_, depth_);
            AnimationInterface runTo = new LoopAnimation(TextureMap.getInstance().getTexture("basic_enemy_walk"), frameLengthModifier_, depth_);
            AnimationInterface rest = new LoopAnimation(TextureMap.getInstance().getTexture("basic_enemy_walk"), frameLengthModifier_, depth_);
            Dictionary<string, Dictionary<string, CharacterActionInterface>> actions = new Dictionary<string, Dictionary<string, CharacterActionInterface>>();
            actions.Add("default", new Dictionary<string, CharacterActionInterface>());
            actions["default"].Add("move", new CharacterRunAction(this, run, 2.0f));
            actions["default"].Add("moveTo", new CharacterRunToAction(this, runTo, 2.0f));
            actions["default"].Add("rest", new CharacterStayStillAction(this, rest));
            actuator_ = new DefaultActuator(actions, this, "default");

            currentDrawColor_ = Color.White;
            health_.update(15);
        }

        public override float getRadius()
        {
            return radius_;
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

            actuator_.update();
            if (drawColorCount_ > 0)
            {
                drawColorCount_--;
            }
            else
            {
                currentDrawColor_ = Color.White;
            }
            if (collidedInto_.Count > 0)
            {
                foreach (CollisionObjectInterface cObj in collidedInto_)
                {
                    if (cObj is ActuatedMainPlayer)
                    {
                        (cObj as ActuatedMainPlayer).damage(1, this);
                    }
                }
                collidedInto_.Clear();
            }
            if (collidedWith_.Count > 0)
            {
                foreach (CollisionObjectInterface cObj in collidedWith_)
                {
                    if (cObj is ActuatedMainPlayer)
                    {
                        (cObj as ActuatedMainPlayer).damage(1, this);
                    }
                }
                collidedWith_.Clear();
            }
        }

        public override void draw(GameTime gameTime)
        {
            actuator_.draw(currentDrawColor_);
        }

        public override void moveTo(Vector2 position)
        {
            actuator_.moveTo(position);
            //movingToward_ = position;
            //atLocation_ = false;
        }

        public override void lookAt(Vector2 location)
        {
            actuator_.lookAt(location);
            //lookingAt_ = location;
        }

        public override ConvexPolygonInterface getBounds()
        {
            return boundsPolygon_;
        }

        public override ActuatorInterface getActuator()
        {
            return actuator_;
        }

        public override void damage(int amount, CollisionObjectInterface obj)
        {
            health_.update(health_.getValue() - amount);
            if (health_.getValue() <= 0)
            {
                die();
                currentDrawColor_ = Color.Brown;
            }
            else
            {
                currentDrawColor_ = Color.Salmon;
                drawColorCount_ = 2;
            }
        }
    }
}
