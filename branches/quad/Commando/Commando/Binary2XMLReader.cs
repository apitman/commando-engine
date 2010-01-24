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
using Microsoft.Xna.Framework.Content;
using System.Xml;
using System.IO;
using Commando;

namespace PlainXMLProcessor
{
    /// <summary>
    /// Used at runtime when Content.Load[ManagedXml]() is called (but with carrots,
    /// not square brackets); responsible for converting the compiled .xnb file into
    /// a ManagedXml.
    /// </summary>
    class Binary2XMLReader : ContentTypeReader<XmlDocument>
    {
        protected override XmlDocument Read(ContentReader input, XmlDocument existingInstance)
        {
            int count = input.ReadInt32();
            char[] chars = input.ReadChars(count);
            string s = new string(chars);
            XmlDocument xmldoc = new XmlDocument();
            try
            {
                xmldoc.Load(new StringReader(s));
            }
            catch (Exception e)
            {
                throw new Exception("Could not load resource file as XML", e);
            }
            return xmldoc;
        }
    }
}
