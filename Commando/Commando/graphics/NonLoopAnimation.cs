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

namespace Commando.graphics
{
    public class NonLoopAnimation : AnimationInterface
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
        
        public NonLoopAnimation(GameTexture sprites,
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

        public NonLoopAnimation(GameTexture sprites,
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

        public NonLoopAnimation(GameTexture sprites,
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
            currentFrame_ = currentFrame_ + numFrames;
            if (currentFrame_ >= totalFrames_)
            {
                currentFrame_ = totalFrames_ - 1;
            }
        }

        public void updateFrameNumber(int frameNumber)
        {
            currentFrame_ = frameNumber;
            if (currentFrame_ >= totalFrames_)
            {
                currentFrame_ = totalFrames_ - 1;
            }
        }

        public void update(Vector2 newPosition, Vector2 newRotation)
        {
            position_ = newPosition;
            rotation_ = newRotation;
        }

        public void setDepth(float depth)
        {
            depth_ = depth;
        }

        public void draw()
        {
            sprites_.drawImage(currentFrame_, position_, rotation_, depth_);
        }

        public void draw(Color color)
        {
            sprites_.drawImageWithColor(currentFrame_, position_, rotation_, depth_, color);
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

        public int getNumFrames()
        {
            return totalFrames_;
        }

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
