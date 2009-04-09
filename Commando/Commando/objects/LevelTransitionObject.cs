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
using System.IO;
using Commando.collisiondetection;
using Microsoft.Xna.Framework;
using Commando.levels;
using Microsoft.Xna.Framework.Storage;

namespace Commando.objects
{
    public class LevelTransitionObject : TransitionObjectAbstract
    {
        protected const float RADIUS = 20f;
        protected static readonly Height HEIGHT = new Height(true, true);
        protected const float DRAW_DEPTH = Constants.DEPTH_LOW;

        protected string nextLevelName_;

        protected List<Vector2> points_;

        protected string nextLevelPath_;

        protected bool isPackaged_;

        protected static readonly List<Vector2> BOUND_POINTS;
        static LevelTransitionObject()
        {
            BOUND_POINTS = new List<Vector2>();
            BOUND_POINTS.Add(new Vector2(-15.0f, -15.0f));
            BOUND_POINTS.Add(new Vector2(15.0f, -15.0f));
            BOUND_POINTS.Add(new Vector2(15.0f, 15.0f));
            BOUND_POINTS.Add(new Vector2(-15.0f, 15.0f));
        }

        public LevelTransitionObject(string nextLevel, CollisionDetectorInterface detector, Vector2 center, List<DrawableObjectAbstract> pipeline, Vector2 position, Vector2 direction, bool isPackaged)
            : base(pipeline, detector, TextureMap.fetchTexture("levelTransition"), position, direction, DRAW_DEPTH, RADIUS, HEIGHT)
        {
            bounds_ = new ConvexPolygon(BOUND_POINTS, center);
            bounds_.rotate(direction_, position_);
            
            nextLevelName_ = nextLevel;
            isPackaged_ = isPackaged;
        }

        public string getNextLevel()
        {
            return nextLevelName_;
        }
        public void setNextLevel(string nextLevel)
        {
            nextLevelName_ = nextLevel;
        }


        public override EngineStateInterface go()
        {
            Engine engine = Settings.getInstance().EngineHandle_;
            Level level;
            if (isPackaged_)
            {
                level = Level.getLevelFromContent(nextLevelName_, engine);
            }
            else
            {
                // TODO
                // Get this to work on PCs without GamerServices
                StorageContainer container = ContainerManager.getOpenContainer();
                string directory = Path.Combine(container.Path, EngineStateLevelSave.DIRECTORY_NAME);
                string nextLevelPath = Path.Combine(directory, nextLevelName_) + EngineStateLevelSave.LEVEL_EXTENSION;

                level = Level.getLevelFromFile(nextLevelPath, engine);
            }
            return new EngineStateGameplay(engine, level);
        }
    }
}
