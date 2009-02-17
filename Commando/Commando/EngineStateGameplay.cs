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
using Commando.controls;
using Commando.levels;
using Commando.objects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Commando.collisiondetection;

namespace Commando
{
    /// <summary>
    /// The state of the engine when the player is moving around
    /// in levels, fighting enemies, etc.
    /// </summary>
    public class EngineStateGameplay : EngineStateInterface
    {
        const int SCREEN_SIZE_X = 375;
        const int SCREEN_SIZE_Y = 375;
        const float HEALTH_BAR_POS_X = 100.0f;
        const float HEALTH_BAR_POS_Y = 350.0f;
        const float WEAPON_ICON_POS_X = 200.0f;
        const float WEAPON_ICON_POS_Y = 350.0f;
        const float HEALTH_TEXT_OFFSET_X = -27.0f;
        const float HEALTH_TEXT_OFFSET_Y = -12.0f;
        const float AMMO_TEXT_POS_X = 300.0f;
        const float AMMO_TEXT_POS_Y = 350.0f;
        const float HUD_DRAW_DEPTH = 0.8f;
        const string HEALTH_BAR_OUTLINE_TEX_NAME = "healthBarOutline";
        const string HEALTH_BAR_FILL_TEX_NAME = "healthBarFiller";
        const string WEAPON_TEX_NAME = "pistol";
        const string HEALTH_TEXT = "Health";
        const string AMMO_TEXT = "%i/20 bullets";
        const string AMMO_REPLACE_TEXT = "%i";
        const float FONT_DRAW_DEPTH = 0.9f;

        //Jared's test stuff
        //protected MainPlayer player_;
        protected ActuatedMainPlayer player_;
        protected DummyEnemy enemy1_;
        protected DummyEnemy enemy2_;
        protected CollisionDetectorInterface collisionDetector_;
        protected List<DrawableObjectAbstract> drawPipeline_;
        //END Jared's test stuff

        protected Engine engine_;
        protected HeadsUpDisplayObject healthBar_;
        protected HeadsUpDisplayWeapon weapon_;
        protected HeadsUpDisplayText ammo_;
        protected List<TileObject> tiles_;
        protected Vector2 healthBarPos_;
        protected Vector2 weaponIconPos_;
        protected Vector2 healthTextPos_;
        protected Vector2 ammoTextPos_;


        /// <summary>
        /// Constructs a state of gameplay
        /// </summary>
        /// <param name="engine">Reference to the engine running the state</param>
        public EngineStateGameplay(Engine engine)
        {
            drawPipeline_ = new List<DrawableObjectAbstract>();
            engine_ = engine;
            engine_.setScreenSize(SCREEN_SIZE_X, SCREEN_SIZE_Y);
            engine_.IsMouseVisible = true;
            //Jared's test stuff
            //player_ = new MainPlayer();
            player_ = new ActuatedMainPlayer(drawPipeline_);
            enemy1_ = new DummyEnemy();
            enemy2_ = new DummyEnemy();
            enemy2_.setPosition(new Vector2(200f, 130f));
            int[,] tiles = new int[,]   {{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                                        {0,7,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,8,0,0,0,0,0,0,0},
                                        {0,2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,4,0,0,0,0,0,0,0},
                                        {0,2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,4,0,0,0,0,0,0,0},
                                        {0,2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,4,0,0,0,0,0,0,0},
                                        {0,2,1,1,1,10,11,12,10,11,12,1,1,1,1,1,1,19,3,3,3,3,3,8,0},
                                        {0,2,1,1,1,13,14,15,13,14,15,1,1,1,1,1,1,1,1,1,1,1,1,4,0},
                                        {0,2,1,1,1,16,17,18,16,17,18,1,1,1,1,1,1,1,1,1,1,1,1,4,0},
                                        {0,2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,4,0},
                                        {0,2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,4,0},
                                        {0,2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,4,0},
                                        {0,2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,4,0},
                                        {0,2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,4,0},
                                        {0,2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,4,0},
                                        {0,2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,4,0},
                                        {0,2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,4,0},
                                        {0,2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,20,5,5,5,5,5,9,0},
                                        {0,2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,4,0,0,0,0,0,0,0},
                                        {0,2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,4,0,0,0,0,0,0,0},
                                        {0,2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,4,0,0,0,0,0,0,0},
                                        {0,6,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,9,0,0,0,0,0,0,0},
                                        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0}};
            tiles_ = Tiler.getTiles(tiles);
            List<Vector2> boundsPoints = new List<Vector2>();
            boundsPoints.Add(new Vector2(142.5f - 30f, 15f - 0f));
            boundsPoints.Add(new Vector2(142.5f - 255f, 15f - 0f));
            boundsPoints.Add(new Vector2(142.5f - 255f, 15f - 30f));
            boundsPoints.Add(new Vector2(142.5f - 30f, 15f - 30f));
            //boundsPoints.Add(new Vector2(142.5f - 255f, 0f));
            //boundsPoints.Add(new Vector2(142.5f - 30f, 0f));
            BoxObject wall1 = new BoxObject(boundsPoints, new Vector2(142.5f, 15f));
            wall1.getBounds().rotate(new Vector2(1.0f, 0.0f), wall1.getPosition());
            boundsPoints.Clear();
            boundsPoints.Add(new Vector2(300f - 255f, 60f - 30f));
            boundsPoints.Add(new Vector2(300f - 255f, 60f - 90f));
            boundsPoints.Add(new Vector2(300f - 345f, 60f - 90f));
            boundsPoints.Add(new Vector2(300f - 345f, 60f - 30f));
            BoxObject wall2 = new BoxObject(boundsPoints, new Vector2(300f, 60f));
            wall2.getBounds().rotate(new Vector2(1.0f, 0.0f), wall2.getPosition());

            boundsPoints.Add(new Vector2(345f, 240f));
            boundsPoints.Add(new Vector2(255f, 240f));
            boundsPoints.Add(new Vector2(255f, 300f));
            boundsPoints.Add(new Vector2(30f, 300f));
            BoundingPolygon walls = new BoundingPolygon(boundsPoints);
            List<Vector2> boxBoundsPoints = new List<Vector2>();
            /*
            boxBoundsPoints.Add(new Vector2(75f, 75f));
            boxBoundsPoints.Add(new Vector2(165f, 75f));
            boxBoundsPoints.Add(new Vector2(165f, 120f));
            boxBoundsPoints.Add(new Vector2(75f, 120f));
            */
            boxBoundsPoints.Add(new Vector2(120f - 75f, 97.5f - 75f));
            boxBoundsPoints.Add(new Vector2(120f - 165f, 97.5f - 75f));
            boxBoundsPoints.Add(new Vector2(120f - 165f, 97.5f - 120f));
            boxBoundsPoints.Add(new Vector2(120f - 75f, 97.5f - 120f));
            BoundingPolygon boxes = new BoundingPolygon(boxBoundsPoints);
            BoxObject box = new BoxObject(boxBoundsPoints, new Vector2(120f, 97.5f));
            List<BoundingPolygon> polygons = new List<BoundingPolygon>();
            polygons.Add(walls);
            polygons.Add(boxes);
            //collisionDetector_ = new CollisionDetector(polygons);
            collisionDetector_ = new SeparatingAxisCollisionDetector();
            box.getBounds().rotate(new Vector2(1.0f, 0.0f), box.getPosition());
            collisionDetector_.register(box);
            collisionDetector_.register(wall1);
            collisionDetector_.register(wall2);
            //END Jared's test stuff

            healthBarPos_ = new Vector2(HEALTH_BAR_POS_X, HEALTH_BAR_POS_Y);
            weaponIconPos_ = new Vector2(WEAPON_ICON_POS_X, WEAPON_ICON_POS_Y);
            healthTextPos_ = new Vector2(HEALTH_BAR_POS_X + HEALTH_TEXT_OFFSET_X, HEALTH_BAR_POS_Y + HEALTH_TEXT_OFFSET_Y);
            ammoTextPos_ = new Vector2(AMMO_TEXT_POS_X, AMMO_TEXT_POS_Y);
            healthBar_ = new HeadsUpDisplayObject(TextureMap.getInstance().getTexture(HEALTH_BAR_FILL_TEX_NAME), healthBarPos_, Vector2.Zero, HUD_DRAW_DEPTH);
            weapon_ = new HeadsUpDisplayWeapon(TextureMap.getInstance().getTexture(WEAPON_TEX_NAME), weaponIconPos_, Vector2.Zero, HUD_DRAW_DEPTH);
            ammo_ = new HeadsUpDisplayText(ammoTextPos_, FONT_DRAW_DEPTH, FontEnum.Kootenay);
            player_.getHealth().addObserver(healthBar_);
            player_.getWeapon().addObserver(weapon_);
            player_.getAmmo().addObserver(ammo_);
            player_.setCollisionDetector(collisionDetector_);
            enemy1_.setCollisionDetector(collisionDetector_);
            enemy2_.setCollisionDetector(collisionDetector_);
        }

        #region EngineStateInterface Members

        /// <summary>
        /// Handles input and moves all characters and objects forward one
        /// frame in time
        /// </summary>
        /// <param name="gameTime">GameTime parameter</param>
        /// <returns>The state of the game for the next frame</returns>
        public EngineStateInterface update(GameTime gameTime)
        {
            InputSet inputs = engine_.getInputs();

            if (inputs.getButton(InputsEnum.CANCEL_BUTTON))
            {
                inputs.setToggle(InputsEnum.CANCEL_BUTTON);
                return new EngineStatePause(engine_, this);
            }

            //Jared's test stuff
            player_.setInputSet(inputs);
            player_.update(gameTime);
            if (!enemy1_.isDead())
            {
                enemy1_.update(gameTime);
            }
            if (!enemy2_.isDead())
            {
                enemy2_.update(gameTime);
            }
            //END Jared's test stuff

            for (int i = drawPipeline_.Count - 1; i >= 0; i--)
            {
                drawPipeline_[i].update(null);
                if (drawPipeline_[i].isDead())
                {
                    drawPipeline_.RemoveAt(i);
                }
            }

            return this;
        }

        /// <summary>
        /// Draws the character, levels, etc.
        /// </summary>
        public void draw()
        {
            engine_.GraphicsDevice.Clear(Color.Chocolate);

            //TEST
            (collisionDetector_ as SeparatingAxisCollisionDetector).draw();
            //

            //Jared's test stuff
            player_.draw(new GameTime());
            enemy1_.draw(new GameTime());
            enemy2_.draw(new GameTime());
            foreach (TileObject tOb in tiles_)
            {
                tOb.draw(new GameTime());
            }

            for (int i = drawPipeline_.Count - 1; i >= 0; i--)
            {
                drawPipeline_[i].draw(null);
            }
            //END Jared's test stuff

            healthBar_.draw(new GameTime());
            TextureMap.getInstance().getTexture(HEALTH_BAR_OUTLINE_TEX_NAME).drawImage(0, healthBarPos_, 0.0f, HUD_DRAW_DEPTH);
            weapon_.draw(new GameTime());

            FontMap.getInstance().getFont(FontEnum.Kootenay).drawString(HEALTH_TEXT, healthTextPos_, Color.Black, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, FONT_DRAW_DEPTH);
            //FontMap.getInstance().getFont(FontEnum.Kootenay).drawString(AMMO_TEXT, ammoTextPos_, Color.Black, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, FONT_DRAW_DEPTH);
            ammo_.drawString(AMMO_TEXT, AMMO_REPLACE_TEXT, Color.Black, 0.0f);
        }

        #endregion
    }
}
