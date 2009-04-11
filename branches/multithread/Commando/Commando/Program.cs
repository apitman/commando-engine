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
#if !XBOX
using System.Windows.Forms;
#endif

namespace Commando
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            try
            {
                using (Engine game = new Engine())
                {
                    game.Run();
                }
            }
            catch (Exception e)
            {
#if !XBOX
                MessageBox.Show(e.Message);
                MessageBox.Show(e.StackTrace);
#else
                // See CrashDebugGame.cs for credits to Nick Gravelyn
                // for this technique and code.
                using (CrashDebugGame game = new CrashDebugGame(e))
                    game.Run();
#endif
            }
        }
    }
}

