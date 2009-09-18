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
using Microsoft.Xna.Framework.Graphics;
using Commando.collisiondetection;
using Commando.graphics.multithreading;

namespace Commando.graphics
{
    public class LoopAnimation : AnimationInterface
    {
        protected GameTexture sprites_;

        protected int currentFrame_;

        protected int totalFrames_;

        protected Vector2 position_;

        protected Vector2 rotation_;

        protected float depth_;

        protected float frameLengthModifier_;

        protected Vector2 moved_;

        protected ConvexPolygonInterface[] boundsPolygon_;

        protected bool perFrameBounds_;

        public LoopAnimation(GameTexture sprites,
                                float frameLengthModifier,
                                float depth,
                                List<List<Vector2>> boundsPoints)
        {
            sprites_ = sprites;
            currentFrame_ = 0;
            totalFrames_ = sprites.getNumberOfImages();
            position_ = Vector2.Zero;
            rotation_ = Vector2.Zero;
            frameLengthModifier_ = frameLengthModifier;
            moved_ = Vector2.Zero;
            depth_ = depth;
            boundsPolygon_ = new ConvexPolygonInterface[boundsPoints.Count];
            for (int i = 0; i < boundsPoints.Count; i++)
            {
                boundsPolygon_[i] = new ConvexPolygon(boundsPoints[i], Vector2.Zero);
            }
            perFrameBounds_ = true;
        }
        
        public LoopAnimation(GameTexture sprites,
                                float frameLengthModifier,
                                float depth,
                                List<Vector2> boundsPoints)
        {
            sprites_ = sprites;
            currentFrame_ = 0;
            totalFrames_ = sprites.getNumberOfImages();
            position_ = Vector2.Zero;
            rotation_ = Vector2.Zero;
            frameLengthModifier_ = frameLengthModifier;
            moved_ = Vector2.Zero;
            depth_ = depth;
            boundsPolygon_ = new ConvexPolygonInterface[1];
            boundsPolygon_[0] = new ConvexPolygon(boundsPoints, Vector2.Zero);
            perFrameBounds_ = false;
        }

        public LoopAnimation(GameTexture sprites,
                                float frameLengthModifier,
                                float depth,
                                ConvexPolygonInterface polygon)
        {
            sprites_ = sprites;
            currentFrame_ = 0;
            totalFrames_ = sprites.getNumberOfImages();
            position_ = Vector2.Zero;
            rotation_ = Vector2.Zero;
            frameLengthModifier_ = frameLengthModifier;
            moved_ = Vector2.Zero;
            depth_ = depth;
            boundsPolygon_ = new ConvexPolygonInterface[1];
            boundsPolygon_[0] = polygon;
            perFrameBounds_ = false;
        }

        public void moveNFramesForward(int numFrames)
        {
            currentFrame_ = (currentFrame_ + numFrames) % totalFrames_;
        }

        public void updateFrameNumber(int frameNumber)
        {
            currentFrame_ = frameNumber % totalFrames_;
        }

        public void update(Vector2 newPosition, Vector2 newRotation)
        {
            moved_.X += newPosition.X - position_.X;
            moved_.Y += newPosition.Y - position_.Y;
            float magnitude = moved_.Length();

            rotation_ = newRotation;
            float rot1 = MathHelper.WrapAngle(getRotationAngle());
            float rot2 = MathHelper.WrapAngle((float)Math.Atan2((double)newPosition.Y, (double)newPosition.X));
            float rotDiff = rot1 - rot2;
            if (magnitude >= frameLengthModifier_)
            {
                float frameDiff = magnitude / frameLengthModifier_;
                if (rotDiff <= MathHelper.PiOver2 && rotDiff >= -MathHelper.PiOver2)
                {
                    currentFrame_ = (currentFrame_ + (int)frameDiff) % totalFrames_;
                }
                else
                {
                    currentFrame_ = (currentFrame_ - (int)frameDiff) % totalFrames_;
                    if (currentFrame_ < 0)
                    {
                        currentFrame_ += totalFrames_;
                    }
                }
                moved_.Normalize();
                moved_ *= magnitude % frameLengthModifier_;
            }
            position_ = newPosition;
        }

        public void setDepth(float depth)
        {
            depth_ = depth;
        }

        public void draw()
        {
            //sprites_.drawImage(currentFrame_, position_, getRotationAngle(), depth_);
            DrawStack stack = DrawBuffer.getInstance().getUpdateStack();
            TextureDrawer td = stack.getNext();
            td.Texture = sprites_;
            td.ImageIndex = currentFrame_;
            td.Position = position_;
            td.Dest = false;
            td.CoordinateType = CoordinateTypeEnum.RELATIVE;
            td.Depth = depth_;
            td.Centered = true;
            td.Color = Color.White;
            td.Effects = SpriteEffects.None;
            td.Direction = rotation_;
            td.Scale = 1.0f;
            stack.push();
        }

        public void draw(Color color)
        {
            //sprites_.drawImageWithColor(currentFrame_, position_, getRotationAngle(), depth_, color);
            DrawStack stack = DrawBuffer.getInstance().getUpdateStack();
            TextureDrawer td = stack.getNext();
            td.Texture = sprites_;
            td.ImageIndex = currentFrame_;
            td.Position = position_;
            td.Dest = false;
            td.CoordinateType = CoordinateTypeEnum.RELATIVE;
            td.Depth = depth_;
            td.Centered = true;
            td.Color = color;
            td.Effects = SpriteEffects.None;
            td.Direction = rotation_;
            td.Scale = 1.0f;
            stack.push();
        }

        public void reset()
        {
            moved_ = Vector2.Zero;
            currentFrame_ = 0;
        }

        public void setPosition(Vector2 position)
        {
            position_ = position;
        }

        public void setRotation(Vector2 rotation)
        {
            rotation_ = rotation;
        }

        /// <summary>
        /// Convert the direction vector into a float representing the angle of the object
        /// </summary>
        /// <returns>Angle of the rotation in radians</returns>
        protected float getRotationAngle()
        {
            return (float)Math.Atan2((double)rotation_.Y, (double)rotation_.X);
        }

        public int getNumFrames()
        {
            return totalFrames_;
        }


        /*
        protected void draw(Vector2 position, float rotation, float depth)
        {
            sprites_.drawImage(currentFrame_, position, rotation, depth);
        }
        */

        public ConvexPolygonInterface getBounds()
        {
            if (perFrameBounds_)
            {
                return boundsPolygon_[currentFrame_];
            }
            else
            {
                return boundsPolygon_[0];
            }
        }
    }
}
