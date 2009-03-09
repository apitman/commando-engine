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
using Commando.collisiondetection;
using Microsoft.Xna.Framework;
using Commando.graphics;
using Commando.ai;
using Commando.objects.weapons;

namespace Commando.objects
{
    public class ActuatedMainPlayer : PlayableCharacterAbstract
    {
        const bool CONTROLSTYLE = false;

        const float TURNSPEED = .30f;

        const float RADIUS = 15.0f;

        protected float radius_;

        protected ConvexPolygonInterface boundsPolygon_;

        protected DefaultActuator actuator_;

        /// <summary>
        /// Create the main player of the game.
        /// </summary>
        public ActuatedMainPlayer(List<DrawableObjectAbstract> pipeline) :
            base(pipeline, new CharacterHealth(), new CharacterAmmo(), new CharacterWeapon(), "Woger Ru", null, null, 8.0f, Vector2.Zero, new Vector2(100.0f, 200.0f), new Vector2(1.0f,0.0f), 0.5f)
        {
            PlayerHelper.Player_ = this;
            
            //TEMP create bounds polygon
            List<Vector2> points = new List<Vector2>();
            points.Add(new Vector2(-5.0f, 0f));
            points.Add(new Vector2(-1.0f, -15.0f));
            points.Add(new Vector2(7.0f, -15.0f));
            points.Add(new Vector2(10.0f, 0f));
            points.Add(new Vector2(7.0f, 15.0f));
            points.Add(new Vector2(-1.0f, 15.0f));
            boundsPolygon_ = new ConvexPolygon(points, Vector2.Zero);
            //ENDTEMP
            
            AnimationInterface run = new LoopAnimation(TextureMap.getInstance().getTexture("PlayerWalkNoPistol"), frameLengthModifier_, depth_);
            AnimationInterface runTo = new LoopAnimation(TextureMap.getInstance().getTexture("PlayerWalkNoPistol"), frameLengthModifier_, depth_);
            AnimationInterface rest = new LoopAnimation(TextureMap.getInstance().getTexture("PlayerWalkNoPistol"), frameLengthModifier_, depth_);
            Dictionary<string, Dictionary<string, CharacterActionInterface>> actions = new Dictionary<string, Dictionary<string, CharacterActionInterface>>();
            actions.Add("default", new Dictionary<string, CharacterActionInterface>());
            actions["default"].Add("move", new CharacterRunAction(this, run, 3.0f));
            actions["default"].Add("moveTo", new CharacterRunToAction(this, runTo, 3.0f));
            actions["default"].Add("rest", new CharacterStayStillAction(this, rest));
            actuator_ = new DefaultActuator(actions, this, "default");

            List<GameTexture> anims = new List<GameTexture>();
            anims.Add(TextureMap.getInstance().getTexture("PlayerWalk"));
            animations_ = new AnimationSet(anims);
            radius_ = RADIUS;
            collisionDetector_ = new CollisionDetector(null);

            Weapon_ = new Shotgun(pipeline, this, new Vector2(60f - 37.5f, 33.5f - 37.5f));
        }

        /// <summary>
        /// Draw the main player at his current position.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void draw(GameTime gameTime)
        {
            //animations_.drawNextFrame(position_, getRotationAngle(), depth_);
            actuator_.draw();
            Weapon_.draw();
        }
        
        /// <summary>
        /// Update the player's current position, animation, and action based on the 
        /// user input.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void update(GameTime gameTime)
        {
            Vector2 rightD = new Vector2(inputSet_.getRightDirectionalX(), inputSet_.getRightDirectionalY());
            Vector2 leftD = new Vector2(inputSet_.getLeftDirectionalX(), -inputSet_.getLeftDirectionalY());
            Vector2 oldDirection = direction_;
            Vector2 oldPosition = position_;
            actuator_.look(rightD);

            if (Settings.getInstance().getMovementType() == MovementType.RELATIVE)
            {
                float rotAngle = getRotationAngle();
                float X = -leftD.Y;
                float Y = leftD.X;
                leftD.X = (float)Math.Cos((double)rotAngle) * X - (float)Math.Sin((double)rotAngle) * Y;
                leftD.Y = (float)Math.Sin((double)rotAngle) * X + (float)Math.Cos((double)rotAngle) * Y;
            }

            if(inputSet_.getButton(Commando.controls.InputsEnum.RIGHT_TRIGGER))
            {
                Weapon_.shoot(collisionDetector_);
                inputSet_.setToggle(Commando.controls.InputsEnum.RIGHT_TRIGGER);
            }
            
            if (leftD.LengthSquared() > 0.2f)
            {
                actuator_.move(leftD);
            }
            else if(direction_ != oldDirection)
            {
//                Vector2 pos = position_;
                position_ = collisionDetector_.checkCollisions(this, position_);
//                if (position_ == pos && collidedInto_.Count != 0)
//                {
//                    actuator_.look(oldDirection);
//                }
            }
            actuator_.update();
            Weapon_.update();
            collidedInto_.Clear();
            collidedWith_.Clear();
            oldPosition -= position_;
            GlobalHelper.getInstance().getCurrentCamera().setCenter(position_.X, position_.Y);

            // TODO Change/fix how this is done, modularize it, etc.
            // Essentially, the player updates his visual location in the WorldState
            // Must remove before adding because Dictionaries don't like duplicate keys
            // Removing a nonexistent key (for first frame) does no harm
            // Also, need to make it so the radius isn't hardcoded - probably all
            //  objects which will have a visual stimulus should have a radius
            WorldState.Visual_.Remove(visualStimulusId_);
            WorldState.Visual_.Add(
                visualStimulusId_,
                new Stimulus(StimulusSource.CharacterAbstract, StimulusType.Position, 5, getPosition(), this)
            );
        }

        public override float getRadius()
        {
            return radius_;
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
        }
    }
}
