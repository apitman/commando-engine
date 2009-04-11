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

/*
 * This code borrows heavily from a tutorial on MultiThreaded XNA programming
 * by Catalin Zima found here 
 *                    http://www.ziggyware.com/readarticle.php?article_id=221
 * 
 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Threading;

namespace Commando.graphics.multithreading
{
    public class DrawBuffer
    {
        private static DrawBuffer instance_;

        protected DrawStack[] stacks_;
        protected volatile int currentUpdateBuffer_;
        protected volatile int currentRenderBuffer_;

        protected AutoResetEvent renderFrameStart_;
        protected AutoResetEvent renderFrameEnd_;
        protected AutoResetEvent updateFrameStart_;
        protected AutoResetEvent updateFrameEnd_;

        protected volatile GameTime gameTime_;

        private DrawBuffer(int size)
        {
            stacks_ = new DrawStack[2];
            stacks_[0] = new DrawStack(size);
            stacks_[1] = new DrawStack(size);

            renderFrameStart_ = new AutoResetEvent(false);
            renderFrameEnd_ = new AutoResetEvent(false);
            updateFrameStart_ = new AutoResetEvent(false);
        }

        public static DrawBuffer getInstance()
        {
            if (instance_ == null)
            {
                throw new Exception("DrawBuffer must be initialized before it can be used!");
            }
            return instance_;
        }

        public static void initialize(int size)
        {
            instance_ = new DrawBuffer(size);
        }

        /// <summary>
        /// Resizes the buffers in a destructive manner
        /// </summary>
        /// <param name="size">The new size of each buffer</param>
        public void resizeTheBuffers(int size)
        {
            stacks_[0].resizeDestructively(size);
            stacks_[1].resizeDestructively(size);
        }

        public void reset()
        {
            //reset the buffer indices
            currentUpdateBuffer_ = 0;
            currentRenderBuffer_ = 1;

            //set all events to non-signaled
            renderFrameStart_.Reset();
            renderFrameEnd_.Reset();
            updateFrameStart_.Reset();
            updateFrameEnd_.Reset();
        }

        public void cleanUp()
        {
            //relese system resources
            renderFrameStart_.Close();
            renderFrameEnd_.Close();
            updateFrameStart_.Close();
            updateFrameEnd_.Close();
        }

        private void swapBuffers()
        {
            currentRenderBuffer_ = currentUpdateBuffer_;
            currentUpdateBuffer_ = (currentUpdateBuffer_ + 1) % 2;
        }

        public void globalStartFrame(GameTime gameTime)
        {
            swapBuffers();

            //signal the render and update threads to start processing
            renderFrameStart_.Set();
            updateFrameStart_.Set();
        }
        public void globalSynchronize()
        {
            //wait until both threads signal that they are finished
            renderFrameEnd_.WaitOne();
            updateFrameEnd_.WaitOne();
        }

        public void startUpdateProcessing()
        {
            //wait for start signal
            updateFrameStart_.WaitOne();
            //ensure cache coherency
            Thread.MemoryBarrier();
        }

        public void startRenderProcessing()
        {
            //wait for start signal
            renderFrameStart_.WaitOne();
            //ensure cache coherency
            Thread.MemoryBarrier();
        }

        public void submitUpdate()
        {
            //update is done
            updateFrameEnd_.Set();
            //ensure cache coherency
            Thread.MemoryBarrier();
        }
        
        public void submitRender()
        {
            //render is done
            renderFrameEnd_.Set();
            //ensure cache coherency
            Thread.MemoryBarrier();
        }

        public DrawStack getUpdateStack()
        {
            return stacks_[currentUpdateBuffer_];
        }

        public DrawStack getRenderStack()
        {
            return stacks_[currentRenderBuffer_];
        }
    }
}
