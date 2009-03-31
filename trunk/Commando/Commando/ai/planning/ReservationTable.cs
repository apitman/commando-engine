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

namespace Commando.ai.planning
{
    /// <summary>
    /// Allows agents to register their intent to utilize a particular resource,
    /// in order to achieve coordination amongst resource usage via social laws.
    /// </summary>
    internal static class ReservationTable
    {
        private static Dictionary<Object, NonPlayableCharacterAbstract> reservations_;

        static ReservationTable()
        {
            reservations_ = new Dictionary<object, NonPlayableCharacterAbstract>();
        }

        /// <summary>
        /// Determine whether a resource is free for reserving.
        /// </summary>
        /// <param name="resource">The resource in question.</param>
        /// <returns>True if the resource can be reserved, false if it is already reserved.</returns>
        internal static bool isFree(Object resource)
        {
            return !reservations_.ContainsKey(resource);
        }

        /// <summary>
        /// Determine whether a resource is reserved.
        /// </summary>
        /// <param name="resource">The resource in question.</param>
        /// <returns>True if the resource is reserved, false if it is free.</returns>
        internal static bool isReserved(Object resource)
        {
            return !isFree(resource);
        }

        /// <summary>
        /// Used to determine whether a resource is reserved by a particular entity.
        /// </summary>
        /// <param name="resource">The resource in question.</param>
        /// <param name="owner">Possible owner of the reservation.</param>
        /// <returns>Whether 'owner' has a reservation for this resource.</returns>
        internal static bool isReservedBy(Object resource, NonPlayableCharacterAbstract owner)
        {
            if (owner == null)
            {
                // The idea behind this exception is that if it did not exist, characters
                // attempting to check a resource for reservation could glean information that it
                // had in fact been consumed, and remove it from their list of beliefs.

                // On second thought, they'll never try and use it anyway so that wouldn't
                // really help all that much.

                // throw new AccessViolationException("ReservationTable should not be used to determine if a resource has been consumed.");
            }
            return (reservations_.ContainsKey(resource) && reservations_[resource] == owner);
        }

        /// <summary>
        /// Determine whether a resource has been consumed.
        /// </summary>
        /// <param name="resource">The resource in question.</param>
        /// <returns>Whether that resource has been consumed.</returns>
        internal static bool isConsumed(Object resource)
        {
            return isReservedBy(resource, null);
        }

        /// <summary>
        /// Attempt to reserve a resource.
        /// </summary>
        /// <param name="resource">The resource in question.</param>
        /// <param name="reserver">Entity attempting to reserve the resource.</param>
        /// <returns>True if the resource was successfully reserved, false otherwise.</returns>
        internal static bool reserveResource(Object resource, NonPlayableCharacterAbstract reserver)
        {
            if (isFree(resource))
            {
                reservations_.Add(resource, reserver);

                BeliefType type = BeliefType.Error;
                if (resource is CoverObject)
                {
                    type = BeliefType.AvailableCover;
                }

                if (type == BeliefType.Error)
                {
                    throw new NotImplementedException("Reservations for this object type not yet implemented");
                }

                // notify other agents in the world that this resource is taken
                List<NonPlayableCharacterAbstract> npcs = WorldState.EnemyList_;
                for (int i = 0; i < npcs.Count; i++)
                {
                    if (npcs[i] != reserver)
                    {
                        npcs[i].AI_.Memory_.removeBelief(type, resource);
                    }
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// Destroy a resource reservation so another can reserve it.
        /// </summary>
        /// <param name="resource">The resource in question.</param>
        /// <param name="owner">Current owner of the resource.</param>
        internal static void freeResource(Object resource, NonPlayableCharacterAbstract owner)
        {
            if (reservations_.ContainsKey(resource))
            {
                if (owner == null)
                {
                    throw new InvalidOperationException("Consumed resources cannot be freed.");
                }

                if (reservations_[resource] == owner)
                {
                    reservations_.Remove(resource);
                }
                else
                {
                    throw new InvalidOperationException("This NPC does not own this resource");
                }
            }
        }

        /// <summary>
        /// Consume a resource so that it can never again be reserved.
        /// </summary>
        /// <param name="resource">The resource in question.</param>
        /// <param name="owner">Current owner of the resource reservation.</param>
        internal static void consumeResource(Object resource, NonPlayableCharacterAbstract owner)
        {
            if (reservations_.ContainsKey(resource))
            {
                if (reservations_[resource] == owner)
                {
                    reservations_[resource] = null;
                }
                else
                {
                    throw new InvalidOperationException("This NPC does not own this resource");
                }
            }
        }

        /// <summary>
        /// Completely wipes all reservations, even those of consumed resources.
        /// Should only be used when switching between levels.
        /// </summary>
        internal static void reset()
        {
            reservations_.Clear();
        }
    }
}
