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
using Microsoft.Xna.Framework.Graphics;
using Commando.levels;
using Commando.graphics;
using Commando.objects.weapons;
using Commando.ai.brains;


namespace Commando.objects.enemies
{
    public class HumanEnemy : NonPlayableCharacterAbstract
    {
        const float TURNSPEED = .30f;

        static readonly float RADIUS;

        const float SPEED = 3.0f;

        protected float radius_;

        protected ConvexPolygonInterface boundsPolygonHigh_;

        protected ConvexPolygonInterface boundsPolygonLow_;

        protected DefaultActuator actuator_;

        protected static readonly List<Vector2> BOUNDSPOINTSHIGH;

        protected static readonly List<Vector2> BOUNDSPOINTSLOW;

        protected Color currentDrawColor_ = Color.Black;

        protected int drawColorCount_ = 0;

        static HumanEnemy()
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
            RADIUS += 1f;
        }

        public HumanEnemy(List<DrawableObjectAbstract> pipeline, Vector2 pos)
            : base(pipeline, new CharacterHealth(), new CharacterAmmo(), new CharacterWeapon(), "human", null, null, 8.0f, Vector2.Zero, pos, new Vector2(1.0f, 0.0f), Constants.DEPTH_HIGH)
        {
            AI_ = new HumanAI(this);

            boundsPolygonLow_ = new ConvexPolygon(BOUNDSPOINTSLOW, Vector2.Zero);
            boundsPolygonHigh_ = new ConvexPolygon(BOUNDSPOINTSHIGH, Vector2.Zero);
            boundsPolygonHigh_.rotate(direction_, position_);
            boundsPolygonLow_.rotate(direction_, position_);
            radius_ = RADIUS;


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

            Weapon_ = new Pistol(pipeline, this, new Vector2(60f - 37.5f, 33.5f - 37.5f));
            height_ = new Height(true, true);
            health_.update(25);
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
            else
            {
                return boundsPolygonLow_;
            }
        }

        public override void update(GameTime gameTime)
        {
            AI_.update();

            actuator_.update();
            Weapon_.update();

            if (drawColorCount_ > 0)
            {
                drawColorCount_--;
            }
            else
            {
                currentDrawColor_ = Color.Black;
            }
        }

        public override void draw(GameTime gameTime)
        {
            actuator_.draw(currentDrawColor_);
            Weapon_.draw();
            AI_.draw();
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
