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
using Commando.controls;

namespace Commando.objects
{
    public class ActuatedMainPlayer : PlayableCharacterAbstract
    {
        const bool CONTROLSTYLE = false;

        const float TURNSPEED = .30f;

        static readonly float RADIUS;

        const float SPEED = 3.0f;

        protected float radius_;

        protected ConvexPolygonInterface boundsPolygonHigh_;

        protected ConvexPolygonInterface boundsPolygonLow_;

        protected DefaultActuator actuator_;

        protected bool pistol_ = false;

        protected static readonly List<Vector2> BOUNDSPOINTSHIGH;

        protected static readonly List<Vector2> BOUNDSPOINTSLOW;

        static ActuatedMainPlayer()
        {
            /*
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
            */
            BOUNDSPOINTSLOW = new List<Vector2>();
            BOUNDSPOINTSLOW.Add(new Vector2(98f - 113.5f, 51f - 37.5f));
            BOUNDSPOINTSLOW.Add(new Vector2(96f - 113.5f, 44f - 37.5f));
            BOUNDSPOINTSLOW.Add(new Vector2(105f - 113.5f, 32f - 37.5f));
            BOUNDSPOINTSLOW.Add(new Vector2(116f - 113.5f, 23f - 37.5f));
            BOUNDSPOINTSLOW.Add(new Vector2(125f - 113.5f, 22f - 37.5f));
            BOUNDSPOINTSLOW.Add(new Vector2(126f - 113.5f, 29f - 37.5f));
            BOUNDSPOINTSLOW.Add(new Vector2(116f - 113.5f, 41f - 37.5f));
            BOUNDSPOINTSLOW.Add(new Vector2(102f - 113.5f, 52f - 37.5f));
            RADIUS = 0.0f;
            foreach (Vector2 vec in BOUNDSPOINTSLOW)
            {
                if (vec.Length() > RADIUS)
                {
                    RADIUS = vec.Length();
                }
            }

            BOUNDSPOINTSHIGH = new List<Vector2>();
            BOUNDSPOINTSHIGH.Add(new Vector2(96f - 113.5f, 53f - 37.5f));
            BOUNDSPOINTSHIGH.Add(new Vector2(96f - 113.5f, 44f - 37.5f));
            BOUNDSPOINTSHIGH.Add(new Vector2(105f - 113.5f, 32f - 37.5f));
            BOUNDSPOINTSHIGH.Add(new Vector2(116f - 113.5f, 23f - 37.5f));
            BOUNDSPOINTSHIGH.Add(new Vector2(125f - 113.5f, 22f - 37.5f));
            BOUNDSPOINTSHIGH.Add(new Vector2(131f - 113.5f, 32f - 37.5f));
            BOUNDSPOINTSHIGH.Add(new Vector2(134f - 113.5f, 46f - 37.5f));
            BOUNDSPOINTSHIGH.Add(new Vector2(130f - 113.5f, 51f - 37.5f));
            BOUNDSPOINTSHIGH.Add(new Vector2(110f - 113.5f, 54f - 37.5f));
            foreach (Vector2 vec in BOUNDSPOINTSHIGH)
            {
                if (vec.Length() > RADIUS)
                {
                    RADIUS = vec.Length();
                }
            }
            RADIUS += 2f;
        }

        public ActuatedMainPlayer(List<DrawableObjectAbstract> pipeline, CollisionDetectorInterface detector, Vector2 position, Vector2 direction)
            : base(pipeline, new CharacterHealth(), new CharacterAmmo(), new CharacterWeapon(), "Woger Ru", detector, null, 8.0f, Vector2.Zero, position, direction, 0.5f)
        {
            boundsPolygonHigh_ = new ConvexPolygon(BOUNDSPOINTSHIGH, Vector2.Zero);
            boundsPolygonHigh_.rotate(direction_, position_);

            boundsPolygonLow_ = new ConvexPolygon(BOUNDSPOINTSLOW, Vector2.Zero);
            boundsPolygonLow_.rotate(direction_, position_);

            AnimationInterface run = new LoopAnimation(TextureMap.getInstance().getTexture("PlayerTemp_Walk_Red"), frameLengthModifier_, depth_);
            AnimationInterface runTo = new LoopAnimation(TextureMap.getInstance().getTexture("PlayerTemp_Walk_Red"), frameLengthModifier_, depth_);
            AnimationInterface rest = new LoopAnimation(TextureMap.getInstance().getTexture("PlayerTemp_Walk_Red"), frameLengthModifier_, depth_);
            AnimationInterface crouch = new LoopAnimation(TextureMap.getInstance().getTexture("PlayerTemp_Walk_Red"), frameLengthModifier_, depth_);
            AnimationInterface cover = new LoopAnimation(TextureMap.getInstance().getTexture("PlayerTemp_Walk_Red"), frameLengthModifier_, depth_);
            AnimationInterface crouch_run = new LoopAnimation(TextureMap.getInstance().getTexture("PlayerTemp_Walk_Green"), frameLengthModifier_, depth_);
            AnimationInterface crouch_runTo = new LoopAnimation(TextureMap.getInstance().getTexture("PlayerTemp_Walk_Green"), frameLengthModifier_, depth_);
            AnimationInterface crouch_rest = new LoopAnimation(TextureMap.getInstance().getTexture("PlayerTemp_Walk_Green"), frameLengthModifier_, depth_);
            AnimationInterface crouch_crouch = new LoopAnimation(TextureMap.getInstance().getTexture("PlayerTemp_Walk_Green"), frameLengthModifier_, depth_);
            AnimationInterface crouch_cover = new LoopAnimation(TextureMap.getInstance().getTexture("PlayerTemp_Walk_Green"), frameLengthModifier_, depth_);
            AnimationInterface cover_run = new LoopAnimation(TextureMap.getInstance().getTexture("PlayerTemp_Walk_Green"), frameLengthModifier_, depth_);
            AnimationInterface cover_runTo = new LoopAnimation(TextureMap.getInstance().getTexture("PlayerTemp_Walk_Green"), frameLengthModifier_, depth_);
            AnimationInterface cover_rest = new LoopAnimation(TextureMap.getInstance().getTexture("PlayerTemp_Walk_Green"), frameLengthModifier_, depth_);
            AnimationInterface cover_crouch = new LoopAnimation(TextureMap.getInstance().getTexture("PlayerTemp_Walk_Green"), frameLengthModifier_, depth_);
            AnimationInterface cover_cover = new LoopAnimation(TextureMap.getInstance().getTexture("PlayerTemp_Walk_Green"), frameLengthModifier_, depth_);
            AnimationInterface cover_shoot = new NonLoopAnimation(TextureMap.fetchTexture("PlayerTemp_StandToShoot"), 1.0f, depth_);
            Dictionary<string, Dictionary<string, CharacterActionInterface>> actions = new Dictionary<string, Dictionary<string, CharacterActionInterface>>();
            //actions.Add("default", new Dictionary<string, CharacterActionInterface>());
            //actions["default"].Add("move", new CharacterRunAction(this, run, 3.0f));
            //actions["default"].Add("moveTo", new CharacterRunToAction(this, runTo, 3.0f));
            //actions["default"].Add("rest", new CharacterStayStillAction(this, rest));
            actions.Add("crouch", new Dictionary<string, CharacterActionInterface>());
            actions["crouch"].Add("move", new CharacterRunAction(this, crouch_run, SPEED));
            actions["crouch"].Add("moveTo", new CharacterRunToAction(this, crouch_runTo, SPEED));
            actions["crouch"].Add("rest", new CharacterStayStillAction(this, crouch_rest));
            actions["crouch"].Add("crouch", new CrouchAction(this, crouch, "stand", new Height(true, true)));
            actions["crouch"].Add("cover", new AttachToCoverAction(this, crouch_cover, "cover", SPEED));
            actions["crouch"].Add("shoot", new CharacterShootAction());
            actions.Add("stand", new Dictionary<string, CharacterActionInterface>());
            actions["stand"].Add("move", new CharacterRunAction(this, run, SPEED));
            actions["stand"].Add("moveTo", new CharacterRunToAction(this, runTo, SPEED));
            actions["stand"].Add("rest", new CharacterStayStillAction(this, rest));
            actions["stand"].Add("crouch", new CrouchAction(this, crouch_crouch, "crouch", new Height(true, false)));
            actions["stand"].Add("cover", new AttachToCoverAction(this, cover, "cover", SPEED));
            actions["stand"].Add("shoot", new CharacterShootAction());
            actions.Add("cover", new Dictionary<string, CharacterActionInterface>());
            actions["cover"].Add("move", new CharacterCoverMoveAction(this, cover_run, SPEED));
            actions["cover"].Add("moveTo", new CharacterCoverMoveAction(this, cover_runTo, SPEED));
            actions["cover"].Add("rest", new CharacterStayStillAction(this, cover_rest));
            actions["cover"].Add("crouch", new CrouchAction(this, cover_crouch, "cover", new Height(true, false)));
            actions["cover"].Add("cover", new DetachFromCoverAction(this, cover_cover, "stand", SPEED));
            actions["cover"].Add("shoot", new CharacterCoverShootAction(this, cover_shoot, 1));
            actuator_ = new DefaultActuator(actions, this, "stand");

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

            // Give the player his currently active weapon
            Weapon_ = new MachineGun(pipeline, this, new Vector2(42f - 37.5f, 47f - 37.5f));
            //Weapon_.update();
            //Weapon_ = new Pistol(pipeline, this, new Vector2(60f - 37.5f, 33.5f - 37.5f));

            // Add the other weapons to the character's inventory
            Inventory_.Weapons_.Enqueue(new Pistol(pipeline, this, new Vector2(60f - 37.5f, 33.5f - 37.5f)));
            Inventory_.Weapons_.Enqueue(new Shotgun(pipeline, this, new Vector2(60f - 37.5f, 33.5f - 37.5f)));

            height_ = new Height(true, true);
        }

        /// <summary>
        /// Create the main player of the game.
        /// </summary>
        public ActuatedMainPlayer(List<DrawableObjectAbstract> pipeline) :
            base(pipeline, new CharacterHealth(), new CharacterAmmo(), new CharacterWeapon(), "Woger Ru", null, null, 8.0f, Vector2.Zero, new Vector2(100.0f, 200.0f), new Vector2(1.0f,0.0f), 0.5f)
        {
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

            // Give the player his currently active weapon
            Weapon_ = new MachineGun(pipeline, this, new Vector2(42f - 37.5f, 47f - 37.5f));
            //Weapon_.update();
            //Weapon_ = new Pistol(pipeline, this, new Vector2(60f - 37.5f, 33.5f - 37.5f));
            
            // Add the other weapons to the character's inventory
            Inventory_.Weapons_.Enqueue(new Pistol(pipeline, this, new Vector2(60f - 37.5f, 33.5f - 37.5f)));
            Inventory_.Weapons_.Enqueue(new Shotgun(pipeline, this, new Vector2(60f - 37.5f, 33.5f - 37.5f)));

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
            if (Settings.getInstance().IsUsingMouse_)
            {
                float centerX = GlobalHelper.getInstance().getCurrentCamera().getScreenWidth() / 2f;
                float centerY = GlobalHelper.getInstance().getCurrentCamera().getScreenHeight() / 2f;
                Vector2 rightDirectional =
                    new Vector2(inputSet_.getRightDirectionalX() - centerX,
                                inputSet_.getRightDirectionalY() - centerY);
                rightDirectional.Normalize();
                inputSet_.setDirectional(InputsEnum.RIGHT_DIRECTIONAL,
                                    rightDirectional.X, rightDirectional.Y);
            }

            if (inputSet_.getButton(Commando.controls.InputsEnum.BUTTON_3))
            {
                actuator_.crouch();
                inputSet_.setToggle(Commando.controls.InputsEnum.BUTTON_3);
            }

            if(inputSet_.getButton(Commando.controls.InputsEnum.BUTTON_4))
            {
                direction_.Length();
            }

            if (lastCoverObject_ != null && inputSet_.getButton(Commando.controls.InputsEnum.BUTTON_4))
            {
                actuator_.cover(lastCoverObject_);
                inputSet_.setToggle(Commando.controls.InputsEnum.BUTTON_4);
            }

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

            // AMP: Resume Here. Note to self.
            // This is a problem. Since a new Shotgun/Pistol is created each time,
            // it starts with another full clip of ammo. This allows the player
            // to switch back and forth for infinite ammo. The solution: start
            // storing the weapons in the player's inventory.
            if (inputSet_.getButton(Commando.controls.InputsEnum.RIGHT_BUMPER))
            {
                inputSet_.setToggle(InputsEnum.RIGHT_BUMPER);
                RangedWeaponAbstract temp = Inventory_.Weapons_.Dequeue();
                Inventory_.Weapons_.Enqueue(Weapon_);
                Weapon_ = temp;
                ammo_.update(Weapon_.CurrentAmmo_);
                //if (pistol_)
                //{
                //    Weapon_ = new Shotgun(pipeline_, this, new Vector2(60f - 37.5f, 33.5f - 37.5f));
                //    pistol_ = false;
                //}
                //else
                //{
                //    Weapon_ = new Pistol(pipeline_, this, new Vector2(60f - 37.5f, 33.5f - 37.5f));
                //    pistol_ = true;
                //}
            }

            if(inputSet_.getButton(Commando.controls.InputsEnum.RIGHT_TRIGGER))
            {
                actuator_.shoot(Weapon_);
            }

            if (inputSet_.getButton(Commando.controls.InputsEnum.BUTTON_1))
            {
                reload();
            }
            
            if (leftD.LengthSquared() > 0.2f)
            {
                actuator_.move(leftD);
            }
            lastCoverObject_ = null;
            actuator_.update();
            Weapon_.update();
            collidedInto_.Clear();
            collidedWith_.Clear();
            oldPosition -= position_;
            //Console.WriteLine(oldPosition);
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
