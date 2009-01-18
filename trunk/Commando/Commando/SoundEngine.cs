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
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;

// IMPORTANT NOTICE:
// The techniques used in constructing this class are based on those found
// at http://www.ziggyware.com/readarticle.php?article_id=40.
//
// Thank you very much for your tutorial, 'zygote'!

namespace Commando
{
    class SoundEngine
    {
        private static SoundEngine instance_;

        private AudioEngine audio_;
        private WaveBank waveBank_;
        private SoundBank soundBank_;

        private SoundEngine()
        {
#if !XBOX
            audio_ = new AudioEngine(@"Content\sounds\Win\sounds.xgs");
            waveBank_ = new WaveBank(audio_,@"Content\sounds\Win\MainMenuWaveBank.xwb");
            soundBank_ = new SoundBank(audio_,@"Content\sounds\Win\MainMenuSoundBank.xsb");
#else
            throw new NotImplementedException();
#endif
        }

        public static SoundEngine getInstance()
        {
            if (instance_ == null)
            {
                instance_ = new SoundEngine();
            }
            return instance_;
        }

        public Cue playCue(string cueName)
        {
            Cue cue = soundBank_.GetCue(cueName);
            cue.Play();
            return cue;
        }

        public static void stopCue(Cue cue)
        {
            cue.Stop(AudioStopOptions.Immediate);
        }

        public static void cleanup()
        {
            if (instance_ != null)
            {
                instance_.audio_.Dispose();
                instance_.waveBank_.Dispose();
                instance_.soundBank_.Dispose();
            }
            instance_ = null;
        }
    }
}
