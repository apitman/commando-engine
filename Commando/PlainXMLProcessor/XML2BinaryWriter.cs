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
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using System.Xml;
using Microsoft.Xna.Framework;

namespace PlainXMLProcessor
{
    /// <summary>
    /// Converts XmlDocument's into a .xnb file which is stored as a resource in "Content."
    /// This occurs at build-time, after the file is imported and processed.
    /// </summary>
    [ContentTypeWriter]
    class XML2BinaryWriter : ContentTypeWriter<XmlDocument>
    {
        protected override void Write(ContentWriter output, XmlDocument value)
        {
            string s = value.InnerXml;
            //output.Write(s.Length);
            output.Write(s.Length);
            output.Write(s.ToCharArray());
        }
        public override string GetRuntimeType(TargetPlatform targetPlatform)
        {
            return typeof(XmlDocument).AssemblyQualifiedName;
        }
        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            if (targetPlatform == TargetPlatform.Windows)
            {
                return @"PlainXMLProcessor.Binary2XMLReader, Commando, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
            }
            else if (targetPlatform == TargetPlatform.Xbox360)
            {
                return @"PlainXMLProcessor.Binary2XMLReader, CommandoXbox, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
            }
            throw new NotImplementedException("Only PC and Xbox360 supported for XML2BinaryWriter");
        }
    }
}
