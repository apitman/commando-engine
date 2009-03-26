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
        const float TURNSPEED = .30f;

        static readonly float RADIUS;
        static readonly float RADIUSCROUCH;

        const float SPEED = 3.0f;

        protected float radius_;

        protected float radiusCrouch_;

        protected ConvexPolygonInterface boundsPolygonHigh_;

        protected ConvexPolygonInterface boundsPolygonLow_;

        protected ConvexPolygonInterface boundsPolygonLowCrouch_;

        protected DefaultActuator actuator_;

        protected bool pistol_ = false;

        protected static readonly List<Vector2> BOUNDSPOINTSHIGH;

        protected static readonly List<Vector2> BOUNDSPOINTSLOW;

        protected static readonly List<Vector2> BOUNDSPOINTSLOWCROUCH;

        static ActuatedMainPlayer()
        {
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

            BOUNDSPOINTSLOWCROUCH = new List<Vector2>();
            BOUNDSPOINTSLOWCROUCH.Add(new Vector2(19f - 37.5f, 36f - 37.5f));
            BOUNDSPOINTSLOWCROUCH.Add(new Vector2(25f - 37.5f, 26f - 37.5f));
            BOUNDSPOINTSLOWCROUCH.Add(new Vector2(54f - 37.5f, 21f - 37.5f));
            BOUNDSPOINTSLOWCROUCH.Add(new Vector2(66f - 37.5f, 46f - 37.5f));
            BOUNDSPOINTSLOWCROUCH.Add(new Vector2(42f - 37.5f, 55f - 37.5f));
            RADIUSCROUCH = 0.0f;
            foreach (Vector2 vec in BOUNDSPOINTSLOWCROUCH)
            {
                if (vec.Length() > RADIUSCROUCH)
                {
                    RADIUSCROUCH = vec.Length();
                }
            }
            RADIUSCROUCH += 1f;
            RADIUS += 1f;
        }

        public ActuatedMainPlayer(List<DrawableObjectAbstract> pipeline, CollisionDetectorInterface detector, Vector2 position, Vector2 direction)
            : base(pipeline, new CharacterHealth(), new CharacterAmmo(), new CharacterWeapon(), "Woger Ru", detector, null, 8.0f, Vector2.Zero, position, direction, 0.5f)
        {
            boundsPolygonHigh_ = new ConvexPolygon(BOUNDSPOINTSHIGH, Vector2.Zero);
            boundsPolygonHigh_.rotate(direction_, position_);

            boundsPolygonLow_ = new ConvexPolygon(BOUNDSPOINTSLOW, Vector2.Zero);
            boundsPolygonLow_.rotate(direction_, position_);

            boundsPolygonLowCrouch_ = new ConvexPolygon(BOUNDSPOINTSLOWCROUCH, Vector2.Zero);
            boundsPolygonLowCrouch_.rotate(direction_, position_);

            AnimationInterface run = new LoopAnimation(TextureMap.getInstance().getTexture("GreenPlayer_Stand_Rifle_Walk"), frameLengthModifier_, depth_);
            AnimationInterface runTo = new LoopAnimation(TextureMap.getInstance().getTexture("GreenPlayer_Stand_Rifle_Walk"), frameLengthModifier_, depth_);
            AnimationInterface rest = new LoopAnimation(TextureMap.getInstance().getTexture("GreenPlayer_Stand_Rifle_Walk"), frameLengthModifier_, depth_);
            AnimationInterface crouch = new LoopAnimation(TextureMap.getInstance().getTexture("GreenPlayer_Stand_Rifle_Walk"), frameLengthModifier_, depth_);
            AnimationInterface cover = new LoopAnimation(TextureMap.getInstance().getTexture("GreenPlayer_Stand_Rifle_Walk"), frameLengthModifier_, depth_);
            AnimationInterface crouch_run = new LoopAnimation(TextureMap.getInstance().getTexture("GreenPlayer_Crouch_Rifle_Walk"), frameLengthModifier_, depth_);
            AnimationInterface crouch_runTo = new LoopAnimation(TextureMap.getInstance().getTexture("GreenPlayer_Crouch_Rifle_Walk"), frameLengthModifier_, depth_);
            AnimationInterface crouch_rest = new LoopAnimation(TextureMap.getInstance().getTexture("GreenPlayer_Crouch_Rifle"), frameLengthModifier_, depth_);
            AnimationInterface crouch_crouch = new LoopAnimation(TextureMap.getInstance().getTexture("GreenPlayer_Crouch_Rifle"), frameLengthModifier_, depth_);
            AnimationInterface crouch_cover = new LoopAnimation(TextureMap.getInstance().getTexture("GreenPlayer_Crouch_Rifle_Walk"), frameLengthModifier_, depth_);
            AnimationInterface cover_run = new LoopAnimation(TextureMap.getInstance().getTexture("GreenPlayer_Crouch_Rifle_Walk"), frameLengthModifier_, depth_);
            AnimationInterface cover_runTo = new LoopAnimation(TextureMap.getInstance().getTexture("GreenPlayer_Crouch_Rifle_Walk"), frameLengthModifier_, depth_);
            AnimationInterface cover_rest = new LoopAnimation(TextureMap.getInstance().getTexture("GreenPlayer_Crouch_Rifle"), frameLengthModifier_, depth_);
            AnimationInterface cover_crouch = new LoopAnimation(TextureMap.getInstance().getTexture("GreenPlayer_Crouch_Rifle"), frameLengthModifier_, depth_);
            AnimationInterface cover_cover = new LoopAnimation(TextureMap.getInstance().getTexture("GreenPlayer_Crouch_Rifle_Walk"), frameLengthModifier_, depth_);
            AnimationInterface cover_shoot = new NonLoopAnimation(TextureMap.fetchTexture("GreenPlayer_StandToShoot_Rifle"), 1.0f, depth_);

            AnimationInterface pistol_run = new LoopAnimation(TextureMap.getInstance().getTexture("GreenPlayer_Stand_Pistol_Walk"), frameLengthModifier_, depth_);
            AnimationInterface pistol_runTo = new LoopAnimation(TextureMap.getInstance().getTexture("GreenPlayer_Stand_Pistol_Walk"), frameLengthModifier_, depth_);
            AnimationInterface pistol_rest = new LoopAnimation(TextureMap.getInstance().getTexture("GreenPlayer_Stand_Pistol_Walk"), frameLengthModifier_, depth_);
            AnimationInterface pistol_crouch = new LoopAnimation(TextureMap.getInstance().getTexture("GreenPlayer_Stand_Pistol_Walk"), frameLengthModifier_, depth_);
            AnimationInterface pistol_cover = new LoopAnimation(TextureMap.getInstance().getTexture("GreenPlayer_Stand_Pistol_Walk"), frameLengthModifier_, depth_);
            AnimationInterface pistol_crouch_run = new LoopAnimation(TextureMap.getInstance().getTexture("GreenPlayer_Crouch_Pistol_Walk"), frameLengthModifier_, depth_);
            AnimationInterface pistol_crouch_runTo = new LoopAnimation(TextureMap.getInstance().getTexture("GreenPlayer_Crouch_Pistol_Walk"), frameLengthModifier_, depth_);
            AnimationInterface pistol_crouch_rest = new LoopAnimation(TextureMap.getInstance().getTexture("GreenPlayer_Crouch_Pistol"), frameLengthModifier_, depth_);
            AnimationInterface pistol_crouch_crouch = new LoopAnimation(TextureMap.getInstance().getTexture("GreenPlayer_Crouch_Pistol"), frameLengthModifier_, depth_);
            AnimationInterface pistol_crouch_cover = new LoopAnimation(TextureMap.getInstance().getTexture("GreenPlayer_Crouch_Pistol_Walk"), frameLengthModifier_, depth_);
            AnimationInterface pistol_cover_run = new LoopAnimation(TextureMap.getInstance().getTexture("GreenPlayer_Crouch_Pistol_Walk"), frameLengthModifier_, depth_);
            AnimationInterface pistol_cover_runTo = new LoopAnimation(TextureMap.getInstance().getTexture("GreenPlayer_Crouch_Pistol_Walk"), frameLengthModifier_, depth_);
            AnimationInterface pistol_cover_rest = new LoopAnimation(TextureMap.getInstance().getTexture("GreenPlayer_Crouch_Pistol"), frameLengthModifier_, depth_);
            AnimationInterface pistol_cover_crouch = new LoopAnimation(TextureMap.getInstance().getTexture("GreenPlayer_Crouch_Pistol"), frameLengthModifier_, depth_);
            AnimationInterface pistol_cover_cover = new LoopAnimation(TextureMap.getInstance().getTexture("GreenPlayer_Crouch_Pistol_Walk"), frameLengthModifier_, depth_);
            AnimationInterface pistol_cover_shoot = new NonLoopAnimation(TextureMap.fetchTexture("GreenPlayer_StandToShoot_Pistol"), 1.0f, depth_);

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
            actions["crouch"].Add("cover", new AttachToCoverAction(this, crouch_cover, "cover", new Height(true, false), SPEED));
            actions["crouch"].Add("shoot", new CharacterShootAction());
            actions["crouch"].Add("throw", new ThrowGrenadeAction(this, crouch_rest, new Vector2(30f, 0f)));
            actions.Add("stand", new Dictionary<string, CharacterActionInterface>());
            actions["stand"].Add("move", new CharacterRunAction(this, run, SPEED));
            actions["stand"].Add("moveTo", new CharacterRunToAction(this, runTo, SPEED));
            actions["stand"].Add("rest", new CharacterStayStillAction(this, rest));
            actions["stand"].Add("crouch", new CrouchAction(this, crouch_crouch, "crouch", new Height(true, false)));
            actions["stand"].Add("cover", new AttachToCoverAction(this, cover, "cover", new Height(true, false), SPEED));
            actions["stand"].Add("shoot", new CharacterShootAction());
            actions["stand"].Add("throw", new ThrowGrenadeAction(this, rest, new Vector2(30f, 0f)));
            actions.Add("cover", new Dictionary<string, CharacterActionInterface>());
            actions["cover"].Add("move", new CharacterCoverMoveAction(this, cover_run, SPEED));
            actions["cover"].Add("moveTo", new CharacterCoverMoveAction(this, cover_runTo, SPEED));
            actions["cover"].Add("rest", new CharacterStayStillAction(this, cover_rest));
            actions["cover"].Add("crouch", new CrouchAction(this, cover_crouch, "cover", new Height(true, false)));
            actions["cover"].Add("cover", new DetachFromCoverAction(this, cover_cover, "stand", new Height(true, true), SPEED));
            actions["cover"].Add("shoot", new CharacterCoverShootAction(this, cover_shoot, 1));
            actions["cover"].Add("throw", new ThrowGrenadeAction(this, cover_rest, new Vector2(30f, 0f)));

            actions.Add("pistol_crouch", new Dictionary<string, CharacterActionInterface>());
            actions["pistol_crouch"].Add("move", new CharacterRunAction(this, pistol_crouch_run, SPEED));
            actions["pistol_crouch"].Add("moveTo", new CharacterRunToAction(this, pistol_crouch_runTo, SPEED));
            actions["pistol_crouch"].Add("rest", new CharacterStayStillAction(this, pistol_crouch_rest));
            actions["pistol_crouch"].Add("crouch", new CrouchAction(this, pistol_crouch, "pistol_stand", new Height(true, true)));
            actions["pistol_crouch"].Add("cover", new AttachToCoverAction(this, pistol_crouch_cover, "pistol_cover", new Height(true, false), SPEED));
            actions["pistol_crouch"].Add("shoot", new CharacterShootAction());
            actions["pistol_crouch"].Add("throw", new ThrowGrenadeAction(this, pistol_crouch_rest, new Vector2(30f, 0f)));
            actions.Add("pistol_stand", new Dictionary<string, CharacterActionInterface>());
            actions["pistol_stand"].Add("move", new CharacterRunAction(this, pistol_run, SPEED));
            actions["pistol_stand"].Add("moveTo", new CharacterRunToAction(this, pistol_runTo, SPEED));
            actions["pistol_stand"].Add("rest", new CharacterStayStillAction(this, pistol_rest));
            actions["pistol_stand"].Add("crouch", new CrouchAction(this, pistol_crouch_crouch, "pistol_crouch", new Height(true, false)));
            actions["pistol_stand"].Add("cover", new AttachToCoverAction(this, pistol_cover, "pistol_cover", new Height(true, false), SPEED));
            actions["pistol_stand"].Add("shoot", new CharacterShootAction());
            actions["pistol_stand"].Add("throw", new ThrowGrenadeAction(this, pistol_rest, new Vector2(30f, 0f)));
            actions.Add("pistol_cover", new Dictionary<string, CharacterActionInterface>());
            actions["pistol_cover"].Add("move", new CharacterCoverMoveAction(this, pistol_cover_run, SPEED));
            actions["pistol_cover"].Add("moveTo", new CharacterCoverMoveAction(this, pistol_cover_runTo, SPEED));
            actions["pistol_cover"].Add("rest", new CharacterStayStillAction(this, pistol_cover_rest));
            actions["pistol_cover"].Add("crouch", new CrouchAction(this, pistol_cover_crouch, "pistol_cover", new Height(true, false)));
            actions["pistol_cover"].Add("cover", new DetachFromCoverAction(this, pistol_cover_cover, "pistol_stand", new Height(true, true), SPEED));
            actions["pistol_cover"].Add("shoot", new CharacterCoverShootAction(this, pistol_cover_shoot, 1));
            actions["pistol_cover"].Add("throw", new ThrowGrenadeAction(this, pistol_cover_rest, new Vector2(30f, 0f)));

            actuator_ = new DefaultActuator(actions, this, "pistol_stand");

            List<GameTexture> anims = new List<GameTexture>();
            anims.Add(TextureMap.getInstance().getTexture("PlayerWalk"));
            animations_ = new AnimationSet(anims);
            if (RADIUS < RADIUSCROUCH)
            {
                radius_ = RADIUSCROUCH;
            }
            else
            {
                radius_ = RADIUS;
            }; 
            //collisionDetector_ = new SeparatingAxisCollisionDetector();
            collisionDetector_ = detector;
            if (collisionDetector_ != null)
            {
                collisionDetector_.register(this);
            }

            // Give the player his currently active weapon
            Weapon_ = new Pistol(pipeline, this, new Vector2(60f - 37.5f, 33.5f - 37.5f));
            //Weapon_.update();
            //Weapon_ = new Pistol(pipeline, this, new Vector2(60f - 37.5f, 33.5f - 37.5f));

            // Add the other weapons to the character's inventory
            //Inventory_.Weapons_.Enqueue(new MachineGun(pipeline, this, new Vector2(42f - 37.5f, 47f - 37.5f)));
            //Inventory_.Weapons_.Enqueue(new Shotgun(pipeline, this, new Vector2(42f - 37.5f, 47f - 37.5f)));

            height_ = new Height(true, true);
        }

        /*
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
            //Inventory_.Weapons_.Enqueue(new Pistol(pipeline, this, new Vector2(60f - 37.5f, 33.5f - 37.5f)));
            //Inventory_.Weapons_.Enqueue(new Shotgun(pipeline, this, new Vector2(60f - 37.5f, 33.5f - 37.5f)));

            height_ = new Height(true, false);
        }
        */

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

            if(inputSet_.getButton(Commando.controls.InputsEnum.LEFT_TRIGGER))
            {
                Grenade grenade = new FragGrenade(pipeline_, collisionDetector_, direction_, Vector2.Zero, direction_);
                actuator_.throwGrenade(grenade);
                inputSet_.setToggle(InputsEnum.LEFT_TRIGGER);
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

            if (inputSet_.getButton(Commando.controls.InputsEnum.RIGHT_BUMPER) && Inventory_.Weapons_.Count > 0)
            {
                inputSet_.setToggle(InputsEnum.RIGHT_BUMPER);
                RangedWeaponAbstract temp = Inventory_.Weapons_.Dequeue();
                Inventory_.Weapons_.Enqueue(Weapon_);
                if (Weapon_ is Pistol)
                {
                    actuator_.setCurrentActionSet(actuator_.getCurrentActionSet().Substring(7));
                }
                Weapon_ = temp;
                ammo_.update(Weapon_.CurrentAmmo_);
                if (Weapon_ is Pistol)
                {
                    actuator_.setCurrentActionSet("pistol_" + actuator_.getCurrentActionSet());
                }
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
            if (height_.blocksHigh_)
            {
                return boundsPolygonLow_;
            }
            else
            {
                return boundsPolygonLowCrouch_;
            }
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
                foreach (RangedWeaponAbstract i in Inventory_.Weapons_)
                {
                    i.setDrawPipeline(pipeline_);
                }
            }
        }

        public override Vector2 getGunHandle(bool pistol)
        {
            if (pistol)
            {
                if (height_.blocksHigh_)
                {
                    return new Vector2(51f - 37.5f, 46f - 37.5f);
                }
                else
                {
                    return new Vector2(59f - 37.5f, 46f - 37.5f);
                }
            }
            else
            {
                if (height_.blocksHigh_)
                {
                    return new Vector2(42f - 37.5f, 47f - 37.5f);
                }
                else
                {
                    return new Vector2(50f - 37.5f, 47f - 37.5f);
                }
            }
        }
    }
}