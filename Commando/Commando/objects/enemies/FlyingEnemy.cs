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
    class FlyingEnemy : NonPlayableCharacterAbstract
    {
        private const float FRAMELENGTHMODIFIER = 4.0f;

        private const float GUNHANDLEX = 11.0f;

        private const float GUNHANDLEY = 0.0f;

        const float SPEED = 3.0f;

        protected Vector2 movingToward_;

        protected bool atLocation_;

        protected Vector2 lookingAt_;

        //protected CollisionDetector collisionDetector_;

        protected ConvexPolygonInterface boundsPolygon_;

        protected ActuatorInterface actuator_;

        protected  static readonly float RADIUS = 12.0f;

        protected float radius_;

        protected Color currentDrawColor_;

        protected int drawColorCount_ = 0;

        public FlyingEnemy(List<DrawableObjectAbstract> pipeline, Vector2 pos) :
            base(pipeline, new CharacterHealth(), new CharacterAmmo(), new CharacterWeapon(), "dummy", null, null, FRAMELENGTHMODIFIER, Vector2.Zero, pos, new Vector2(1.0f, 0.0f), 0.49f)
        {
            AI_ = new DummyAI(this);

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
            boundsPolygon_ = new CircularConvexPolygon(radius_, position_);

            
            AnimationInterface run = new LoopAnimation(TextureMap.getInstance().getTexture("flyingenemy"), frameLengthModifier_, depth_, boundsPolygon_);
            AnimationInterface runTo = new LoopAnimation(TextureMap.getInstance().getTexture("flyingenemy"), frameLengthModifier_, depth_, boundsPolygon_);
            AnimationInterface rest = new LoopAnimation(TextureMap.getInstance().getTexture("flyingenemy"), frameLengthModifier_, depth_, boundsPolygon_);
            AnimationInterface[] restAnims = new AnimationInterface[2];
            restAnims[0] = rest;
            restAnims[1] = rest;
            AnimationInterface[] shoot = new AnimationInterface[1];
            shoot[0] = rest;

            List<string> levels = new List<string>();
            levels.Add("look");
            levels.Add("upper");

            Dictionary<string, Dictionary<string, CharacterActionInterface>> actions = new Dictionary<string, Dictionary<string, CharacterActionInterface>>();
            actions.Add("default", new Dictionary<string, CharacterActionInterface>());
            actions["default"].Add("move", new CharacterRunAction(this, run, 2.0f, "upper"));
            actions["default"].Add("moveTo", new CharacterRunToAction(this, runTo, 2.0f, "upper"));
            actions["default"].Add("rest", new CharacterStayStillAction(this, restAnims, levels, "upper", "upper"));
            actions["default"].Add("crouch", new NoAction("upper"));
            actions["default"].Add("cover", new NoAction("upper"));
            actions["default"].Add("shoot", new CharacterShootAction(this, shoot, "upper", 0));
            actions["default"].Add("look", new CharacterLookAction(this, "look"));
            actions["default"].Add("lookAt", new CharacterLookAtAction(this, "look"));
            actions["default"].Add("reload", new NoAction("upper"));
            actions["default"].Add("throw", new NoAction("upper"));
            actuator_ = new MultiLevelActuator(actions, levels, this, "default", "rest", "upper", "upper");
            
            currentDrawColor_ = Color.White;
            health_.update(15);

            Weapon_ = new DroneGun(pipeline_, this, new Vector2(GUNHANDLEX, GUNHANDLEY));
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

        public override void die()
        {
            base.die();
            ShrapnelInfo info = new ShrapnelInfo();
            info.COUNT_MIN = 30;
            info.COUNT_MAX = 40;
            info.VELOCITY_MIN = -3;
            info.VELOCITY_MAX = 3;
            info.LIFE_MIN = 3;
            info.LIFE_MAX = 12;
            info.SIZE = 2;
            ShrapnelGenerator.createShrapnel(pipeline_, position_, Color.Yellow, Constants.DEPTH_DEBUG_LINES, ref info);
        }
    }
}
