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
using System.Collections;
using System.Collections.Generic;
using Commando.levels;
using Microsoft.Xna.Framework;

namespace Commando.ai
{
    /// <summary>
    /// Finds paths by implementing A* on an 8-directional grid of the map,
    /// using a linear distance heuristic.
    /// </summary>
    public class AStarPathfinder
    {

        /// 
        /// Some A* terms:
        /// g - cost to a node
        /// h - heuristic estimate of a node
        /// f - g + h, used to compare nodes for selection
        /// openlist - nodes which have been reached but not tested/expanded
        /// 
        /// NOTE: Almost all of the structures here are static, even though they
        /// must be replaced with each new usage of the class.  The static data
        /// structures are used for performance reasons.  This algorithm is very
        /// costly in CPU time and it is beneficial to reduce the number of params
        /// being passed around, as well as keeping allocated memory so that it is
        /// not reallocated with each use of the algorithm.
        /// 

        const int SEARCH_SPACE_HEIGHT = 60;
        const int SEARCH_SPACE_WIDTH = 60;

        // Preallocated search nodes in a 2D grid
        static AStarPathfinderNode[,] searchSpace_ =
            new AStarPathfinderNode[SEARCH_SPACE_WIDTH, SEARCH_SPACE_HEIGHT];
        
        // 2D array of booleans for whether the corresponding index in searchSpace
        //  has been touched by the current search
        // i.e., true = reached in current search, false = garbage data
        protected static bool[,] touched_ =
            new bool[SEARCH_SPACE_WIDTH, SEARCH_SPACE_HEIGHT];

        // TODO Currently being tested, an alternate implementation of touched_
        protected static BitArray touched2_ =
            new BitArray(SEARCH_SPACE_WIDTH * SEARCH_SPACE_HEIGHT);

        // List of nodes which have been touched but not expanded
        protected static List<TileIndex> openlist_ =
            new List<TileIndex>(SEARCH_SPACE_HEIGHT * SEARCH_SPACE_WIDTH);

        // Starting point of the search
        protected static TileIndex start_;

        // Target tile of the search
        protected static TileIndex goal_;

        // Radius of the current unit searching for a path, in tiles
        protected static int searchRadius_;

        // Height of the current unit searching for a path
        protected static TileHeight searchHeight_;

        // TileGrid being which contains information on passability
        protected static Tile[,] grid_;

        /*
        /// <summary>
        /// Possibly unnecessary function, try without it
        /// </summary>
        protected static void init()
        {
            // testing
            reset();
            for (int i = 0; i < 60; i++)
            {
                for (int j = 0; j < 60; j++)
                {
                    searchSpace[i, j].open = false;
                }
            }
        }
        */

        /// <summary>
        /// Initializes static structures for a new search
        /// </summary>
        protected static void reset()
        {
            openlist_.Clear();
            for (int i = 0; i < 60; i++)
            {
                for (int j = 0; j < 60; j++)
                {
                    touched_[i, j] = false;
                }
            }
        }

        /// <summary>
        /// Hide default constructor
        /// </summary>
        protected AStarPathfinder() {}

        /// <summary>
        /// Add legal neighbor nodes to the open list.
        /// </summary>
        /// <param name="cur">Current position in the grid</param>
        /// <param name="cost">Cost to reach this position</param>
        protected static void expand(ref TileIndex cur, float cost)
        {
            int x = cur.x_;
            int y = cur.y_;

            // Mark nodes in 8 directions around the current position
            mark(x - 1, y - 1, cost + 1.41f, ref cur);
            mark(x - 1, y, cost + 1.0f, ref cur);
            mark(x - 1, y + 1, cost + 1.41f, ref cur);
            mark(x, y - 1, cost + 1.0f, ref cur);
            mark(x, y + 1, cost + 1.0f, ref cur);
            mark(x + 1, y - 1, cost + 1.41f, ref cur);
            mark(x + 1, y, cost + 1.0f, ref cur);
            mark(x + 1, y + 1, cost + 1.41f, ref cur);
        }

        /// <summary>
        /// Add a particular node in the search space to the open list if
        /// it qualifies.
        /// </summary>
        /// <param name="x">X Index of the node to be added</param>
        /// <param name="y">Y Index of the node to be added</param>
        /// <param name="g">Cost to reach this node</param>
        /// <param name="parent">Node expanded to reach this node</param>
        protected static void mark(int x, int y, float g, ref TileIndex parent)
        {
            // Verify that we are not outside the search space
            if(x < 0 || x >= SEARCH_SPACE_WIDTH || x < 0 || y >= SEARCH_SPACE_HEIGHT)
            {
                return;
            }

            // Verify that we can walk on this particular tile
            if ((int)searchHeight_ <= (int)grid_[x,y].tileHeight_)
            {
                return;
            }
            
            // If we haven't been here yet or our new f-value is better, update it
            // and add to the openlist
            float h = heuristicDistance(x, y);
            float f = g + h;
            if (!touched_[x, y] || (searchSpace_[x, y].open && g < searchSpace_[x, y].g))
            {

                searchSpace_[x, y].parent = parent;
                searchSpace_[x, y].g = g;
                searchSpace_[x, y].f = f;

                // Hasn't been found before, so add it to openlist
                if (!touched_[x, y])
                {
                    touched_[x, y] = true;
                    searchSpace_[x, y].open = true;

                    TileIndex tile = new TileIndex(x, y); // stack
                    openlist_.Add(tile);
                }
            }
            
        }

        /// <summary>
        /// Scan the openlist for the lowest f-value, which is the next node to
        /// expand, remove it from the openlist, and return it.
        /// </summary>
        /// <returns>The next node for A* to expand</returns>
        protected static TileIndex getNext()
        {
            int bestX = -1;
            int bestY = -1;
            int bestIndex = -1;

            float bestVal = float.MaxValue;

            for (int i = 0; i < openlist_.Count; i++)
            {
                int curX = openlist_[i].x_;
                int curY = openlist_[i].y_;

                if (searchSpace_[curX, curY].f < bestVal && searchSpace_[curX, curY].open)
                {
                    bestVal = searchSpace_[curX, curY].f;
                    bestX = curX;
                    bestY = curY;
                    bestIndex = i;
                }

            }

            TileIndex best = new TileIndex(bestX, bestY); // stack

            openlist_.RemoveAt(bestIndex);

            return best;
        }

        /// <summary>
        /// Calculates straight-line distance from a TileIndex x,y to the goal
        /// </summary>
        /// <param name="x">X Coordinate of the TileIndex</param>
        /// <param name="y">Y Coordinate of the TileIndex</param>
        /// <returns>Distance from the provided values to the goal</returns>
        protected static float heuristicDistance(int x, int y)
        {
            return (float)Math.Sqrt(Math.Pow((goal_.x_ - x), 2) + Math.Pow((goal_.y_ - y), 2));
        }

        /// <summary>
        /// Run the A* algorithm to get a navigation path.
        /// </summary>
        /// <param name="grid">The area being searched</param>
        /// <param name="start">Index of the start tile.</param>
        /// <param name="destination">Index of the destination tile.</param>
        /// <param name="radius">Radius of the character performing the search</param>
        /// <param name="height">Height of the character performing the search</param>
        /// <returns>Waypoints which should allow the character to reach the goal.</returns>
        public static List<TileIndex>
            run(TileGrid grid, Vector2 start, Vector2 destination, float radius, TileHeight height)
        {
            // Initialize the static structures used by the search
            reset();

            start_ = TileGrid.getTileIndex(start);
            goal_ = TileGrid.getTileIndex(destination);
            searchRadius_ =
                (int)Math.Ceiling(radius / ((TileGrid.TILEWIDTH + TileGrid.TILEHEIGHT) / 2.0f));
            grid_ = grid.getTiles();

            TileIndex cur = start_;
            searchSpace_[cur.x_, cur.y_].g = 0;
            searchSpace_[cur.x_, cur.y_].f = heuristicDistance(cur.x_, cur.y_);
            searchSpace_[cur.x_, cur.y_].parent = new TileIndex(-1, -1); // stack
            touched_[cur.x_, cur.y_] = true;

            // As long as we aren't at the goal, keep expanding outward and checking
            // the next most promising node
            while (!TileIndex.equals(goal_, cur))
            {
                searchSpace_[cur.x_, cur.y_].open = false;
                expand(ref cur, searchSpace_[cur.x_, cur.y_].g);

                // If we are out of nodes to check, no path exists
                if (openlist_.Count == 0)
                {
                    return null;
                }
                else
                {
                    cur = getNext();
                }
            }

            return recreatePath();
        }

        /// <summary>
        /// Rewind up the search space results to build the path from
        /// start to finish.
        /// </summary>
        /// <returns>TileIndexes which form a path from (start, goal]</returns>
        protected static List<TileIndex> recreatePath()
        {
            List<TileIndex> path = new List<TileIndex>();
            TileIndex cur = goal_;
            while (!TileIndex.equals(cur,start_))
            {
                path.Insert(0, cur);
                cur = searchSpace_[cur.x_, cur.y_].parent;
            }

            return path;
        }

        /*
        public static void Main()
        {

            TileType[,] map = new TileType[60, 60];

            TileIndex start;
            TileIndex end;

            Random r = new Random();

            Pathfinder.init();

            for (int i = 0; i < 50; i++)
            {
                start.x = r.Next(60);
                start.y = r.Next(60);
                end.x = r.Next(60);
                end.y = r.Next(60);

                System.Console.Write(start.x);
                System.Console.Write(",");
                System.Console.Write(start.y);
                System.Console.Write(" to ");
                System.Console.Write(end.x);
                System.Console.Write(",");
                System.Console.Write(end.y);
                System.Console.Write(" took ");

                Stopwatch sw = new Stopwatch();
                sw.Start();
                int result = aStar(start, end);
                sw.Stop();

                System.Console.WriteLine(sw.ElapsedMilliseconds);
            }

            for (int i = 0; i < 12; i++)
            {
                start.x = 0;
                start.y = 0;
                end.x = i * 5;
                end.y = i * 5;

                System.Console.Write(start.x);
                System.Console.Write(",");
                System.Console.Write(start.y);
                System.Console.Write(" to ");
                System.Console.Write(end.x);
                System.Console.Write(",");
                System.Console.Write(end.y);
                System.Console.Write(" took ");

                Stopwatch sw = new Stopwatch();
                sw.Start();
                int result = aStar(start, end);
                sw.Stop();

                System.Console.WriteLine(sw.ElapsedMilliseconds);
            }
        }
         */
    }

    struct AStarPathfinderNode
    {
        internal float g;
        internal float f;
        internal TileIndex parent;
        internal bool open;
    }

}
