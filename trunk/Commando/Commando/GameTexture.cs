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
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using System.Xml;

namespace Commando
{
    public class GameTexture
    {
        //Texture image for this GameTexture
        internal Texture2D texture_ { get; set; }

        //Each Vector4 is the bounds (x of top right corner, y of top right corner,
        //tileWidth, tileHeight) of each individual frame, or image in this texture
        internal Rectangle[] imageDimensions_ { get; set; }

        internal SpriteBatch spriteBatch_ { get; set; }

        protected static GlobalHelper helper_;

        /// <summary>
        /// Default constructor for a GameTexture
        /// </summary>
        public GameTexture()
        {
            helper_ = GlobalHelper.getInstance();
        }

        /// <summary>
        /// Create a GameTexture from the specified name, with the specified SpriteBatch and
        /// GraphicsDevice.
        /// </summary>
        /// <param name="filepath">Name of the image file</param>
        /// <param name="spriteBatch">SpriteBatch for the game</param>
        /// <param name="graphics">GraphicsDevice for the game</param>
        public GameTexture(string filename, SpriteBatch spriteBatch, GraphicsDevice graphics)
        {
            //TODO: add functionality to read from a file to get imageDims

            spriteBatch_ = spriteBatch;

            texture_ = TextureMap.getInstance().getContent().Load<Texture2D>(filename);

            imageDimensions_ = new Rectangle[1];
            imageDimensions_[0] = new Rectangle(0, 0, texture_.Width, texture_.Height);
            helper_ = GlobalHelper.getInstance();
        }

        /// <summary>
        /// Create a GameTexture with the specified imageDimensions.
        /// </summary>
        /// <param name="filepath">Name of the image file</param>
        /// <param name="spriteBatch">SpriteBatch for the game</param>
        /// <param name="graphics">GraphicsDevice for the game</param>
        /// <param name="imageDimensions">ImageDimensions for the texture</param>
        public GameTexture(string filename, SpriteBatch spriteBatch, GraphicsDevice graphics, Rectangle[] imageDimensions)
        {
            //TODO: add functionality to read from a file to get imageDims

            spriteBatch_ = spriteBatch;

            texture_ = TextureMap.getInstance().getContent().Load<Texture2D>(filename);

            imageDimensions_ = imageDimensions;
            helper_ = GlobalHelper.getInstance();
        }

        /// <summary>
        /// Create a copy of the GameTexture gTexture.
        /// </summary>
        /// <param name="gTexture">The GameTexture to be copied</param>
        public GameTexture(GameTexture gTexture)
        {
            spriteBatch_ = gTexture.spriteBatch_;

            texture_ = gTexture.texture_;

            Array.Copy(gTexture.imageDimensions_, imageDimensions_, gTexture.imageDimensions_.GetLength(0));
            helper_ = GlobalHelper.getInstance();
        }

        /// <summary>
        /// Load a texture from an xml file.
        /// </summary>
        /// <param name="filepath">Filename of the xml file</param>
        /// <param name="spriteBatch">SpriteBatch of the game</param>
        /// <param name="graphics">GraphicsDevice of the game</param>
        /// <returns>KeyValuePair with the key(name of the texture) and value(GameTexture loaded)</returns>
        public static KeyValuePair<string, GameTexture> loadTextureFromFile(string filename, SpriteBatch spriteBatch, GraphicsDevice graphics)
        {
            XmlTextReader reader = new XmlTextReader(filename);
            reader.ReadToFollowing("Key");
            string key = reader.ReadElementString();
            reader.ReadToFollowing("ImageFilename");
            string imageFilename = reader.ReadElementString();
            reader.ReadToFollowing("ImageDimensions");
            XmlReader dimReader = reader.ReadSubtree();
            dimReader.ReadToFollowing("NumberOfImages");
            int numberOfImages = dimReader.ReadElementContentAsInt();
            Rectangle[] imageDimensions = new Rectangle[numberOfImages];
            for (int i = 0; i < numberOfImages; i++)
            {
                dimReader.ReadToFollowing("Image");
                int x, y, w, h;
                XmlReader imReader = dimReader.ReadSubtree();
                imReader.ReadToFollowing("x");
                x = imReader.ReadElementContentAsInt();
                imReader.ReadToFollowing("y");
                y = imReader.ReadElementContentAsInt();
                imReader.ReadToFollowing("w");
                w = imReader.ReadElementContentAsInt();
                imReader.ReadToFollowing("h");
                h = imReader.ReadElementContentAsInt();
                imageDimensions[i] = new Rectangle(x, y, w, h);
            }
            return new KeyValuePair<string, GameTexture>(key, new GameTexture(imageFilename, spriteBatch, graphics, imageDimensions));
        }

        /// <summary>
        /// Get the image of this texture
        /// </summary>
        /// <returns>Texture2D for this GameTexture</returns>
        public Texture2D getTexture()
        {
            return texture_;
        }

        /// <summary>
        /// Set the image of this texture
        /// </summary>
        /// <param name="tex">Texture2D for this GameTexture</param>
        public void setTexture(Texture2D tex)
        {
            texture_ = tex;
        }

        /// <summary>
        /// Get the imageDimensions for this GameTexture
        /// </summary>
        /// <returns>Array of Rectangles representing each image's dimension</returns>
        public Rectangle[] getImageDimensions()
        {
            return imageDimensions_;
        }

        /// <summary>
        /// Set the imageDimensions for this GameTexture
        /// </summary>
        /// <param name="dims">Array of Rectangles representing each image's dimension</param>
        public void setImageDimensions(Rectangle[] dims)
        {
            Array.Copy(dims, imageDimensions_, dims.GetLength(0));
        }

        /// <summary>
        /// Get the number of images in this Texture.
        /// </summary>
        /// <returns>number of images</returns>
        public int getNumberOfImages()
        {
            return imageDimensions_.GetLength(0);
        }

        /// <summary>
        /// Get the SpriteBatch for the game.
        /// </summary>
        /// <returns>The SpriteBatch</returns>
        public SpriteBatch getSpriteBatch()
        {
            return spriteBatch_;
        }

        /// <summary>
        /// Set the SpriteBatch for the game.
        /// </summary>
        /// <param name="spriteBatch">The SpriteBatch</param>
        public void setSpriteBatch(SpriteBatch spriteBatch)
        {
            spriteBatch_ = spriteBatch;
        }

        /// <summary>
        /// Draws the image to the screen with a rotation.
        /// </summary>
        /// <param name="imageNumber">Number of the image to draw</param>
        /// <param name="position">Position to draw the image at</param>
        /// <param name="rotation">Rotation of the image</param>
        /// <param name="depth">Drawing depth of the image</param>
        public void drawImageAbsolute(int imageNumber, Vector2 position, float rotation, float depth)
        {
            Vector2 originOfImage = new Vector2(((float)imageDimensions_[imageNumber].Width) / 2.0f, ((float)imageDimensions_[imageNumber].Height) / 2.0f);
            spriteBatch_.Draw(texture_, position, imageDimensions_[imageNumber], Color.White, rotation, originOfImage, 1.0f, SpriteEffects.None, depth);
        }

        /// <summary>
        /// Draws the image to the screen with a rotation.
        /// </summary>
        /// <param name="imageNumber">Number of the image to draw</param>
        /// <param name="position">Position to draw the image at</param>
        /// <param name="depth">Drawing depth of the image</param>
        /// <param name="color">Color to draw the image</param>
        public void drawImageAbsolute(int imageNumber, Vector2 position, float depth, Color color)
        {
            Vector2 originOfImage = new Vector2(((float)imageDimensions_[imageNumber].Width) / 2.0f, ((float)imageDimensions_[imageNumber].Height) / 2.0f);
            spriteBatch_.Draw(texture_, position, imageDimensions_[imageNumber], color, 0, originOfImage, 1.0f, SpriteEffects.None, depth);
        }

        /// <summary>
        /// Draws the image to the screen with a rotation.
        /// </summary>
        /// <param name="imageNumber">Number of the image to draw</param>
        /// <param name="position">Position to draw the image at</param>
        /// <param name="direction">Direction of the image</param>
        /// <param name="depth">Drawing depth of the image</param>
        public void drawImageAbsolute(int imageNumber, Vector2 position, Vector2 direction, float depth)
        {
            float rotation = CommonFunctions.getAngle(direction);
            Vector2 originOfImage = new Vector2(((float)imageDimensions_[imageNumber].Width) / 2.0f, ((float)imageDimensions_[imageNumber].Height) / 2.0f);
            spriteBatch_.Draw(texture_, position, imageDimensions_[imageNumber], Color.White, rotation, originOfImage, 1.0f, SpriteEffects.None, depth);
        }

        /// <summary>
        /// Draw the image to the screen with no rotation.
        /// </summary>
        /// <param name="imageNumber">Number of the image to draw</param>
        /// <param name="position">Position to draw the image at</param>
        /// <param name="depth">Drawing depth of the image</param>
        public void drawImageAbsolute(int imageNumber, Vector2 position, float depth)
        {
            spriteBatch_.Draw(texture_, position, imageDimensions_[imageNumber], Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, depth);
        }
        
        /// <summary>
        /// Draw the image to the screen with the specified dimensions and a rotation.
        /// </summary>
        /// <param name="imageNumber">Number of the image to draw</param>
        /// <param name="destinationDims">Rectange where the image is to be drawn.  First two values are the top left corner, then the tileWidth and tileHeight</param>
        /// <param name="rotation">Rotation of the image</param>
        /// <param name="depth">Drawing depth of the image</param>
        public void drawImageWithDimAbsolute(int imageNumber, Rectangle destinationDims, float rotation, float depth)
        {
            Vector2 originOfImage = new Vector2(((float)imageDimensions_[imageNumber].Width) / 2.0f, ((float)imageDimensions_[imageNumber].Height) / 2.0f);
            spriteBatch_.Draw(texture_, destinationDims, imageDimensions_[imageNumber], Color.White, rotation, originOfImage, SpriteEffects.None, depth);
        }

        /// <summary>
        /// Draw the image to the screen with the specified dimensions and a rotation.
        /// </summary>
        /// <param name="imageNumber">Number of the image to draw</param>
        /// <param name="destinationDims">Rectange where the image is to be drawn.  First two values are the top left corner, then the tileWidth and tileHeight</param>
        /// <param name="direction">Direction of the image</param>
        /// <param name="depth">Drawing depth of the image</param>
        public void drawImageWithDimAbsolute(int imageNumber, Rectangle destinationDims, Vector2 direction, float depth)
        {
            float rotation = CommonFunctions.getAngle(direction);
            Vector2 originOfImage = new Vector2(((float)imageDimensions_[imageNumber].Width) / 2.0f, ((float)imageDimensions_[imageNumber].Height) / 2.0f);
            spriteBatch_.Draw(texture_, destinationDims, imageDimensions_[imageNumber], Color.White, rotation, originOfImage, SpriteEffects.None, depth);
        }

        /// <summary>
        /// Draw the image to the screen with the specified dimensions and a rotation.
        /// </summary>
        /// <param name="imageNumber">Number of the image to draw</param>
        /// <param name="destinationDims">Rectange where the image is to be drawn.  First two values are the top left corner, then the tileWidth and tileHeight</param>
        /// <param name="depth">Drawing depth of the image</param>
        public void drawImageWithDimAbsolute(int imageNumber, Rectangle destinationDims, float depth)
        {
            spriteBatch_.Draw(texture_, destinationDims, imageDimensions_[imageNumber], Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, depth);
        }

        public void drawImageWithDimAbsolute(int imageNumber, Rectangle destinationDims, float rotation, float depth, Vector2 origin, Color color)
        {
            Vector2 originOfImage = new Vector2(((float)imageDimensions_[imageNumber].Width) / 2.0f, ((float)imageDimensions_[imageNumber].Height) / 2.0f);
            spriteBatch_.Draw(texture_, destinationDims, imageDimensions_[imageNumber], color, rotation, origin, SpriteEffects.None, depth);
        }

        public void drawImageWithDimAbsolute(int imageNumber, Rectangle destinationDims, Vector2 direction, float depth, Vector2 origin, Color color)
        {
            float rotation = CommonFunctions.getAngle(direction);
            Vector2 originOfImage = new Vector2(((float)imageDimensions_[imageNumber].Width) / 2.0f, ((float)imageDimensions_[imageNumber].Height) / 2.0f);
            spriteBatch_.Draw(texture_, destinationDims, imageDimensions_[imageNumber], color, rotation, origin, SpriteEffects.None, depth);
        }

        public void drawImageWithDimAbsolute(int imageNumber, Rectangle destinationDims, float depth, Color color)
        {
            Vector2 originOfImage = new Vector2(((float)imageDimensions_[imageNumber].Width) / 2.0f, ((float)imageDimensions_[imageNumber].Height) / 2.0f);
            spriteBatch_.Draw(texture_, destinationDims, imageDimensions_[imageNumber], color, 0f, Vector2.Zero, SpriteEffects.None, depth);
        }

        public void drawImageWithColorAbsolute(int imageNumber, Vector2 position, float rotation, float depth, Color color)
        {
            Vector2 originOfImage = new Vector2(((float)imageDimensions_[imageNumber].Width) / 2.0f, ((float)imageDimensions_[imageNumber].Height) / 2.0f);
            spriteBatch_.Draw(texture_, position, imageDimensions_[imageNumber], color, rotation, originOfImage, 1.0f, SpriteEffects.None, depth);
        }

        public void drawImageWithColorAbsolute(int imageNumber, Vector2 position, Vector2 direction, float depth, Color color)
        {
            float rotation = CommonFunctions.getAngle(direction);
            Vector2 originOfImage = new Vector2(((float)imageDimensions_[imageNumber].Width) / 2.0f, ((float)imageDimensions_[imageNumber].Height) / 2.0f);
            spriteBatch_.Draw(texture_, position, imageDimensions_[imageNumber], color, rotation, originOfImage, 1.0f, SpriteEffects.None, depth);
        }

        /// <summary>
        /// Draws the image to the screen with a rotation.
        /// </summary>
        /// <param name="imageNumber">Number of the image to draw</param>
        /// <param name="position">Position to draw the image at</param>
        /// <param name="rotation">Rotation of the image</param>
        /// <param name="depth">Drawing depth of the image</param>
        public void drawImage(int imageNumber, Vector2 position, float rotation, float depth)
        {
            position.X -= helper_.getCurrentCamera().getX();
            position.Y -= helper_.getCurrentCamera().getY();
            Vector2 originOfImage = new Vector2(((float)imageDimensions_[imageNumber].Width) / 2.0f, ((float)imageDimensions_[imageNumber].Height) / 2.0f);
            spriteBatch_.Draw(texture_, position, imageDimensions_[imageNumber], Color.White, rotation, originOfImage, 1.0f, SpriteEffects.None, depth);
        }

        /// <summary>
        /// Draws the image to the screen with a rotation.
        /// </summary>
        /// <param name="imageNumber">Number of the image to draw</param>
        /// <param name="position">Position to draw the image at</param>
        /// <param name="direction">Direction of the image</param>
        /// <param name="depth">Drawing depth of the image</param>
        public void drawImage(int imageNumber, Vector2 position, Vector2 direction, float depth)
        {
            float rotation = CommonFunctions.getAngle(direction);
            position.X -= helper_.getCurrentCamera().getX();
            position.Y -= helper_.getCurrentCamera().getY();
            Vector2 originOfImage = new Vector2(((float)imageDimensions_[imageNumber].Width) / 2.0f, ((float)imageDimensions_[imageNumber].Height) / 2.0f);
            spriteBatch_.Draw(texture_, position, imageDimensions_[imageNumber], Color.White, rotation, originOfImage, 1.0f, SpriteEffects.None, depth);
        }

        /// <summary>
        /// Draw the image to the screen with no rotation.
        /// </summary>
        /// <param name="imageNumber">Number of the image to draw</param>
        /// <param name="position">Position to draw the image at</param>
        /// <param name="depth">Drawing depth of the image</param>
        public void drawImage(int imageNumber, Vector2 position, float depth)
        {
            position.X -= helper_.getCurrentCamera().getX();
            position.Y -= helper_.getCurrentCamera().getY();
            spriteBatch_.Draw(texture_, position, imageDimensions_[imageNumber], Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, depth);
        }

        /// <summary>
        /// Draw the image to the screen with the specified dimensions and a rotation.
        /// </summary>
        /// <param name="imageNumber">Number of the image to draw</param>
        /// <param name="destinationDims">Rectange where the image is to be drawn.  First two values are the top left corner, then the tileWidth and tileHeight</param>
        /// <param name="rotation">Rotation of the image</param>
        /// <param name="depth">Drawing depth of the image</param>
        public void drawImageWithDim(int imageNumber, Rectangle destinationDims, float rotation, float depth)
        {
            destinationDims.X -= (int)helper_.getCurrentCamera().getX();
            destinationDims.Y -= (int)helper_.getCurrentCamera().getY();
            Vector2 originOfImage = new Vector2(((float)imageDimensions_[imageNumber].Width) / 2.0f, ((float)imageDimensions_[imageNumber].Height) / 2.0f);
            spriteBatch_.Draw(texture_, destinationDims, imageDimensions_[imageNumber], Color.White, rotation, originOfImage, SpriteEffects.None, depth);
        }

        /// <summary>
        /// Draw the image to the screen with the specified dimensions and a rotation.
        /// </summary>
        /// <param name="imageNumber">Number of the image to draw</param>
        /// <param name="destinationDims">Rectange where the image is to be drawn.  First two values are the top left corner, then the tileWidth and tileHeight</param>
        /// <param name="direction">Direction of the image</param>
        /// <param name="depth">Drawing depth of the image</param>
        public void drawImageWithDim(int imageNumber, Rectangle destinationDims, Vector2 direction, float depth)
        {
            float rotation = CommonFunctions.getAngle(direction);
            destinationDims.X -= (int)helper_.getCurrentCamera().getX();
            destinationDims.Y -= (int)helper_.getCurrentCamera().getY();
            Vector2 originOfImage = new Vector2(((float)imageDimensions_[imageNumber].Width) / 2.0f, ((float)imageDimensions_[imageNumber].Height) / 2.0f);
            spriteBatch_.Draw(texture_, destinationDims, imageDimensions_[imageNumber], Color.White, rotation, originOfImage, SpriteEffects.None, depth);
        }

        /// <summary>
        /// Draw the image to the screen with the specified dimensions and a rotation.
        /// </summary>
        /// <param name="imageNumber">Number of the image to draw</param>
        /// <param name="destinationDims">Rectange where the image is to be drawn.  First two values are the top left corner, then the tileWidth and tileHeight</param>
        /// <param name="depth">Drawing depth of the image</param>
        public void drawImageWithDim(int imageNumber, Rectangle destinationDims, float depth)
        {
            destinationDims.X -= (int)helper_.getCurrentCamera().getX();
            destinationDims.Y -= (int)helper_.getCurrentCamera().getY();
            spriteBatch_.Draw(texture_, destinationDims, imageDimensions_[imageNumber], Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, depth);
        }

        public void drawImageWithDim(int imageNumber, Rectangle destinationDims, float rotation, float depth, Vector2 origin, Color color)
        {
            destinationDims.X -= (int)helper_.getCurrentCamera().getX();
            destinationDims.Y -= (int)helper_.getCurrentCamera().getY();
            Vector2 originOfImage = new Vector2(((float)imageDimensions_[imageNumber].Width) / 2.0f, ((float)imageDimensions_[imageNumber].Height) / 2.0f);
            spriteBatch_.Draw(texture_, destinationDims, imageDimensions_[imageNumber], color, rotation, origin, SpriteEffects.None, depth);
        }

        public void drawImageWithDim(int imageNumber, Rectangle destinationDims, Vector2 direction, float depth, Vector2 origin, Color color)
        {
            float rotation = CommonFunctions.getAngle(direction);
            destinationDims.X -= (int)helper_.getCurrentCamera().getX();
            destinationDims.Y -= (int)helper_.getCurrentCamera().getY();
            Vector2 originOfImage = new Vector2(((float)imageDimensions_[imageNumber].Width) / 2.0f, ((float)imageDimensions_[imageNumber].Height) / 2.0f);
            spriteBatch_.Draw(texture_, destinationDims, imageDimensions_[imageNumber], color, rotation, origin, SpriteEffects.None, depth);
        }

        public void drawImageWithColor(int imageNumber, Vector2 position, float rotation, float depth, Color color)
        {
            position.X -= helper_.getCurrentCamera().getX();
            position.Y -= helper_.getCurrentCamera().getY();
            Vector2 originOfImage = new Vector2(((float)imageDimensions_[imageNumber].Width) / 2.0f, ((float)imageDimensions_[imageNumber].Height) / 2.0f);
            spriteBatch_.Draw(texture_, position, imageDimensions_[imageNumber], color, rotation, originOfImage, 1.0f, SpriteEffects.None, depth);
        }

        public void drawImageWithColor(int imageNumber, Vector2 position, Vector2 direction, float depth, Color color)
        {
            float rotation = CommonFunctions.getAngle(direction);
            position -= helper_.getCurrentCamera().getPosition();
            Vector2 originOfImage = new Vector2(((float)imageDimensions_[imageNumber].Width) / 2.0f, ((float)imageDimensions_[imageNumber].Height) / 2.0f);
            spriteBatch_.Draw(texture_, position, imageDimensions_[imageNumber], color, rotation, originOfImage, 1.0f, SpriteEffects.None, depth);
        }

        internal TextureDrawer getDrawer(Vector2 position, float depth)
        {
            TextureDrawer def = new TextureDrawer(this, position, depth);
            def.Origin =
                new Vector2(((float)imageDimensions_[def.ImageIndex].Width) / 2.0f, ((float)imageDimensions_[def.ImageIndex].Height) / 2.0f);
            return def;
        }
    }

    internal enum CoordinateTypeEnum
    {
        RELATIVE,
        ABSOLUTE
    }

    internal class TextureDrawer
    {
        internal GameTexture Texture { get; private set; }
        internal Vector2 Position { get; private set; }
        internal float Rotation { get; set; }
        internal Vector2 Direction
        {
            get
            {
                return CommonFunctions.getVector(Rotation);
            }

            set
            {
                Rotation = CommonFunctions.getAngle(value);
            }
        }
        internal CoordinateTypeEnum CoordinateType { get; set; }
        internal float Depth { get; private set; }
        internal Color Color { get; set; }
        internal int ImageIndex { get; set; }
        internal SpriteEffects Effects { get; set; }
        internal Vector2 Origin { get; set; }
        internal float Scale { get; set; }

        private TextureDrawer() { }

        internal TextureDrawer(GameTexture texture, Vector2 position, float depth)
        {
            Texture = texture;
            Position = position;
            Depth = depth;
            Rotation = 0;
            CoordinateType = CoordinateTypeEnum.RELATIVE;
            Color = Color.White;
            ImageIndex = 0;
            Effects = SpriteEffects.None;
            Scale = 1.0f;
        }

        internal void draw()
        {
            if (this.CoordinateType == CoordinateTypeEnum.RELATIVE)
            {
                GlobalHelper helper = GlobalHelper.getInstance();
                this.Position -= helper.getCurrentCamera().getPosition();
            }
            this.Texture.spriteBatch_.Draw(
                              this.Texture.texture_,
                              this.Position,
                              this.Texture.imageDimensions_[this.ImageIndex],
                              this.Color,
                              this.Rotation,
                              this.Origin,
                              this.Scale,
                              this.Effects,
                              this.Depth);
        }
    }
}
