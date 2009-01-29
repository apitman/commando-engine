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

namespace Commando.collisiondetection
{
    class CollisionDetectionGrid
    {
        protected static int INITIALLISTCAPACITY = 40;
        protected List<CollisionObjectAbstract>[,] grid_;

        public CollisionDetectionGrid(int dim_x, int dim_y)
        {
            grid_ = new List<CollisionObjectAbstract>[dim_x, dim_y];

            for (int i = 0; i < dim_x; i++)
            {
                for (int j = 0; j < dim_y; j++)
                {
                    grid_[i, j] = new List<CollisionObjectAbstract>(INITIALLISTCAPACITY);
                }
            }
        }

        public void addElement(int x, int y, CollisionObjectAbstract obj)
        {

        }

        public List<CollisionObjectAbstract> getElementsAt(int x, int y)
        {
            return grid_[x, y];
        }
    }
}
