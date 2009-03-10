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
using Commando.objects;

namespace Commando.levels
{
    class Level
    {
        protected Tileset tileSet_;

        protected int height_;

        protected int width_;

        protected TileObject[,] tiles_;

        protected List<NonPlayableCharacterAbstract> enemies_;

        protected PlayableCharacterAbstract player_;

        protected List<LevelObjectAbstract> items_;

        public Level(Tileset tileSet, TileObject[,] tiles)
        {
            tileSet_ = tileSet;
            tiles_ = tiles;
            height_ = tiles_.GetLength(0);
            width_ = tiles_.GetLength(1);
            player_ = null;
            enemies_ = new List<NonPlayableCharacterAbstract>();
            items_ = new List<LevelObjectAbstract>();
        }

        public int getHeight()
        {
            return height_;
        }

        public int getWidth()
        {
            return width_;
        }

        public TileObject[,] getTiles()
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

        public void getEnemies(List<NonPlayableCharacterAbstract> enemies)
        {
            enemies_ = enemies;
        }

        public void getItems(List<LevelObjectAbstract> items)
        {
            items_ = items;
        }

        public void getPlayer(PlayableCharacterAbstract player)
        {
            player_ = player;
        }

        public static Level getLevelFromFile(string filename)
        {
            throw new NotImplementedException();
        }

        public static void writeLevelToFile(string filename)
        {
            throw new NotImplementedException();
        }
    }
}
