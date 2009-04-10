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

namespace Commando
{
    internal static class PerformanceLogger
    {
        internal static Dictionary<MetricType, Dictionary<long, int>> data_ =
            new Dictionary<MetricType, Dictionary<long, int>>();

        internal static void addValue(MetricType type, long value)
        {
            Dictionary<long, int> histogram;
            if (!data_.TryGetValue(type, out histogram))
            {
                histogram = new Dictionary<long, int>();
                data_.Add(type, histogram);
                histogram.Add(value, 1);
                return;
            }

            if (histogram.ContainsKey(value))
            {
                histogram[value] += 1;
            }
            else
            {
                histogram.Add(value, 1);
            }
        }

        internal static string printMetric(MetricType metric)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine();
            sb.AppendLine();
            sb.AppendLine();
            sb.AppendLine(metric.ToString());

            Dictionary<long, int> histogram;
            if (!data_.TryGetValue(metric, out histogram))
            {
                sb.AppendLine("No data collected");
                return sb.ToString();
            }

            /*
            for (Dictionary<long, int>.Enumerator i = histogram.GetEnumerator(); i.MoveNext(); )
            {
                KeyValuePair<long, int> pair = i.Current;
                sb.Append(pair.Key);
                sb.Append(": ");
                sb.AppendLine(pair.Value);
            }
             */

            List<long> keys = histogram.Keys.ToList<long>();
            keys.Sort();
            foreach (long key in keys)
            {
                sb.Append(key);
                sb.Append(": ");
                sb.AppendLine(histogram[key].ToString());
            }

            return sb.ToString();
        }
    }

    public class PerformanceMonitorException : Exception
    {
        public PerformanceMonitorException(string message)
            : base(message)
        {

        }

        public override string StackTrace
        {
            get
            {
                return "";
            }
        }
    }

    enum MetricType
    {
        UPDATE,
        DRAW,
        PLAN,
        PATHFIND,
        RAYCAST
    }
}
