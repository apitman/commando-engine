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

namespace Commando
{
    internal static class CommLogger
    {
        private static string output_ = "";
        private static int msgsSent_ = 0;
        private static int msgsRecvd_ = 0;
        private static int redundantMsgs_ = 0;
        private static int freshMsgs_ = 0;

        internal static void addOutput(string value)
        {
            output_ += value;
            output_ += "\n";
        }

        internal static string printOutput()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine();
            sb.AppendLine();
            sb.AppendLine();
            sb.AppendLine("===============");
            sb.AppendLine("COMM SYS LOGGER");
            sb.AppendLine("===============");
            sb.AppendLine("Messages Sent  : " + msgsSent_.ToString());
            sb.AppendLine("Messages Rcvd  : " + msgsRecvd_.ToString());
            sb.AppendLine("Redundant Msgs : " + redundantMsgs_.ToString());
            sb.AppendLine("Fresh Msgs     : " + freshMsgs_.ToString());
            sb.AppendLine();
            sb.AppendLine(output_);

            return sb.ToString();
        }

        internal static void sentMsg()
        {
            msgsSent_++;
        }

        internal static void recvdMsg(bool isRedundant)
        {
            msgsRecvd_++;
            if (isRedundant)
            {
                redundantMsgs_++;
            }
            else
            {
                freshMsgs_++;
            }
        }
    }

}
