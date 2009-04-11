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
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;
using System.Xml;
using System.IO;

namespace PlainXMLProcessor
{
    /// <summary>
    /// Runs at compile time to convert the strings loaded by the XMLImporter
    /// into an actual XmlDocument.
    /// </summary>
    [ContentProcessor(DisplayName = "PlainXMLProcessor.XMLProcessor")]
    public class XMLProcessor : ContentProcessor<string, XmlDocument>
    {
        public override XmlDocument Process(string input, ContentProcessorContext context)
        {
            // This technique from:
            // http://www.velocityreviews.com/forums/t296139-read-xml-from-string-instead-of-file-c.html

            XmlDocument xmldoc = new XmlDocument();
            try
            {
                xmldoc.Load(new StringReader(input));
            }
            catch (Exception e)
            {
                throw new InvalidContentException("Imported content does not appear to be a valid XML string", e);
            }
            return xmldoc;
        }
    }
}