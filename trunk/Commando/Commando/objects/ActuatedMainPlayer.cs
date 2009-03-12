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
using Commando.levels;

namespace Commando.objects
{
    public class ActuatedMainPlayer : PlayableCharacterAbstract
    {
        const bool CONTROLSTYLE = false;

        const float TURNSPEED = .30f;

        const float RADIUS = 15.0f;

        protected float radius_;

        protected ConvexPolygonInterface boundsPolygonHigh_;

        protected ConvexPolygonInterface boundsPolygonLow_;

        protected DefaultActuator actuator_;

        protected bool pistol_ = false;

        protected static readonly List<Vector2> BOUNDSPOINTSHIGH;

        protected static readonly List<Vector2> BOUNDSPOINTSLOW;

        static ActuatedMainPlayer()
        {
            BOUNDSPOINTSHIGH = new List<Vector2>();
            BOUNDSPOINTSHIGH.Add(new Vector2(-5.0f, 0f));
            BOUNDSPOINTSHIGH.Add(new Vector2(-1.0f, -15.0f));
            BOUNDSPOINTSHIGH.Add(new Vector2(7.0f, -15.0f));
            BOUNDSPOINTSHIGH.Add(new Vector2(34.0f, 0f));
            BOUNDSPOINTSHIGH.Add(new Vector2(7.0f, 15.0f));
            BOUNDSPOINTSHIGH.Add(new Vector2(-1.0f, 15.0f));

            BOUNDSPOINTSLOW = new List<Vector2>();
            BOUNDSPOINTSLOW.Add(new Vector2(-5.0f, 0f));
            BOUNDSPOINTSLOW.Add(new Vector2(-1.0f, -15.0f));
            BOUNDSPOINTSLOW.Add(new Vector2(7.0f, -15.0f));
            BOUNDSPOINTSLOW.Add(new Vector2(10.0f, -3.0f));
            BOUNDSPOINTSLOW.Add(new Vector2(7.0f, 15.0f));
            BOUNDSPOINTSLOW.Add(new Vector2(-1.0f, 15.0f));
        }

        public ActuatedMainPlayer(List<DrawableObjectAbstract> pipeline, CollisionDetectorInterface detector, Vector2 position, Vector2 direction)
            : base(pipeline, new CharacterHealth(), new CharacterAmmo(), new CharacterWeapon(), "Woger Ru", detector, null, 8.0f, Vector2.Zero, position, direction, 0.5f)
        {
            PlayerHelper.Player_ = this;

            boundsPolygonHigh_ = new ConvexPolygon(BOUNDSPOINTSHIGH, Vector2.Zero);

            boundsPolygonLow_ = new ConvexPolygon(BOUNDSPOINTSLOW, Vector2.Zero);

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
            //collisionDetector_ = new SeparatingAxisCollisionDetector();
            collisionDetector_ = detector;
            if (collisionDetector_ != null)
            {
                collisionDetector_.register(this);
            }

            Weapon_ = new Shotgun(pipeline, this, new Vector2(60f - 37.5f, 33.5f - 37.5f));
            //Weapon_.update();
            //Weapon_ = new Pistol(pipeline, this, new Vector2(60f - 37.5f, 33.5f - 37.5f));
            height_ = new Height(true, true);
        }

        /// <summary>
        /// Create the main player of the game.
        /// </summary>
        public ActuatedMainPlayer(List<DrawableObjectAbstract> pipeline) :
            base(pipeline, new CharacterHealth(), new CharacterAmmo(), new CharacterWeapon(), "Woger Ru", null, null, 8.0f, Vector2.Zero, new Vector2(100.0f, 200.0f), new Vector2(1.0f,0.0f), 0.5f)
        {
            PlayerHelper.Player_ = this;

            boundsPolygonHigh_ = new ConvexPolygon(BOUNDSPOINTSHIGH, Vector2.Zero);
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
            collisionDetector_ = new SeparatingAxisCollisionDetector();

            Weapon_ = new Shotgun(pipeline, this, new Vector2(60f - 37.5f, 33.5f - 37.5f));
            //Weapon_.update();
            //Weapon_ = new Pistol(pipeline, this, new Vector2(60f - 37.5f, 33.5f - 37.5f));
            height_ = new Height(true, false);
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

            if (inputSet_.getButton(Commando.controls.InputsEnum.RIGHT_BUMPER))
            {
                if (pistol_)
                {
                    Weapon_ = new Shotgun(pipeline_, this, new Vector2(60f - 37.5f, 33.5f - 37.5f));
                    pistol_ = false;
                }
                else
                {
                    Weapon_ = new Pistol(pipeline_, this, new Vector2(60f - 37.5f, 33.5f - 37.5f));
                    pistol_ = true;
                }
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

        }

        public override float getRadius()
        {
            return radius_;
        }

        public override ConvexPolygonInterface getBounds(HeightEnum height)
        {
            if (height == HeightEnum.HIGH)
            {
                return boundsPolygonHigh_;
            }
            return boundsPolygonLow_;
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
                this.die();
            }
        }

        public void setDrawPipeline(List<DrawableObjectAbstract> pipeline)
        {
            pipeline_ = pipeline;
            if (pipeline_ != null)
            {
                pipeline_.Add(this);
                Weapon_.setDrawPipeline(pipeline_);
            }
        }
    }
}
