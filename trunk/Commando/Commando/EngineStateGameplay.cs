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
        const float AMMO_TEXT_OFFSET_X = 50.0f;
        const float AMMO_TEXT_OFFSET_Y = -14.0f;
        const float HUD_DRAW_DEPTH = 0.8f;
        const string HEALTH_BAR_OUTLINE_TEX_NAME = "healthBarOutline";
        const string HEALTH_BAR_FILL_TEX_NAME = "healthBarFiller";
        const string WEAPON_TEX_NAME = "pistol";
        const string HEALTH_TEXT = "Health";
        const string AMMO_TEXT = "20/20 bullets";
        const float FONT_DRAW_DEPTH = 0.9f;

        //Jared's test stuff
        protected MainPlayer player_;
        protected DummyEnemy enemy_;
        protected CollisionDetector collisionDetector_;
        //END Jared's test stuff

        protected Engine engine_;
        protected HeadsUpDisplayObject healthBar_;
        protected HeadsUpDisplayObject weapon_;
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
            engine_ = engine;
            engine_.setScreenSize(SCREEN_SIZE_X, SCREEN_SIZE_Y);
            engine_.IsMouseVisible = true;
            //Jared's test stuff
            player_ = new MainPlayer();
            enemy_ = new DummyEnemy();
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
            List<Point> boundsPoints = new List<Point>();
            boundsPoints.Add(new Point(30, 30));
            boundsPoints.Add(new Point(255, 30));
            boundsPoints.Add(new Point(255, 90));
            boundsPoints.Add(new Point(345, 90));
            boundsPoints.Add(new Point(345, 240));
            boundsPoints.Add(new Point(255, 240));
            boundsPoints.Add(new Point(255, 300));
            boundsPoints.Add(new Point(30, 300));
            BoundingPolygon walls = new BoundingPolygon(boundsPoints);
            List<Point> boxBoundsPoints = new List<Point>();
            boxBoundsPoints.Add(new Point(75, 75));
            boxBoundsPoints.Add(new Point(165, 75));
            boxBoundsPoints.Add(new Point(165, 120));
            boxBoundsPoints.Add(new Point(75, 120));
            BoundingPolygon boxes = new BoundingPolygon(boxBoundsPoints);
            List<BoundingPolygon> polygons = new List<BoundingPolygon>();
            polygons.Add(walls);
            polygons.Add(boxes);
            collisionDetector_ = new CollisionDetector(polygons);
            //END Jared's test stuff

            healthBarPos_ = new Vector2(HEALTH_BAR_POS_X, HEALTH_BAR_POS_Y);
            weaponIconPos_ = new Vector2(WEAPON_ICON_POS_X, WEAPON_ICON_POS_Y);
            healthTextPos_ = new Vector2(HEALTH_BAR_POS_X + HEALTH_TEXT_OFFSET_X, HEALTH_BAR_POS_Y + HEALTH_TEXT_OFFSET_Y);
            ammoTextPos_ = new Vector2(WEAPON_ICON_POS_X + AMMO_TEXT_OFFSET_X, WEAPON_ICON_POS_Y + AMMO_TEXT_OFFSET_Y);
            healthBar_ = new HeadsUpDisplayObject(TextureMap.getInstance().getTexture(HEALTH_BAR_FILL_TEX_NAME), healthBarPos_, new Vector2(0.0f), HUD_DRAW_DEPTH);
            weapon_ = new HeadsUpDisplayObject(TextureMap.getInstance().getTexture(WEAPON_TEX_NAME), weaponIconPos_, new Vector2(0.0f), HUD_DRAW_DEPTH);
            player_.getHealth().addObserver(healthBar_);
            player_.getWeapon().addObserver(weapon_);
            player_.setCollisionDetector(collisionDetector_);
            enemy_.setCollisionDetector(collisionDetector_);
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
            enemy_.update(gameTime);
            //END Jared's test stuff

            return this;
        }

        /// <summary>
        /// Draws the character, levels, etc.
        /// </summary>
        public void draw()
        {
            engine_.GraphicsDevice.Clear(Color.Chocolate);

            //Jared's test stuff
            player_.draw(new GameTime());
            enemy_.draw(new GameTime());
            foreach (TileObject tOb in tiles_)
            {
                tOb.draw(new GameTime());
            }
            //END Jared's test stuff

            healthBar_.draw(new GameTime());
            TextureMap.getInstance().getTexture(HEALTH_BAR_OUTLINE_TEX_NAME).drawImage(0, healthBarPos_, 0.0f, HUD_DRAW_DEPTH);
            weapon_.draw(new GameTime());

            FontMap.getInstance().getFont(FontEnum.Kootenay).drawString(HEALTH_TEXT, healthTextPos_, Color.Black, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, FONT_DRAW_DEPTH);
            FontMap.getInstance().getFont(FontEnum.Kootenay).drawString(AMMO_TEXT, ammoTextPos_, Color.Black, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, FONT_DRAW_DEPTH);
        }

        #endregion
    }
}
