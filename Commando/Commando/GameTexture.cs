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

namespace Commando
{
    class GameTexture
    {
        //Texture image for this GameTexture
        protected Texture2D texture_;

        //Each Vector4 is the bounds (x of top right corner, y of top right corner,
        //width, height) of each individual frame, or image in this texture
        protected Rectangle[] imageDimensions_;

        protected SpriteBatch spriteBatch_;

        public GameTexture()
        {

        }

        public GameTexture(string filename, SpriteBatch spriteBatch, GraphicsDevice graphics)
        {
            //TODO: add functionality to read from a file to get imageDims

            spriteBatch_ = spriteBatch;

            texture_ = Texture2D.FromFile(graphics, filename);

            imageDimensions_ = new Rectangle[1];
            imageDimensions_[0] = new Rectangle(0, 0, texture_.Width, texture_.Height);
        }

        public GameTexture(GameTexture gTexture)
        {
            spriteBatch_ = gTexture.spriteBatch_;

            texture_ = gTexture.texture_;

            Array.Copy(gTexture.imageDimensions_, imageDimensions_, gTexture.imageDimensions_.GetLength(0));
        }

        public Texture2D getTexture()
        {
            return texture_;
        }

        public void setTexture(Texture2D tex)
        {
            texture_ = tex;
        }

        public Rectangle[] getImageDimensions()
        {
            return imageDimensions_;
        }

        public void setImageDimensions(Rectangle[] dims)
        {
            Array.Copy(dims, imageDimensions_, dims.GetLength(0));
        }

        public int getNumberOfImages()
        {
            return imageDimensions_.GetLength(0);
        }

        public SpriteBatch getSpriteBatch()
        {
            return spriteBatch_;
        }

        public void setSpriteBatch(SpriteBatch spriteBatch)
        {
            spriteBatch_ = spriteBatch;
        }

        //preconditions: texture_ and imageDimensions_ are not NULL
        //postconditions: the specified image is drawn to the screen at origin with the
        //  specified rotation and depth.
        public void drawImage(int imageNumber, Vector2 position, float rotation, float depth)
        {
            Vector2 originOfImage = new Vector2(((float)imageDimensions_[imageNumber].Width) / 2.0f, ((float)imageDimensions_[imageNumber].Height) / 2.0f);
            spriteBatch_.Draw(texture_, position, imageDimensions_[imageNumber], Color.White, rotation, originOfImage, 1.0f, SpriteEffects.None, depth);
        }

        public void drawImageWithDim(int imageNumber, Rectangle destinationDims, float rotation, float depth)
        {
            Vector2 originOfImage = new Vector2(((float)imageDimensions_[imageNumber].Width) / 2.0f, ((float)imageDimensions_[imageNumber].Height) / 2.0f);
            spriteBatch_.Draw(texture_, destinationDims, imageDimensions_[imageNumber], Color.White, rotation, originOfImage, SpriteEffects.None, depth);
        }

        public void drawImageWithDim(int imageNumber, Rectangle destinationDims, float depth)
        {
            spriteBatch_.Draw(texture_, destinationDims, imageDimensions_[imageNumber], Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, depth);
        }
    }
}
