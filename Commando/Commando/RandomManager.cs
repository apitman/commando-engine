﻿/*
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

namespace Commando
{
    /// <summary>
    /// Allows universal access to a single Random object.
    /// Otherwise multiple Random objects created in a short period of time
    ///     all produce the same values.
    /// </summary>
    public static class RandomManager
    {
        private static double cachedValue = 0f;
        private static bool cached = false;

        private static Random random;

        static RandomManager()
        {
            random = new Random();
        }

        public static Random get()
        {
            return random;
        }

        public static float nextNormalDistValue(float mean, float std)
        {
            return (float)nextNormalDistValue((double)mean, (double)std);
        }

        public static double nextNormalDistValue(double mean, double std)
        {
            if (cached)
            {
                cached = false;
                return cachedValue*std + mean;
            }

            // Box-Muller algorithm, polar-style, takes two random numbers
            //  from a uniform distribution over [-1, +1] and converts them
            //  to a standard normal distribution (Z)
            Random r = get();
            while (!cached)
            {
                double u = r.NextDouble() * 2 - 1;
                double v = r.NextDouble() * 2 - 1;
                double s = u * u + v * v;
                if (s == 0 || s > 1)
                    continue;

                double rad = Math.Sqrt((-2 * Math.Log(s))/s);
                double z0 = u * rad;
                double z1 = v * rad;
                cached = true;
                cachedValue = z1;
                return (z0 * std + mean);
            }

            // make compiler happy - otherwise it thinks not all code paths
            //  return a value
            throw new Exception("Unexpected code path in RandomManager");
        }
    }
}
