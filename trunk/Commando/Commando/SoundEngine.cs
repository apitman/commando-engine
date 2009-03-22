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

using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

// Important Notice:
// The techniques used in constructing this class are based on those found
// at http://www.ziggyware.com/readarticle.php?article_id=40.
//
// Thank you very much for your tutorial, 'zygote'!

namespace Commando
{
    /// <summary>
    /// A simple class for pulling sounds from a compiled XACT project and
    /// playing them.  Employs the Singleton pattern.
    /// </summary>
    internal class SoundEngine
    {
        private static SoundEngine instance_;

        private AudioEngine audio_;
        private WaveBank waveBank_;
        private SoundBank soundBank_;

        internal Cue Music { get; set; }

        /// <summary>
        /// Private constructor as per the Singleton pattern which reads the
        /// compiled sound files into memory.
        /// </summary>
        private SoundEngine()
        {
            // These files are automatically created in the output directory
            //  matching the relative path of wherever the .xap file is located
            audio_ = new AudioEngine(@"Content\Audio\sounds.xgs");
            waveBank_ = new WaveBank(audio_,@"Content\Audio\waves1.xwb");
            soundBank_ = new SoundBank(audio_,@"Content\Audio\sounds1.xsb");
        }

        /// <summary>
        /// Provides the instance of the class as per the Singleton pattern.
        /// </summary>
        /// <returns>The only instance of SoundEngine.</returns>
        public static SoundEngine getInstance()
        {
            if (instance_ == null)
            {
                instance_ = new SoundEngine();
            }
            return instance_;
        }

        /// <summary>
        /// Plays a sound based on a provided key.
        /// </summary>
        /// <param name="cueName">The cue key from the XACT project.</param>
        /// <returns>Returns a handle to the sound.</returns>
        public Cue playCue(string cueName)
        {
            Cue cue = soundBank_.GetCue(cueName);
            cue.Play();
            return cue;
        }

        /// <summary>
        /// Stops a sound immediately.
        /// </summary>
        /// <param name="cue">The handle of the sound to stop.</param>
        public static void stopCue(Cue cue)
        {
            cue.Stop(AudioStopOptions.Immediate);
        }

        /// <summary>
        /// Cleans up the instance's resources.
        /// </summary>
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

        public void changeAllVolume(float amount)
        {
            AudioCategory cat = audio_.GetCategory("Music");
            cat.SetVolume(amount);
        }
    }
}
