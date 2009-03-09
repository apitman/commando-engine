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
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;

namespace Commando
{
    /// <summary>
    /// MediaPlayerHelper contains a map of song names to loaded song content, as well
    /// as helper functions for fading and managing music which is played by MediaPlayer.
    /// </summary>
    internal static class MediaPlayerHelper
    {
        static Dictionary<string, Song> songs;

        /// <summary>
        /// Initializes static data structures.
        /// </summary>
        static MediaPlayerHelper()
        {
            songs = new Dictionary<string,Song>();
        }

        /// <summary>
        /// Loads songs from the ContentManager.
        /// </summary>
        /// <param name="cm">ContentManager containing the music files.</param>
        internal static void loadSongs(ContentManager cm)
        {
            // TODO
            // Change this to use an XML file
            songs["epic"] = cm.Load<Song>("Audio/music/epic");
        }

        /// <summary>
        /// Retrieve a loaded song.
        /// </summary>
        /// <param name="songName">Name of Song to load.</param>
        /// <returns>The Song matching the provided name, or null if it is not found.</returns>
        internal static Song getSong(string songName)
        {
            return songs[songName];
        }

        internal static void update()
        {
            throw new NotImplementedException();
        }

        internal static void nextSongFade(int fadeOutFrames, int fadeInFrames, string songName)
        {
            throw new NotImplementedException();
        }
    }
}
