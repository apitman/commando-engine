/*
// ***************************************************************************
// * Copyright 2009 Eric Barnes, Ken Hartsook, Andrew Pitman, & Jared Segal  *
// *                                                                         *
// * Licensed under the Apache License, Version 2.0 (the "License");         *
// * you may not use this file except in compliance with the License.        *
// * You may obtain a copy of the License at                                 *
// *                                                                         *
// * http://www.apache.org/licenses/LICENSE-2.0                              *
// *                                                                         *
// * Unless required by applicable law or agreed to in writing, software     *
// * distributed under the License is distributed on an "AS IS" BASIS,       *
// * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.*
// * See the License for the specific language governing permissions and     *
// * limitations under the License.                                          *
// ***************************************************************************
*/

using System;
using System.Collections;
using System.Collections.Generic;
using Commando.levels;
using Microsoft.Xna.Framework;
using System.Diagnostics;

// TODO
//
// Change the implementation of the bool array to a bitset?
//
// Change the List openlist to a linked list stored inside of the
//  SearchSpace data structure by adding "next" and "previous" pointers
//  inside of AStarPathfinderNode
//

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

#if DEBUG
        public static Stopwatch clock = new Stopwatch();
#endif

        const short SEARCH_SPACE_HEIGHT = 60;
        const short SEARCH_SPACE_WIDTH = 60;

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
        protected static float searchRadius_;

        // Height of the current unit searching for a path
        protected static Height searchHeight_;

        // TileGrid being which contains information on passability
        protected static TileGrid grid_;

        // Since the AStar search space can be superimposed onto a TileGrid
        //  of any size, these offsets are used to adjust the lookup
        protected static short gridXOffset_;
        protected static short gridYOffset_;

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

        protected static Tile getTile(TileIndex index)
        {
            return grid_.getTile(index.x_ + gridXOffset_, index.y_ + gridYOffset_);
        }

        protected static Tile getTile(int x, int y)
        {
            return grid_.getTile(x + gridXOffset_, y + gridYOffset_);
        }

        /// <summary>
        /// Initializes static structures for a new search
        /// </summary>
        protected static void reset()
        {
            // CHANGE THIS
            gridXOffset_ = 0;
            gridYOffset_ = 0;

            openlist_.Clear();
            for (int i = 0; i < SEARCH_SPACE_WIDTH; i++)
            {
                for (int j = 0; j < SEARCH_SPACE_HEIGHT; j++)
                {
                    touched_[i, j] = false;
                }
            }
        }

        /// <summary>
        /// Hide default constructor
        /// </summary>
        protected AStarPathfinder() { }

        /// <summary>
        /// Add legal neighbor nodes to the open list.
        /// </summary>
        /// <param name="cur">Current position in the grid</param>
        /// <param name="cost">Cost to reach this position</param>
        protected static void expand(ref TileIndex cur, float cost)
        {
            short x = cur.x_;
            short y = cur.y_;

            // Mark nodes in 8 directions around the current position
            mark((short)(x - 1), (short)(y - 1), cost + 1.41f, ref cur);
            mark((short)(x - 1), y, cost + 1.0f, ref cur);
            mark((short)(x - 1), (short)(y + 1), cost + 1.41f, ref cur);
            mark(x, (short)(y - 1), cost + 1.0f, ref cur);
            mark(x, (short)(y + 1), cost + 1.0f, ref cur);
            mark((short)(x + 1), (short)(y - 1), cost + 1.41f, ref cur);
            mark((short)(x + 1), y, cost + 1.0f, ref cur);
            mark((short)(x + 1), (short)(y + 1), cost + 1.41f, ref cur);
        }

        /// <summary>
        /// Add a particular node in the search space to the open list if
        /// it qualifies.
        /// </summary>
        /// <param name="x">X Index of the node to be added</param>
        /// <param name="y">Y Index of the node to be added</param>
        /// <param name="g">Cost to reach this node</param>
        /// <param name="parent">Node expanded to reach this node</param>
        protected static void mark(short x, short y, float g, ref TileIndex parent)
        {
            // Verify that we are not outside the search space
            if (x < 0 || x >= SEARCH_SPACE_WIDTH || y < 0 || y >= SEARCH_SPACE_HEIGHT)
            {
                return;
            }

            // Verify that we can walk on this particular tile
            if (collision(getTile(x,y)))
            {
                return;
            }

            // If we haven't been here yet or our cost is better, update it
            // and add to the openlist
            if (!touched_[x, y] || (searchSpace_[x, y].open && g < searchSpace_[x, y].g))
            {
                searchSpace_[x, y].parent = parent;
                searchSpace_[x, y].g = g;
                searchSpace_[x, y].f = g + heuristicDistance(x, y);

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
            short bestX = -1;
            short bestY = -1;
            int bestIndex = -1;

            float bestVal = float.MaxValue;

            for (int i = 0; i < openlist_.Count; i++)
            {
                short curX = openlist_[i].x_;
                short curY = openlist_[i].y_;

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
        /// <param name="tileHeight">Height of the character performing the search</param>
        /// <returns>Waypoints which should allow the character to reach the goal.</returns>
        public static List<TileIndex>
            calculateExactPath(TileGrid grid, TileIndex start, TileIndex destination, float radius, Height height)
        {
#if DEBUG
            clock.Start();
#endif

            setupSearch(grid, start, destination, radius, height);

            //Console.WriteLine("Pathfind Start: " + start_.x_ + "," + start_.y_);
            //Console.WriteLine("Pathfind Goal: " + goal_.x_ + "," + goal_.y_);

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
#if DEBUG
                    clock.Stop();
#endif
                    return null;
                }
                else
                {
                    cur = getNext();
                }
            }
#if DEBUG
            clock.Stop();
#endif
            return recreatePath();
        }


        public static List<TileIndex>
            calculateExactPath(TileGrid grid, Vector2 start, Vector2 destination, float radius, Height height)
        {
            return calculateExactPath(grid, grid.getTileIndex(start), grid.getTileIndex(destination), radius, height);
        }

        public static List<TileIndex>
            calculateNearbyPath(TileGrid grid, TileIndex start, TileIndex destination, float radius, Height height, float threshold)
        {
            setupSearch(grid, start, destination, radius, height);

            //Console.WriteLine("Pathfind Start: " + start_.x_ + "," + start_.y_);
            //Console.WriteLine("Pathfind Goal: " + goal_.x_ + "," + goal_.y_);

            TileIndex cur = start_;
            searchSpace_[cur.x_, cur.y_].g = 0;
            searchSpace_[cur.x_, cur.y_].f = heuristicDistance(cur.x_, cur.y_);
            searchSpace_[cur.x_, cur.y_].parent = new TileIndex(-1, -1); // stack
            touched_[cur.x_, cur.y_] = true;

            // As long as we aren't near enough to the goal, keep expanding outward and checking
            // the next most promising node
            while (heuristicDistance(cur.x_, cur.y_) > threshold)
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

        public static List<TileIndex>
            calculateNearbyPath(TileGrid grid, Vector2 start, Vector2 destination, float radius, Height height, float threshold)
        {
            return calculateNearbyPath(grid, grid.getTileIndex(start), grid.getTileIndex(destination), radius, height, threshold);
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
            while (!TileIndex.equals(cur, start_))
            {
                TileIndex adjusted = cur;
                adjusted.x_ += gridXOffset_;
                adjusted.y_ += gridYOffset_;
                path.Insert(0, adjusted);
                cur = searchSpace_[cur.x_, cur.y_].parent;
            }

            /*
            for (int i = 0; i < path.Count; i++)
            {
                Console.WriteLine("Goto " + path[i].x_ + "," + path[i].y_);
            }
            */

            return path;
        }

        protected static bool collision(Tile tile)
        {
            return tile.collides(searchHeight_, searchRadius_);
        }

        protected static void setupSearch(TileGrid grid, TileIndex start, TileIndex destination, float radius, Height height)
        {
            // Initialize the static structures used by the search
            reset();

            start_ = start;
            goal_ = destination;

            // Project our start and goal nodes into the search space, if possible
            // TODO
            // For now, if they are farther apart than the size of the search space in
            // any dimension, we give up - in the future, they will try to get as close
            // as possible
            short manhattanX = (short)Math.Abs(start_.x_ - goal_.x_);
            short manhattanY = (short)Math.Abs(start_.y_ - goal_.y_);
            short leftOffset = (short)Math.Floor((SEARCH_SPACE_WIDTH - manhattanX) / 2.0f);
            short rightOffset = (short)Math.Ceiling((SEARCH_SPACE_WIDTH - manhattanX) / 2.0f);
            short bottomOffset = (short)Math.Floor((SEARCH_SPACE_HEIGHT - manhattanY) / 2.0f);
            short topOffset = (short)Math.Ceiling((SEARCH_SPACE_HEIGHT - manhattanY) / 2.0f);
            gridXOffset_ = (short)(Math.Min(start_.x_, goal_.x_) - leftOffset);
            gridYOffset_ = (short)(Math.Min(start_.y_, goal_.y_) - topOffset);

            if (gridXOffset_ < 0) gridXOffset_ = 0;
            if (gridYOffset_ < 0) gridYOffset_ = 0;
            start_.x_ -= gridXOffset_;
            start_.y_ -= gridYOffset_;
            goal_.x_ -= gridXOffset_;
            goal_.y_ -= gridYOffset_;

            searchRadius_ =
                (float)(radius / ((TileGrid.TILEWIDTH + TileGrid.TILEHEIGHT) / 2.0f));
            grid_ = grid;
            searchHeight_ = height;
        }
    }

    struct AStarPathfinderNode
    {
        internal float g;
        internal float f;
        internal TileIndex parent;
        internal bool open;
    }

}
