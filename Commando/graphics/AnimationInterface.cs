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

namespace Commando.graphics
{
    public interface AnimationInterface
    {
        void updateFrameNumber(int frameNumber);

        void moveNFramesForward(int numFrames);

        void setPosition(Vector2 position);

        void setRotation(Vector2 rotation);

        void update(Vector2 newPosition, Vector2 newRotation);

        void setDepth(float depth);

        void draw();

        void draw(Color color);

        void reset();

        //TODO: Add functionality to get the current frame's bounds
    }
}
