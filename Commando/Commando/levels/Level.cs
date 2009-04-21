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
using System.Xml;
using Commando.objects;
using Microsoft.Xna.Framework;
using Commando.objects.enemies;
using Commando.collisiondetection;
using Commando.ai;
using Commando.graphics;

namespace Commando.levels
{
    /// <summary>
    /// Encapsulates all of the components which make up a level of the game.
    /// </summary>
    public class Level
    {
        // Drawing pipeline containing all drawable entities in the level
        public List<DrawableObjectAbstract> Pipeline_ =
            new List<DrawableObjectAbstract>();

        // Collision detector containing all collidables in the level
        public CollisionDetectorInterface CollisionDetector_ =
            new SeparatingAxisCollisionDetector();

        protected Tileset tileSet_;

        protected int height_;

        protected int width_;

        protected int[,] tiles_;

        protected List<NonPlayableCharacterAbstract> enemies_ =
            new List<NonPlayableCharacterAbstract>();

        protected List<LevelObjectAbstract> items_ =
            new List<LevelObjectAbstract>();

        /// <summary>
        /// Right now, we're not gonna use this.
        /// </summary>
        protected PlayableCharacterAbstract player_;

        protected Vector2 playerStartLocation_;

        /// <summary>
        /// True if this level was loaded from Content, false if it was
        /// loaded from the file system.  It is used to set LevelTransitionObjects
        /// appropriately with this flag.
        /// </summary>
        protected bool isPackagedLevel_;

        /// <summary>
        /// Private constructor; use factory methods to create.
        /// </summary>
        /// <param name="isPackaged">True if this level is being loaded from
        /// packaged Content, false if it is being loaded from the file system.
        /// </param>
        private Level(bool isPackaged)
        {
            isPackagedLevel_ = isPackaged;
        }

        public void setPlayerStartLocation(Vector2 pos)
        {
            playerStartLocation_ = pos;
        }
        public Vector2 getPlayerStartLocation()
        {
            return playerStartLocation_;
        }
        public int getHeight()
        {
            return height_;
        }

        public int getWidth()
        {
            return width_;
        }

        public int[,] getTiles()
        {
            return tiles_;
        }

        public List<NonPlayableCharacterAbstract> getEnemies()
        {
            return enemies_;
        }

        public List<LevelObjectAbstract> getItems()
        {
            return items_;
        }

        public PlayableCharacterAbstract getPlayer()
        {
            return player_;
        }

        public void setEnemies(List<NonPlayableCharacterAbstract> enemies)
        {
            enemies_ = enemies;
        }

        public void setItems(List<LevelObjectAbstract> items)
        {
            items_ = items;
        }

        public void setPlayer(PlayableCharacterAbstract player)
        {
            player_ = player;
        }

        /// <summary>
        /// Factory method to load a level from the file system.
        /// </summary>
        /// <param name="filepath">Complete path to the level Xml.</param>
        /// <param name="engine">Game engine.</param>
        /// <returns>An initialized level.</returns>
        public static Level getLevelFromFile(string filepath, Engine engine)
        {
            Level level;
            using (ManagedXml manager = new ManagedXml(engine))
            {
                XmlDocument doc = manager.loadFromFile(filepath);
                level = new Level(false);
                level.initializeLevelFromXml(doc, engine);
            }
            return level;
        }

        /// <summary>
        /// Factory method to load a level from packaged Content.
        /// </summary>
        /// <param name="levelname">Content-based path to the level.</param>
        /// <param name="engine">Game engine.</param>
        /// <returns>An initialized level.</returns>
        public static Level getLevelFromContent(string levelname, Engine engine)
        {
            Level level;
            using (ManagedXml manager = new ManagedXml(engine))
            {
                XmlDocument doc = manager.load(levelname);
                level = new Level(true);
                level.initializeLevelFromXml(doc, engine);
            }
            return level;
        }

        /// <summary>
        /// Parses Xml to populate the level with enemies, objects, etc.
        /// </summary>
        /// <param name="doc">XmlDocument containing level data.</param>
        /// <param name="engine">Game engine.</param>
        /// <returns></returns>
        protected void initializeLevelFromXml(XmlDocument doc, Engine engine)
        {
            // First load the tiles
            XmlElement ele = (XmlElement)doc.GetElementsByTagName("level")[0];
            width_ = Convert.ToInt32(ele.GetAttribute("numTilesWide"));
            height_ = Convert.ToInt32(ele.GetAttribute("numTilesTall"));

            tiles_ = new int[height_, width_];

            XmlNodeList tList = doc.GetElementsByTagName("tile");
            for (int i = 0; i < height_; i++)
            {
                for (int j = 0; j < width_; j++)
                {
                    XmlElement ele2 = (XmlElement)tList[j + i * width_];
                    int tempInt = Convert.ToInt32(ele2.GetAttribute("index"));
                    tiles_[i, j] = tempInt;
                }
            }

            // Load the tileset similar to this
            XmlElement tilesetEle = (XmlElement)doc.GetElementsByTagName("tileset")[0];
            if (tilesetEle == null)
            {
                tileSet_ = Tileset.getTilesetFromContent(@"XML\defaultTileset", engine);
            }
            else
            {
                tileSet_ = Tileset.getTilesetFromContent(tilesetEle.InnerText, engine);
            }

            // Now load the enemies
            tList = doc.GetElementsByTagName("enemy");
            for (int i = 0; i < tList.Count; i++)
            {
                parseEnemy(tList[i]);
            }
            // Now load the healthBoxes, ammoBoxes, weapon pickups, and level transitions
            tList = doc.GetElementsByTagName("item");
            for (int i = 0; i < tList.Count; i++)
            {
                parseItem(tList[i]);
            }
            tList = doc.GetElementsByTagName("overlay");
            for (int i = 0; i < tList.Count; i++)
            {
                parseOverlay(tList[i]);
            }
            // Load player location from file
            if (doc.GetElementsByTagName("playerLocation").Count > 0)
            {
                XmlElement playerLocation = (XmlElement)doc.GetElementsByTagName("playerLocation")[0];
                playerStartLocation_ = new Vector2((float)Convert.ToInt32(playerLocation.GetAttribute("x")), (float)Convert.ToInt32(playerLocation.GetAttribute("y")));
            }

            //TODO: get from XML
            //player_ = new ActuatedMainPlayer(pipeline, null, new Vector2(100f, 200f), new Vector2(1.0f, 0.0f));
            //
            player_ = null;
        }
        
        /// <summary>
        /// Push this level into an Xml document.
        /// </summary>
        /// <param name="filepath">Location to store the Xml.</param>
        public void writeLevelToFile(string filepath)
        {
            // Create the document
            XmlDocument doc = new XmlDocument();

            // Add level attributes
            XmlElement levelElement = doc.CreateElement("level");
            levelElement.SetAttribute("numTilesWide", width_.ToString());
            levelElement.SetAttribute("numTilesTall", height_.ToString());
            levelElement.SetAttribute("screenSizeX", "375");
            levelElement.SetAttribute("screenSizeY", "375");

            // Add the tiles
            XmlElement tilesElement = doc.CreateElement("tiles");
            for (int i = 0; i < height_; i++)
            {
                for (int j = 0; j < width_; j++)
                {
                    XmlElement tileElement = doc.CreateElement("tile");
                    tileElement.SetAttribute("index", tiles_[i, j].ToString());
                    tilesElement.AppendChild(tileElement);
                }
            }
            levelElement.AppendChild(tilesElement);

            // Add the enemies
            XmlElement enemiesElement = doc.CreateElement("enemies");
            for (int i = 0; i < enemies_.Count; i++)
            {

                XmlElement enemyElement = doc.CreateElement("enemy");

                if (enemies_[i] is DummyEnemy)
                    enemyElement.SetAttribute("name", "dummy");

                else if (enemies_[i] is HumanEnemy)
                    enemyElement.SetAttribute("name", "human");
                enemyElement.SetAttribute("posX", Convert.ToInt32(enemies_[i].getPosition().X).ToString());
                enemyElement.SetAttribute("posY", Convert.ToInt32(enemies_[i].getPosition().Y).ToString());
                //int rotationDegrees = Convert.ToInt32(CommonFunctions.getAngle(enemies_[i].getDirection()) * 180 / Math.PI);
                //enemyElement.SetAttribute("rotation", rotationDegrees.ToString());
                enemyElement.SetAttribute("rotationX", Convert.ToInt32(enemies_[i].getDirection().X * 100).ToString());
                enemyElement.SetAttribute("rotationY", Convert.ToInt32(enemies_[i].getDirection().Y * 100).ToString());
                enemyElement.SetAttribute("allegiance", enemies_[i].Allegiance_.ToString());
                enemiesElement.AppendChild(enemyElement);
            }
            levelElement.AppendChild(enemiesElement);

            // Add HealthBoxes
            // Add AmmoBoxes
            XmlElement itemsElement = doc.CreateElement("items");
            for (int i = 0; i < items_.Count; i++)
            {
                if (items_[i] is HealthBox)
                {
                    // HealthBox
                    XmlElement hBoxElement = doc.CreateElement("item");
                    hBoxElement.SetAttribute("type", "hBox");
                    hBoxElement.SetAttribute("posX", Convert.ToInt32(items_[i].getPosition().X).ToString());
                    hBoxElement.SetAttribute("posY", Convert.ToInt32(items_[i].getPosition().Y).ToString());
                    itemsElement.AppendChild(hBoxElement);
                }
                else if (items_[i] is AmmoBox)
                {
                    // AmmoBox
                    XmlElement aBoxElement = doc.CreateElement("item");
                    aBoxElement.SetAttribute("type", "aBox");
                    aBoxElement.SetAttribute("posX", Convert.ToInt32(items_[i].getPosition().X).ToString());
                    aBoxElement.SetAttribute("posY", Convert.ToInt32(items_[i].getPosition().Y).ToString());
                    itemsElement.AppendChild(aBoxElement);
                }
                else if (items_[i] is LevelTransitionObject)
                {

                    LevelTransitionObject myTrans = items_[i] as LevelTransitionObject;
                    XmlElement aTransElement = doc.CreateElement("item");
                    aTransElement.SetAttribute("type", "aTrans");
                    aTransElement.SetAttribute("posX", Convert.ToInt32(myTrans.getPosition().X).ToString());
                    aTransElement.SetAttribute("posY", Convert.ToInt32(myTrans.getPosition().Y).ToString());
                    aTransElement.SetAttribute("nextLevel", myTrans.getNextLevel());

                    itemsElement.AppendChild(aTransElement);
                }
                else if (items_[i] is WeaponBox)
                {
                    WeaponBox myWpnBox = items_[i] as WeaponBox;
                    XmlElement aWpnBoxElement = doc.CreateElement("item");
                    aWpnBoxElement.SetAttribute("type", "aWpnBox");
                    aWpnBoxElement.SetAttribute("posX", Convert.ToInt32(myWpnBox.getPosition().X).ToString());
                    aWpnBoxElement.SetAttribute("posY", Convert.ToInt32(myWpnBox.getPosition().Y).ToString());
                    string wpnType;
                    if (myWpnBox.WeapnType == WeaponBox.WeaponType.MachineGun)
                        wpnType = "machineGun";
                    else if (myWpnBox.WeapnType == WeaponBox.WeaponType.Pistol)
                        wpnType = "pistol";
                    else if (myWpnBox.WeapnType == WeaponBox.WeaponType.Shotgun)
                        wpnType = "shotgun";
                    else
                        throw new NotImplementedException("Don't know how to save this weapon!");
                    aWpnBoxElement.SetAttribute("weaponType", wpnType);
                    itemsElement.AppendChild(aWpnBoxElement);
                }
                levelElement.AppendChild(itemsElement);
            }
            // Add playerLocation
            XmlElement playerLocElement = doc.CreateElement("playerLocation");
            playerLocElement.SetAttribute("x", Convert.ToInt32(playerStartLocation_.X).ToString());
            playerLocElement.SetAttribute("y", Convert.ToInt32(playerStartLocation_.Y).ToString());
            levelElement.AppendChild(playerLocElement);

            // Finish up and save the document
            doc.AppendChild(levelElement);
            doc.Save(filepath);

            doc = null;
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        /// <summary>
        /// Changes a tile in the level at the specified location. 
        /// Will automatically expand the level if necessary.
        /// </summary>
        /// <param name="newTileIndex">Index of the new tile</param>
        /// <param name="newTilePosX">Refers to tile coordinates, not pixels</param>
        /// <param name="newTilePosY">Refers to tile coordinates, not pixels</param>
        /// <param name="pipeline">The draw pipeline for the new tile</param>
        public void changeTile(int newTileIndex, int newTilePosX, int newTilePosY, List<DrawableObjectAbstract> pipeline)
        {
            
            // First, check to see if we need to expand our level
            if (newTilePosX >= width_)
            {
                // Expand right, or expand right AND down
                int newNumTilesWide = newTilePosX + 1;
                int newNumTilesTall = Math.Max(height_, newTilePosY + 1);

                int[,] newTiles = new int[newNumTilesTall, newNumTilesWide];

                for (int i = 0; i < newNumTilesTall; i++)
                {
                    for (int j = 0; j < newNumTilesWide; j++)
                    {
                        if (j < width_ && i < height_)
                        {
                            // Already have this tile
                            newTiles[i, j] = tiles_[i, j];
                        }
                        else if (i == newTilePosY && j == newTilePosX)
                        {
                            // New tile we are planting
                            Vector2 pos = new Vector2((float)Tiler.tileSideLength_ * j, (float)Tiler.tileSideLength_ * i);
                            newTiles[i, j] = newTileIndex;
                        }
                        else
                        {
                            // Default (black) tile
                            Vector2 pos = new Vector2((float)Tiler.tileSideLength_ * j, (float)Tiler.tileSideLength_ * i);
                            newTiles[i, j] = 0; // TODO Cannot make assumption that tileId=0 in tileset is "empty"
                        }
                    }
                }
                width_ = newNumTilesWide;
                height_ = newNumTilesTall;
                tiles_ = newTiles;
            }
            else if (newTilePosY >= height_)
            {
                // Expand down
                int newNumTilesWide = width_;
                int newNumTilesTall = newTilePosY + 1;

                int[,] newTiles = new int[newNumTilesTall, newNumTilesWide];

                for (int i = 0; i < newNumTilesTall; i++)
                {
                    for (int j = 0; j < newNumTilesWide; j++)
                    {
                        if (i < height_)
                        {
                            // Already have this tile
                            newTiles[i, j] = tiles_[i, j];
                        }
                        else if (i == newTilePosY && j == newTilePosX)
                        {
                            // New tile we are planting
                            Vector2 pos = new Vector2((float)Tiler.tileSideLength_ * j, (float)Tiler.tileSideLength_ * i);
                            newTiles[i, j] = newTileIndex;
                        }
                        else
                        {
                            // Default (black) tile
                            Vector2 pos = new Vector2((float)Tiler.tileSideLength_ * j, (float)Tiler.tileSideLength_ * i);
                            newTiles[i, j] = 0; // TODO Cannot make assumption that tileId=0 in tileset is "empty"
                        }
                    }
                }
                width_ = newNumTilesWide;
                height_ = newNumTilesTall;
                tiles_ = newTiles;
            }
            else
            {
                // No expansion, replace a TileObject in tiles_
                tiles_[newTilePosY, newTilePosX] = newTileIndex;
            }
            
        }

        /// <summary>
        /// Parse a node for an enemy and add it to the drawing pipeline as well
        /// as the enemies list.
        /// </summary>
        /// <param name="enemyNode">Node containing Xml data for the enemy.</param>
        protected void parseEnemy(XmlNode enemyNode)
        {
            NonPlayableCharacterAbstract enemy;
            XmlElement enemyEle = (XmlElement)enemyNode;

            string name = enemyEle.GetAttribute("name");
            switch (name)
            {
                case "dummy":
                    enemy = new DummyEnemy(Pipeline_, new Vector2((float)Convert.ToInt32(enemyEle.GetAttribute("posX")), (float)Convert.ToInt32(enemyEle.GetAttribute("posY"))));
                    break;
                case "human":
                    enemy = new HumanEnemy(Pipeline_, new Vector2((float)Convert.ToInt32(enemyEle.GetAttribute("posX")), (float)Convert.ToInt32(enemyEle.GetAttribute("posY"))));
                    break;
                case "bigboss":
                    enemy = new BigBoss(Pipeline_, new Vector2((float)Convert.ToInt32(enemyEle.GetAttribute("posX")), (float)Convert.ToInt32(enemyEle.GetAttribute("posY"))));
                    break;
                default:
                    throw new NotImplementedException("Unknown enemy name in level file");
            }
 
            Vector2 rotation = new Vector2(Convert.ToInt32(enemyEle.GetAttribute("rotationX")) / 100.0f, Convert.ToInt32(enemyEle.GetAttribute("rotationY")) / 100.0f);
            enemy.setDirection(rotation);
            string team = enemyEle.GetAttribute("allegiance");
            if (team != null && team != string.Empty)
            {
                enemy.Allegiance_ = Convert.ToInt32(team);
            }

            string commLevel = enemyEle.GetAttribute("commLevel");
            if (commLevel != string.Empty)
            {
                Commando.ai.SystemCommunication.CommunicationLevel cL;
                if (commLevel == "Low")
                {
                    cL = Commando.ai.SystemCommunication.CommunicationLevel.Low;
                }
                else if (commLevel == "Medium")
                {
                    cL = Commando.ai.SystemCommunication.CommunicationLevel.Medium;
                }
                else if (commLevel == "High")
                {
                    cL = Commando.ai.SystemCommunication.CommunicationLevel.High;
                }
                else
                {
                    cL = Commando.ai.SystemCommunication.CommunicationLevel.Medium;
                }
                enemy.AI_.CommunicationSystem_.communicationLevel_ = cL;
            }

            // TODO: AMP Fix it so we don't have to do this next line of code
            enemy.getActuator().update(); // Makes it so the enemies are drawn in the correct position

            enemies_.Add(enemy);
        }

        /// <summary>
        /// Parse a node for an item and add it to the drawing pipeline
        /// as well as the image list.
        /// </summary>
        /// <param name="itemNode">Node containing Xml data for the item.</param>
        protected void parseItem(XmlNode itemNode)
        {
            XmlElement itemElement = (XmlElement)itemNode;
            if (itemElement.GetAttribute("type") == "hBox")
            {
                Vector2 pos = new Vector2((float)Convert.ToInt32(itemElement.GetAttribute("posX")), (float)Convert.ToInt32(itemElement.GetAttribute("posY")));
                items_.Add(new HealthBox(null, Pipeline_, pos, new Vector2(1.0f, 0.0f), Constants.DEPTH_LOW));
            }
            else if (itemElement.GetAttribute("type") == "aBox")
            {
                Vector2 pos = new Vector2((float)Convert.ToInt32(itemElement.GetAttribute("posX")), (float)Convert.ToInt32(itemElement.GetAttribute("posY")));
                items_.Add(new AmmoBox(null, Pipeline_, pos, new Vector2(1.0f, 0.0f), Constants.DEPTH_LOW));
            }
            else if (itemElement.GetAttribute("type") == "aTrans")
            {
                Vector2 pos = new Vector2((float)Convert.ToInt32(itemElement.GetAttribute("posX")), (float)Convert.ToInt32(itemElement.GetAttribute("posY")));
                string nextLevel;
                nextLevel = itemElement.GetAttribute("nextLevel");
                bool usesStory = itemElement.GetAttribute("story").ToLower() == "true";
                int storyDuration = 0;
                if (usesStory)
                {
                    storyDuration = Convert.ToInt32(itemElement.GetAttribute("storyDuration"));
                }
                string storyText = itemElement.GetAttribute("storyText");
                string storyImgPath = itemElement.GetAttribute("storyImage"); // AMP: TODO Resume here
                items_.Add(new LevelTransitionObject(nextLevel, null, Vector2.Zero, Pipeline_, new Vector2(pos.X, pos.Y), new Vector2(1f, 0f), this.isPackagedLevel_, usesStory, storyDuration, storyImgPath, storyText));
            }
            else if (itemElement.GetAttribute("type") == "aWpnBox")
            {
                Vector2 pos = new Vector2((float)Convert.ToInt32(itemElement.GetAttribute("posX")), (float)Convert.ToInt32(itemElement.GetAttribute("posY")));
                WeaponBox.WeaponType mytype;
                if (itemElement.GetAttribute("weaponType") == "machineGun")
                    mytype = WeaponBox.WeaponType.MachineGun;
                else if (itemElement.GetAttribute("weaponType") == "pistol")
                    mytype = WeaponBox.WeaponType.Pistol;
                else
                    mytype = WeaponBox.WeaponType.Shotgun;
                items_.Add(new WeaponBox(null, Pipeline_, pos, Vector2.Zero, Constants.DEPTH_LOW, mytype));
            }
        }

        /// <summary>
        /// Parse a node for an overlay graphic and add it to the drawing pipeline.
        /// </summary>
        /// <param name="overlayNode">Node containing Xml data for the overlay.</param>
        protected void parseOverlay(XmlNode overlayNode)
        {
            XmlElement overlayElement = (XmlElement)overlayNode;

            GameTexture myimage = TextureMap.fetchTexture(overlayElement.GetAttribute("texture"));            
            float posX = (float)Convert.ToInt32(overlayElement.GetAttribute("posX"));
            float posY = (float)Convert.ToInt32(overlayElement.GetAttribute("posY"));
            float depth = (float)Convert.ToDouble(overlayElement.GetAttribute("depth"));
            LevelOverlay myOverlay = new LevelOverlay(Pipeline_, myimage,new Vector2(posX, posY), Vector2.Zero, depth );
            Pipeline_.Add(myOverlay);

        }

        /// <summary>
        /// Performs several functions to prepare a Level for the Gameplay state,
        /// including setting up the WorldState, TileGrid, determining cover positions,
        /// adding objects to the collision detector, etc.
        /// </summary>
        public void initializeForGameplay()
        {
            Tile[,] tilesForCollisionGeneration = new Tile[getHeight(), getWidth()];
            for (int i = 0; i < getHeight(); i++)
            {
                for (int j = 0; j < getWidth(); j++)
                {
                    Height h = tileSet_.getHeight(tiles_[i, j]);

                    if (h.blocksHigh_)
                    {
                        tilesForCollisionGeneration[i, j].highDistance_ = 0f;
                    }
                    else
                    {
                        tilesForCollisionGeneration[i, j].highDistance_ = 1f;
                    }

                    if (h.blocksLow_)
                    {
                        tilesForCollisionGeneration[i, j].lowDistance_ = 0f;
                    }
                    else
                    {
                        tilesForCollisionGeneration[i, j].lowDistance_ = 1f;
                    }
                }
            }
            List<BoxObject> boxesToBeAddedForReal = Tiler.mergeBoxes(tilesForCollisionGeneration);

            // Calculate distances from walls to each tile
            Tile[,] tilesForGrid = CoverGenerator.generateRealTileDistances(tilesForCollisionGeneration);
            if (Settings.getInstance().IsInDebugMode_)
            {
                for (int i = 0; i < tilesForGrid.GetLength(0); i++)
                {
                    for (int j = 0; j < tilesForGrid.GetLength(1); j++)
                    {
                        Console.Write(tilesForGrid[i, j].highDistance_.ToString("F1"));
                        Console.Write(" ");
                    }
                    Console.WriteLine();
                }
            }
            GlobalHelper.getInstance().setCurrentLevelTileGrid(new TileGrid(tilesForGrid));

            // Generate cover objects and add them to collision detection
            List<CoverObject> coverObjects = CoverGenerator.generateCoverObjects(tilesForGrid);
            for (int i = 0; i < boxesToBeAddedForReal.Count; i++)
            {
                CollisionDetector_.register(boxesToBeAddedForReal[i]);
            }

            // Add other entities to collision detection and WorldState
            for (int i = 0; i < getEnemies().Count; i++)
            {
                getEnemies()[i].setCollisionDetector(CollisionDetector_);
            }
            for (int i = 0; i < getItems().Count; i++)
            {
                getItems()[i].setCollisionDetector(CollisionDetector_);
                if (getItems()[i] is AmmoBox)
                    WorldState.AmmoList_.Add(getItems()[i] as AmmoBox);
            }
            for (int i = 0; i < coverObjects.Count; i++)
            {
                coverObjects[i].setCollisionDetector(CollisionDetector_);
                WorldState.CoverList_.Add(coverObjects[i]);
            }
            List<CharacterAbstract> characterList = new List<CharacterAbstract>();
            for (int i = 0; i < getEnemies().Count; i++)
            {
                characterList.Add(getEnemies()[i]);
            }
            if (player_ != null)
            {
                characterList.Add(player_);
            }
            WorldState.CharacterList_ = characterList;
        }
        public void initializeForEditor()
        {
            
            Tile[,] tilesForCollisionGeneration = new Tile[getHeight(), getWidth()];
            for (int i = 0; i < getHeight(); i++)
            {
                for (int j = 0; j < getWidth(); j++)
                {
                    Height h = tileSet_.getHeight(tiles_[i, j]);

                    if (h.blocksHigh_)
                    {
                        tilesForCollisionGeneration[i, j].highDistance_ = 0f;
                    }
                    else
                    {
                        tilesForCollisionGeneration[i, j].highDistance_ = 1f;
                    }

                    if (h.blocksLow_)
                    {
                        tilesForCollisionGeneration[i, j].lowDistance_ = 0f;
                    }
                    else
                    {
                        tilesForCollisionGeneration[i, j].lowDistance_ = 1f;
                    }
                }
            }
            List<BoxObject> boxesToBeAddedForReal = Tiler.mergeBoxes(tilesForCollisionGeneration);

            // Calculate distances from walls to each tile
            Tile[,] tilesForGrid = CoverGenerator.generateRealTileDistances(tilesForCollisionGeneration);
            if (Settings.getInstance().IsInDebugMode_)
            {
                for (int i = 0; i < tilesForGrid.GetLength(0); i++)
                {
                    for (int j = 0; j < tilesForGrid.GetLength(1); j++)
                    {
                        Console.Write(tilesForGrid[i, j].highDistance_.ToString("F1"));
                        Console.Write(" ");
                    }
                    Console.WriteLine();
                }
            }
            GlobalHelper.getInstance().setCurrentLevelTileGrid(new TileGrid(tilesForGrid));

            // Generate cover objects and add them to collision detection
           
        }
        /// <summary>
        /// Draws all of the tiles which compose the level, and nothing else.
        /// </summary>
        public void draw()
        {
            // TODO
            // Might be easy to make an optimization that only tries to
            //  draw the tiles which are within the view of the camera
            Camera cam = GlobalHelper.getInstance().getCurrentCamera();
            float sHeight = cam.getScreenHeight();
            float sWidth = cam.getScreenWidth();
            TileGrid grid = GlobalHelper.getInstance().getCurrentLevelTileGrid();
            TileIndex lowBound = grid.getTileIndex(cam.getPosition());
            TileIndex highBound = grid.getTileIndex(cam.getPosition() + new Vector2(sWidth, sHeight));
            int lowY = (lowBound.y_ - 1 >= 0) ? lowBound.y_ - 1: 0;
            int highY = (highBound.y_ + 1 < height_) ? highBound.y_ + 1 : height_;
            int lowX = (lowBound.x_ - 1>= 0) ? lowBound.x_ - 1: 0;
            int highX = (highBound.x_ + 1 < width_) ? highBound.x_ + 1: width_;
            for (int i = lowY; i < highY; i++)
            {
                for (int j = lowX; j < highX; j++)
                {
                    Vector2 v = new Vector2(j * tileSet_.TILE_SIZE_X, i * tileSet_.TILE_SIZE_Y);
                    tileSet_.draw(v, tiles_[i, j]);
                }
            }
        }
    }
}
