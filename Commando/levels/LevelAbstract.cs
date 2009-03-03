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

namespace Commando.levels
{
    abstract class LevelAbstract
    {

        #region Members

        protected List<RoomAbstract> rooms_;

        protected Dictionary<string, string> properties_;

        protected string name_;

        protected int levelNumber_;

        protected GameTexture introScreen_;

        #endregion

        #region Constructors

        public LevelAbstract() : 
            this(new List<RoomAbstract>(), new Dictionary<string,string>(), "Level", 0, null)
        {
        }

        public LevelAbstract(string filename) :
            this()
        {
            //TODO: add code to load level from XML file
        }

        public LevelAbstract(List<RoomAbstract> rooms, Dictionary<string, string> properties, string name, int levelNumber, GameTexture introScreen)
        {
            rooms_ = rooms;
            properties_ = properties;
            name_ = name;
            levelNumber_ = levelNumber;
            introScreen_ = introScreen;
        }

        #endregion

        #region Getters

        public List<RoomAbstract> getRooms()
        {
            return rooms_;
        }

        public Dictionary<string, string> getProperties()
        {
            return properties_;
        }

        public abstract LevelAbstract getNext();

        public string getName()
        {
            return name_;
        }

        public int getLevelNumber()
        {
            return levelNumber_;
        }

        public GameTexture getIntroScreen()
        {
            return introScreen_;
        }

        #endregion

        #region Setters

        public void setRooms(List<RoomAbstract> rooms)
        {
            rooms_ = rooms;
        }

        public void setProperties(Dictionary<string, string> properties)
        {
            properties_ = properties;
        }

        public void setName(string name)
        {
            name_ = name;
        }

        public void setLevelNumber(int levelNumber)
        {
            levelNumber_ = levelNumber;
        }

        public void setIntroScreen(GameTexture introScreen)
        {
            introScreen_ = introScreen;
        }

        #endregion

        public abstract void printIntroScreen();

        public abstract void initializeLevel();
    }
}
