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

namespace Commando.graphics
{
    public class CharacterReloadAction : CharacterActionInterface
    {
        protected const int PRIORITY = 13;

        protected int priority_;

        protected string actionLevel_;

        protected AnimationInterface[] animation_;

        protected CharacterAbstract character_;

        protected bool finished_;

        protected int currentFrame_;

        public CharacterReloadAction(CharacterAbstract character,
                                        AnimationInterface[] animation,
                                        string actionLevel)
        {
            character_ = character;
            animation_ = animation;
            actionLevel_ = actionLevel;
            finished_ = true;
            currentFrame_ = 0;
            priority_ = PRIORITY;
        }

        public void update()
        {
            int animSet = 0;
            if(animation_.GetLength(0) > 1)
            {
                animSet = character_.getActuator().getCurrentAnimationSet();
            }
            animation_[animSet].setPosition(character_.getPosition());
            animation_[animSet].setRotation(character_.getDirection());
            animation_[animSet].updateFrameNumber(currentFrame_);
            currentFrame_++;
            if (currentFrame_ >= animation_[animSet].getNumFrames())
            {
                character_.reload();
                SoundEngine.getInstance().playCue("gun-reload-1");
                finished_ = true;
            }
        }

        public void draw()
        {
            int animSet = 0;
            if(animation_.GetLength(0) > 1)
            {
                animSet = character_.getActuator().getCurrentAnimationSet();
            }
            animation_[animSet].draw();
        }

        public void draw(Microsoft.Xna.Framework.Graphics.Color color)
        {
            int animSet = 0;
            if(animation_.GetLength(0) > 1)
            {
                animSet = character_.getActuator().getCurrentAnimationSet();
            }
            animation_[animSet].draw(color);
        }

        public bool isFinished()
        {
            return finished_;
        }

        public CharacterActionInterface interrupt(CharacterActionInterface newAction)
        {
            if (newAction == this || (!finished_ && newAction.getPriority() <= priority_))
            {
                return this;
            }
            newAction.start();
            return newAction;
        }

        public int getPriority()
        {
            return priority_;
        }

        public string getActionLevel()
        {
            return actionLevel_;
        }

        public void setParameters(ActionParameters parameters)
        {
            
        }

        public void setCharacter(CharacterAbstract character)
        {
            character_ = character;
        }

        public void start()
        {
            finished_ = false;
            currentFrame_ = 0;
        }

        public Commando.collisiondetection.ConvexPolygonInterface getBounds(Commando.levels.HeightEnum height)
        {
            int animSet = 0;
            if(animation_.GetLength(0) > 1)
            {
                animSet = character_.getActuator().getCurrentAnimationSet();
            }
            return animation_[animSet].getBounds();
        }
    }
}
