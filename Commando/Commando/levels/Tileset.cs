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
using Commando.graphics.multithreading;

namespace Commando.levels
{
    /// <summary>
    /// Data structure for a tileset read in from an XML file; maps tile index
    /// numbers to pictures and tile information such as heights.
    /// </summary>
    public class Tileset
    {
        protected Dictionary<int, Height> heights_ { get; private set;}
        protected GameTexture texture_;

        internal int TILE_SIZE_X;
        internal int TILE_SIZE_Y;

        /// <summary>
        /// Private constructor; use factory methods.
        /// </summary>
        private Tileset()
        {
            heights_ = new Dictionary<int, Height>();
        }

        /// <summary>
        /// Loads a Tileset from packaged Content.
        /// </summary>
        /// <param name="tilesetName">Content-based path for the tileset.</param>
        /// <param name="engine">The game engine.</param>
        /// <returns>The tileset with that name.</returns>
        public static Tileset getTilesetFromContent(string tilesetName, Engine engine)
        {
            Tileset tileset;
            using (ManagedXml doc = engine.Content.Load<ManagedXml>(tilesetName))
            {
                tileset = Tileset.getTilesetFromXML(doc, engine.spriteBatch_, engine.GraphicsDevice);
            }
            GC.Collect();
            GC.WaitForPendingFinalizers();
            return tileset;
        }

        /// <summary>
        /// Constructs a TileSet from an ManagedXml.
        /// </summary>
        /// <param name="doc">XML data containing Tileset related tags.</param>
        /// <param name="spriteBatch">Sprite batch which will draw the tiles.</param>
        /// <param name="graphics">Graphics device which will draw the tiles.</param>
        /// <returns>The constructed TileSet.</returns>
        protected static Tileset getTilesetFromXML(ManagedXml doc, SpriteBatch spriteBatch, GraphicsDevice graphics)
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
            XmlNodeList heightsNodes = doc.GetElementsByTagName("tile-heights");
            if (heightsNodes.Count != 1)
            {
                throw new Exception("Error in Tileset XML, should be exactly one tile-heights tag");
            }
            XmlNode heightsNode = heightsNodes[0];
            if (heightsNode.ChildNodes.Count != total_tiles)
            {
                throw new Exception("Number of height tags do not match total-tiles");
            }
            for (int i = 0; i < heightsNode.ChildNodes.Count; i++)
            {
                XmlElement e = (XmlElement)heightsNode.ChildNodes[i];
                bool lowH = e.GetAttribute("l").ToLower() == "true";
                bool highH = e.GetAttribute("h").ToLower() == "true";
                Height h = new Height(lowH, highH);
                returnTileset.heights_.Add(i, h);
            }

            returnTileset.TILE_SIZE_X = tile_size_x;
            returnTileset.TILE_SIZE_Y = tile_size_y;

            returnTileset.texture_ = new GameTexture(image_file,
                spriteBatch,
                graphics,
                calculateImageDimensions(total_tiles, tiles_wide, tiles_high, tile_size_x, tile_size_y));

            return returnTileset;
        }

        /// <summary>
        /// Lookup the height of a specific tile type.
        /// </summary>
        /// <param name="tileID">ID of the tile type.</param>
        /// <returns>The height corresponding to that tile.</returns>
        public Height getHeight(int tileID)
        {
            return heights_[tileID];
        }

        /// <summary>
        /// Draw a tile from the tileset
        /// </summary>
        /// <param name="position">Top-left corner of the drawing rectangle</param>
        /// <param name="tileID">ID of the tile to be drawn</param>
        public void draw(Vector2 position, int tileID)
        {
            texture_.drawImage(tileID, position, lookupDepth(tileID));
            DrawStack stack = DrawBuffer.getInstance().getUpdateStack();
            TextureDrawer td = stack.getNext();
            td.set(texture_,
                    tileID,
                    position,
                    CoordinateTypeEnum.RELATIVE,
                    lookupDepth(tileID),
                    false,
                    Color.White,
                    0.0f,
                    1.0f);
            stack.push();
        }

        /// <summary>
        /// Determine appropriate drawing depth based on the height of
        /// a Tile ID.
        /// </summary>
        /// <param name="tileID">ID of the tile being drawn.</param>
        /// <returns>A depth at which to draw the tile.</returns>
        protected float lookupDepth(int tileID)
        {
            Height h = heights_[tileID];
            if (h.blocksHigh_)
                return Constants.DEPTH_HIGH;
            else if (h.blocksLow_)
                return Constants.DEPTH_LOW;
            else
                return Constants.DEPTH_GROUND;
        }

        /// <summary>
        /// Create rectangles for each individual tile within the tileset
        /// image, based on the parameters.
        /// </summary>
        /// <param name="totalTiles">Number of tiles in the image.</param>
        /// <param name="tilesX">Number of tiles in a row in the image.</param>
        /// <param name="tilesY">Number of tiles in a col in the image.</param>
        /// <param name="tileWidth">Width, in pixels, of a single tile.</param>
        /// <param name="tileHeight">Height, in pixels, of a single tile.</param>
        /// <returns>An array of rectangles corresponding to tiles, in order of TileID.</returns>
        private static Rectangle[] calculateImageDimensions(int totalTiles, int tilesX, int tilesY, int tileWidth, int tileHeight)
        {
            Rectangle[] imageDimensions = new Rectangle[totalTiles];

            for (int i = 0; i < totalTiles; i++)
            {
                int row = i / tilesX;
                int col = (i - row * tilesX);

                // 2 Pixel buffer in each direction to prevent sprite bleeding
                //  when using texture filtering; 2 because of 1 for each side, so
                //  the first of 2 pixels is the duplicated right edge of one tile
                //  and the second is the duplicated left edge of the next tile,
                //  and similar in the up/down direction as well.
                imageDimensions[i].X = col * (tileWidth + 2);
                imageDimensions[i].Y = row * (tileHeight + 2);
                imageDimensions[i].Width = tileWidth;
                imageDimensions[i].Height = tileHeight;
            }

            return imageDimensions;
        }
    }
}
