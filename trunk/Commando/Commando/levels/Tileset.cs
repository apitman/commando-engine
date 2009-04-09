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
using System.Xml;
using Microsoft.Xna.Framework.Graphics;

namespace Commando.levels
{
    /// <summary>
    /// Data structure for a tileset read in from an XML file; maps tile index
    /// numbers to pictures and tile information such as heights.
    /// </summary>
    public class Tileset
    {
        protected Dictionary<int, Height> heights_ { get; private set;}
        protected Dictionary<int, GameTexture> textures_{ get; private set;}

        public Tileset()
        {
            heights_ = new Dictionary<int, Height>();
            textures_ = new Dictionary<int, GameTexture>();
        }

        public static Tileset constructTileset(string filepath)
        {
            throw new NotImplementedException("Tileset class not ready");
        }

        public static Tileset getTilesetFromContent(string tilesetName, Engine e)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(tilesetName);
            return Tileset.getTilesetFromXML(doc, e.spriteBatch_, e.GraphicsDevice);
        }

        protected static Tileset getTilesetFromXML(XmlDocument doc, SpriteBatch spriteBatch, GraphicsDevice graphics)
        {
            Tileset returnTileset = new Tileset();
            XmlElement ele = (XmlElement)doc.GetElementsByTagName("image-file")[0];
            string image_file = ele.InnerText;
            ele = (XmlElement)doc.GetElementsByTagName("tile-size")[0];
            int tile_size_x = Convert.ToInt32(ele.GetAttribute("x"));
            int tile_size_y = Convert.ToInt32(ele.GetAttribute("y"));
            ele = (XmlElement)doc.GetElementsByTagName("total-tiles")[0];
            int total_tiles = Convert.ToInt32(ele.InnerText);
            ele = (XmlElement)doc.GetElementsByTagName("tiles-high")[0];
            int tiles_high = Convert.ToInt32(ele.InnerText);
            ele = (XmlElement)doc.GetElementsByTagName("tiles-wide")[0];
            int tiles_wide = Convert.ToInt32(ele.InnerText);
            XmlNodeList heightNodes = doc.GetElementsByTagName("tile-heights");
            for (int i = 0; i < heightNodes.Count; i++)
            {
                XmlElement e = (XmlElement)heightNodes[i];
                bool lowH = e.GetAttribute("l").ToLower() == "true";
                bool highH = e.GetAttribute("h").ToLower() == "true";
                Height h = new Height(lowH, highH);
                returnTileset.heights_.Add(i, h);
                int xStart = (i * (tile_size_x + 1)) % (tiles_wide * (tile_size_x + 1));
                int yStart = (tile_size_y + 1) * ((i * (tile_size_x + 1)) / (tiles_wide * (tile_size_x + 1))); // I want integer division here AMP
                Rectangle r = new Rectangle(xStart, yStart, tile_size_x, tile_size_y);
                //GameTexture gT = new GameTexture(image_file, SpriteBatch, GraphicsDevice, r);
                //returnTileset.textures_.Add(i, gT);
            }

            return returnTileset;
        }

        internal void test()
        {
            int totalTiles = 15;
            int tilesWide = 5;
            int tilesHigh = 3;

            int width = 15;
            int height = 15;

            Rectangle[] imageDimensions = new Rectangle[totalTiles];

            for (int i = 0; i < totalTiles; i++)
            {
                int row = i / tilesWide;
                int col = (i - row * tilesWide);
                imageDimensions[i].X = row * (width + 1);
                imageDimensions[i].Y = col * (height + 1);
                imageDimensions[i].Width = width;
                imageDimensions[i].Height = height;
            }

            /*
            GameTexture texture = new GameTexture(
                filepath,
                spriteBatch,
                graphics,
                imageDimensions);
             */
        }
    }
}
