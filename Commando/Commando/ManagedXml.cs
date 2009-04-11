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
using System.Xml;

namespace Commando
{
    /// <summary>
    /// An XmlDocument with the IDisposable interface which removes all
    /// of its children's links to one another, making it easier for the
    /// garbage collector to detect and remove this garbage.
    /// </summary>
    public class ManagedXml : XmlDocument, IDisposable
    {
        public ManagedXml()
            : base()
        {

        }

        public void Dispose()
        {
            // TODO
            // Figure out how to make it so that this line doesn't actually
            // mess up the cached content.

            //XmlHelper.cleanupNode(this);
        }
    }
}
