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


using System.Collections.Generic;
using Commando.collisiondetection;
using Commando.graphics;
using Commando.objects.weapons;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Commando.levels;
using Commando.ai.brains;

namespace Commando.objects.enemies
{
    public class BigBoss : NonPlayableCharacterAbstract
    {
        private const float FRAMELENGTHMODIFIER = 4.0f;

        private const float GUNHANDLEX = 11.0f;

        private const float GUNHANDLEY = 0.0f;

        const float SPEED = 5.0f;

        protected Vector2 movingToward_;

        protected bool atLocation_;

        protected Vector2 lookingAt_;

        //protected CollisionDetector collisionDetector_;

        protected ConvexPolygonInterface boundsPolygon_;

        protected ActuatorInterface actuator_;

        protected  static readonly float RADIUS;

        protected float radius_;

        protected Color currentDrawColor_;

        protected int drawColorCount_ = 0;

        protected bool alt_ = false;

        protected static readonly List<Vector2> BOUNDS;

        static BigBoss()
        {
            BOUNDS = new List<Vector2>();
            BOUNDS.Add(new Vector2(45f - 75f, 33f - 75f));
            BOUNDS.Add(new Vector2(45f - 75f,117f - 75f));
            BOUNDS.Add(new Vector2(62f - 75f, 124f - 75f));
            BOUNDS.Add(new Vector2(80f - 75f, 124f - 75f));
            BOUNDS.Add(new Vector2(93f - 75f, 118f - 75f));
            BOUNDS.Add(new Vector2(95f - 75f, 75f - 75f));
            BOUNDS.Add(new Vector2(93f - 75f, 32f - 75f));
            BOUNDS.Add(new Vector2(80f - 75f, 26f - 75f));
            BOUNDS.Add(new Vector2(62f - 75f, 26f - 75f));
            RADIUS = 0f;
            foreach (Vector2 vec in BOUNDS)
            {
                if (vec.Length() > RADIUS)
                {
                    RADIUS = vec.Length();
                }
            }
        }

        public BigBoss(List<DrawableObjectAbstract> pipeline, Vector2 pos) :
            base(pipeline, new CharacterHealth(), new CharacterAmmo(), new CharacterWeapon(), "dummy", null, null, FRAMELENGTHMODIFIER, Vector2.Zero, pos, new Vector2(1.0f, 0.0f), 0.49f)
        {
            AI_ = new BossAI(this);

            List<GameTexture> animationTextures = new List<GameTexture>();
            animationTextures.Add(TextureMap.getInstance().getTexture("basic_enemy_walk"));
            animations_ = new AnimationSet(animationTextures);
            movingToward_ = new Vector2(150.0f, 260.0f);
            lookingAt_ = new Vector2(250.0f, 60.0f);
            atLocation_ = false;
            collisionDetector_ = null;
            radius_ = RADIUS;
            Allegiance_ = 2; 
            height_ = new Height(true, true);
            ConvexPolygonInterface bounds = new ConvexPolygon(BOUNDS, Vector2.Zero);
            boundsPolygon_ = new CircularConvexPolygon(20, position_);

            
            AnimationInterface run = new LoopAnimation(TextureMap.fetchTexture("GreenPlayer_Blank"), frameLengthModifier_, depth_, boundsPolygon_);
            AnimationInterface runTo = new LoopAnimation(TextureMap.fetchTexture("GreenPlayer_Blank"), frameLengthModifier_, depth_, boundsPolygon_);
            AnimationInterface rest = new LoopAnimation(TextureMap.fetchTexture("GreenPlayer_Blank"), frameLengthModifier_, depth_, boundsPolygon_);
            AnimationInterface top = new LoopAnimation(TextureMap.fetchTexture("Boss"), frameLengthModifier_, depth_, bounds);
            AnimationInterface[] restAnims = new AnimationInterface[3];
            restAnims[0] = rest;
            restAnims[1] = rest;
            restAnims[2] = top;
            AnimationInterface[] shoot = new AnimationInterface[1];
            shoot[0] = top;

            List<string> levels = new List<string>();
            levels.Add("look");
            levels.Add("lower");
            levels.Add("upper");

            Dictionary<string, Dictionary<string, CharacterActionInterface>> actions = new Dictionary<string, Dictionary<string, CharacterActionInterface>>();
            actions.Add("default", new Dictionary<string, CharacterActionInterface>());
            actions["default"].Add("move", new CharacterRunAction(this, run, 2.0f, "lower"));
            actions["default"].Add("moveTo", new CharacterRunToAction(this, runTo, 2.0f, "lower"));
            actions["default"].Add("rest", new CharacterStayStillAction(this, restAnims, levels, "upper", "lower"));
            actions["default"].Add("crouch", new NoAction("lower"));
            actions["default"].Add("cover", new NoAction("lower"));
            actions["default"].Add("shoot", new CharacterShootAction(this, shoot, "upper", 0));
            actions["default"].Add("look", new CharacterLookAction(this, "look"));
            actions["default"].Add("lookAt", new CharacterLookAtAction(this, "look"));
            actions["default"].Add("reload", new NoAction("lower"));
            actions["default"].Add("throw", new CharacterShootAction(this, shoot, "upper", 0));
            actuator_ = new MultiLevelActuator(actions, levels, this, "default", "rest", "lower", "upper");
            
            currentDrawColor_ = Color.White;
            health_.update(300);

            Weapon_ = new MachineGun(pipeline_, this, getGunHandle(true));
        }

        public override float getRadius()
        {
            return radius_;
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
                currentDrawColor_ = Color.White;
            }
        }

        public override void draw(GameTime gameTime)
        {
            actuator_.draw(currentDrawColor_);
            AI_.draw();
        }

        public void moveTo(Vector2 position)
        {
        }

        public void lookAt(Vector2 location)
        {
        }

        public override ConvexPolygonInterface getBounds(HeightEnum height)
        {
            if (Height.getHeight(height).collides(height_))
            {
                return boundsPolygon_;
            }
            return null;
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
                SoundEngine.getInstance().playCue("fart-3");
                currentDrawColor_ = Color.Brown;
            }
            else
            {
                currentDrawColor_ = Color.Salmon;
                drawColorCount_ = 2;
            }
        }

        public override Vector2 getGunHandle(bool pistol)
        {
            if (alt_)
            {
                if (pistol)
                {
                    alt_ = false;
                    return new Vector2(-38f, -31f);
                }
                return new Vector2(-44f, -17f);
            }
            else
            {
                if (pistol)
                {
                    alt_ = true;
                    return new Vector2(113f, 106f);
                }
                return new Vector2(119f, 92f);
            }
        }
    }
}
