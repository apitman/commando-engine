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
using Microsoft.Xna.Framework;
using Commando.levels;
using System.Diagnostics;

namespace Commando.ai
{
    /// <summary>
    /// Raycaster performs various useful functions for lines and line segments,
    /// such as whether a line segment is unobstructed or where a ray will become
    /// obstructed by an obstacle in the TileGrid.
    /// </summary>
    static internal class Raycaster
    {
#if DEBUG
        public static Stopwatch clock = new Stopwatch();
        public static int frameCount = 0;
#endif

        /// <summary>
        /// Number of samples points per tile to check for obstruction
        /// </summary>
        const int SAMPLE_FREQ = 3;

        /// <summary>
        /// Derived distance between sample points
        /// </summary>
        const float SAMPLE_LENGTH = (TileGrid.TILEHEIGHT + TileGrid.TILEWIDTH) / (2 * SAMPLE_FREQ);
        const float SAMPLE_LENGTH_SQ = SAMPLE_LENGTH * SAMPLE_LENGTH;

        /// <summary>
        /// Determines whether an entity can "see" another entity, or more specifically, if the
        ///     line segment between the two entities is unobstructed.
        /// </summary>
        /// <param name="start">Position of entity trying to see a target</param>
        /// <param name="dest">Position of target</param>
        /// <param name="visionHeight">Heights at which the source entity can see</param>
        /// <param name="targetHeight">Heights at which the target can be seen</param>
        /// <returns>True if the source entity can see the target, false otherwise</returns>
        static internal bool canSeePoint(Vector2 start, Vector2 dest, Height visionHeight, Height targetHeight)
        {
#if DEBUG
            clock.Start();
            frameCount++;
#endif

            TileGrid grid = GlobalHelper.getInstance().getCurrentLevelTileGrid();

            Vector2 direction = dest - start;
            direction.Normalize();

            Vector2 sampleInterval = direction * SAMPLE_LENGTH;

            Vector2 current = start;
            Height currentVisionHeight = new Height(true, true);
            while ((current - dest).LengthSquared() > SAMPLE_LENGTH_SQ)
            {
                Tile tile = grid.getTile(current);
                if (tile.highDistance_ == 0)
                    currentVisionHeight.blocksHigh_ = false;
                if (tile.lowDistance_ == 0)
                    currentVisionHeight.blocksLow_ = false;
                if (!currentVisionHeight.collides(visionHeight))
                {
#if DEBUG
                    clock.Stop();
#endif
                    return false;
                }
                current += sampleInterval;
            }
#if DEBUG
            clock.Stop();
#endif
            return currentVisionHeight.collides(targetHeight);
        }

        /// <summary>
        /// Determines an approximate point that a ray intersects an obstacle
        /// </summary>
        /// <param name="start">Source point of the ray</param>
        /// <param name="direction">Direction the ray is being projected</param>
        /// <param name="h">Heights at which to look for an obstruction</param>
        /// <returns>A guess position of where the ray is first obstructed</returns>
        static internal Vector2 roughCollision(Vector2 start, Vector2 direction, Height h)
        {
            TileGrid grid = GlobalHelper.getInstance().getCurrentLevelTileGrid();

            direction.Normalize();

            Vector2 sampleInterval = direction * SAMPLE_LENGTH;

            Vector2 current = start;
            while (true)
            {
                Tile tile = grid.getTile(current);
                if (tile.collides(h, 0))
                {
                    return current;
                }
                current += sampleInterval;
            }
        }

        static internal bool canSeeObject(Vector2 start, Vector2 objectPos, float objectRadius, Height visionHeight, Height objectHeight)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Determines whether a target object is within a specific field of view.
        /// </summary>
        /// <param name="facing">Direction the observer is facing</param>
        /// <param name="position">Position of the observer</param>
        /// <param name="targetPosition">Position of the target</param>
        /// <param name="fieldAngle">Range, in radians, of the observer's field of view</param>
        /// <returns></returns>
        static internal bool inFieldOfView(Vector2 facing, Vector2 position, Vector2 targetPosition, float fieldAngle)
        {
            Vector2 relativePosition = targetPosition - position;

            if (facing.X == 0)
                facing.X = 0.001f;
            double facingAngle = Math.Atan(facing.Y / facing.X);
            if (facing.X < 0)
                facingAngle += Math.PI;
            facingAngle = normalizeAngle(facingAngle);

            if (relativePosition.X == 0)
                relativePosition.X = 0.001f;
            double relativeAngle = Math.Atan(relativePosition.Y / relativePosition.X);
            if (relativePosition.X < 0)
                relativeAngle += Math.PI;
            relativeAngle = normalizeAngle(relativeAngle);

            double shortestAngle = Math.Min(normalizeAngle(facingAngle - relativeAngle),
                                            normalizeAngle(relativeAngle - facingAngle));

            return shortestAngle <= fieldAngle / 2;
        }

        static internal double normalizeAngle(double angle)
        {
            while (angle < 0)
                angle += Math.PI * 2;
            while (angle > Math.PI * 2)
                angle -= Math.PI * 2;
            return angle;
        }

        static internal float normalizeAngle(float angle)
        {
            while (angle < 0)
                angle += (float)Math.PI * 2;
            while (angle > Math.PI * 2)
                angle -= (float)Math.PI * 2;
            return angle;
        }
    }
}
